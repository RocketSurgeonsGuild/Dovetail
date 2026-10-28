using System.Text;
using System.Text.RegularExpressions;

namespace Joinery.Dovetail.Build.Tasks;

/// <summary>
///     Builds ECMA-262 patterns that match configuration keys the way the configuration binder does: ignoring case. Each letter
///     becomes a character class (<c>[Mm]</c>) rather than relying on <c>(?i)</c>, which JavaScript's <c>RegExp</c> doesn't accept.
/// </summary>
internal static class CaseInsensitivePattern
{
    private static readonly TimeSpan MatchTimeout = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     A pattern that matches exactly <paramref name="names" />, ignoring case: <c>^(?:[Mm][Ee]...|...)$</c>.
    /// </summary>
    public static string For(IEnumerable<string> names) => "^(?:" + string.Join("|", names.Select(Escape)) + ")$";

    /// <summary>
    ///     A pattern that matches exactly <paramref name="name" />, ignoring case.
    /// </summary>
    public static string For(string name) => For([name]);

    /// <summary>
    ///     Finds which of an object's keys a <c>patternProperties</c> entry is the case-insensitive twin of.
    /// </summary>
    public sealed class NameLookup
    {
        private readonly IReadOnlyList<string> _names;
        private readonly Dictionary<string, string> _byPattern = [with(StringComparer.Ordinal)];

        /// <summary>
        ///     A lookup over <paramref name="names" />.
        /// </summary>
        public NameLookup(IReadOnlyList<string> names)
        {
            _names = names;
            foreach (var name in names)
            {
                var pattern = For(name);
                if (!_byPattern.ContainsKey(pattern)) _byPattern.Add(pattern, name);
            }
        }

        /// <summary>
        ///     The name <paramref name="pattern" /> is the case-insensitive pattern of (the one <see cref="For(string)" /> builds,
        ///     or failing that the first name it matches), or <see langword="null" />. A pattern .NET can't parse matches nothing.
        /// </summary>
        public string? Find(string pattern)
        {
            if (_byPattern.TryGetValue(pattern, out var name)) return name;
            try
            {
                var regex = new Regex(pattern, RegexOptions.CultureInvariant, MatchTimeout);
                return _names.FirstOrDefault(regex.IsMatch);
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (RegexMatchTimeoutException)
            {
                return null;
            }
        }
    }

    private static string Escape(string name)
    {
        var builder = new StringBuilder(name.Length * 4);
        foreach (var character in name)
        {
            var upper = char.ToUpperInvariant(character);
            var lower = char.ToLowerInvariant(character);
            if (upper != lower)
            {
                builder.Append('[').Append(upper).Append(lower);
                if (character != upper && character != lower) builder.Append(character);
                builder.Append(']');
            }
            else if (IsSyntaxCharacter(character))
            {
                builder.Append('\\').Append(character);
            }
            else
            {
                builder.Append(character);
            }
        }

        return builder.ToString();
    }

    // ECMA-262's SyntaxCharacter: what has to be escaped outside a character class. Escaping anything else (but "/") is an
    // error in Unicode mode.
    private static bool IsSyntaxCharacter(char character) => character is '^' or '$' or '\\' or '.' or '*' or '+' or '?' or '(' or ')' or '[' or ']' or '{' or '}' or '|';
}
