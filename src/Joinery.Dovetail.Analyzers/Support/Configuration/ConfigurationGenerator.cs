using Microsoft.CodeAnalysis;

namespace Joinery.Dovetail.Support.Configuration;

/// <summary>
///     Describes every <c>[DovetailConfiguration]</c> section as a JSON Schema fragment and generates the reflection-free options
///     registration for it. It runs apart from the joint generator and shares no helpers, diagnostics or build properties with it.
/// </summary>
[Generator]
public sealed class ConfigurationGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context) => ConfigurationSchemaPipeline.Register(context);
}
