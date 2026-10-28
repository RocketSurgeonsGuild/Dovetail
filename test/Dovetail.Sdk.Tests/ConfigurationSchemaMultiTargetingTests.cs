using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

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

        package.Files.Count(z => z == "ConfigurationSchema.json").ShouldBe(1);
        project.ReadFile("lib/obj/Release/net10.0/ConfigurationSchema.json").ShouldContain("Net10");
        project.ReadFile("lib/obj/Release/net11.0/ConfigurationSchema.json").ShouldContain("Net11");
        project.ReadPackageFile("lib", "ConfigurationSchema.json").ShouldBe(project.ReadFile("lib/obj/Release/net10.0/ConfigurationSchema.json"));
        project.Warnings("lib").ShouldNotContain(z => z.StartsWith("NU5118", StringComparison.Ordinal));
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

        project.ReadFile("host/obj/appsettings.schema.json").ShouldContain("Net10");
        project.ReadFile("host/obj/Debug/net11.0/appsettings.schema.json").ShouldContain("Net11");
        File.Exists(Path.Combine(project.Directory, "host/obj/Debug/net10.0/appsettings.schema.json")).ShouldBeFalse();
        project.ReadFile("host/bin/Debug/net10.0/appsettings.schema.json").ShouldBe(project.ReadFile("host/obj/appsettings.schema.json"));
        project.ReadFile("host/bin/Debug/net11.0/appsettings.schema.json").ShouldBe(project.ReadFile("host/obj/Debug/net11.0/appsettings.schema.json"));
    }
}
