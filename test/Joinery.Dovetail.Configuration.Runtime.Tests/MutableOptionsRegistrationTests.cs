using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Joinery.Dovetail.Configuration.Runtime.Tests;

/// <summary>
///     Covers <see cref="DovetailConfigurationOptionsExtensions" /> for a mutable type the options pipeline could create itself,
///     which generated configuration joints register the same way as any other type. <see cref="GreetingsOptions.Create" />
///     stands in for the generated <c>Create…</c> method.
/// </summary>
public sealed class MutableOptionsRegistrationTests
{
    [Test]
    public async Task Create_FillsIOptions()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(
                           TestConfiguration.Build(("Greetings:Message", "Hello"), ("Greetings:Recipients:0", "Ada"), ("Greetings:Recipients:1", "Grace"))
                       );
        services.AddDovetailConfigurationOptions("Greetings", GreetingsOptions.Create);

        await using var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<GreetingsOptions>>().Value;
        options.Message.ShouldBe("Hello");
        options.Recipients.ShouldBe(["Ada", "Grace"]);
    }

    [Test]
    public async Task Create_ResolvesConfiguration_FromTheServiceProvider()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Greetings:Message", "replaced")));
        services.AddDovetailConfigurationOptions("Greetings", GreetingsOptions.Create);
        // Registered after the section: the configuration in effect is the one the provider resolves when the options are created.
        services.AddSingleton<IConfiguration>(TestConfiguration.Build(("Greetings:Message", "from the provider")));

        await using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IOptions<GreetingsOptions>>().Value.Message.ShouldBe("from the provider");
    }

    [Test]
    public async Task Create_Reload_ReachesMonitorAndSnapshot_Once()
    {
        var configuration = TestConfiguration.Build(("Greetings:Message", "initial"), ("Greetings:Recipients:0", "Ada"));
        var services = new ServiceCollection().AddSingleton<IConfiguration>(configuration);
        services.AddDovetailConfigurationOptions("Greetings", GreetingsOptions.Create);

        await using var provider = services.BuildServiceProvider();

        var monitor = provider.GetRequiredService<IOptionsMonitor<GreetingsOptions>>();
        monitor.CurrentValue.Message.ShouldBe("initial");
        ReadSnapshot(provider).Message.ShouldBe("initial");

        var changes = new List<string?>();
        using var subscription = monitor.OnChange((_, name) => changes.Add(name));

        configuration["Greetings:Message"] = "updated";
        configuration["Greetings:Recipients:0"] = "Grace";
        configuration.Reload();

        changes.ShouldBe([Options.DefaultName]);
        monitor.CurrentValue.Message.ShouldBe("updated");
        // A reload creates a new instance, so the list holds the new items only.
        monitor.CurrentValue.Recipients.ShouldBe(["Grace"]);
        ReadSnapshot(provider).Message.ShouldBe("updated");
    }

    [Test]
    public async Task Create_RunsConfigureAndPostConfigure_ThenValidators_OnTheCreatedInstance()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(TestConfiguration.Build(("Greetings:Recipients:0", "Ada")));
        services.Configure<GreetingsOptions>(options => options.Recipients.Add("configured before"));
        services.AddDovetailConfigurationOptions("Greetings", GreetingsOptions.Create);
        services.Configure<GreetingsOptions>(options => options.Recipients.Add("configured after"));
        services.PostConfigure<GreetingsOptions>(options => options.Message = "post-configured");
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<GreetingsOptions>, MessageValidator>());

        await using var provider = services.BuildServiceProvider();

        // The created instance comes first, whatever the order of registration; configure actions then run on it.
        var options = provider.GetRequiredService<IOptions<GreetingsOptions>>().Value;
        options.Recipients.ShouldBe(["Ada", "configured before", "configured after"]);
        options.Message.ShouldBe("post-configured");

        services.PostConfigure<GreetingsOptions>(options => options.Message = "");
        await using var failing = services.BuildServiceProvider();
        var exception = Should.Throw<OptionsValidationException>(() => failing.GetRequiredService<IOptions<GreetingsOptions>>().Value);
        exception.Failures.ShouldBe(["A greeting needs a message."]);
    }

    [Test]
    public async Task Create_ValidateOnStart_OnTheReturnedBuilder_FailsHostStart()
    {
        var builder = Host.CreateEmptyApplicationBuilder(new());
        builder.Configuration.AddInMemoryCollection([new("Greetings:Message", "")]);
        builder
           .Services.AddDovetailConfigurationOptions("Greetings", GreetingsOptions.Create)
           .Validate(options => options.Message.Length > 0, "A greeting needs a message.")
           .ValidateOnStart();

        using var host = builder.Build();

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => host.StartAsync());
        exception.Failures.ShouldBe(["A greeting needs a message."]);
    }

    [Test]
    public void Create_RejectsAnEmptySectionPath()
    {
        var services = new ServiceCollection();

        Should.Throw<ArgumentException>(() => services.AddDovetailConfigurationOptions("", GreetingsOptions.Create));
    }

    private static GreetingsOptions ReadSnapshot(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<GreetingsOptions>>().Value;
    }

    private sealed class MessageValidator : IValidateOptions<GreetingsOptions>
    {
        public ValidateOptionsResult Validate(string? name, GreetingsOptions options) =>
            options.Message.Length > 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail("A greeting needs a message.");
    }
}
