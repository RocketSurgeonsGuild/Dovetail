using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// <c>Joinery.Dovetail.Sdk.Console</c> chains the Dovetail SDK, adds ConsoleAppFramework and turns on <c>DovetailConsoleApp</c>, which makes
/// Joinery.Dovetail.Hosting write the application's <c>Program.cs</c> and declare the <c>ConsoleApp</c> joint. These run applications:
/// what they print is only right when the SDK wrote an entry point that built the host from the application's joints and ran the command
/// the command line named.
/// </summary>
[TargetFrameworks]
public class ConsoleAppTests(string targetFramework)
{
    private const string Commands = """
        using ConsoleAppFramework;
        using Joinery.Dovetail;
        using Joinery.Dovetail.Attributes;
        using Joinery.Dovetail.Joints;
        using Microsoft.Extensions.Configuration;
        using Microsoft.Extensions.DependencyInjection;

        namespace app;

        public class Greeter(IConfiguration configuration)
        {
            public string Greet(string name) => $"{configuration["Greeting"] ?? "Hello"} {name}";
        }

        public class GreetCommands(Greeter greeter)
        {
            [Command("greet")]
            public void Greet(string name = "world") => System.Console.WriteLine("Greeting=" + greeter.Greet(name));
        }

        // One class contributes the application's services and commands, as any other Dovetail joint contributes to what it configures.
        [DovetailExport]
        internal class AppJoint : IServiceJoint, IConsoleAppJoint
        {
            public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<Greeter>();

            public void Register(IDovetailContext context, ConsoleApp.ConsoleAppBuilder app) => app.Add<GreetCommands>();
        }
        """;

    [Test]
    public async Task ConsoleSdk_WritesProgram_AndRunsTheCommandAJointRegistered()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk.Console")
            )
           // No OutputType: the SDK makes the project an application.
           .AddProject("app/app.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
           .AddFile("app/Commands.cs", Commands);

        var output = await project.Run("app", "greet --name Dovetail");

        output.ShouldContain("Greeting=Hello Dovetail");
    }

    [Test]
    public async Task ConsoleSdk_ConfigurationFromTheHostReachesTheCommand()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk.Console")
            )
           .AddProject("app/app.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework))
           .AddFile("app/Commands.cs", Commands)
           .AddFile(
                "app/Settings.cs",
                """
                using Joinery.Dovetail;
                using Joinery.Dovetail.Attributes;
                using Joinery.Dovetail.Joints;
                using Microsoft.Extensions.Configuration;

                namespace app;

                [DovetailExport]
                internal class SettingsJoint : IConfigurationJoint
                {
                    public void Register(IDovetailContext context, IConfigurationBuilder builder) =>
                        builder.AddInMemoryCollection(new System.Collections.Generic.Dictionary<string, string?> { ["Greeting"] = "Howdy" });
                }
                """
            );

        var output = await project.Run("app", "greet --name Dovetail");

        output.ShouldContain("Greeting=Howdy Dovetail");
    }

    [Test]
    public async Task DovetailUseDefaultProgram_False_LeavesTheEntryPointToTheApplication()
    {
        using var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk.Console")
            )
           .AddProject(
                "app/app.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework)
                   .Property("DovetailUseDefaultProgram", "false")
            )
           .AddFile("app/Program.cs", "System.Console.WriteLine(\"Custom entry point\");");

        var output = await project.Run("app");

        output.ShouldContain("Custom entry point");
    }
}
