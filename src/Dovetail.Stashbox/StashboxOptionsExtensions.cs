namespace Dovetail.Stashbox;

/// <summary>
///     Extension members for accessing <see cref="StashboxOptions" /> from the convention context.
/// </summary>
[PublicAPI]
public static class StashboxOptionsExtensions
{
    extension(IDovetailContext context)
    {
        /// <summary>
        ///     Gets the <see cref="StashboxOptions" /> from the context, adding a default instance if one is not already present.
        /// </summary>
        public StashboxOptions StashboxOptions => context.GetOrAdd(() => new StashboxOptions());
    }
}
