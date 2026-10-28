using Microsoft.Extensions.Configuration;

namespace Joinery.Dovetail.Configuration.Runtime.Tests;

internal static class TestConfiguration
{
    public static IConfigurationRoot Build(params (string Key, string? Value)[] values) =>
        new ConfigurationBuilder()
           .AddInMemoryCollection(values.Select(static pair => new KeyValuePair<string, string?>(pair.Key, pair.Value)))
           .Build();
}
