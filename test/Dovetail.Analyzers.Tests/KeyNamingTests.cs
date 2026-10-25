using System.Text;
using System.Text.Json;
using Dovetail.Support.Configuration;

namespace Dovetail.Analyzers.Tests;

/// <summary>
///     The generator's own spelling of camel-case and dash-case keys (it targets netstandard2.0, so it can't use
///     <see cref="JsonNamingPolicy" />) must agree with the real <see cref="JsonNamingPolicy.CamelCase" /> and
///     <see cref="JsonNamingPolicy.KebabCaseLower" /> for every input.
/// </summary>
public class KeyNamingTests
{
    private static readonly string[] Corpus =
    [
        // Plain words.
        "Name", "RetryCount", "MaxConnections", "HttpClient", "BaseAddress", "ConnectionString", "A", "B", "AB", "Ab", "aB", "a", "z",
        // Already lower or camel.
        "name", "retryCount", "alreadylower", "alreadyCamelCase", "lowerWithUPPER",
        // Acronyms.
        "HTTPServer", "UseSSL", "XMLHttpRequest", "URL", "ID", "UserID", "IOStream", "SHA512Hash", "XMLReader", "DBContext", "OAuth", "ABTest", "TCPIPPort", "ASPNETCore",
        "HTTP", "HTTPS", "UseHTTPS", "MyURLs", "GetXMLHTTPRequest", "ABCDefGHI",
        // Digits.
        "Level2Cache", "Http2Enabled", "Value1", "1Value", "123", "Value123", "V2", "V2Value", "Http2", "Utf8Encoding", "Base64String", "X509Certificate", "Point3D", "Version1_0", "A1B2C3", "2FA",
        "Level2CacheSize10", "ABC123Def", "abc123Def",
        // Underscores, dashes and other punctuation.
        "_private", "_Private", "__double", "snake_case_name", "Snake_Case_Name", "SNAKE_CASE", "Trailing_", "_", "__", "kebab-case", "Kebab-Case", "KEBAB-CASE", "dots.in.name", "Dots.In.Name",
        "Name:Sub", "A_B", "A-B", "a_B", "Mixed_UPPER_andLower", "$Dollar", "@At", "#Hash", "Percent%Sign", "With/Slash", "A.B.C",
        // Spaces.
        " Leading", "Trailing ", " Both ", "Two Words", "Two  Spaces", "Two   Spaces   Trail  ", "TWO WORDS", "two words", "Two Words And More", "A B", "A  B", "a b", "A b", "AB cd", "HTTP Server", " Nbsp",
        "Tab\tSeparated", "Line\nBreak", "Ideo　Space",
        // Non-ASCII letters.
        "Ärger", "ÄrgerLich", "ÉcoleName", "éCole", "ÅngströmUnit", "ÖÖÖ", "ÖÖÖber", "ÜberUser", "straßeNumber", "Straße", "STRASSE", "İstanbul", "Iİ", "ıI", "ΣΑΣ", "ΣΑΣName", "σΑΣ", "Ωmega", "ΩmegaΩ",
        "ДомОтель", "Дом", "ДОМ", "日本語", "日本語Name", "Name日本語", "Ǆ", "ǅ", "ǆ", "ǅName", "ǄName", "ʰModifier", "ªOrdinal", "ÀÉÎÕÜ", "ÀÉÎÕÜx",
        // Surrogate pairs.
        "\U0001D504Fraktur", "Name\U0001D504", "\U0001D504", "a\U0001D504B", "\U00010400Deseret", "\U00010428desert", "😀Emoji", "Emoji😀", "Emo😀ji", "A😀B",
        // Edge cases.
        "", " ", "  ", "-", "--", "A-", "-A", "A_", "_A",
    ];

    private static readonly char[] FuzzAlphabet =
    [
        'A', 'B', 'C', 'X', 'Z', 'a', 'b', 'c', 'x', 'z', '0', '1', '9', '_', '-', ' ', '.', ':', 'É', 'é', 'Ö', 'ö', 'ß', 'Σ', 'σ', 'ς', 'İ', 'ı', 'Ǆ', 'ǅ', 'ǆ', 'ʰ', '日', 'Д', 'д', ' ', '　', '\t', '\ud835',
        '\udd04',
    ];

    [Test]
    public Task Camel_matches_JsonNamingPolicy_on_the_corpus()
    {
        Mismatches(Corpus, KeyNaming.Camel, JsonNamingPolicy.CamelCase).ShouldBeEmpty();
        return Task.CompletedTask;
    }

    [Test]
    public Task Dash_matches_JsonNamingPolicy_on_the_corpus()
    {
        Mismatches(Corpus, KeyNaming.Dash, JsonNamingPolicy.KebabCaseLower).ShouldBeEmpty();
        return Task.CompletedTask;
    }

    [Test]
    public Task Camel_and_dash_match_JsonNamingPolicy_on_generated_names()
    {
        // A fixed seed keeps a failure reproducible.
        var random = new Random(20260101);
        var names = new List<string>();
        for (var count = 0; count < 200_000; count++)
        {
            var length = random.Next(0, 12);
            var builder = new StringBuilder(length);
            for (var index = 0; index < length; index++)
            {
                builder.Append(FuzzAlphabet[random.Next(FuzzAlphabet.Length)]);
            }

            names.Add(builder.ToString());
        }

        Mismatches(names, KeyNaming.Camel, JsonNamingPolicy.CamelCase).ShouldBeEmpty();
        Mismatches(names, KeyNaming.Dash, JsonNamingPolicy.KebabCaseLower).ShouldBeEmpty();
        return Task.CompletedTask;
    }

    [Test]
    [Arguments("HTTPServer", "HTTPServer", "httpServer", "http-server")]
    [Arguments("UseSSL", "UseSSL", "useSSL", "use-ssl")]
    [Arguments("XMLHttpRequest", "XMLHttpRequest", "xmlHttpRequest", "xml-http-request")]
    [Arguments("Level2Cache", "Level2Cache", "level2Cache", "level2-cache")]
    [Arguments("Http2Enabled", "Http2Enabled", "http2Enabled", "http2-enabled")]
    [Arguments("RetryCount", "RetryCount", "retryCount", "retry-count")]
    [Arguments("name", "Name", "name", "name")]
    [Arguments("_private", "_private", "_private", "_private")]
    [Arguments("snake_case", "Snake_case", "snake_case", "snake_case")]
    [Arguments("X", "X", "x", "x")]
    [Arguments("", "", "", "")]
    [Arguments("Greetings", "Greetings", "greetings", "greetings")]
    [Arguments("HttpClient", "HttpClient", "httpClient", "http-client")]
    [Arguments("OpenAI", "OpenAI", "openAI", "open-ai")]
    public Task Spells_names_in_each_style(string name, string pascal, string camel, string dash)
    {
        KeyNaming.Apply(name, KeyStyle.Pascal).ShouldBe(pascal);
        KeyNaming.Apply(name, KeyStyle.Camel).ShouldBe(camel);
        KeyNaming.Apply(name, KeyStyle.Dash).ShouldBe(dash);
        return Task.CompletedTask;
    }

    private static string[] Mismatches(IEnumerable<string> names, Func<string, string> actual, JsonNamingPolicy expected) =>
    [
        .. names
          .Where(z => actual(z) != expected.ConvertName(z))
          .Take(20)
          .Select(z => $"{Escape(z)}: expected {Escape(expected.ConvertName(z))} but was {Escape(actual(z))}"),
    ];

    private static string Escape(string value) => "\"" + string.Concat(value.Select(z => z is >= ' ' and < '\u007f' ? z.ToString() : $"\\u{(int)z:x4}")) + "\"";
}
