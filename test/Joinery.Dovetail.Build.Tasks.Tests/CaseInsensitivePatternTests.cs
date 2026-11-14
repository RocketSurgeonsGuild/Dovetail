using System.Text.RegularExpressions;

namespace Joinery.Dovetail.Build.Tasks.Tests;

/// <summary>
///     The case-insensitive key patterns: ECMA-262 character classes, with regex syntax characters escaped.
/// </summary>
public class CaseInsensitivePatternTests
{
    [Test]
    public async Task Letters_BecomeCharacterClasses_AndSyntaxCharactersAreEscaped()
    {
        await CaseInsensitivePattern.For("Max_2").Should().BeEqualTo("^(?:[Mm][Aa][Xx]_2)$");
        await CaseInsensitivePattern.For("a.b(c)").Should().BeEqualTo(@"^(?:[Aa]\.[Bb]\([Cc]\))$");
        await CaseInsensitivePattern.For(["Url", "Retry-After"]).Should().BeEqualTo("^(?:[Uu][Rr][Ll]|[Rr][Ee][Tt][Rr][Yy]-[Aa][Ff][Tt][Ee][Rr])$");
        await CaseInsensitivePattern.For([]).Should().BeEqualTo("^(?:)$");
    }

    [Test]
    [Arguments("a.b(c)", "A.B(C)", true)]
    [Arguments("a.b(c)", "aXb(c)", false)]
    [Arguments("Retry-After", "retry-after", true)]
    [Arguments("x+y*", "X+Y*", true)]
    [Arguments("x+y*", "xxyy", false)]
    [Arguments("Größe", "GRÖSSE", false)]
    [Arguments("Größe", "GRÖßE", true)]
    public async Task Patterns_MatchTheirNameInAnyCase_AndNothingElse(string name, string key, bool matches) =>
        await new Regex(CaseInsensitivePattern.For(name), RegexOptions.ECMAScript).IsMatch(key).Should().BeEqualTo(matches);

    [Test]
    public async Task TheLookup_FindsTheKeyATwinPatternBelongsTo()
    {
        var lookup = new CaseInsensitivePattern.NameLookup(["Message", "Count"]);

        await lookup.Find("^(?:[Cc][Oo][Uu][Nn][Tt])$").Should().BeEqualTo("Count");
        await lookup.Find("^[Mm]essage$").Should().BeEqualTo("Message");
        await lookup.Find("^(?:[Nn]ame)$").Should().BeNull();
        await lookup.Find("(unbalanced").Should().BeNull();
    }
}
