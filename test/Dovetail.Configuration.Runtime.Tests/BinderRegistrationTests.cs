using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Dovetail.Configuration.Runtime.Tests;

/// <summary>
///     Covers the reflection-free binding overload of <see cref="DovetailConfigurationOptionsExtensions" />, which generated
///     configuration joints call for mutable types. <see cref="GreetingsOptions.Bind" /> stands in for the generated binder.
/// </summary>
public sealed class BinderRegistrationTests
{
    [Test]
    public async Task Bind_FillsIOptions()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(
                           TestConfiguration.Build(("Greetings:Message", "Hello"), ("Greetings:Recipients:0", "Ada"), ("Greetings:Recipients:1", "Grace"))
                       );
        services.AddDovetailConfigurationOptions<GreetingsOptions>("Greetings", GreetingsOptions.Bind);

        await using var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<GreetingsOptions>>().Value;
        options.Message.ShouldBe("Hello");
        options.Recipients.ShouldBe(["Ada", "Grace"]);
    }

    [Test]
    public async Task Bind_AppliesEveryRegistration_InRegistrationOrder()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(
                           TestConfiguration.Build(("Late:Recipients:0", "Late"), ("C:Recipients:0", "C"), ("a:Recipients:0", "a"), ("Early:Recipients:0", "Early"))
                       );
        services.AddDovetailConfigurationOptions<GreetingsOptions>("Late", GreetingsOptions.Bind);
        services.AddDovetailConfigurationOptions<GreetingsOptions>("C", GreetingsOptions.Bind);
        services.AddDovetailConfigurationOptions<GreetingsOptions>("a", GreetingsOptions.Bind);
        services.AddDovetailConfigurationOptions<GreetingsOptions>("Early", GreetingsOptions.Bind);

        await using var provider = services.BuildServiceProvider();

        // The options pipeline applies them as registered: neither the path nor anything else reorders them.
        provider.GetRequiredService<IOptions<GreetingsOptions>>().Value.Recipients.ShouldBe(["Late", "C", "a", "Early"]);
    }

    [Test]
    public async Task Bind_RepeatedPath_AddsARegistrationEachTime()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Greetings:Recipients:0", "Ada")));
        services.AddDovetailConfigurationOptions("Greetings", Tagged("first"));
        services.AddDovetailConfigurationOptions("greetings", Tagged("second"));

        await using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IOptions<GreetingsOptions>>().Value.Recipients.ShouldBe(["Ada", "first", "Ada", "second"]);
    }

    [Test]
    public async Task Bind_AppliesEachRegistration_WhereItWasRegistered()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(TestConfiguration.Build(("First:Recipients:0", "First"), ("Second:Recipients:0", "Second")));
        services.Configure<GreetingsOptions>(options => options.Recipients.Add("configured before"));
        services.AddDovetailConfigurationOptions<GreetingsOptions>("Second", GreetingsOptions.Bind);
        services.Configure<GreetingsOptions>(options => options.Recipients.Add("configured between"));
        services.AddDovetailConfigurationOptions<GreetingsOptions>("First", GreetingsOptions.Bind);
        services.Configure<GreetingsOptions>(options => options.Recipients.Add("configured after"));

        services.Count(descriptor => descriptor.ServiceType == typeof(IConfigureOptions<GreetingsOptions>)).ShouldBe(5);
        services.Count(descriptor => descriptor.ServiceType == typeof(IOptionsChangeTokenSource<GreetingsOptions>)).ShouldBe(1);

        await using var provider = services.BuildServiceProvider();

        provider
           .GetRequiredService<IOptions<GreetingsOptions>>()
           .Value.Recipients.ShouldBe(["configured before", "Second", "configured between", "First", "configured after"]);
    }

    [Test]
    public async Task Bind_ResolvesConfiguration_FromTheServiceProvider()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Greetings:Message", "replaced")));
        services.AddDovetailConfigurationOptions<GreetingsOptions>("Greetings", GreetingsOptions.Bind);
        // Registered after the section: the configuration in effect is the one the provider resolves when the options are created.
        services.AddSingleton<IConfiguration>(TestConfiguration.Build(("Greetings:Message", "from the provider")));

        await using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IOptions<GreetingsOptions>>().Value.Message.ShouldBe("from the provider");
    }

    [Test]
    public async Task Bind_Reload_ReachesMonitorAndSnapshot_Once()
    {
        var configuration = TestConfiguration.Build(("Greetings:Message", "initial"), ("Extra:Recipients:0", "Ada"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
        services.AddDovetailConfigurationOptions<GreetingsOptions>("Greetings", GreetingsOptions.Bind);
        services.AddDovetailConfigurationOptions<GreetingsOptions>("Extra", GreetingsOptions.Bind);

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<GreetingsOptions>>();
        monitor.CurrentValue.Message.ShouldBe("initial");
        ReadSnapshot(provider).Message.ShouldBe("initial");

        var changes = new List<string?>();
        using var subscription = monitor.OnChange((_, name) => changes.Add(name));

        configuration["Greetings:Message"] = "updated";
        configuration["Extra:Recipients:0"] = "Grace";
        configuration.Reload();

        changes.ShouldBe([Options.DefaultName]);
        monitor.CurrentValue.Message.ShouldBe("updated");
        monitor.CurrentValue.Recipients.ShouldBe(["Grace"]);
        ReadSnapshot(provider).Message.ShouldBe("updated");
    }

    [Test]
    public async Task Bind_LeavesNamedOptionsAlone()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Greetings:Message", "Hello")));
        services.AddDovetailConfigurationOptions<GreetingsOptions>("Greetings", GreetingsOptions.Bind);
        services.Configure<GreetingsOptions>("named", options => options.Recipients.Add("named only"));

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<GreetingsOptions>>();
        monitor.CurrentValue.Message.ShouldBe("Hello");
        monitor.CurrentValue.Recipients.ShouldBeEmpty();
        var named = monitor.Get("named");
        named.Message.ShouldBe("");
        named.Recipients.ShouldBe(["named only"]);
    }

    [Test]
    public async Task Bind_ValidateOnStart_OnTheReturnedBuilder_FailsHostStart()
    {
        var builder = Host.CreateEmptyApplicationBuilder(new());
        builder.Configuration.AddInMemoryCollection([new("Greetings:Message", "")]);
        builder
           .Services.AddDovetailConfigurationOptions<GreetingsOptions>("Greetings", GreetingsOptions.Bind)
           .Validate(options => options.Message.Length > 0, "A greeting needs a message.")
           .ValidateOnStart();

        using var host = builder.Build();

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => host.StartAsync());
        exception.Failures.ShouldBe(["A greeting needs a message."]);
    }

    [Test]
    public void Bind_RejectsAnEmptySectionPath()
    {
        var services = new ServiceCollection();

        Should.Throw<ArgumentException>(() => services.AddDovetailConfigurationOptions<GreetingsOptions>("", GreetingsOptions.Bind));
    }

    private static Action<IConfiguration, GreetingsOptions> Tagged(string tag) =>
        (configuration, options) =>
        {
            GreetingsOptions.Bind(configuration, options);
            options.Recipients.Add(tag);
        };

    private static GreetingsOptions ReadSnapshot(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<GreetingsOptions>>().Value;
    }
}
