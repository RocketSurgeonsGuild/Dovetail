using Joinery.Dovetail.Serilog.Tests;

// A logging provider in one of the namespaces that Microsoft.Extensions.Logging's own providers (console, debug, event source, event log) live
// in: Joinery.Dovetail.Serilog treats those as the default providers, which it replaces, by namespace. The namespace is the point of this type.
#pragma warning disable IDE0130
namespace Microsoft.Extensions.Logging.Dovetail.Serilog.Tests;
#pragma warning restore IDE0130

/// <summary>
///     Stands in for a default logging provider that is registered after the Serilog joint has removed the ones that were registered before it.
/// </summary>
public sealed class FakeDefaultProvider : CapturingProvider;
