using Microsoft.Extensions.Configuration;

using Tomlyn;

namespace Dovetail.Configuration.Toml;

/// <summary>
///     A TOML file based <see cref="FileConfigurationProvider" />.
/// </summary>
/// <remarks>
///     The toml configuration provider
/// </remarks>
/// <param name="source">The source settings used to load the TOML file.</param>
public class TomlConfigurationProvider(TomlConfigurationSource source) : FileConfigurationProvider(source)
{
    /// <inheritdoc />
    /// <exception cref="FormatException">The TOML content in <paramref name="stream" /> could not be parsed.</exception>
    public override void Load(Stream stream)
    {
        var parser = new TomlConfigurationStreamParser();
        try
        {
            Data = parser.Parse(stream);
        }
        catch (TomlException e)
        {
            throw new FormatException($"Could not parse the TOML file: {e.Message}", e);
        }
    }
}
