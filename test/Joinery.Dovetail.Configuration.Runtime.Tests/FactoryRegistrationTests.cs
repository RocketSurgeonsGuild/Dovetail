using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Joinery.Dovetail.Configuration.Runtime.Tests;

/// <summary>
///     Covers <see cref="DovetailConfigurationOptionsExtensions" /> for types the options pipeline can't create itself (no
///     parameterless constructor, or init-only members). <see cref="ServerRecord.Create" /> and
///     <see cref="EndpointOptions.Create" /> stand in for the generated <c>Create…</c> methods.
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
        await server.Host.Should().BeEqualTo("example.org");
        await server.Port.Should().BeEqualTo(8080);
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

        await services.Count(descriptor => descriptor.ServiceType == typeof(IOptionsFactory<ServerRecord>)).Should().BeEqualTo(1);
        await using var provider = services.BuildServiceProvider();

        // The first registration wins, and nothing is merged from the second section.
        var server = provider.GetRequiredService<IOptions<ServerRecord>>().Value;
        await server.Host.Should().BeEqualTo("localhost");
        await server.Port.Should().BeEqualTo(8080);
        await server.Tags.Should().BeEquivalentTo(["c"], CollectionOrdering.Matching);
        await server.Note.Should().BeEqualTo("created by Server");
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
        await server.Tags.Should().BeEquivalentTo(["configured"], CollectionOrdering.Matching);
        await server.Note.Should().BeEqualTo("post-configured");

        configuration["Server:Port"] = "0";
        using var scope = provider.CreateScope();
        var exception = await ( (Func<object?>)( () => scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ServerRecord>>().Value ) ).Should().Throw<OptionsValidationException>();
        await exception.Failures.Should().BeEquivalentTo(["Port must be between 1 and 65535, but was 0."], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Create_ForANonDefaultName_Throws_NamingTheTypeAndTheName()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Server:Host", "example.org")));
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        var exception = await ( (Action)( () => provider.GetRequiredService<IOptionsMonitor<ServerRecord>>().Get("tenant-a") ) ).Should().Throw<InvalidOperationException>();
        await exception.Message.Should().Contain(nameof(ServerRecord));
        await exception.Message.Should().Contain("tenant-a");
        await provider.GetRequiredService<IOptionsMonitor<ServerRecord>>().CurrentValue.Host.Should().BeEqualTo("example.org");
    }

    [Test]
    public async Task Create_Reload_ReachesMonitorAndSnapshot_Once()
    {
        var configuration = TestConfiguration.Build(("Server:Host", "example.org"), ("Server:Port", "8080"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<ServerRecord>>();
        await monitor.CurrentValue.Port.Should().BeEqualTo(8080);
        await ReadSnapshot(provider).Port.Should().BeEqualTo(8080);

        var changes = new List<string?>();
        using var subscription = monitor.OnChange((_, name) => changes.Add(name));

        configuration["Server:Port"] = "9090";
        configuration.Reload();

        await changes.Should().BeEquivalentTo([Options.DefaultName], CollectionOrdering.Matching);
        await monitor.CurrentValue.Host.Should().BeEqualTo("example.org");
        await monitor.CurrentValue.Port.Should().BeEqualTo(9090);
        await ReadSnapshot(provider).Port.Should().BeEqualTo(9090);
    }

    [Test]
    public async Task Create_ApplicationFactoryRegisteredBeforehand_Wins()
    {
        var registered = new ServerRecord("registered", 1);
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Server:Host", "example.org")));
        services.AddSingleton<IOptionsFactory<ServerRecord>>(new FixedFactory(registered));
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        await provider.GetRequiredService<IOptions<ServerRecord>>().Value.Should().BeSameReferenceAs(registered);
    }

    [Test]
    public async Task Create_ApplicationFactoryRegisteredAfterwards_Wins()
    {
        var registered = new ServerRecord("registered", 1);
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Server:Host", "example.org")));
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);
        services.AddSingleton<IOptionsFactory<ServerRecord>>(new FixedFactory(registered));

        await using var provider = services.BuildServiceProvider();

        await provider.GetRequiredService<IOptions<ServerRecord>>().Value.Should().BeSameReferenceAs(registered);
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

        var exception = await ( (Func<Task>)( () => host.StartAsync() ) ).Should().Throw<OptionsValidationException>();
        await exception.Failures.Should().BeEquivalentTo(["A server needs a port."], CollectionOrdering.Matching);
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
