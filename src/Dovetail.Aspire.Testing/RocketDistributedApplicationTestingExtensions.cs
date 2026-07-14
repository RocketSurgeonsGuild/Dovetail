using System.ComponentModel;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Dovetail.Testing.Joints;

[PublicAPI]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DovetailDistributedApplicationTestingHelpers
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async ValueTask<DistributedApplication> Configure(
        IDistributedApplicationTestingBuilder builder,
        DovetailContextBuilder contextBuilder,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(contextBuilder);

        contextBuilder
           .AddIfMissing(DovetailHostType.Live)
           .AddIfMissing(builder)
           .AddIfMissing(builder.GetType(), builder)
           .AddIfMissing(builder.Configuration)
           .AddIfMissing<IConfiguration>(builder.Configuration)
           .AddIfMissing(builder.Configuration.GetType(), builder.Configuration)
           .AddIfMissing(builder.Environment)
           .AddIfMissing(builder.Environment.GetType(), builder.Environment);

        var context = await DovetailContext.FromAsync(contextBuilder, cancellationToken).ConfigureAwait(false);
        builder.Configuration.AddInMemoryCollection(
            new Dictionary<string, string?> { ["RocketSurgeryDovetails:HostType"] = context.GetHostType().ToString(), }
        );

        await builder.ApplyDistributedApplicationTesting(context, cancellationToken).ConfigureAwait(false);
        var host = await builder.BuildAsync(cancellationToken).ConfigureAwait(false);
        await host.ApplyHostCreated(context, cancellationToken).ConfigureAwait(false);
        return host;
    }
}
