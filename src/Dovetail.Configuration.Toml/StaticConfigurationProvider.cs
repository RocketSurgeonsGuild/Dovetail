using Microsoft.Extensions.Configuration;

namespace Dovetail.Configuration.Toml;

internal sealed class StaticConfigurationProvider : ConfigurationProvider
{
    public StaticConfigurationProvider(IDictionary<string, string?> data) => Data = data ?? throw new ArgumentNullException(nameof(data));
}
