using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Dovetail.Configuration.Aot.Tests;

/// <summary>
///     Compiles registrations shaped like the generated configuration joint's under the trimming and AOT analyzers (this project
///     is <c>IsAotCompatible</c>, with IL2026, IL3050 and IL2091 as errors), and checks that they still resolve.
/// </summary>
public sealed class GeneratedShapeRegistrationTests
{
    [Test]
    public async Task Registrations_ResolveOptions()
    {
        var services = new ServiceCollection()
                      .AddSingleton<IConfiguration>(
                           new ConfigurationBuilder()
                              .AddInMemoryCollection([new("Greetings:Message", "Hello"), new("Server:Host", "example.org"), new("Server:Port", "8080")])
                              .Build()
                       );
        Register(services);

        await using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<IOptions<GreetingsOptions>>().Value.Message.ShouldBe("Hello");
        provider.GetRequiredService<IOptionsMonitor<ServerRecord>>().CurrentValue.ShouldBe(new ServerRecord("example.org", 8080));
    }

    [Test]
    public async Task Registrations_RunTheValidator()
    {
        var services = new ServiceCollection().AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        Register(services);

        await using var provider = services.BuildServiceProvider();

        Should.Throw<OptionsValidationException>(() => provider.GetRequiredService<IOptions<GreetingsOptions>>().Value);
    }

    // The calls the generator emits into IServiceJoint.Register: static calls with a positional order, and ValidateOnStart
    // wrapping the returned builder. Generated code also writes global:: and every type argument; dotnet format drops them here.
    private static void Register(IServiceCollection services)
    {
        OptionsBuilderExtensions.ValidateOnStart(
            DovetailConfigurationOptionsExtensions.AddDovetailConfigurationOptions<GreetingsOptions>(
                services,
                "Greetings",
                static (configuration, instance) => Binder.BindGreetings(configuration, instance),
                0
            )
        );
        DovetailConfigurationOptionsExtensions.AddDovetailConfigurationOptions(
            services,
            "Server",
            static configuration => Binder.CreateServer(configuration),
            0
        );
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<GreetingsOptions>, GreetingsValidator>());
    }

    private sealed class GreetingsOptions
    {
        public string Message { get; set; } = "";
    }

    private sealed record ServerRecord(string Host, int Port);

    private sealed class GreetingsValidator : IValidateOptions<GreetingsOptions>
    {
        public ValidateOptionsResult Validate(string? name, GreetingsOptions options) =>
            options.Message.Length > 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail("A greeting needs a message.");
    }

    // Stands in for the generated binder.
    private static class Binder
    {
        public static void BindGreetings(IConfiguration configuration, GreetingsOptions instance)
        {
            if (configuration["Message"] is { } message) instance.Message = message;
        }

        public static ServerRecord CreateServer(IConfiguration configuration) =>
            new(
                configuration["Host"] ?? "localhost",
                int.TryParse(configuration["Port"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var port) ? port : 80
            );
    }
}
