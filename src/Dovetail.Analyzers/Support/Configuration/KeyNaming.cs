using System.Globalization;
using System.Text;

namespace Dovetail.Support.Configuration;

/// <summary>The analyzer's copy of <c>Dovetail.Attributes.ConfigurationKeyStyle</c>; the values are the same.</summary>
internal enum KeyStyle
{
    Undefined = 0,
    Pascal = 1,
    Camel = 2,
    Dash = 3,
    Underscore = 4,
}

/// <summary>
///     Spells a declared member or type name in a <see cref="KeyStyle" />. <see cref="KeyStyle.Camel" /> and
///     <see cref="KeyStyle.Dash" /> and <see cref="KeyStyle.Underscore" /> reimplement <c>JsonNamingPolicy.CamelCase</c>,
///     <c>JsonNamingPolicy.KebabCaseLower</c> and <c>JsonNamingPolicy.SnakeCaseLower</c> (the generator targets netstandard2.0,
///     where <c>System.Text.Json</c> naming policies don't exist), so word splitting matches what developers know from JSON. The
///     tests compare all three against the real policies. <see cref="KeyStyle.Undefined" /> keeps the declared name.
/// </summary>
internal static class KeyNaming
{
    /// <summary>The key for a declared name, in <paramref name="style" />.</summary>
    public static string Apply(string name, KeyStyle style) => style switch
    {
        KeyStyle.Pascal => Pascal(name),
        KeyStyle.Camel => Camel(name),
        KeyStyle.Dash => Dash(name),
        KeyStyle.Underscore => Underscore(name),
        _ => name,
    };

    /// <summary>The name with its first letter upper-cased.</summary>
    public static string Pascal(string name)
    {
        if (name.Length == 0 || !char.IsLower(name[0]))
        {
            return name;
        }

        var chars = name.ToCharArray();
        chars[0] = char.ToUpperInvariant(chars[0]);
        return new(chars);
    }

    /// <summary>Like <c>JsonNamingPolicy.CamelCase</c>: the leading run of upper-case letters is lower-cased, keeping the last one of a run that starts a word.</summary>
    public static string Camel(string name)
    {
        if (name.Length == 0 || !char.IsUpper(name[0]))
        {
            return name;
        }

        var chars = name.ToCharArray();
        for (var index = 0; index < chars.Length; index++)
        {
            if (index == 1 && !char.IsUpper(chars[index]))
            {
                break;
            }

            var hasNext = index + 1 < chars.Length;

            // Stop when the next character is already lower-case, so HTTPServer becomes httpServer.
            if (index > 0 && hasNext && !char.IsUpper(chars[index + 1]))
            {
                // A following space still lower-cases the current character before stopping.
                if (chars[index + 1] == ' ')
                {
                    chars[index] = char.ToLowerInvariant(chars[index]);
                }

                break;
            }

            chars[index] = char.ToLowerInvariant(chars[index]);
        }

        return new(chars);
    }

    /// <summary>Like <c>JsonNamingPolicy.KebabCaseLower</c>: lower-case words joined with <c>-</c>.</summary>
    public static string Dash(string name) => Separated(name, '-');

    /// <summary>Like <c>JsonNamingPolicy.SnakeCaseLower</c>: lower-case words joined with <c>_</c>.</summary>
    public static string Underscore(string name) => Separated(name, '_');

    private static string Separated(string name, char separator)
    {
        var builder = new StringBuilder(name.Length + name.Length / 4 + 1);
        var state = SeparatorState.NotStarted;
        for (var index = 0; index < name.Length; index++)
        {
            var current = name[index];
            var category = CharUnicodeInfo.GetUnicodeCategory(current);
            if (category == UnicodeCategory.UppercaseLetter)
            {
                switch (state)
                {
                    case SeparatorState.LowercaseLetterOrDigit:
                    case SeparatorState.SpaceSeparator:
                        // An upper-case letter after lower-case letters, digits or spaces starts a new word.
                        builder.Append(separator);
                        break;
                    case SeparatorState.UppercaseLetter:
                        // In a run of upper-case letters only the last one starts a word, and only when lower-case follows:
                        // XMLReader is xml-reader, SHA512Hash is sha512-hash.
                        if (index + 1 < name.Length && char.IsLower(name[index + 1]))
                        {
                            builder.Append(separator);
                        }

                        break;
                    case SeparatorState.NotStarted:
                    default:
                        break;
                }

                builder.Append(char.ToLowerInvariant(current));
                state = SeparatorState.UppercaseLetter;
            }
            else if (category is UnicodeCategory.LowercaseLetter or UnicodeCategory.DecimalDigitNumber)
            {
                if (state == SeparatorState.SpaceSeparator)
                {
                    // Spaces between words collapse to one separator.
                    builder.Append(separator);
                }

                builder.Append(current);
                state = SeparatorState.LowercaseLetterOrDigit;
            }
            else if (category == UnicodeCategory.SpaceSeparator)
            {
                // Spaces at the start and end are dropped.
                if (state != SeparatorState.NotStarted)
                {
                    state = SeparatorState.SpaceSeparator;
                }
            }
            else
            {
                // Anything else (including the separator itself and surrogates) is kept and restarts word detection.
                builder.Append(current);
                state = SeparatorState.NotStarted;
            }
        }

        return builder.ToString();
    }

    private enum SeparatorState
    {
        NotStarted,
        UppercaseLetter,
        LowercaseLetterOrDigit,
        SpaceSeparator,
    }
}
