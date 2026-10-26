using System.Text.RegularExpressions;

namespace Dovetail.Build.Tasks.Tests;

/// <summary>
///     The case-insensitive key patterns: ECMA-262 character classes, with regex syntax characters escaped.
/// </summary>
public class CaseInsensitivePatternTests
{
    [Test]
    public void Letters_BecomeCharacterClasses_AndSyntaxCharactersAreEscaped()
    {
        CaseInsensitivePattern.For("Max_2").ShouldBe("^(?:[Mm][Aa][Xx]_2)$");
        CaseInsensitivePattern.For("a.b(c)").ShouldBe(@"^(?:[Aa]\.[Bb]\([Cc]\))$");
        CaseInsensitivePattern.For(["Url", "Retry-After"]).ShouldBe("^(?:[Uu][Rr][Ll]|[Rr][Ee][Tt][Rr][Yy]-[Aa][Ff][Tt][Ee][Rr])$");
        CaseInsensitivePattern.For([]).ShouldBe("^(?:)$");
    }

    [Test]
    [Arguments("a.b(c)", "A.B(C)", true)]
    [Arguments("a.b(c)", "aXb(c)", false)]
    [Arguments("Retry-After", "retry-after", true)]
    [Arguments("x+y*", "X+Y*", true)]
    [Arguments("x+y*", "xxyy", false)]
    [Arguments("Größe", "GRÖSSE", false)]
    [Arguments("Größe", "GRÖßE", true)]
    public void Patterns_MatchTheirNameInAnyCase_AndNothingElse(string name, string key, bool matches) =>
        new Regex(CaseInsensitivePattern.For(name), RegexOptions.ECMAScript).IsMatch(key).ShouldBe(matches);

    [Test]
    public void TheLookup_FindsTheKeyATwinPatternBelongsTo()
    {
        var lookup = new CaseInsensitivePattern.NameLookup(["Message", "Count"]);

        lookup.Find("^(?:[Cc][Oo][Uu][Nn][Tt])$").ShouldBe("Count");
        lookup.Find("^[Mm]essage$").ShouldBe("Message");
        lookup.Find("^(?:[Nn]ame)$").ShouldBeNull();
        lookup.Find("(unbalanced").ShouldBeNull();
    }
}
