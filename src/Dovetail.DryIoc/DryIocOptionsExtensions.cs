namespace Dovetail.DryIoc;

/// <summary>
///     Options for building the DryIoc Container
/// </summary>
[PublicAPI]
public static class DryIocOptionsExtensions
{
    extension(IDovetailContext context)
    {
        /// <summary>
        ///     Gets the DryIocOptions from the context
        /// </summary>
        /// <returns></returns>
        public DryIocOptions DryIocOptions => context.GetOrAdd(() => new DryIocOptions());
    }
}
