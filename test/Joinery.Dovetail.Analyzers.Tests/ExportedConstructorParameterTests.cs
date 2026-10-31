using Joinery.Dovetail.Infrastructure;
using Microsoft.CodeAnalysis;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
/// An exported joint can take constructor parameters (LaunchPad's <c>TimeConvention(PrimitiveOptions? options = null)</c> is
/// one). <c>Exports.Rivet()</c> has no parameters and no builder, so the generated code must not look anything up there: it
/// hands the metadata a factory and the context builder resolves the parameters from its properties when the joint is
/// created. These tests compile what the generator emits and run it, the same way <see cref="DovetailContextBuilder.AppendJoint{T}" />
/// resolves a joint at runtime.
/// </summary>
public class ExportedConstructorParameterTests() : GeneratorTest()
{
    private const string RequiredAndOptionalParameters = """
        [DovetailExport]
        public class Constructed : ConstructedJoint
        {
            public Constructed(IWidget widget, WidgetOptions? options = null, string label = "fallback", int retries = 3, WidgetMode mode = WidgetMode.Slow, int? limit = null)
                : base(widget, options, label, retries, mode, limit) { }
        }
        """;

    [Test]
    public async Task Generates_Code_That_Compiles_For_Required_And_Optional_Constructor_Parameters()
    {
        var result = await Generate(RequiredAndOptionalParameters);

        CompileErrors(result).ShouldBeEmpty();
        await Verify(ExportedSource(result), "cs");
    }

    [Test]
    public async Task Resolves_The_Parameters_From_The_Context_When_The_Joint_Is_Created()
    {
        var result = await Generate(RequiredAndOptionalParameters);
        var widget = new Widget();
        var options = new WidgetOptions();

        // Both are set after the metadata exists, so the joint can only see them if it is created from the context's properties.
        var builder = DovetailContextBuilder.Create(Rivet(result), new Dictionary<object, object>(), []);
        builder.Set<IWidget>(widget).Set(options);
        var joint = await CreateJoint(builder);

        joint.Arguments.ShouldBe([widget, options, "fallback", 3, WidgetMode.Slow, null]);
    }

    [Test]
    public async Task Resolves_The_Parameters_From_The_Properties_The_Builder_Starts_With()
    {
        var result = await Generate(RequiredAndOptionalParameters);
        var widget = new Widget();

        var properties = new Dictionary<object, object>
        {
            [typeof(IWidget)] = widget,
            [typeof(string)] = "supplied",
            [typeof(int)] = 8,
            [typeof(WidgetMode)] = WidgetMode.Fast,
            // The key is the parameter's declared type, so int? is not int.
            [typeof(int?)] = 13,
        };
        var joint = await CreateJoint(DovetailContextBuilder.Create(Rivet(result), properties, []));

        joint.Arguments.ShouldBe([widget, null, "supplied", 8, WidgetMode.Fast, 13]);
    }

    [Test]
    public async Task An_Optional_Parameter_Falls_Back_To_Its_Default_When_The_Context_Has_No_Value()
    {
        var result = await Generate(RequiredAndOptionalParameters);
        var widget = new Widget();

        var builder = DovetailContextBuilder.Create(Rivet(result), new Dictionary<object, object>(), []).Set<IWidget>(widget);
        var joint = await CreateJoint(builder);

        joint.Arguments.ShouldBe([widget, null, "fallback", 3, WidgetMode.Slow, null]);
    }

    [Test]
    public async Task A_Required_Parameter_Without_A_Value_Fails_With_A_Message_That_Names_It()
    {
        var result = await Generate(RequiredAndOptionalParameters);

        var builder = DovetailContextBuilder.Create(Rivet(result), new Dictionary<object, object>(), []);
        var create = () => builder.CreateAsync(TestContext.CancellationToken).AsTask();

        var exception = await create.ShouldThrowAsync<InvalidOperationException>();
        exception.Message.ShouldContain(typeof(IWidget).FullName!);
        exception.Message.ShouldContain("widget");
        exception.Message.ShouldContain("Constructed");
    }

    [Test]
    public async Task A_Joint_Read_Outside_A_Context_Builder_Uses_Defaults_And_Fails_For_Required_Parameters()
    {
        var onlyOptional = await Generate(
            """
            [DovetailExport]
            public class Constructed : ConstructedJoint
            {
                public Constructed(WidgetOptions? options = null, int retries = 3) : base(null, options, "label", retries, WidgetMode.Slow, null) { }
            }
            """
        );
        var required = await Generate(RequiredAndOptionalParameters);

        ( (ConstructedJoint)Rivet(onlyOptional).Single().Joint ).Arguments.ShouldBe([null, null, "label", 3, WidgetMode.Slow, null]);
        Should.Throw<InvalidOperationException>(() => Rivet(required).Single().Joint);
    }

    [Test]
    public async Task Keeps_The_Declared_Default_Of_Every_Kind_Of_Optional_Parameter()
    {
        var result = await Generate(
            """
            [DovetailExport]
            public class Defaulted : IDovetailJoint
            {
                public Defaulted(
                    byte b = 3, sbyte sb = -4, short s = 5, ushort us = 6, uint ui = 7, long l = -8, ulong ul = 9,
                    float f = 1.5f, double d = 0.25, decimal m = 2.5m, char c = 'x', bool flag = true,
                    string text = "a \"quoted\" \\ value\n", string? nothing = null,
                    WidgetMode mode = WidgetMode.Fast, WidgetMode? maybeMode = WidgetMode.Fast, int? maybeNothing = null, int? maybeSome = 11,
                    WidgetMarker marker = default, WidgetOptions? options = null
                )
                {
                    Arguments = [b, sb, s, us, ui, l, ul, f, d, m, c, flag, text, nothing, mode, maybeMode, maybeNothing, maybeSome, marker, options];
                }

                public object?[] Arguments { get; }
            }
            """
        );

        CompileErrors(result).ShouldBeEmpty();
        var joint = Rivet(result).Single().Joint;
        joint.GetType().GetProperty("Arguments")!.GetValue(joint).ShouldBe(
            new object?[]
            {
                (byte)3, (sbyte)-4, (short)5, (ushort)6, 7u, -8L, 9UL, 1.5f, 0.25d, 2.5m, 'x', true, "a \"quoted\" \\ value\n", null,
                WidgetMode.Fast, WidgetMode.Fast, null, 11, default(WidgetMarker), null,
            }
        );
    }

    [Test]
    public async Task A_Joint_Without_Constructor_Parameters_Is_Still_Created_Directly()
    {
        var result = await Generate(
            """
            [DovetailExport]
            public class Constructed : ConstructedJoint
            {
                public Constructed() : base(null, null, "label", 0, WidgetMode.Slow, null) { }
            }
            """
        );

        CompileErrors(result).ShouldBeEmpty();
        ExportedSource(result).ShouldContain("new DovetailJointMetadata(new Sample.Constructed(), ");
    }

    [Test]
    public async Task A_Joint_With_A_Constructor_That_Is_Not_Public_Compiles()
    {
        var result = await Generate(
            """
            [DovetailExport]
            public class Constructed : ConstructedJoint
            {
                protected Constructed() : base(null, null, "label", 0, WidgetMode.Slow, null) { }
            }
            """
        );

        CompileErrors(result).ShouldBeEmpty();
    }

    private async Task<GeneratorTestResults> Generate(string declarations) =>
        await Builder
             .AddReferences(typeof(ConstructedJoint))
             .AddSources(
                  $$"""
                    using Joinery.Dovetail;
                    using Joinery.Dovetail.Analyzers.Tests;
                    using Joinery.Dovetail.Attributes;
                    using Joinery.Dovetail.Infrastructure;

                    namespace Sample
                    {
                        {{declarations}}
                    }
                    """
              )
             .Build()
             .GenerateAsync(TestContext.CancellationToken);

    private static IEnumerable<Diagnostic> CompileErrors(GeneratorTestResults result) => result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error);

    private static string ExportedSource(GeneratorTestResults result) =>
        result
           .Results[typeof(DovetailAttributesGenerator)]
           .SyntaxTrees
           .Single(z => z.FilePath.EndsWith("Exported_Dovetails.g.cs", StringComparison.Ordinal))
           .ToString();

    /// <summary>
    /// The metadata <c>Exports.Rivet()</c> returns from the assembly the generator output was compiled into.
    /// </summary>
    private static IEnumerable<IDovetailJointMetadata> Rivet(GeneratorTestResults result)
    {
        var assembly = result.Assembly;
        assembly.ShouldNotBeNull(string.Join(Environment.NewLine, CompileErrors(result)));

        return (IEnumerable<IDovetailJointMetadata>)assembly.GetType("Exports")!.GetMethod("Rivet")!.Invoke(null, null)!;
    }

    private async Task<ConstructedJoint> CreateJoint(DovetailContextBuilder builder)
    {
        var context = await builder.CreateAsync(TestContext.CancellationToken);
        return context.Joints.OfType<ConstructedJoint>().Single();
    }
}

public interface IWidget;

public sealed class Widget : IWidget;

public sealed class WidgetOptions;

public readonly struct WidgetMarker;

public enum WidgetMode
{
    Slow = 0,
    Fast = 1,
}

/// <summary>
/// Remembers what the generated code handed its constructor, which the compiled test joints derive from.
/// </summary>
public abstract class ConstructedJoint(IWidget? widget, WidgetOptions? options, string label, int retries, WidgetMode mode, int? limit) : IDovetailJoint
{
    public object?[] Arguments { get; } = [widget, options, label, retries, mode, limit];
}
