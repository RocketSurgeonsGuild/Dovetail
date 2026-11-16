using Microsoft.Build.Utilities.ProjectCreation;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Dovetail's side of Optionary's configuration schema. Optionary writes the schema; Dovetail sets Optionary's hook properties from its own
/// detection (<c>Joinery.Dovetail.targets</c>): an application the SDK detected is a schema host, one that embeds its configuration keeps
/// the schema out of its output, and <c>DovetailEnableZeroAllocValidation=false</c> turns Optionary's ZeroAlloc validation off. Without
/// Optionary there is no schema and nothing to warn about. The generic schema cases (merging, packing, editors) are Optionary's own tests.
/// </summary>
[TargetFrameworks]
public class ConfigurationSchemaTests(string targetFramework)
{
    /// <summary>
    /// The application's own section, as a hand-written fragment in the shape Optionary's generator writes, so these don't depend on
    /// what that generator makes of a type.
    /// </summary>
    internal const string AppFragment = """"
        [assembly: System.Reflection.AssemblyMetadata("Optionary.Schema.v1:App", """{"type":"object","title":"App","properties":{"Name":{"type":"string"}},"patternProperties":{"^(?:[Nn][Aa][Mm][Ee])$":{"type":"string"}}}""")]
        """";

    private const string HostSchema = "host/obj/appsettings.schema.json";

    [Test]
    public async Task SdkApplication_IsAnOptionarySchemaHost_AndALibraryIsLeftToOptionary()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("web/web.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, sdk: "Microsoft.NET.Sdk.Web"))
           .AddProject("lib/lib.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework));

        var web = await project.Evaluate("web", "-getProperty:OptionarySchemaHost -getProperty:OptionarySchemaCopyToOutput -getProperty:OptionaryZeroAllocValidation");
        var lib = await project.Evaluate("lib", "-getProperty:OptionarySchemaHost -getProperty:OptionarySchemaCopyToOutput -getProperty:OptionaryZeroAllocValidation");

        await web.GetProperty("Properties").GetProperty("OptionarySchemaHost").GetString().Should().BeEqualTo("true");
        await web.GetProperty("Properties").GetProperty("OptionarySchemaCopyToOutput").GetString().Should().BeEqualTo("");
        await web.GetProperty("Properties").GetProperty("OptionaryZeroAllocValidation").GetString().Should().BeEqualTo("");
        await lib.GetProperty("Properties").GetProperty("OptionarySchemaHost").GetString().Should().BeEqualTo("");
    }

    [Test]
    public async Task DovetailSwitches_SetOptionarysHooks()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                "host/host.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                   .Property("DovetailEmbedConfiguration", "true")
                   .Property("DovetailEnableZeroAllocValidation", "false")
            );

        var host = await project.Evaluate("host", "-getProperty:OptionarySchemaHost -getProperty:OptionarySchemaCopyToOutput -getProperty:OptionaryZeroAllocValidation");

        await host.GetProperty("Properties").GetProperty("OptionarySchemaHost").GetString().Should().BeEqualTo("true");
        await host.GetProperty("Properties").GetProperty("OptionarySchemaCopyToOutput").GetString().Should().BeEqualTo("false");
        await host.GetProperty("Properties").GetProperty("OptionaryZeroAllocValidation").GetString().Should().BeEqualTo("false");
    }

    /// <summary>
    /// What a user sets wins over Dovetail's detection.
    /// </summary>
    [Test]
    public async Task OptionaryHooks_SetByTheProject_AreKept()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                "host/host.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                   .Property("DovetailEmbedConfiguration", "true")
                   .Property("OptionarySchemaHost", "false")
                   .Property("OptionarySchemaCopyToOutput", "true")
            );

        var host = await project.Evaluate("host", "-getProperty:OptionarySchemaHost -getProperty:OptionarySchemaCopyToOutput");

        await host.GetProperty("Properties").GetProperty("OptionarySchemaHost").GetString().Should().BeEqualTo("false");
        await host.GetProperty("Properties").GetProperty("OptionarySchemaCopyToOutput").GetString().Should().BeEqualTo("true");
    }

    /// <summary>
    /// A host that embeds its configuration (as MAUI and Uno applications do) has no configuration files next to it, so the schema
    /// stays in <c>obj/</c> for editors and isn't copied to the output.
    /// </summary>
    [Test]
    public async Task EmbeddedConfigurationHost_KeepsTheSchemaInObj()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject(
                "host/host.csproj",
                ProjectCreator
                   .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                   .Property("DovetailEmbedConfiguration", "true")
                   .ItemPackageReference("Optionary", Config.OptionaryVersion)
            )
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", AppFragment);

        await project.BuildProject("host");

        await project.ReadFile(HostSchema).Should().Contain("\"App\"");
        await File.Exists(Path.Combine(project.Directory, "host", "bin", "Debug", targetFramework, "appsettings.schema.json")).Should().BeFalse();
    }

    /// <summary>
    /// An application that only references Joinery.Dovetail.Hosting (no Joinery.Dovetail.Sdk, no direct Dovetail reference) and
    /// Optionary is a schema host through Optionary's own detection.
    /// </summary>
    [Test]
    public async Task HostingOnlyHost_WithOptionary_WritesTheSchema_AndCopiesItToItsOutput()
    {
        var host = ProjectCreator
                  .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                  .ItemPackageReference("Joinery.Dovetail.Hosting", Config.DovetailPackageVersion("Joinery.Dovetail.Hosting"))
                  .ItemPackageReference("Optionary", Config.OptionaryVersion);
        using var project = new SdkTestProject();
        project
           .AddProject("host/host.csproj", host)
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", AppFragment);

        await project.BuildProject("host");

        await project.ReadFile(HostSchema).Should().Contain("\"App\"");
        await project.ReadFile($"host/bin/Debug/{targetFramework}/appsettings.schema.json").Should().BeEqualTo(project.ReadFile(HostSchema));
    }

    /// <summary>
    /// Without Optionary a Dovetail application has no schema, and Dovetail says nothing about it.
    /// </summary>
    [Test]
    public async Task HostWithoutOptionary_HasNoSchema_AndNoWarnings()
    {
        using var project = new SdkTestProject();
        project
           .AddProject("Directory.Build.props", ProjectCreator.Create().Sdk("Joinery.Dovetail.Sdk"))
           .AddProject("host/host.csproj", ProjectCreator.Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe"))
           .AddFile("host/Program.cs", "System.Console.WriteLine(\"hello\");")
           .AddFile("host/Fragments.cs", AppFragment);

        var build = await project.BuildProject("host");

        await build.FindChildrenRecursive<Microsoft.Build.Logging.StructuredLogger.Warning>().Select(z => z.Text).Should().BeEmpty();
        await File.Exists(Path.Combine(project.Directory, HostSchema)).Should().BeFalse();
        await File.Exists(Path.Combine(project.Directory, "host", "bin", "Debug", targetFramework, "appsettings.schema.json")).Should().BeFalse();
    }
}
