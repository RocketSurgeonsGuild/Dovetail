using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.Benchmarks;

/// <summary>
///     Runs <see cref="DovetailJointAnalyzer" /> over the same compilation the generator benchmarks use: the joint checks the
///     generator made on every run before they moved to the analyzer.
/// </summary>
[MemoryDiagnoser]
public class AnalyzerBenchmarks
{
    private Compilation _compilation = null!;
    private readonly ImmutableArray<DiagnosticAnalyzer> _analyzers = [new DovetailJointAnalyzer()];

    [GlobalSetup]
    public void Setup()
    {
        var generator = new GeneratorBenchmarks();
        generator.Setup();
        _compilation = generator.Compilation;
    }

    /// <summary>Every diagnostic of the compilation, including those reported when it ends: a build.</summary>
    [Benchmark]
    public Task<ImmutableArray<Diagnostic>> Analyze() => _compilation.WithAnalyzers(_analyzers).GetAnalyzerDiagnosticsAsync();
}
