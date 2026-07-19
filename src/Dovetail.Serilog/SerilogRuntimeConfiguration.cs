namespace Dovetail.Serilog;

/// <summary>
///     Hand-authored stand-in for the generator-emitted configuration class that
///     <c>openspec/changes/dovetail-managed-configuration</c> will eventually produce for
///     <c>appsettings.json</c>'s <c>Serilog</c> section (see design.md, Decision 4). Dogfoods the
///     runtime half of the pipeline (task 8.3) ahead of the generator landing.
/// </summary>
public sealed record SerilogRuntimeConfiguration
{
    /// <summary>
    ///     The minimum Serilog level to emit, e.g. <c>"Information"</c>, <c>"Debug"</c>, <c>"Warning"</c>.
    /// </summary>
    public string MinimumLevel { get; set; } = "Information";

    /// <summary>
    ///     Whether the console sink should be enabled.
    /// </summary>
    public bool WriteToConsole { get; set; } = true;
}
