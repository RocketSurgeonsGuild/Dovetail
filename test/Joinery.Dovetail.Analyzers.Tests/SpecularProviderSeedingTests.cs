using Microsoft.CodeAnalysis;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
///     An application that references Specular should not have to hand-write the line that gives its Joints the provider. When
///     the compilation references Specular and emits a provider, the generated <c>Imports</c> seeds
///     <c>SpecularProvider.Instance</c> into the context builder, without overwriting a provider the application set itself.
///     Dovetail takes no dependency on Specular, so a compilation without it must get nothing: no code and no diagnostic.
/// </summary>
public class SpecularProviderSeedingTests() : GeneratorTest()
{
    private const string ImportsHint = "Imported_Assembly_Dovetails.g.cs";

    /// <summary>
    ///     AddIfMissing rather than Set: a provider the application passed in, or set on the builder, must win.
    /// </summary>
    private const string SeedingCall = ".AddIfMissing<global::Specular.ISpecularProvider>(global::SpecularProvider.Instance)";

    [Test]
    public async Task Seeds_The_Provider_Into_The_Context_Builder_When_Specular_Is_Referenced()
    {
        var result = await ApplicationWithSpecular().Build().GenerateAsync(TestContext.CancellationToken);

        Problems(result).ShouldBeEmpty();
        await Verify(result);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("true")]
    [Arguments("True")]
    [Arguments("not a boolean")]
    public async Task Seeds_Unless_Specular_Is_Told_Not_To_Emit_A_Provider(string? specularEmitProvider)
    {
        // Specular's generator reads SpecularEmitProvider as a boolean that defaults to true, and so does this.
        var builder = ApplicationWithSpecular();
        if (specularEmitProvider is not null) builder = builder.AddGlobalOption("build_property.SpecularEmitProvider", specularEmitProvider);

        var result = await builder.Build().GenerateAsync(TestContext.CancellationToken);

        GeneratedSource(result, ImportsHint).ShouldContain(SeedingCall);
        Problems(result).ShouldBeEmpty();
    }

    [Test]
    public async Task Seeds_A_Provider_That_Only_Exists_After_Specular_Has_Generated_It()
    {
        var result = await ApplicationWithSpecular().Build().GenerateAsync(TestContext.CancellationToken);

        // A generator doesn't see what another one adds, so the provider can't be found as a type while generating. Detecting it
        // that way would never fire in a real build, which is why the seeding keys off the reference and SpecularEmitProvider.
        result.InputCompilation.GetTypeByMetadataName("SpecularProvider").ShouldBeNull();
        result.FinalCompilation.GetTypeByMetadataName("SpecularProvider").ShouldNotBeNull();
        GeneratedSource(result, ImportsHint).ShouldContain(SeedingCall);
        Problems(result).ShouldBeEmpty();
    }

    [Test]
    public async Task Seeds_In_A_Test_Host_Too()
    {
        var result = await ApplicationWithSpecular()
                          .AddGlobalOption("build_property.IsTestProject", "true")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        var imports = GeneratedSource(result, ImportsHint);
        imports.ShouldContain("DovetailHostType.UnitTest");
        imports.ShouldContain(SeedingCall);
        Problems(result).ShouldBeEmpty();
    }

    [Test]
    public async Task Seeds_Without_Warning_When_A_Referenced_Assembly_Shares_Its_Internals_And_Its_Own_Provider()
    {
        // A test host often sees the internals of the application it tests, and the application's provider has the same name as its own.
        // The generated code means the one in this assembly, which the compiler picks, and warns (CS0436) that it did.
        var runtimePackage = await SpecularStandIns.CreateRuntimePackage(TestContext.CancellationToken);
        var application = await GeneratorTestContextBuilder
                               .Create()
                               .WithProjectName("Application")
                               .AddCompilationReferences(runtimePackage)
                               .AddSources(
                                    """
                                    [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("TestHost")]
                                    """,
                                    SpecularStandIns.GeneratedProvider
                                )
                               .Build()
                               .GenerateAsync(TestContext.CancellationToken);

        var result = await ApplicationWithSpecular()
                          .WithProjectName("TestHost")
                          .AddCompilationReferences(application)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        GeneratedSource(result, ImportsHint).ShouldContain(SeedingCall);
        // Specular's own generated provider names itself the same way, so the only warning this can pin down is the one the seeding would add.
        Problems(result).Where(z => z.Location.SourceTree?.FilePath.EndsWith(ImportsHint, StringComparison.Ordinal) == true).ShouldBeEmpty();
    }

    [Test]
    public async Task Seeds_Nothing_And_Warns_About_Nothing_When_Specular_Is_Not_Referenced()
    {
        var result = await WithSharedDeps()
                          .AddSources(ExportedJoint)
                          .IgnoreOutputFile("Exported_Dovetails.g.cs")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        GeneratedSource(result, ImportsHint).ShouldNotContain("Specular");
        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.ShouldBeEmpty();
        Problems(result).ShouldBeEmpty();
        await Verify(result);
    }

    [Test]
    public async Task Seeds_Nothing_When_Specular_Is_Referenced_But_Has_No_Provider_To_Seed_From_This_Assembly()
    {
        // A library that doesn't generate its own provider keeps getting the application's through the context.
        var result = await ApplicationWithSpecular()
                          .AddGlobalOption("build_property.SpecularEmitProvider", "false")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        GeneratedSource(result, ImportsHint).ShouldNotContain("Specular");
        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.ShouldBeEmpty();
        // No provider exists to name, so seeding one anyway would fail the build here.
        Problems(result).ShouldBeEmpty();
    }

    [Test]
    public async Task Seeds_Nothing_When_The_Assembly_Does_Not_Import()
    {
        var result = await ApplicationWithSpecular()
                          .AddGlobalOption("build_property.ImportDovetailAssembly", "false")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        HasGeneratedSource(result, ImportsHint).ShouldBeFalse();
        Problems(result).ShouldBeEmpty();
    }

    [Test]
    public async Task Seeds_Nothing_For_A_Type_Named_ISpecularProvider_That_Is_Not_Specular_s_Interface()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               ExportedJoint,
                               """
                               namespace Specular
                               {
                                   internal class ISpecularProvider
                                   {
                                   }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        GeneratedSource(result, ImportsHint).ShouldNotContain("Specular");
        Problems(result).ShouldBeEmpty();
    }

    private const string ExportedJoint = """
        using Joinery.Dovetail;
        using Joinery.Dovetail.Attributes;
        using Joinery.Dovetail.Infrastructure;

        namespace Joinery.Dovetail.Tests
        {
            [DovetailExport]
            internal class Contrib : IDovetailJoint { }
        }
        """;

    /// <summary>
    ///     An application that references Specular (its runtime package and its generator) and a library that exports a joint.
    /// </summary>
    private GeneratorTestContextBuilder ApplicationWithSpecular() => WithSharedDeps()
       .AddCompilationReferences(SpecularStandIns.CreateRuntimePackage(TestContext.CancellationToken).GetAwaiter().GetResult())
       .WithGenerator<FakeSpecularGenerator>()
       .AddSources(ExportedJoint)
       // The snapshots here only cover what the seeding changes, the imports: not how exports are written, nor the stand-in provider.
       .IgnoreOutputFile("Exported_Dovetails.g.cs")
       .IgnoreOutputFile("SpecularProvider.g.cs");

    /// <summary>
    ///     Every warning and error in the final compilation, which includes the generated code compiling against what Specular generates.
    /// </summary>
    private static IEnumerable<Diagnostic> Problems(GeneratorTestResults result) => result.FinalDiagnostics.Where(z => z.Severity >= DiagnosticSeverity.Warning);

    private static bool HasGeneratedSource(GeneratorTestResults result, string hintName) =>
        result.Results[typeof(DovetailAttributesGenerator)].SyntaxTrees.Any(z => z.FilePath.EndsWith(hintName, StringComparison.Ordinal));

    private static string GeneratedSource(GeneratorTestResults result, string hintName) =>
        result.Results[typeof(DovetailAttributesGenerator)].SyntaxTrees.Single(z => z.FilePath.EndsWith(hintName, StringComparison.Ordinal)).ToString();
}
