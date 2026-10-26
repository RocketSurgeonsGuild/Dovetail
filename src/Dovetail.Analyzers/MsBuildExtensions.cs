using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Dovetail;

internal static class MsBuildExtensions
{
    extension(AnalyzerConfigOptions options)
    {
        public T? GetBuildProperty<T>(string propertyName, Func<string, T?> transform) => options.TryGetValue($"build_property.{propertyName}", out var value) ? transform(value) : default;
    }
    extension(IAssemblySymbol assembly)
    {
        public ImmutableDictionary<string, TypedConstant> GetAssemblyMetadataAttributes(Func<string, bool> predicate)
        {
            var attributes = assembly.GetAttributes();
            // AssemblyMetadata allows repeated keys (Dovetail's own configuration schema fragments repeat theirs); the first wins.
            return attributes
                .Select(z => z is { AttributeClass.MetadataName: "AssemblyMetadataAttribute", ConstructorArguments: [{ Value: string { Length: > 0, } key, }, var value,], } ? (key, value) : default)
                .Where(z => z.key is { } && predicate(z.key))
                .GroupBy(z => z.key, StringComparer.Ordinal)
                .ToImmutableDictionary(z => z.Key, z => z.First().value);
        }
    }

    extension(Compilation compilation)
    {
        public IEnumerable<DovetailConfigurationData> GetDovetailReferences() => compilation.References
            .Select(compilation.GetAssemblyOrModuleSymbol)
            .OfType<IAssemblySymbol>()
            .Select(symbol => DovetailConfigurationData.FromAssemblyAttributes(symbol, "Export"))
            .OfType<DovetailConfigurationData>()
            .OrderBy(z => z);
    }
}
