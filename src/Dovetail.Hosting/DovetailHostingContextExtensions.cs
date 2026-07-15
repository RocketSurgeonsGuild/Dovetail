using Microsoft.Extensions.Hosting;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Dovetail;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
///     Extension members for <see cref="IDovetailContext" /> host environment access.
/// </summary>
[PublicAPI]
public static class DovetailHostingContextExtensions
{
    extension(IDovetailContext context)
    {
        /// <summary>
        ///     The underlying host environment. Populated by the host during startup.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IHostEnvironment" /> has not been registered in the context's properties.</exception>
        public IHostEnvironment Environment =>
            context.Properties.Get<IHostEnvironment>()
            ?? throw new InvalidOperationException(
                "IHostEnvironment has not been registered in the Dovetail context. Ensure the host populates context.Properties with IHostEnvironment."
            );
    }
}
