namespace Dovetail.DryIoc;

/// <summary>
///     Extension members for accessing <see cref="DryIocOptions" /> from the convention context.
/// </summary>
[PublicAPI]
public static class DryIocOptionsExtensions
{
    extension(IDovetailContext context)
    {
        /// <summary>
        ///     Gets the <see cref="DryIocOptions" /> from the context, adding a default instance if one is not already present.
        /// </summary>
        public DryIocOptions DryIocOptions => context.GetOrAdd(() => new DryIocOptions());
    }
}
