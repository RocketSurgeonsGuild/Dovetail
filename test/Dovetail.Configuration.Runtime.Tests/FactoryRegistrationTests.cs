using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Dovetail.Configuration.Runtime.Tests;

/// <summary>
///     Covers the reflection-free creating overload of <see cref="DovetailConfigurationOptionsExtensions" />, which generated
///     configuration joints call for types the options pipeline can't create. <see cref="ServerRecord.Create" /> and
///     <see cref="EndpointOptions.Create" /> stand in for the generated factories.
/// </summary>
public sealed class FactoryRegistrationTests
{
    [Test]
    public async Task Create_BuildsATypeWithoutAParameterlessConstructor()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(TestConfiguration.Build(("Server:Host", "example.org"), ("Server:Port", "8080")));
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        var server = provider.GetRequiredService<IOptions<ServerRecord>>().Value;
        server.Host.ShouldBe("example.org");
        server.Port.ShouldBe(8080);
    }

    [Test]
    public async Task Create_IgnoresASecondCreatingRegistration_ForTheSameType()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(
                           TestConfiguration.Build(
                               ("Defaults:Server:Host", "default-host"),
                               ("Defaults:Server:Port", "80"),
                               ("Server:Port", "8080"),
                               ("Server:Tags:0", "c")
                           )
                       );
        services.AddDovetailConfigurationOptions("Server", Noted(ServerRecord.Create, "created by Server"));
        services.AddDovetailConfigurationOptions("Defaults:Server", Noted(ServerRecord.Create, "created by Defaults:Server"));

        services.Count(descriptor => descriptor.ServiceType == typeof(IOptionsFactory<ServerRecord>)).ShouldBe(1);
        await using var provider = services.BuildServiceProvider();

        // The first registration wins, and nothing is merged from the second section.
        var server = provider.GetRequiredService<IOptions<ServerRecord>>().Value;
        server.Host.ShouldBe("localhost");
        server.Port.ShouldBe(8080);
        server.Tags.ShouldBe(["c"]);
        server.Note.ShouldBe("created by Server");
    }

    [Test]
    public async Task Create_RunsConfigureAndPostConfigure_ThenValidators()
    {
        var configuration = TestConfiguration.Build(("Server:Host", "example.org"), ("Server:Port", "8080"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);
        services.Configure<ServerRecord>(server => server.Tags.Add("configured"));
        services.PostConfigure<ServerRecord>(server => server.Note = "post-configured");
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<ServerRecord>, ServerRecordValidator>());

        await using var provider = services.BuildServiceProvider();

        var server = provider.GetRequiredService<IOptions<ServerRecord>>().Value;
        server.Tags.ShouldBe(["configured"]);
        server.Note.ShouldBe("post-configured");

        configuration["Server:Port"] = "0";
        using var scope = provider.CreateScope();
        var exception = Should.Throw<OptionsValidationException>(() => scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ServerRecord>>().Value);
        exception.Failures.ShouldBe(["Port must be between 1 and 65535, but was 0."]);
    }

    [Test]
    public async Task Create_ThenAppliesBindRegistrations_OntoTheCreatedInstance()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(TestConfiguration.Build(("Server:Host", "example.org"), ("Server:Tags:0", "created"), ("ExtraServer:Tags:0", "bound")));
        services.AddDovetailConfigurationOptions<ServerRecord>("ExtraServer", ServerRecord.BindTags);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IOptions<ServerRecord>>().Value.Tags.ShouldBe(["created", "bound"]);
    }

    [Test]
    public async Task Create_Reload_ReachesMonitorAndSnapshot_Once()
    {
        var configuration = TestConfiguration.Build(("Server:Host", "example.org"), ("Server:Port", "8080"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<ServerRecord>>();
        monitor.CurrentValue.Port.ShouldBe(8080);
        ReadSnapshot(provider).Port.ShouldBe(8080);

        var changes = new List<string?>();
        using var subscription = monitor.OnChange((_, name) => changes.Add(name));

        configuration["Server:Port"] = "9090";
        configuration.Reload();

        changes.ShouldBe([Options.DefaultName]);
        monitor.CurrentValue.Host.ShouldBe("example.org");
        monitor.CurrentValue.Port.ShouldBe(9090);
        ReadSnapshot(provider).Port.ShouldBe(9090);
    }

    [Test]
    public async Task Create_LeavesNamedOptionsToTheBaseFactory()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(TestConfiguration.Build(("Endpoint:Url", "https://example.org"), ("Server:Port", "8080")));
        services.AddDovetailConfigurationOptions("Endpoint", EndpointOptions.Create);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        var endpoints = provider.GetRequiredService<IOptionsMonitor<EndpointOptions>>();
        endpoints.CurrentValue.Url.ShouldBe("https://example.org");
        endpoints.Get("named").Url.ShouldBe("http://localhost");

        // The base factory needs a public parameterless constructor, which ServerRecord doesn't have.
        Should.Throw<MissingMethodException>(() => provider.GetRequiredService<IOptionsMonitor<ServerRecord>>().Get("named"));
    }

    [Test]
    public async Task Create_KeepsAFactoryAlreadyRegisteredForTheType()
    {
        var registered = new ServerRecord("registered", 1);
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Server:Host", "example.org")));
        services.AddSingleton<IOptionsFactory<ServerRecord>>(new FixedFactory(registered));
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IOptions<ServerRecord>>().Value.ShouldBeSameAs(registered);
    }

    [Test]
    public async Task Create_ValidateOnStart_OnTheReturnedBuilder_FailsHostStart()
    {
        var builder = Host.CreateEmptyApplicationBuilder(new());
        builder.Configuration.AddInMemoryCollection([new("Server:Port", "0")]);
        builder
           .Services.AddDovetailConfigurationOptions("Server", ServerRecord.Create)
           .Validate(server => server.Port > 0, "A server needs a port.")
           .ValidateOnStart();

        using var host = builder.Build();

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => host.StartAsync());
        exception.Failures.ShouldBe(["A server needs a port."]);
    }

    private static Func<IConfiguration, ServerRecord> Noted(Func<IConfiguration, ServerRecord> create, string note) =>
        configuration =>
        {
            var server = create(configuration);
            server.Note = note;
            return server;
        };

    private static ServerRecord ReadSnapshot(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ServerRecord>>().Value;
    }

    private sealed class FixedFactory(ServerRecord server) : IOptionsFactory<ServerRecord>
    {
        public ServerRecord Create(string name) => server;
    }
}
