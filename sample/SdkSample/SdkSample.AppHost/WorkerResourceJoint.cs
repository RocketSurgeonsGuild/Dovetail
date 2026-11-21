using Aspire.Hosting;
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;

namespace SdkSample.AppHost;

/// <summary>
///     Adds SdkSample.Worker as the <c>worker</c> resource. It references <c>web</c>, so Aspire hands it the web application's endpoints
///     as configuration (which SdkSample.ServiceDefaults' service discovery resolves), and starts it once <c>web</c> is healthy.
///     <see cref="DependsOnJointAttribute{T}" /> runs <see cref="WebResourceJoint" /> first, so the resource is in the context.
/// </summary>
[DovetailExport]
[DependsOnJoint<WebResourceJoint>]
public sealed class WorkerResourceJoint : IDistributedApplicationBuilderJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IDistributedApplicationBuilder builder)
    {
        var web = context.Get<WebResource>()
                  ?? throw new InvalidOperationException($"{nameof(WebResourceJoint)} has not added the web resource.");
        builder
           .AddDotnetProject("worker", "../SdkSample.Worker/SdkSample.Worker.csproj")
           .WithReference(web.Builder)
           .WaitFor(web.Builder);
    }
}
