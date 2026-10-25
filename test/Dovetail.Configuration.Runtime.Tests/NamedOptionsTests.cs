using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Dovetail.Configuration.Runtime.Tests;

/// <summary>
///     Covers named options and <see cref="IDovetailConfigurationProvider" />: the default provider supplies the application's
///     configuration for the default options only, and an application's own provider supplies it for any name, as a
///     multi-tenant application needs.
/// </summary>
public sealed class NamedOptionsTests
{
    [Test]
    public async Task DefaultProvider_CreatesTheDefaultOptions_FromTheApplicationsConfiguration()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(TestConfiguration.Build(("Endpoint:Url", "https://example.org"), ("Server:Host", "example.org")));
        services.AddDovetailConfigurationOptions("Endpoint", EndpointOptions.Create);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IOptions<EndpointOptions>>().Value.Url.ShouldBe("https://example.org");
        provider.GetRequiredService<IOptionsMonitor<ServerRecord>>().CurrentValue.Host.ShouldBe("example.org");
        provider.GetRequiredService<IDovetailConfigurationProvider>().GetConfiguration(Options.DefaultName)
                .ShouldBeSameAs(provider.GetRequiredService<IConfiguration>());
    }

    [Test]
    public async Task DefaultProvider_ThrowsForNamedOptions_NamingTheOptionsAndTheRemedy()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Greetings:Message", "Hello")));
        services.AddDovetailConfigurationOptions("Greetings", GreetingsOptions.Create);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);

        await using var provider = services.BuildServiceProvider();

        var greetings = Should.Throw<InvalidOperationException>(() => provider.GetRequiredService<IOptionsMonitor<GreetingsOptions>>().Get("tenant-a"));
        greetings.Message.ShouldContain("tenant-a");
        greetings.Message.ShouldContain(nameof(IDovetailConfigurationProvider));
        Should.Throw<InvalidOperationException>(() => provider.GetRequiredService<IOptionsMonitor<ServerRecord>>().Get("tenant-a")).Message.ShouldContain("tenant-a");
        provider.GetRequiredService<IOptionsMonitor<GreetingsOptions>>().CurrentValue.Message.ShouldBe("Hello");
    }

    [Test]
    public async Task CustomProvider_CreatesDifferentOptions_PerOptionsName()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Server:Host", "default-host"), ("Server:Port", "80")));
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);
        services.AddSingleton<IDovetailConfigurationProvider>(
            new PerNameProvider(
                new()
                {
                    [Options.DefaultName] = TestConfiguration.Build(("Server:Host", "default-host"), ("Server:Port", "80")),
                    ["tenant-a"] = TestConfiguration.Build(("Server:Host", "a.example.org"), ("Server:Port", "8001")),
                    ["tenant-b"] = TestConfiguration.Build(("Server:Host", "b.example.org"), ("Server:Port", "8002")),
                }
            )
        );

        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var snapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ServerRecord>>();
        (snapshot.Value.Host, snapshot.Value.Port).ShouldBe(("default-host", 80));
        (snapshot.Get("tenant-a").Host, snapshot.Get("tenant-a").Port).ShouldBe(("a.example.org", 8001));
        (snapshot.Get("tenant-b").Host, snapshot.Get("tenant-b").Port).ShouldBe(("b.example.org", 8002));
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task ApplicationProvider_ReplacesTheDefaultOne(bool registeredBeforeTheOptions)
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Greetings:Message", "from the application")));
        var replacement = new PerNameProvider(new() { [Options.DefaultName] = TestConfiguration.Build(("Greetings:Message", "from the provider")) });
        if (registeredBeforeTheOptions) services.AddSingleton<IDovetailConfigurationProvider>(replacement);

        services.AddDovetailConfigurationOptions("Greetings", GreetingsOptions.Create);
        if (!registeredBeforeTheOptions) services.AddSingleton<IDovetailConfigurationProvider>(replacement);

        await using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IDovetailConfigurationProvider>().ShouldBeSameAs(replacement);
        provider.GetRequiredService<IOptions<GreetingsOptions>>().Value.Message.ShouldBe("from the provider");
    }

    [Test]
    public async Task NamedOptions_RunConfigurePostConfigureAndValidators()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build());
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);
        services.AddSingleton<IDovetailConfigurationProvider>(
            new PerNameProvider(
                new()
                {
                    [Options.DefaultName] = TestConfiguration.Build(("Server:Port", "80")),
                    ["tenant-a"] = TestConfiguration.Build(("Server:Port", "8001")),
                    ["tenant-b"] = TestConfiguration.Build(("Server:Port", "0")),
                }
            )
        );
        services.Configure<ServerRecord>("tenant-a", server => server.Tags.Add("configured"));
        services.PostConfigure<ServerRecord>("tenant-a", server => server.Note = "post-configured");
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<ServerRecord>, ServerRecordValidator>());

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<ServerRecord>>();
        var tenantA = monitor.Get("tenant-a");
        tenantA.Port.ShouldBe(8001);
        tenantA.Tags.ShouldBe(["configured"]);
        tenantA.Note.ShouldBe("post-configured");
        monitor.CurrentValue.Tags.ShouldBeEmpty();
        var exception = Should.Throw<OptionsValidationException>(() => monitor.Get("tenant-b"));
        exception.Failures.ShouldBe(["Port must be between 1 and 65535, but was 0."]);
    }

    [Test]
    public async Task CustomProvider_KeepsDefaultReloadsReachingTheMonitor()
    {
        var configuration = TestConfiguration.Build(("Server:Port", "8080"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);
        services.AddSingleton<IDovetailConfigurationProvider>(
            new PerNameProvider(new() { [Options.DefaultName] = configuration, ["tenant-a"] = TestConfiguration.Build(("Server:Port", "1")) })
        );

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<ServerRecord>>();
        monitor.CurrentValue.Port.ShouldBe(8080);
        var changes = new List<string?>();
        using var subscription = monitor.OnChange((_, name) => changes.Add(name));

        configuration["Server:Port"] = "9090";
        configuration.Reload();

        changes.ShouldBe([Options.DefaultName]);
        monitor.CurrentValue.Port.ShouldBe(9090);
        monitor.Get("tenant-a").Port.ShouldBe(1);
    }

    [Test]
    public async Task NamedOptions_AreCreatedAgain_WhenTheirConfigurationReloads()
    {
        var tenantA = TestConfiguration.Build(("Server:Port", "1"));
        var tenantB = TestConfiguration.Build(("Server:Port", "2"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build());
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);
        services.AddSingleton<IDovetailConfigurationProvider>(
            new PerNameProvider(new() { [Options.DefaultName] = TestConfiguration.Build(("Server:Port", "80")), ["tenant-a"] = tenantA, ["tenant-b"] = tenantB })
        );

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<ServerRecord>>();
        monitor.Get("tenant-a").Port.ShouldBe(1);
        monitor.Get("tenant-b").Port.ShouldBe(2);

        tenantA["Server:Port"] = "11";
        tenantA.Reload();
        tenantB["Server:Port"] = "22";

        // Only the name whose configuration reloaded is created again, and it keeps following later reloads.
        monitor.Get("tenant-a").Port.ShouldBe(11);
        monitor.Get("tenant-b").Port.ShouldBe(2);
        monitor.CurrentValue.Port.ShouldBe(80);
        tenantA["Server:Port"] = "111";
        tenantA.Reload();
        monitor.Get("tenant-a").Port.ShouldBe(111);
    }

    [Test]
    public async Task NamedOptions_ReloadedByTheMonitor_DoNotNotifyOnChangeListeners()
    {
        var tenantA = TestConfiguration.Build(("Server:Port", "1"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build());
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);
        services.AddSingleton<IDovetailConfigurationProvider>(new PerNameProvider(new() { [Options.DefaultName] = TestConfiguration.Build(("Server:Port", "80")), ["tenant-a"] = tenantA }));

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<ServerRecord>>();
        monitor.Get("tenant-a").Port.ShouldBe(1);
        var changes = new List<string?>();
        using var subscription = monitor.OnChange((_, name) => changes.Add(name));

        tenantA["Server:Port"] = "11";
        tenantA.Reload();

        // The monitor only notifies for the names of its change token sources, which is the default name.
        changes.ShouldBeEmpty();
        monitor.Get("tenant-a").Port.ShouldBe(11);
        // A snapshot creates its options from the current configuration, and needs none of this.
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ServerRecord>>().Get("tenant-a").Port.ShouldBe(11);
    }

    [Test]
    public async Task NamedOptions_FailingValidation_AreNotCached_AndAreValidatedAgainOnTheNextRead()
    {
        var tenantB = TestConfiguration.Build(("Server:Port", "0"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build());
        services.AddDovetailConfigurationOptions("Server", ServerRecord.Create);
        services.AddSingleton<IDovetailConfigurationProvider>(new PerNameProvider(new() { [Options.DefaultName] = TestConfiguration.Build(("Server:Port", "80")), ["tenant-b"] = tenantB }));
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<ServerRecord>, ServerRecordValidator>());

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<ServerRecord>>();
        Should.Throw<OptionsValidationException>(() => monitor.Get("tenant-b"));

        tenantB["Server:Port"] = "443";
        tenantB.Reload();

        monitor.Get("tenant-b").Port.ShouldBe(443);
    }

    private sealed class PerNameProvider(Dictionary<string, IConfiguration> configurations) : IDovetailConfigurationProvider
    {
        public IConfiguration GetConfiguration(string optionsName) =>
            configurations.TryGetValue(optionsName, out var configuration)
                ? configuration
                : throw new InvalidOperationException($"No configuration for '{optionsName}'.");
    }
}
