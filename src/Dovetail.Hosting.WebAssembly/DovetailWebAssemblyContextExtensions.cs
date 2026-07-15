using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace Dovetail;

/// <summary>
///     Extension members for <see cref="IDovetailContext" /> WebAssembly host environment access.
/// </summary>
[PublicAPI]
public static class DovetailWebAssemblyContextExtensions
{
    extension(IDovetailContext context)
    {
        /// <summary>
        ///     The underlying WebAssembly host environment. Populated by the host during startup.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IWebAssemblyHostEnvironment" /> has not been registered in the context's properties.</exception>
        public IWebAssemblyHostEnvironment Configuration =>
            context.Get<IWebAssemblyHostEnvironment>()
            ?? throw new InvalidOperationException(
                "IWebAssemblyHostEnvironment has not been registered in the Dovetail context. Ensure the host populates context.Properties with IWebAssemblyHostEnvironment."
            );
    }
}
