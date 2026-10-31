using System.Text.RegularExpressions;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Every <c>Register</c> on the docs site has to match an interface the packages generate, and this is where each C# code block that
/// mentions one is accounted for: a block that implements a joint is compiled by <see cref="DocumentationExampleTests" />, and the two
/// other kinds are checked against the code they show. A block with a <c>Register</c> that is none of the three fails the build, so a
/// new example cannot go unchecked.
/// </summary>
public partial class DocumentationExcerptTests
{
    /// <summary>
    /// A block that mentions <c>Register</c> is an implementation of a joint (compiled), an excerpt of a file of the repository, or the
    /// generated interface. Anything else is a fragment the other tests cannot see, which is how a wrong signature stays in the docs.
    /// </summary>
    [Test]
    public void EveryRegisterOnTheDocsSite_IsCompiledOrCopiedFromCodeThatBuilds()
    {
        var unaccounted = DocumentationCodeBlock
                        .All()
                        .Where(z => z.HasRegister && !z.ImplementsAJoint && z.ExcerptOf is null && !z.DeclaresAJointInterface)
                        .Select(z => z.ToString())
                        .ToArray();

        unaccounted.ShouldBeEmpty(
            "each of these blocks has a Register that no test checks: make it a complete class that implements a joint interface, begin it with a comment "
          + "naming the file it is copied from (// sample/..., // src/... or // test/...), or declare the joint interface that the build generates"
        );
    }

    /// <summary>
    /// An excerpt is a copy: each of its lines is a line of the file it names, which belongs to a project that builds. The first line,
    /// the comment that names the file, is not part of the file.
    /// </summary>
    [Test]
    public void EveryExcerptOnTheDocsSite_IsMadeOfLinesOfTheFileItNames()
    {
        var excerpts = DocumentationCodeBlock.All().Where(z => z.HasRegister && z.ExcerptOf is not null).ToArray();
        excerpts.ShouldNotBeEmpty("the dovetail-joint reference page excerpts DatabaseServiceJoint from the sample");

        foreach (var excerpt in excerpts)
        {
            var file = Path.Combine(Config.RootDirectory, excerpt.ExcerptOf!);
            File.Exists(file).ShouldBeTrue($"{excerpt} names {excerpt.ExcerptOf}, which does not exist");

            var linesOfTheFile = File.ReadAllLines(file).Select(Normalize).ToHashSet(StringComparer.Ordinal);
            var missing = excerpt
                         .Code.Split('\n')
                         .Skip(1)
                         .Select(Normalize)
                         .Where(z => z.Length > 0 && !linesOfTheFile.Contains(z))
                         .ToArray();

            missing.ShouldBeEmpty($"{excerpt} is not a copy of {excerpt.ExcerptOf}: these lines are not in the file");
        }
    }

    /// <summary>
    /// The reference page shows what a <c>DovetailJoint</c> generates, abridged, so every <c>Register</c> it declares is a line of the
    /// generated code. The generated files are in the <c>obj/</c> directories of <c>src/</c>, written when the projects were built
    /// (this project builds the Serilog one, and packs them all).
    /// </summary>
    [Test]
    public void EveryGeneratedInterfaceOnTheDocsSite_IsLineForLineWhatTheBuildGenerates()
    {
        var shown = DocumentationCodeBlock.All().Where(z => z.DeclaresAJointInterface).ToArray();
        shown.ShouldNotBeEmpty("the dovetail-joint reference page shows the interface the Serilog declaration generates");

        var generated = Directory
                       .EnumerateFiles(Path.Combine(Config.RootDirectory, "src"), "DovetailJoint*.g.cs", SearchOption.AllDirectories)
                       .Where(z => z.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
                       .SelectMany(File.ReadAllLines)
                       .Select(Normalize)
                       .ToHashSet(StringComparer.Ordinal);
        generated.ShouldNotBeEmpty("the source projects have not been built, so there is no generated code to compare with");

        foreach (var block in shown)
        {
            var registers = block.Code.Split('\n').Select(Normalize).Where(z => z.Contains("Register(", StringComparison.Ordinal)).ToArray();
            registers.ShouldNotBeEmpty($"{block} declares a joint interface without a Register");
            registers.Where(z => !generated.Contains(z)).ShouldBeEmpty($"{block} shows Register declarations that the build does not generate");
        }
    }

    /// <summary>
    /// Compares lines without regard to the indentation and the spacing between tokens.
    /// </summary>
    private static string Normalize(string line) => Whitespace().Replace(line, " ").Trim();

    [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant)]
    private static partial Regex Whitespace();
}
