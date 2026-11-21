using Aspire.Hosting;
using SdkSample.AppHost;

// No resources here: the joints add them (WebResourceJoint, WorkerResourceJoint). ConfigureDovetail applies every
// IDistributedApplicationBuilderJoint of this project and the assemblies it references, in dependency order, then builds the
// application.
await using var app = await DistributedApplication.CreateBuilder(args).ConfigureDovetail();
await app.RunAsync();
