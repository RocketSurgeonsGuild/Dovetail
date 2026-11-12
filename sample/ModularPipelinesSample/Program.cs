using ModularPipelines;
using ModularPipelinesSample;

// ConfigureDovetail (generated into the project's root namespace) applies the joints, BuildJoint among them, then builds the pipeline.
await using var pipeline = await Pipeline.CreateBuilder(args).ConfigureDovetail();
await pipeline.RunAsync();
