using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace SdkSample.Diagnostics;

internal sealed class DiagnosticsReport(IOptions<DiagnosticsOptions> diagnostics, IOptions<HostOptions> host, IOptions<ConsoleLifetimeOptions> console) : IDiagnosticsReport
{
    public string Describe()
    {
        var options = diagnostics.Value;
        return $"Diagnostics '{options.ServiceName}': level {options.MinimumLevel}, sampling {options.SampleRatio:0%}, host shutdown timeout {host.Value.ShutdownTimeout.TotalSeconds:0}s, console status messages {(console.Value.SuppressStatusMessages ? "off" : "on")}.";
    }
}
