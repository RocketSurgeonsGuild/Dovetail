using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Projects that target both frameworks: each target framework reads its own assembly, so a library packs only the first one's
/// ConfigurationSchema.json (a second root file would be NU5118), and a host writes the stable obj/appsettings.schema.json
/// from its first target framework only, so the two inner builds never write the same file.
/// </summary>
public class ConfigurationSchemaMultiTargetingTests
{
    /// <summary>
    /// A section whose schema differs per target framework.
    /// </summary>
    private const string PerFrameworkFragment = """"
        #if NET11_0_OR_GREATER
        [assembly: System.Reflection.AssemblyMetadata("Dovetail.ConfigurationSchema.v1:Greetings", """{"type":"object","title":"Greetings","properties":{"Net11":{"type":"string"}}}""")]
        #else
        [assembly: System.Reflection.AssemblyMetadata("Dovetail.ConfigurationSchema.v1:Greetings", """{"type":"object","title":"Greetings","properties":{"Net10":{"type":"string"}}}""")]
        #endif
        """";

    [Test]
    public async Task Library_PacksTheSchemaOfItsFirstTargetFramework()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("lib/DovetailSchemaTestMultiTargeted.csproj", ProjectCreator.Templates.SdkCsproj(targetFrameworks: ["net10.0", "net11.0"]))
           .AddFile("lib/Fragments.cs", PerFrameworkFragment);

        var package = await project.PackToLocalFeed("lib");

        await package.Files.Count(z => z == "ConfigurationSchema.json").Should().BeEqualTo(1);
        await project.ReadFile("lib/obj/Release/net10.0/ConfigurationSchema.json").Should().Contain("Net10");
        await project.ReadFile("lib/obj/Release/net11.0/ConfigurationSchema.json").Should().Contain("Net11");
        await project.ReadPackageFile("lib", "ConfigurationSchema.json").Should().BeEqualTo(project.ReadFile("lib/obj/Release/net10.0/ConfigurationSchema.json"));
        await project.Warnings("lib").Should().All(z => !z.StartsWith("NU5118", StringComparison.Ordinal));
    }

    [Test]
    public async Task Host_WritesTheStableSchemaFromItsFirstTargetFramework()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("host/host.csproj", ProjectCreator.Templates.SdkCsproj(targetFrameworks: ["net10.0", "net11.0"], outputType: "Exe"))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", PerFrameworkFragment);

        await project.VerifyBuildWarnings("host");

        await project.ReadFile("host/obj/appsettings.schema.json").Should().Contain("Net10");
        await project.ReadFile("host/obj/Debug/net11.0/appsettings.schema.json").Should().Contain("Net11");
        await File.Exists(Path.Combine(project.Directory, "host/obj/Debug/net10.0/appsettings.schema.json")).Should().BeFalse();
        await project.ReadFile("host/bin/Debug/net10.0/appsettings.schema.json").Should().BeEqualTo(project.ReadFile("host/obj/appsettings.schema.json"));
        await project.ReadFile("host/bin/Debug/net11.0/appsettings.schema.json").Should().BeEqualTo(project.ReadFile("host/obj/Debug/net11.0/appsettings.schema.json"));
    }
}
