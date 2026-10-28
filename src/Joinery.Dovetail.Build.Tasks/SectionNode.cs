namespace Joinery.Dovetail.Build.Tasks;

/// <summary>
///     A node of the configuration section tree: section paths split on <c>:</c>, compared ignoring case as the configuration
///     binder does, so <c>Greetings</c> and <c>greetings</c> land on the same node.
/// </summary>
/// <remarks>
///     Everything is ordered independently of the order the fragments arrive in (that is, of reference order), so the files built
///     from the tree are byte-identical for the same set of fragments.
/// </remarks>
internal sealed class SectionNode
{
    private readonly Dictionary<string, SectionNode> _children = [with(StringComparer.OrdinalIgnoreCase)];
    private readonly List<ConfigurationSchemaFragment> _fragments = [];

    private SectionNode(string name) => Name = name;

    /// <summary>
    ///     The segment's display spelling. When fragments spell it differently, the ordinal-smallest spelling wins, so upper case
    ///     beats lower case: <c>Greetings</c> over <c>greetings</c>.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     The fragments that declare exactly this section: identical ones once, ordered by declaring assembly, then schema text.
    /// </summary>
    public IReadOnlyList<ConfigurationSchemaFragment> Fragments => field ??= _fragments
                                                                                       .OrderBy(z => z.Source, StringComparer.Ordinal)
                                                                                       .ThenBy(z => z.Json, StringComparer.Ordinal)
                                                                                       .GroupBy(z => z.Json, StringComparer.Ordinal)
                                                                                       .Select(z => z.First())
                                                                                       .ToArray();

    /// <summary>
    ///     The sections below this one, ordered by name (ignoring case, then ordinally).
    /// </summary>
    public IReadOnlyList<SectionNode> Children => field ??= _children
                                                                     .Values
                                                                     .OrderBy(z => z.Name, StringComparer.OrdinalIgnoreCase)
                                                                     .ThenBy(z => z.Name, StringComparer.Ordinal)
                                                                     .ToArray();

    /// <summary>
    ///     Builds the tree; the root stands for the configuration root and declares nothing itself.
    /// </summary>
    public static SectionNode CreateTree(IEnumerable<ConfigurationSchemaFragment> fragments)
    {
        var root = new SectionNode("");
        foreach (var fragment in fragments)
        {
            var node = root;
            foreach (var segment in fragment.Path)
            {
                node = node.GetOrAddChild(segment);
            }

            node._fragments.Add(fragment);
        }

        return root;
    }

    private SectionNode GetOrAddChild(string segment)
    {
        if (_children.TryGetValue(segment, out var child))
        {
            if (string.CompareOrdinal(segment, child.Name) < 0) child.Name = segment;
        }
        else
        {
            _children.Add(segment, child = new(segment));
        }

        return child;
    }
}
