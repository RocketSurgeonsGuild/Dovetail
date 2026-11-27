using Microsoft.CodeAnalysis.Diagnostics;

namespace Joinery.Dovetail;

internal static class MsBuildExtensions
{
    extension(AnalyzerConfigOptions options)
    {
        public T? GetBuildProperty<T>(string propertyName, Func<string, T?> transform) => options.TryGetValue($"build_property.{propertyName}", out var value) ? transform(value) : default;
    }
}
