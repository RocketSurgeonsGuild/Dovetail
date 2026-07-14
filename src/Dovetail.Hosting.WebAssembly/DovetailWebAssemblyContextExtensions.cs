using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace Dovetail;

/// <summary>
///     Extension members for IDovetailContext configuration access.
/// </summary>
[PublicAPI]
public static class DovetailWebAssemblyContextExtensions
{
    extension(IDovetailContext context)
    {
        /// <summary>
        ///     The underlying configuration. Populated from ConfigurationManager on web hosts.
        /// </summary>
        public IWebAssemblyHostEnvironment Configuration =>
            context.Get<IWebAssemblyHostEnvironment>()
            ?? throw new InvalidOperationException(
                "IWebAssemblyHostEnvironment has not been registered in the Dovetail context. Ensure the host populates context.Properties with IWebAssemblyHostEnvironment."
            );
    }
}
