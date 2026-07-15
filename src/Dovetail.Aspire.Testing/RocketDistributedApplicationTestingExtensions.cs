using System.ComponentModel;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Dovetail.Testing.Joints;

/// <summary>
///     Shared helpers used to wire an <see cref="IDistributedApplicationTestingBuilder" /> up to a Dovetail
///     <see cref="IDovetailContext" />. Not intended to be called directly by consumers; hidden from IntelliSense
///     via <see cref="EditorBrowsableState.Never" />.
/// </summary>
[PublicAPI]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DovetailDistributedApplicationTestingHelpers
{
    /// <summary>
    ///     Populates the context, applies shared configuration, and builds the distributed application test host.
    /// </summary>
    /// <param name="builder">The distributed application testing builder to configure.</param>
    /// <param name="contextBuilder">The context builder used to create the <see cref="IDovetailContext" />.</param>
    /// <param name="cancellationToken">The cancellation token used while applying configuration and building the host.</param>
    /// <returns>The built and configured <see cref="DistributedApplication" /> test host.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> or <paramref name="contextBuilder" /> is <see langword="null" />.</exception>
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
