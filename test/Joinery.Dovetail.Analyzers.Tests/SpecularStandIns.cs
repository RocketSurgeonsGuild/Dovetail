using Microsoft.CodeAnalysis;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
///     Stand-ins for Specular, which Dovetail takes no dependency on. They keep the shape the generator relies on: the runtime
///     package contributes <c>Specular.ISpecularProvider</c> to the compilation, and Specular's own generator then adds an
///     internal <c>SpecularProvider</c> with a static <c>Instance</c> to the global namespace.
/// </summary>
internal static class SpecularStandIns
{
    /// <summary>The runtime package: the interface a Joint reads from the context.</summary>
    public static async Task<GeneratorTestResults> CreateRuntimePackage(CancellationToken cancellationToken) =>
        await GeneratorTestContextBuilder
             .Create()
             .WithProjectName("Specular")
             .AddSources(
                  """
                  namespace Specular;

                  public interface ISpecularProvider
                  {
                  }
                  """
              )
             .Build()
             .GenerateAsync(cancellationToken);

    /// <summary>The source Specular's generator emits when an assembly emits a provider.</summary>
    public const string GeneratedProvider = """
        internal sealed class SpecularProvider : Specular.ISpecularProvider
        {
            public static Specular.ISpecularProvider Instance { get; } = new SpecularProvider();

            private SpecularProvider()
            {
            }
        }
        """;
}

/// <summary>
///     Stands in for Specular's generator. A generator doesn't see what another generator adds, so the provider this adds is
///     invisible to Dovetail's generator, exactly as the real one is during a build.
/// </summary>
public sealed class FakeSpecularGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context) =>
        context.RegisterSourceOutput(
            context.AnalyzerConfigOptionsProvider,
            static (productionContext, options) =>
            {
                // Specular's own rule: the provider is emitted unless SpecularEmitProvider is explicitly false.
                var emitsProvider = !options.GlobalOptions.TryGetValue("build_property.SpecularEmitProvider", out var value)
                 || !bool.TryParse(value, out var parsed)
                 || parsed;
                if (emitsProvider) productionContext.AddSource("SpecularProvider.g.cs", SpecularStandIns.GeneratedProvider);
            }
        );
}
