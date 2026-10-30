using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Joinery.Dovetail.Serilog;

/// <summary>
///     Reads the minimum level of the logger from the <c>Serilog:MinimumLevel</c> section of the configuration, with nothing but
///     <see cref="IConfiguration" /> and enum parsing, so nothing needs reflection and it is trimming and Native AOT safe: <c>Default</c>
///     (or the section's own value, <c>"MinimumLevel": "Debug"</c>) and the levels per source under <c>Override</c>.
/// </summary>
/// <remarks>
///     The levels are held by <see cref="LoggingLevelSwitch" />es, which are updated when the configuration reloads. Only the levels that
///     were configured when the logger was set up follow a reload: a level that is added later, or that is removed or invalid, leaves the
///     level in effect as it is.
/// </remarks>
internal sealed class SerilogMinimumLevelConfiguration : IDisposable
{
    /// <summary>
    ///     The path of the section.
    /// </summary>
    internal const string SectionPath = "Serilog:MinimumLevel";

    /// <summary>
    ///     Applies the levels in the configuration to <paramref name="loggerConfiguration" />.
    /// </summary>
    /// <returns>The levels, to be disposed with the logger, or <see langword="null" /> when the configuration has none.</returns>
    /// <exception cref="InvalidOperationException">A value is not the name of a <see cref="LogEventLevel" />.</exception>
    public static SerilogMinimumLevelConfiguration? Apply(IConfiguration configuration, LoggerConfiguration loggerConfiguration)
    {
        var section = configuration.GetSection(SectionPath);
        if (!section.Exists())
        {
            return null;
        }

        LoggingLevelSwitch? defaultSwitch = null;
        if (DefaultValue(section) is { } defaultValue)
        {
            defaultSwitch = new(Parse(defaultValue, section[DefaultKey] is null ? section.Path : $"{section.Path}:{DefaultKey}"));
            loggerConfiguration.MinimumLevel.ControlledBy(defaultSwitch);
        }

        Dictionary<string, LoggingLevelSwitch> overrideSwitches = [];
        foreach (var source in section.GetSection(OverrideKey).GetChildren())
        {
            var overrideSwitch = new LoggingLevelSwitch(Parse(source.Value, source.Path));
            overrideSwitches[source.Key] = overrideSwitch;
            loggerConfiguration.MinimumLevel.Override(source.Key, overrideSwitch);
        }

        return defaultSwitch is null && overrideSwitches.Count == 0 ? null : new(section, defaultSwitch, overrideSwitches);
    }

    /// <inheritdoc />
    public void Dispose() => _registration.Dispose();

    private SerilogMinimumLevelConfiguration(IConfigurationSection section, LoggingLevelSwitch? defaultSwitch, Dictionary<string, LoggingLevelSwitch> overrideSwitches)
    {
        _section = section;
        _defaultSwitch = defaultSwitch;
        _overrideSwitches = overrideSwitches;
        _registration = ChangeToken.OnChange(section.GetReloadToken, Reload);
    }

    private void Reload()
    {
        if (_defaultSwitch is { } && TryParse(DefaultValue(_section), out var defaultLevel))
        {
            _defaultSwitch.MinimumLevel = defaultLevel;
        }

        var overrides = _section.GetSection(OverrideKey);
        foreach (var (source, overrideSwitch) in _overrideSwitches)
        {
            if (TryParse(overrides[source], out var overrideLevel))
            {
                overrideSwitch.MinimumLevel = overrideLevel;
            }
        }
    }

    /// <summary>
    ///     The default level is <c>Default</c> in the object form of the section, or the value of the section in its string form.
    /// </summary>
    private static string? DefaultValue(IConfigurationSection section) => section[DefaultKey] ?? section.Value;

    private static LogEventLevel Parse(string? value, string path) =>
        TryParse(value, out var level)
            ? level
            : throw new InvalidOperationException(
                $"The configuration value '{path}' is '{value}', which is not a Serilog level. Use one of {string.Join(", ", Enum.GetNames<LogEventLevel>())}."
            );

    /// <summary>
    ///     Only the names of the levels: not their numbers, which <see cref="Enum.TryParse{TEnum}(string?, bool, out TEnum)" /> also takes.
    /// </summary>
    private static bool TryParse(string? value, out LogEventLevel level) =>
        Enum.TryParse(value, ignoreCase: true, out level)
     && Enum.IsDefined(level)
     && string.Equals(level.ToString(), value, StringComparison.OrdinalIgnoreCase);

    private const string DefaultKey = "Default";
    private const string OverrideKey = "Override";
    private readonly LoggingLevelSwitch? _defaultSwitch;
    private readonly Dictionary<string, LoggingLevelSwitch> _overrideSwitches;
    private readonly IDisposable _registration;
    private readonly IConfigurationSection _section;
}
