#:package Microsoft.VisualStudio.SolutionPersistence
#:package Sourcy.Git
#:package Sourcy.DotNet
#:package Rocket.Surgery.ModularPipelines.Extensions
#:package Rocket.Surgery.AssemblyInfo
#:property ImportConventions=true
#:property JsonSerializerIsReflectionEnabledByDefault=true

using Build;
using ModularPipelines;
using ModularPipelines.Plugins;
using Rocket.Surgery.ModularPipelines.Extensions;

var pipelineBuilder = Pipeline.CreateBuilder(args);
PluginRegistry.Register(new ConventionsPlugin(ConventionContextBuilder.Create(Imports.Instance)
.AddIfMissing(nameof(AssemblyInfo.Project.BuildScriptsRoot), AssemblyInfo.Project.BuildScriptsRoot)));
await pipelineBuilder.Build().RunAsync();
