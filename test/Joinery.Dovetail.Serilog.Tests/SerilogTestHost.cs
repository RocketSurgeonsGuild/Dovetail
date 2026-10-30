using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Joinery.Dovetail.Serilog.Tests;

internal static class SerilogTestHost
{
    /// <summary>
    ///     Builds a generic host with Dovetail applied, the way an application does, and the given settings in its configuration.
    /// </summary>
    public static async ValueTask<IHost> BuildAsync(IReadOnlyDictionary<string, string?>? settings = null, Action<DovetailContextBuilder>? configure = null)
    {
        var builder = Host.CreateApplicationBuilder();
        if (settings is { })
        {
            builder.Configuration.AddInMemoryCollection(settings);
        }

        return await builder.ConfigureDovetail(configure ?? ( static _ => { } ));
    }

    /// <summary>
    ///     A name for a <see cref="CapturingSink" /> that no other test uses.
    /// </summary>
    public static string NewSinkName() => Guid.NewGuid().ToString("N");
}
