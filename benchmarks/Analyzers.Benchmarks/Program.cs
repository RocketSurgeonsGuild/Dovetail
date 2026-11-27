using Analyzers.Benchmarks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

// In process: a separate benchmark build would rebuild the analyzer's per-Roslyn variants into one output folder, where
// they collide. The generator is plain library code here, so running it in this process measures the same thing.
var config = ManualConfig
            .Create(DefaultConfig.Instance)
            .AddJob(Job.ShortRun.WithToolchain(InProcessEmitToolchain.Instance).WithId("net10.0"));

BenchmarkSwitcher.FromTypes([typeof(GeneratorBenchmarks), typeof(AnalyzerBenchmarks)]).Run(args, config);
