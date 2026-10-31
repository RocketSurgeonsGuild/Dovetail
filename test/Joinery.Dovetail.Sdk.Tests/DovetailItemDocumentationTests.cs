using System.Text.RegularExpressions;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// The comment at the top of <c>Joinery.Dovetail.Joints.targets</c>, <c>Joinery.Dovetail.Hosts.targets</c> and
/// <c>Joinery.Dovetail.Templates.targets</c> and the metadata table of the matching reference page both list the metadata the targets
/// read. They drifted from the targets before (a default that was never applied, values nobody listed); this keeps all three saying the
/// same thing. The defaults themselves are checked where they take effect, by <see cref="DovetailItemReferenceTests" />.
/// </summary>
public partial class DovetailItemDocumentationTests
{
    /// <summary>
    /// Lists the metadata a target reads, the metadata its header comment lists and the rows of its page's table, and compares them.
    /// </summary>
    /// <param name="items">
    /// The item types the targets read the metadata of, separated by semicolons: <c>DovetailTemplate</c> is read where templates are
    /// resolved, and again, as <c>DovetailEmittedTemplate</c>, where they are written.
    /// </param>
    [Test]
    [Arguments("DovetailJoint", "src/Joinery.Dovetail/build/Joinery.Dovetail.Joints.targets", "docs/src/content/docs/reference/dovetail-joint.md")]
    [Arguments("DovetailHost", "src/Joinery.Dovetail/build/Joinery.Dovetail.Hosts.targets", "docs/src/content/docs/reference/dovetail-host.md")]
    [Arguments("DovetailTemplate;DovetailEmittedTemplate", "src/Joinery.Dovetail/build/Joinery.Dovetail.Templates.targets", "docs/src/content/docs/reference/dovetail-template.md")]
    public void HeaderCommentAndReferenceTable_ListExactlyTheMetadataTheTargetsRead(string items, string targetsPath, string pagePath)
    {
        var targets = File.ReadAllText(Path.Combine(Config.RootDirectory, targetsPath));

        // %(DovetailJoint.Name) is a read. A name with a leading underscore is the target's own scratch metadata and Identity is the item's name.
        var itemTypes = string.Join('|', items.Split(';').Select(Regex.Escape));
        var read = Regex.Matches(targets, $@"%\((?:{itemTypes})\.(?<name>[A-Za-z]\w*)\)")
                        .Select(z => z.Groups["name"].Value)
                        .Where(z => z != "Identity")
                        .Distinct()
                        .Order(StringComparer.Ordinal)
                        .ToArray();

        // Each metadata line of the first comment in the file: six spaces, the name (or names, separated by commas), then a dash.
        var header = Regex.Match(targets, "<!--(?<comment>.*?)-->", RegexOptions.Singleline).Groups["comment"].Value;
        var documentedInHeader = HeaderEntry()
                                .Matches(header)
                                .SelectMany(z => z.Groups["names"].Value.Split(", "))
                                .Distinct()
                                .Order(StringComparer.Ordinal)
                                .ToArray();

        // Each row of the table under the page's "Metadata" heading starts with the name in backticks.
        var page = File.ReadAllText(Path.Combine(Config.RootDirectory, pagePath));
        var table = Regex.Match(page, @"^## Metadata\s*$(?<table>.*?)(?=^## )", RegexOptions.Multiline | RegexOptions.Singleline).Groups["table"].Value;
        var documentedInTable = TableRow().Matches(table).Select(z => z.Groups["name"].Value).Order(StringComparer.Ordinal).ToArray();

        read.ShouldNotBeEmpty();
        documentedInHeader.ShouldBe(read, $"the comment at the top of {targetsPath}");
        documentedInTable.ShouldBe(read, $"the metadata table of {pagePath}");
    }

    [GeneratedRegex(@"^ {6}(?<names>[A-Za-z]\w*(?:, [A-Za-z]\w*)*)\s+[-—]", RegexOptions.Multiline)]
    private static partial Regex HeaderEntry();

    [GeneratedRegex(@"^\|\s*`(?<name>[A-Za-z]\w*)`", RegexOptions.Multiline)]
    private static partial Regex TableRow();
}
