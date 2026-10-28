using System.ComponentModel.DataAnnotations;
using Joinery.Dovetail.Attributes;
using Microsoft.Extensions.Logging;

namespace SdkSample.Diagnostics;

/// <summary>
///     Options for the diagnostics report. Bound from the <c>Diagnostics</c> section. Next to it, this library configures
///     <c>HostOptions</c> (section <c>Host</c>) and <c>ConsoleLifetimeOptions</c> (section <c>Console</c>) from Microsoft's
///     packages; those two are declared in <c>AssemblyInfo.cs</c>.
/// </summary>
[DovetailConfiguration]
public sealed class DiagnosticsOptions
{
    /// <summary>
    ///     The name the report is published under.
    /// </summary>
    public string ServiceName { get; set; } = "sdk-sample";

    /// <summary>
    ///     The least serious level the report includes.
    /// </summary>
    public LogLevel MinimumLevel { get; set; } = LogLevel.Information;

    /// <summary>
    ///     The share of requests that are traced, from 0 (none) to 1 (all).
    /// </summary>
    [Range(0.0, 1.0)]
    public double SampleRatio { get; set; } = 1.0;
}
