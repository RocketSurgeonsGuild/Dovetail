using System.ComponentModel;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.FileProviders;

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
        await ApplyConfiguration(context, builder.Configuration, builder.Environment.ContentRootPath, cancellationToken).ConfigureAwait(false);
        await builder.Services.ApplyService(context, cancellationToken).ConfigureAwait(false);
        await builder.ApplyPipelineBuilder(context, cancellationToken).ConfigureAwait(false);
        return context;
    }

    // The pipeline's own configuration holds only the environment variables and the command line, which keep the highest precedence,
    // so Dovetail's configuration goes in front of them. The joints run on a builder of their own so their sources can be chained in
    // that position; it shares the pipeline configuration's properties, so a file provider or base path set on the pipeline
    // (SetBasePath, SetFileProvider) is the one the file sources of the joints resolve against.
    //
    // A file-based app (dotnet run build/Build.cs) keeps its configuration files next to its entry point file, while the pipeline's
    // default file provider is rooted at the content root, the current directory. Unless the pipeline was pointed somewhere else, the
    // joints read from the directory of the entry point file instead. The SDK writes that directory into the runtimeconfig.json, so a
    // published or copied app still names the source directory of the machine it was built on, which needn't exist where it runs.
    private static async ValueTask ApplyConfiguration(
        IDovetailContext context,
        ConfigurationManager configuration,
        string contentRootPath,
        CancellationToken cancellationToken
    )
    {
        var dovetailConfiguration = new ConfigurationBuilder();
        foreach (var property in ( (IConfigurationBuilder)configuration ).Properties)
        {
            dovetailConfiguration.Properties[property.Key] = property.Value;
        }

        // Dovetail:ConfigurationDirectory, when set, wins over both.
        var configurationDirectory = DovetailSettings.GetConfiguredDirectory(configuration)
                                  ?? ( DovetailSettings.GetEntryPointFileDirectory() is { } entryPointFileDirectory
                                     && IsTheContentRoot(dovetailConfiguration, contentRootPath)
                                          ? entryPointFileDirectory
                                          : null );
        if (configurationDirectory is not null)
        {
            dovetailConfiguration.SetBasePath(configurationDirectory);
        }

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

    // Whether the file sources would resolve against the content root: there's no file provider, or the one there is reads the content
    // root, as the pipeline's default does.
    private static bool IsTheContentRoot(IConfigurationBuilder builder, string contentRootPath) =>
        !builder.Properties.TryGetValue("FileProvider", out var fileProvider)
     || ( fileProvider is PhysicalFileProvider physical
         && string.Equals(
                Path.TrimEndingDirectorySeparator(Path.GetFullPath(physical.Root)),
                Path.TrimEndingDirectorySeparator(Path.GetFullPath(contentRootPath)),
                OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal
            ) );
}
