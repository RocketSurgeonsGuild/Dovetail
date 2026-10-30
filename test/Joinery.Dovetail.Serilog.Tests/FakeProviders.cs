using Joinery.Dovetail.Serilog.Tests;

// Joinery.Dovetail.Serilog tells the default providers it replaces by their full type names, so two stand-ins are needed to test it. Both live in
// namespaces of Microsoft.Extensions.Logging's own providers; only the first has the full name of one of them.
#pragma warning disable IDE0130, CS0436

namespace Microsoft.Extensions.Logging.Debug
{
    /// <summary>
    ///     Has the full name of Microsoft.Extensions.Logging.Debug's provider, one of the default providers, so it stands in for one that is registered after
    ///     the Serilog joint has removed the ones registered before it. The type of the same name from that package is hidden by this one in this assembly.
    /// </summary>
    internal sealed class DebugLoggerProvider : CapturingProvider;
}

namespace Microsoft.Extensions.Logging.Dovetail.Serilog.Tests
{
    /// <summary>
    ///     A provider that lives in a namespace of Microsoft.Extensions.Logging's own providers, as Application Insights' and Azure App Service's do,
    ///     without being one of the default providers.
    /// </summary>
    public sealed class FakeNamespacedProvider : CapturingProvider;
}

namespace Joinery.Dovetail.Serilog.Tests
{
    /// <summary>
    ///     The stand-ins, without the name conflict leaking into the tests.
    /// </summary>
    public static class FakeProviders
    {
        /// <summary>
        ///     A provider with the full name of the default debug provider.
        /// </summary>
        public static CapturingProvider NewDefaultProvider() => new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider();
    }
}
