using System.ComponentModel;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace Joinery.Dovetail;

/// <summary>
///     Wires a ModularPipelines <see cref="PipelineBuilder" /> up to a Dovetail <see cref="IDovetailContext" />. Called by the
///     <c>ConfigureDovetail</c> entry points generated for the ModularPipelines host and by <see cref="DovetailPipelinePlugin" />;
///     hidden from IntelliSense via <see cref="EditorBrowsableState.Never" />.
/// </summary>
[PublicAPI]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class DovetailPipelineHelpers
{
    /// <summary>
    ///     Applies the joints to the builder, builds the pipeline and applies the <c>HostCreated</c> joints to it.
    /// </summary>
    /// <param name="builder">The pipeline builder to configure.</param>
    /// <param name="contextBuilder">The context builder used to create the <see cref="IDovetailContext" />.</param>
    /// <param name="cancellationToken">The cancellation token used while applying the joints and building the pipeline.</param>
    /// <returns>The built pipeline, ready to run.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> or <paramref name="contextBuilder" /> is <see langword="null" />.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async ValueTask<IPipeline> Configure(PipelineBuilder builder, DovetailContextBuilder contextBuilder, CancellationToken cancellationToken)
    {
        var context = await Apply(builder, contextBuilder, cancellationToken).ConfigureAwait(false);
        var pipeline = await builder.BuildAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await pipeline.ApplyHostCreated(context, cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            await pipeline.DisposeAsync().ConfigureAwait(false);
            throw;
        }

        return pipeline;
    }

    /// <summary>
    ///     Populates the context and applies the configuration, service (and with them logging) and <c>PipelineBuilder</c> joints to the
    ///     builder, without building it.
    /// </summary>
    /// <param name="builder">The pipeline builder to configure.</param>
    /// <param name="contextBuilder">The context builder used to create the <see cref="IDovetailContext" />.</param>
    /// <param name="cancellationToken">The cancellation token used while applying the joints.</param>
    /// <returns>The context the joints ran with.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> or <paramref name="contextBuilder" /> is <see langword="null" />.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async ValueTask<IDovetailContext> Apply(PipelineBuilder builder, DovetailContextBuilder contextBuilder, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(contextBuilder);
        contextBuilder
           .AddIfMissing(DovetailHostType.Live)
           .AddIfMissing(builder)
           .AddIfMissing(builder.Configuration)
           .AddIfMissing<IConfiguration>(builder.Configuration)
           .AddIfMissing(builder.Environment)
           .AddIfMissing(builder.Environment.GetType(), builder.Environment)
           .AddIfMissing("ApplicationName", builder.Environment.ApplicationName)
           .AddIfMissing("EnvironmentName", builder.Environment.EnvironmentName);

        var context = await DovetailContext.FromAsync(contextBuilder, cancellationToken).ConfigureAwait(false);
        await ApplyConfiguration(context, builder.Configuration, cancellationToken).ConfigureAwait(false);
        await builder.Services.ApplyService(context, cancellationToken).ConfigureAwait(false);
        await builder.ApplyPipelineBuilder(context, cancellationToken).ConfigureAwait(false);
        return context;
    }

    // The pipeline's own configuration holds only the environment variables and the command line, which keep the highest precedence,
    // so Dovetail's configuration goes in front of them.
    private static async ValueTask ApplyConfiguration(IDovetailContext context, ConfigurationManager configuration, CancellationToken cancellationToken)
    {
        var dovetailConfiguration = new ConfigurationBuilder();
        await dovetailConfiguration.ApplyConfiguration(context, cancellationToken).ConfigureAwait(false);
        if (dovetailConfiguration.Sources is { Count: > 0, })
        {
            IConfigurationBuilder builder = configuration;
            var insertIndex = builder.Sources.Count;
            for (var i = 0; i < builder.Sources.Count; i++)
            {
                if (builder.Sources[i] is CommandLineConfigurationSource or EnvironmentVariablesConfigurationSource)
                {
                    insertIndex = i;
                    break;
                }
            }

            builder.Sources.Insert(insertIndex, new ChainedConfigurationSource { Configuration = dovetailConfiguration.Build(), ShouldDisposeConfiguration = true, });
        }

        configuration.AddInMemoryCollection(new Dictionary<string, string?> { ["RocketSurgeryDovetails:HostType"] = context.GetHostType().ToString(), });
    }
}
