using Microsoft.Extensions.Configuration;

namespace Dovetail;

/// <summary>
///     Extension members for IDovetailContext configuration access.
/// </summary>
[PublicAPI]
public static class DovetailConfigurationContextExtensions
{
    extension(IDovetailContext context)
    {
        /// <summary>
        ///     The underlying configuration. Populated from ConfigurationManager on web hosts.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IConfiguration" /> has not been registered in the context's properties.</exception>
        public IConfiguration Configuration =>
            context.Properties.Get<IConfiguration>()
            ?? throw new InvalidOperationException(
                "IConfiguration has not been registered in the Dovetail context. Ensure the host populates context.Properties with IConfiguration."
            );
    }
}
