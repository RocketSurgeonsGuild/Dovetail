using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

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
    public async Task Create_MergesSeveralSections_KeyByKey_InOrder()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(
                           TestConfiguration.Build(
                               ("Defaults:Server:Host", "default-host"),
                               ("Defaults:Server:Port", "80"),
                               ("Defaults:Server:Tags:0", "a"),
                               ("Defaults:Server:Tags:1", "b"),
                               ("Server:Port", "8080"),
                               ("Server:Tags:0", "c")
                           )
                       );
        // Registered in the opposite order to the one they merge in.
        services.AddDovetailConfigurationOptions("Server", Noted(ServerRecord.Create, "created by Server"), order: 1);
        services.AddDovetailConfigurationOptions("Defaults:Server", Noted(ServerRecord.Create, "created by Defaults:Server"));

        await using var provider = services.BuildServiceProvider();

        var server = provider.GetRequiredService<IOptions<ServerRecord>>().Value;
        server.Host.ShouldBe("default-host");
        server.Port.ShouldBe(8080);
        server.Tags.ShouldBe(["c", "b"]);
        server.Note.ShouldBe("created by Defaults:Server");
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
    public async Task Create_ThenBindsTheBindingSections_ExceptThoseOnACreatingPath()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(
                           TestConfiguration.Build(
                               ("Server:Host", "example.org"),
                               ("Server:Tags:0", "created"),
                               ("ExtraServer:Tags:0", "bound")
                           )
                       );
        // Binding "server" again would append "created" twice: the creating section already applied that path.
        services.AddDovetailConfigurationOptions<ServerRecord>("server", ServerRecord.BindTags);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create, order: 1);
        services.AddDovetailConfigurationOptions<ServerRecord>("ExtraServer", ServerRecord.BindTags, order: 2);

        await using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IOptions<ServerRecord>>().Value.Tags.ShouldBe(["created", "bound"]);
    }

    [Test]
    public async Task Create_Reload_ReachesMonitorAndSnapshot_Once()
    {
        var configuration = TestConfiguration.Build(("Defaults:Server:Host", "example.org"), ("Server:Port", "8080"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
        services.AddDovetailConfigurationOptions("Defaults:Server", ServerRecord.Create);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create, order: 1);

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
    public async Task Create_FromSeveralSections_ReleasesTheMergedConfiguration()
    {
        var configuration = new TrackingConfiguration(TestConfiguration.Build(("Defaults:Server:Host", "example.org"), ("Server:Port", "8080")));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
        services.AddDovetailConfigurationOptions("Defaults:Server", ServerRecord.Create);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create, order: 1);

        await using var provider = services.BuildServiceProvider();

        var factory = provider.GetRequiredService<IOptionsFactory<ServerRecord>>();
        for (var i = 0; i < 3; i++)
        {
            factory.Create(Options.DefaultName).Port.ShouldBe(8080);
        }

        // The merged configuration watches each section's reload token while it exists.
        configuration.RegistrationsMade.ShouldBe(6);
        configuration.ActiveRegistrations.ShouldBe(0);
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

    /// <summary>
    ///     Wraps a configuration so that it, and every section it hands out, share one reload token that counts its callback
    ///     registrations.
    /// </summary>
    private sealed class TrackingConfiguration(IConfiguration inner) : IConfiguration
    {
        private readonly TrackingChangeToken _token = new();

        public int RegistrationsMade => _token.RegistrationsMade;

        public int ActiveRegistrations => _token.ActiveRegistrations;

        public string? this[string key] { get => inner[key]; set => inner[key] = value; }

        public IEnumerable<IConfigurationSection> GetChildren() => inner.GetChildren();

        public IChangeToken GetReloadToken() => _token;

        public IConfigurationSection GetSection(string key) => new TrackingSection(inner.GetSection(key), _token);
    }

    private sealed class TrackingSection(IConfigurationSection inner, IChangeToken token) : IConfigurationSection
    {
        public string? this[string key] { get => inner[key]; set => inner[key] = value; }

        public string Key => inner.Key;

        public string Path => inner.Path;

        public string? Value { get => inner.Value; set => inner.Value = value; }

        public IEnumerable<IConfigurationSection> GetChildren() => inner.GetChildren();

        public IChangeToken GetReloadToken() => token;

        public IConfigurationSection GetSection(string key) => inner.GetSection(key);
    }

    private sealed class TrackingChangeToken : IChangeToken
    {
        private int _made;
        private int _active;

        public int RegistrationsMade => Volatile.Read(ref _made);

        public int ActiveRegistrations => Volatile.Read(ref _active);

        public bool HasChanged => false;

        public bool ActiveChangeCallbacks => true;

        public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
        {
            Interlocked.Increment(ref _made);
            Interlocked.Increment(ref _active);
            return new Registration(this);
        }

        private sealed class Registration(TrackingChangeToken token) : IDisposable
        {
            private int _disposed;

            public void Dispose()
            {
                if (Interlocked.Exchange(ref _disposed, 1) == 0) Interlocked.Decrement(ref token._active);
            }
        }
    }
}
