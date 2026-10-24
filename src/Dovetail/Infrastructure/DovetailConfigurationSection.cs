using Microsoft.Extensions.Configuration;

namespace Dovetail.Infrastructure;

/// <summary>
///     One configuration section registered for <typeparamref name="TOptions" /> by
///     <see cref="DovetailConfigurationOptionsExtensions" />, kept in the service collection as a singleton so every section
///     registered for the type, from any assembly, can be applied in one place.
/// </summary>
/// <typeparam name="TOptions">The options type.</typeparam>
internal sealed class DovetailConfigurationSection<TOptions>
    where TOptions : class
{
    /// <summary>
    ///     Initializes a section that binds onto an instance the options pipeline created.
    /// </summary>
    /// <param name="path">The configuration section path.</param>
    /// <param name="order">The order the section applies in; lower first.</param>
    /// <param name="bind">Binds the section onto an existing instance.</param>
    public DovetailConfigurationSection(string path, int order, Action<IConfiguration, TOptions> bind)
    {
        Path = path;
        Order = order;
        Bind = bind;
    }

    /// <summary>
    ///     Initializes a section that creates the instance.
    /// </summary>
    /// <param name="path">The configuration section path.</param>
    /// <param name="order">The order the section applies in; lower first.</param>
    /// <param name="create">Creates an instance from the section.</param>
    public DovetailConfigurationSection(string path, int order, Func<IConfiguration, TOptions> create)
    {
        Path = path;
        Order = order;
        Create = create;
    }

    /// <summary>
    ///     The configuration section path.
    /// </summary>
    public string Path { get; }

    /// <summary>
    ///     The order the section applies in; lower first.
    /// </summary>
    public int Order { get; }

    /// <summary>
    ///     Binds the section onto an existing instance, or <see langword="null" /> for a section that creates the instance.
    /// </summary>
    public Action<IConfiguration, TOptions>? Bind { get; }

    /// <summary>
    ///     Creates an instance from the section, or <see langword="null" /> for a section that binds onto one.
    /// </summary>
    public Func<IConfiguration, TOptions>? Create { get; }

    /// <summary>
    ///     Splits the registered sections into the ones that create the instance and the ones that bind onto it, each in the order
    ///     they apply.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A path registered more than once (compared case-insensitively) applies once, as applying it twice would append its
    ///         lists twice. The registration with the lowest <see cref="Order" /> wins, and at equal order the first registration.
    ///         A binding section whose path is also a creating section is dropped, because creating already applied it.
    ///     </para>
    ///     <para>
    ///         The remaining sections are ordered by <see cref="Order" />, then by path compared case-insensitively, then ordinally.
    ///     </para>
    /// </remarks>
    /// <param name="sections">The registered sections, in registration order.</param>
    /// <param name="create">The sections that create the instance, in the order they merge.</param>
    /// <param name="bind">The sections that bind onto the instance, in the order they bind.</param>
    public static void Arrange(
        IEnumerable<DovetailConfigurationSection<TOptions>> sections,
        out DovetailConfigurationSection<TOptions>[] create,
        out DovetailConfigurationSection<TOptions>[] bind
    )
    {
        var createByPath = new Dictionary<string, DovetailConfigurationSection<TOptions>>(StringComparer.OrdinalIgnoreCase);
        var bindByPath = new Dictionary<string, DovetailConfigurationSection<TOptions>>(StringComparer.OrdinalIgnoreCase);
        foreach (var section in sections)
        {
            var byPath = section.Create is null ? bindByPath : createByPath;
            if (!byPath.TryGetValue(section.Path, out var existing) || section.Order < existing.Order)
            {
                byPath[section.Path] = section;
            }
        }

        foreach (var path in createByPath.Keys)
        {
            bindByPath.Remove(path);
        }

        create = Sort(createByPath.Values);
        bind = Sort(bindByPath.Values);
    }

    private static DovetailConfigurationSection<TOptions>[] Sort(ICollection<DovetailConfigurationSection<TOptions>> sections)
    {
        var sorted = new DovetailConfigurationSection<TOptions>[sections.Count];
        sections.CopyTo(sorted, 0);
        Array.Sort(sorted, Compare);
        return sorted;
    }

    private static int Compare(DovetailConfigurationSection<TOptions> x, DovetailConfigurationSection<TOptions> y)
    {
        var result = x.Order.CompareTo(y.Order);
        if (result == 0)
        {
            result = StringComparer.OrdinalIgnoreCase.Compare(x.Path, y.Path);
        }

        return result == 0 ? StringComparer.Ordinal.Compare(x.Path, y.Path) : result;
    }
}
