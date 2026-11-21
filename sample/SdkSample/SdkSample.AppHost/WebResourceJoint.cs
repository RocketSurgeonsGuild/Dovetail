using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Dotnet;
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;

namespace SdkSample.AppHost;

/// <summary>
///     Adds SdkSample.Web, by path, as the <c>web</c> resource: its endpoints come from its launch profile, and the dashboard reports it
///     healthy once <c>/health</c> answers. The resource is left in the context for the joints that depend on this one.
/// </summary>
[DovetailExport]
public sealed class WebResourceJoint : IDistributedApplicationBuilderJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IDistributedApplicationBuilder builder)
    {
        var web = builder
                 .AddDotnetProject("web", "../SdkSample.Web/SdkSample.Web.csproj")
                 .WithExternalHttpEndpoints()
                 .WithHttpHealthCheck("/health");
        context.Set(new WebResource(web));
    }
}

/// <summary>The <c>web</c> resource, which <see cref="WebResourceJoint" /> adds and shares through the context.</summary>
/// <param name="Builder">The resource's builder, to reference it from another resource.</param>
public sealed record WebResource(IResourceBuilder<DotnetProjectResource> Builder);
