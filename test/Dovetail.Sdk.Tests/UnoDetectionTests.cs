using Microsoft.Build.Utilities.ProjectCreation;

namespace Dovetail.Sdk.Tests;

/// <summary>
/// Covers the Uno branch of <c>src/Dovetail.Sdk/Sdk/Dovetail.HostDetection.targets</c>. An Uno app whose <c>UnoFeatures</c>
/// include Hosting gets the Uno host - ahead of the WebAssembly branch its browserwasm head would otherwise pick - with the
/// navigate entry points and embed-only configuration; without Hosting there's no host. Uno.Sdk itself isn't used: the
/// properties it sets (<c>UsingUnoSdk</c>, the raw <c>UnoFeatures</c> string, and for the browserwasm head
/// <c>UsingMicrosoftNETSdkWebAssembly</c>) are set on the project, which is only evaluated.
/// </summary>
[TargetFrameworks]
public class UnoDetectionTests(string targetFramework)
{
    private static readonly string[] Properties = ["_DovetailApplicationDetected", "DovetailEmbedConfiguration", "ImportDovetailAssembly"];
    private static readonly string[] Items = ["DovetailHost", "PackageReference"];

    [Test]
    public async Task UnoApp_WithHosting_DetectsUnoHostAndInjectsHostingUno()
    {
        // Formatted the way the Uno templates write it, so the detection has to normalize whitespace and case.
        using var project = CreateProject("\n        Material;\n        HOSTING ;\n        Navigation;\n    ");

        await project.VerifyEvaluation("app", Properties, Items);
    }

    [Test]
    public async Task UnoApp_WithoutHosting_NoHostDetected()
    {
        using var project = CreateProject("Material;Toolkit");

        await project.VerifyEvaluation("app", Properties, Items);
    }

    [Test]
    public async Task UnoBrowserWasmHead_DetectsUnoHostNotWebAssembly()
    {
        using var project = CreateProject("Hosting", browserWasm: true);

        await project.VerifyEvaluation("app", Properties, Items);
    }

    private SdkTestProject CreateProject(string unoFeatures, bool browserWasm = false)
    {
        var app = ProjectCreator
                 .Templates.SdkCsproj(targetFramework: targetFramework, outputType: "Exe")
                 .Property("UsingUnoSdk", "true")
                 .Property("UnoFeatures", unoFeatures);
        if (browserWasm)
        {
            app.Property("UsingMicrosoftNETSdkWebAssembly", "true");
        }

        var project = new SdkTestProject();
        project
           .AddProject(
                "Directory.Build.props",
                ProjectCreator
                   .Create()
                   .Sdk("Joinery.Dovetail.Sdk")
            )
           .AddProject("app/app.csproj", app);
        return project;
    }
}
