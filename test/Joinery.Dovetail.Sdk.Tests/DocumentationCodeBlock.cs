using System.Text.RegularExpressions;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// A C# code block of a page of the docs site (<c>docs/src/content/docs</c>), read straight from the markdown so that the tests that
/// check the examples see what a reader sees.
/// </summary>
/// <param name="Path">The page, relative to the repository root, with forward slashes.</param>
/// <param name="Line">The line of the page the first line of <paramref name="Code" /> is on, counting from 1.</param>
/// <param name="Code">The lines between the fences, as they are written.</param>
internal sealed partial record DocumentationCodeBlock(string Path, int Line, string Code)
{
    /// <summary>
    /// Every page of the docs site, with its path relative to the repository root (forward slashes) and its lines.
    /// </summary>
    public static IEnumerable<(string Path, string[] Lines)> Pages()
    {
        var docs = System.IO.Path.Combine(Config.RootDirectory, "docs", "src", "content", "docs");
        foreach (var page in Directory.EnumerateFiles(docs, "*.md*", SearchOption.AllDirectories).Order(StringComparer.Ordinal))
        {
            yield return (System.IO.Path.GetRelativePath(Config.RootDirectory, page).Replace('\\', '/'), File.ReadAllLines(page));
        }
    }

    /// <summary>
    /// Every <c>c#</c>, <c>csharp</c> and <c>cs</c> code block on the docs site.
    /// </summary>
    public static IReadOnlyList<DocumentationCodeBlock> All()
    {
        var blocks = new List<DocumentationCodeBlock>();
        foreach (var (relativePath, lines) in Pages())
        {
            for (var i = 0; i < lines.Length; i++)
            {
                if (!OpeningFence().IsMatch(lines[i]))
                {
                    continue;
                }

                var end = i + 1;
                while (end < lines.Length && !ClosingFence().IsMatch(lines[end]))
                {
                    end++;
                }

                // Line numbers count from 1, and the code starts on the line after the fence.
                blocks.Add(new(relativePath, i + 2, string.Join('\n', lines[( i + 1 )..end])));
                i = end;
            }
        }

        return blocks;
    }

    /// <summary>
    /// The block declares a class, struct or record that implements a joint interface (<c>IServiceJoint</c>, <c>ISerilogAsyncJoint</c>,
    /// <c>IHostCreatedJoint&lt;THost&gt;</c>), which is what a reader copies to write a joint.
    /// </summary>
    public bool ImplementsAJoint => JointImplementation().IsMatch(Code);

    /// <summary>
    /// The block declares a joint interface, such as the generated code the reference page abridges.
    /// </summary>
    public bool DeclaresAJointInterface => JointInterface().IsMatch(Code);

    /// <summary>
    /// The block declares or calls a method called <c>Register</c>, the member every joint interface has.
    /// </summary>
    public bool HasRegister => RegisterMethod().IsMatch(Code);

    /// <summary>
    /// The block is an excerpt of a file of the repository when its first line is a comment with the path of that file, such as
    /// <c>// sample/Sample.Core/Databases/DatabaseServiceDovetail.cs</c>; this is that path, relative to the repository root. The
    /// project the file belongs to builds, so the excerpt does not need compiling again, only to be a copy.
    /// </summary>
    public string? ExcerptOf => ExcerptComment().Match(Code) is { Success: true } match ? match.Groups["path"].Value : null;

    /// <summary>
    /// The block starts with <c>using</c> directives of its own, so it reads as a complete file. One without them is a fragment, and
    /// leaves its usings to the reader.
    /// </summary>
    public bool HasUsings => UsingDirective().IsMatch(Code);

    /// <summary>
    /// Names the block in a message, in the form an editor and MSBuild link to.
    /// </summary>
    public override string ToString() => $"{Path}({Line})";

    [GeneratedRegex(@"^\s*```(?:c#|csharp|cs)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex OpeningFence();

    [GeneratedRegex(@"^\s*```\s*$", RegexOptions.CultureInvariant)]
    private static partial Regex ClosingFence();

    // The base list may follow a primary constructor, whose parameters can have defaults (and so equals signs).
    [GeneratedRegex(@"\b(?:class|struct|record)\s+\w+\s*(?:\([^)]*\))?\s*:[^{;]*\bI\w*Joint\b", RegexOptions.Singleline | RegexOptions.CultureInvariant)]
    private static partial Regex JointImplementation();

    [GeneratedRegex(@"\binterface\s+I\w*Joint\b", RegexOptions.CultureInvariant)]
    private static partial Regex JointInterface();

    [GeneratedRegex(@"\bRegister\s*\(", RegexOptions.CultureInvariant)]
    private static partial Regex RegisterMethod();

    [GeneratedRegex(@"\A\s*//\s*(?<path>(?:src|sample|test)/\S+\.cs)\s*(?:\r?\n|\z)", RegexOptions.CultureInvariant)]
    private static partial Regex ExcerptComment();

    [GeneratedRegex(@"^\s*using\s+[\w.]+\s*;", RegexOptions.Multiline | RegexOptions.CultureInvariant)]
    private static partial Regex UsingDirective();
}
