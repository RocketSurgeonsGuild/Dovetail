using BenchmarkDotNet.Attributes;
using Joinery.Dovetail.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Analyzers.Benchmarks;

/// <summary>
///     Runs Dovetail's generator the way the IDE does: once from scratch, and again after each edit. An edit benchmark alternates
///     between two compilations that differ only in the edited file, so every invocation is one edit the driver hasn't seen.
/// </summary>
[MemoryDiagnoser]
public class GeneratorBenchmarks
{
    /// <summary>How many files declare joints.</summary>
    private const int JointFiles = 20;

    /// <summary>How many files have nothing to do with Dovetail.</summary>
    private const int UnrelatedFiles = 100;
    private CSharpCompilation _jointEdited = null!;
    private CSharpCompilation _unrelatedEdited = null!;
    private CSharpParseOptions _parseOptions = null!;

    private GeneratorDriver _jointDriver = null!;
    private GeneratorDriver _unrelatedDriver = null!;
    private bool _jointToggle;
    private bool _unrelatedToggle;

    /// <summary>The compilation the generator runs over, which <see cref="AnalyzerBenchmarks" /> analyzes too.</summary>
    internal CSharpCompilation Compilation { get; private set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        _parseOptions = new(LanguageVersion.Preview);
        var trees = new List<SyntaxTree>();
        for (var i = 0; i < JointFiles; i++)
        {
            trees.Add(Parse(JointSource(i, ""), $"Joints{i}.cs"));
        }

        for (var i = 0; i < UnrelatedFiles; i++)
        {
            trees.Add(Parse(UnrelatedSource(i, ""), $"Unrelated{i}.cs"));
        }

        Compilation = CSharpCompilation.Create(
            "Sample",
            trees,
            References(),
            new(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable)
        );

        // An edit above the joints that doesn't change them: their spans move, nothing else does.
        _jointEdited = Compilation.ReplaceSyntaxTree(trees[0], Parse(JointSource(0, "// edited\r\n"), "Joints0.cs"));
        _unrelatedEdited = Compilation.ReplaceSyntaxTree(trees[JointFiles], Parse(UnrelatedSource(0, "// edited\r\n"), "Unrelated0.cs"));

        _jointDriver = CreateDriver().RunGenerators(Compilation);
        _unrelatedDriver = CreateDriver().RunGenerators(Compilation);
    }

    /// <summary>The first run, with nothing cached: a build.</summary>
    [Benchmark(Baseline = true)]
    public GeneratorDriver Cold() => CreateDriver().RunGenerators(Compilation);

    /// <summary>A keystroke in a file that declares joints, which doesn't change them.</summary>
    [Benchmark]
    public GeneratorDriver Edit_joint_file()
    {
        _jointToggle = !_jointToggle;
        return _jointDriver = _jointDriver.RunGenerators(_jointToggle ? _jointEdited : Compilation);
    }

    /// <summary>A keystroke in a file that has nothing to do with Dovetail.</summary>
    [Benchmark]
    public GeneratorDriver Edit_unrelated_file()
    {
        _unrelatedToggle = !_unrelatedToggle;
        return _unrelatedDriver = _unrelatedDriver.RunGenerators(_unrelatedToggle ? _unrelatedEdited : Compilation);
    }

    private GeneratorDriver CreateDriver() =>
        CSharpGeneratorDriver.Create(
            [new DovetailAttributesGenerator().AsSourceGenerator()],
            parseOptions: _parseOptions,
            driverOptions: new(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: false)
        );

    private SyntaxTree Parse(string source, string path) => CSharpSyntaxTree.ParseText(source, _parseOptions, path);

    /// <summary>The framework's assemblies, as an application has them, and Dovetail's.</summary>
    private static IEnumerable<MetadataReference> References() =>
        ( (string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")! )
       .Split(Path.PathSeparator)
       .Append(typeof(DovetailExportAttribute).Assembly.Location)
       .Distinct(StringComparer.Ordinal)
       .Select(path => MetadataReference.CreateFromFile(path));

    private static string JointSource(int index, string prefix) =>
        $$"""
          {{prefix}}using Joinery.Dovetail;
          using Joinery.Dovetail.Attributes;

          namespace Sample.Joints{{index}};

          [DovetailExport, DependsOnJoint(typeof(Second{{index}})), DovetailCategory("Core")]
          internal class First{{index}} : IDovetailJoint
          {
              public First{{index}}(System.IServiceProvider services, int retries = 3) { }
          }

          [DovetailExport, LiveJoint]
          internal class Second{{index}} : IDovetailJoint { }

          [DovetailExport, BeforeJoint(typeof(First{{index}}))]
          internal class Third{{index}} : IDovetailJoint { }
          """;

    private static string UnrelatedSource(int index, string prefix) =>
        $$"""
          {{prefix}}namespace Sample.Unrelated{{index}};

          public class Service{{index}}
          {
              public int Value { get; set; }

              public string Describe() => $"Service {{index}}: {Value}";
          }
          """;
}
