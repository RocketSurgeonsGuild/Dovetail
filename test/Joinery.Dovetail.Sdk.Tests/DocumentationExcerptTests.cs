using System.Reflection;
using System.Text.RegularExpressions;

namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Every <c>Register</c> on the docs site has to match an interface the packages generate, and this is where each C# code block that
/// mentions one is accounted for: a block that implements a joint is compiled by <see cref="DocumentationExampleTests" />, and the two
/// other kinds are checked against the code they show. A block with a <c>Register</c> that is none of the three fails the build, so a
/// new example cannot go unchecked. The table of joint kinds is compared with the interfaces by reflection.
/// </summary>
public partial class DocumentationExcerptTests
{
    /// <summary>
    /// A block that mentions <c>Register</c> is an implementation of a joint (compiled), an excerpt of a file of the repository, or the
    /// generated interface. Anything else is a fragment the other tests cannot see, which is how a wrong signature stays in the docs.
    /// </summary>
    [Test]
    public async Task EveryRegisterOnTheDocsSite_IsCompiledOrCopiedFromCodeThatBuilds()
    {
        var unaccounted = DocumentationCodeBlock
                        .All()
                        .Where(z => z.HasRegister && !z.ImplementsAJoint && z.ExcerptOf is null && !z.DeclaresAJointInterface)
                        .Select(z => z.ToString())
                        .ToArray();

        await unaccounted.Should().BeEmpty().Because("each of these blocks has a Register that no test checks: make it a complete class that implements a joint interface, begin it with a comment "
          + "naming the file it is copied from (// sample/..., // src/... or // test/...), or declare the joint interface that the build generates");
    }

    /// <summary>
    /// An excerpt is a copy: each of its lines is a line of the file it names, which belongs to a project that builds. The first line,
    /// the comment that names the file, is not part of the file.
    /// </summary>
    [Test]
    public async Task EveryExcerptOnTheDocsSite_IsMadeOfLinesOfTheFileItNames()
    {
        var excerpts = DocumentationCodeBlock.All().Where(z => z.HasRegister && z.ExcerptOf is not null).ToArray();
        await excerpts.Should().NotBeEmpty().Because("the dovetail-joint reference page excerpts DatabaseServiceJoint from the sample");

        foreach (var excerpt in excerpts)
        {
            var file = Path.Combine(Config.RootDirectory, excerpt.ExcerptOf!);
            await File.Exists(file).Should().BeTrue().Because($"{excerpt} names {excerpt.ExcerptOf}, which does not exist");

            var linesOfTheFile = File.ReadAllLines(file).Select(Normalize).ToHashSet(StringComparer.Ordinal);
            var missing = excerpt
                         .Code.Split('\n')
                         .Skip(1)
                         .Select(Normalize)
                         .Where(z => z.Length > 0 && !linesOfTheFile.Contains(z))
                         .ToArray();

            await missing.Should().BeEmpty().Because($"{excerpt} is not a copy of {excerpt.ExcerptOf}: these lines are not in the file");
        }
    }

    /// <summary>
    /// The reference page shows what a <c>DovetailJoint</c> generates, abridged, so every <c>Register</c> it declares is a line of the
    /// generated code. The generated files are in the <c>obj/</c> directories of <c>src/</c>, written when the projects were built
    /// (this project builds the Serilog one, and packs them all).
    /// </summary>
    [Test]
    public async Task EveryGeneratedInterfaceOnTheDocsSite_IsLineForLineWhatTheBuildGenerates()
    {
        var shown = DocumentationCodeBlock.All().Where(z => z.DeclaresAJointInterface).ToArray();
        await shown.Should().NotBeEmpty().Because("the dovetail-joint reference page shows the interface the Serilog declaration generates");

        var generated = Directory
                       .EnumerateFiles(Path.Combine(Config.RootDirectory, "src"), "DovetailJoint*.g.cs", SearchOption.AllDirectories)
                       .Where(z => z.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
                       .SelectMany(File.ReadAllLines)
                       .Select(Normalize)
                       .ToHashSet(StringComparer.Ordinal);
        await generated.Should().NotBeEmpty().Because("the source projects have not been built, so there is no generated code to compare with");

        foreach (var block in shown)
        {
            var registers = block.Code.Split('\n').Select(Normalize).Where(z => z.Contains("Register(", StringComparison.Ordinal)).ToArray();
            await registers.Should().NotBeEmpty().Because($"{block} declares a joint interface without a Register");
            await registers.Where(z => !generated.Contains(z)).Should().BeEmpty().Because($"{block} shows Register declarations that the build does not generate");
        }
    }

    /// <summary>
    /// The concepts page that explains the joint kinds lists what <c>Register</c> takes for each of them in a table, with the interface
    /// in one column and the signature in the next. This project references Joinery.Dovetail, so the rows are compared with the
    /// interfaces themselves, and with their async twins, which the page says take a trailing <c>CancellationToken</c> with a default.
    /// </summary>
    [Test]
    public async Task EveryJointKindInATableOnTheDocsSite_HasTheRegisterTheTableShows()
    {
        var rows = DocumentationCodeBlock
                  .Pages()
                  .SelectMany(page => page.Lines.Select((line, index) => (page.Path, Line: index + 1, Row: KindRow().Match(line))))
                  .Where(z => z.Row.Success)
                  .ToArray();
        await rows.Should().NotBeEmpty().Because("defining-joints.md lists what Register takes for each kind of joint");

        var assembly = typeof(IDovetailContext).Assembly;
        foreach (var (path, line, row) in rows)
        {
            var arity = row.Groups["arguments"].Success ? row.Groups["arguments"].Value.Split(',').Length : 0;
            var suffix = arity > 0 ? $"`{arity}" : "";
            var name = row.Groups["interface"].Value;
            var documented = row.Groups["parameters"].Value;

            var joint = assembly.GetType($"Joinery.Dovetail.Joints.{name}{suffix}");
            await joint.Should().NotBeNull().Because($"{path}({line}) names {name}, which {assembly.GetName().Name} does not generate");
            await Parameters(RegisterOf(joint)).Should().BeEqualTo(documented).Because($"{path}({line}): {name}.Register");

            var asyncName = name.Replace("Joint", "AsyncJoint", StringComparison.Ordinal);
            var asyncJoint = assembly.GetType($"Joinery.Dovetail.Joints.{asyncName}{suffix}");
            await asyncJoint.Should().NotBeNull().Because($"{path}({line}) says {name} has an async twin, and there is no {asyncName}");
            var asyncRegister = RegisterOf(asyncJoint);
            await Parameters(asyncRegister).Should().BeEqualTo($"{documented}, CancellationToken cancellationToken").Because($"{path}({line}): {asyncName}.Register");
            await asyncRegister.ReturnType.Should().BeEqualTo(typeof(ValueTask)).Because($"{asyncName}.Register returns a ValueTask");
            await asyncRegister.GetParameters()[^1].HasDefaultValue.Should().BeTrue().Because($"the CancellationToken of {asyncName}.Register has a default");
        }
    }

    private static MethodInfo RegisterOf(Type joint) => joint.GetMethods().Single(z => z.Name == "Register");

    private static string Parameters(MethodInfo method) => string.Join(", ", method.GetParameters().Select(z => $"{z.ParameterType.Name} {z.Name}"));

    // | Setup | `ISetupJoint` | `Register(IDovetailContext context)` |, and IHostCreatedJoint<THost> for a generic kind.
    [GeneratedRegex(@"^\|[^|]*\|\s*`(?<interface>I\w+Joint)(?:<(?<arguments>[^>]*)>)?`\s*\|\s*`Register\((?<parameters>[^)]*)\)`\s*\|", RegexOptions.CultureInvariant)]
    private static partial Regex KindRow();

    /// <summary>
    /// Compares lines without regard to the indentation and the spacing between tokens.
    /// </summary>
    private static string Normalize(string line) => Whitespace().Replace(line, " ").Trim();

    [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant)]
    private static partial Regex Whitespace();
}
