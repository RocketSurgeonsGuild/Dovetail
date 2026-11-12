#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace Joinery.Dovetail;

/// <summary>
///     A ModularPipelines plugin that applies Dovetail's joints to the pipeline builder when it is added, for an application that builds
///     and runs the pipeline itself: <c>builder.AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()))</c>.
/// </summary>
/// <remarks>
///     <para>
///         The joints run when the plugin is added, so modules and services the application adds afterwards come after theirs. The
///         <c>HostCreated</c> joints do not run, as the plugin never sees the built pipeline; the generated <c>ConfigureDovetail</c>
///         entry points run them.
///     </para>
///     <para>
///         <see cref="IModularPipelinesPlugin.Configure" /> is synchronous, so the plugin blocks on the asynchronous joints. That is
///         safe because Dovetail never resumes on the calling thread's context.
///     </para>
/// </remarks>
/// <param name="contextBuilder">The context builder used to create the <see cref="IDovetailContext" />.</param>
[PublicAPI]
public sealed class DovetailPipelinePlugin(DovetailContextBuilder contextBuilder) : IModularPipelinesPlugin
{

    /// <inheritdoc />
    public string Name => "Dovetail";

    /// <summary>
    ///     The context the joints ran with, once the plugin has been added to a builder.
    /// </summary>
    public IDovetailContext? Context { get; private set; }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public void Configure(PipelineBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var apply = DovetailPipelineHelpers.Apply(builder, contextBuilder, CancellationToken.None);
        Context = apply.IsCompletedSuccessfully ? apply.Result : apply.AsTask().GetAwaiter().GetResult();
    }
}
