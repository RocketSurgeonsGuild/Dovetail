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
    public async Task With_Several_Joints_The_Failure_For_A_Required_Parameter_Is_The_Inner_Exception_Of_The_One_From_Ordering_Them()
    {
        // With more than one joint the context orders them with a comparer, which reads their Joint, and Array.Sort wraps whatever
        // the comparer throws. The exception that names the type, the parameter and the joint is the inner one.
        var result = await Generate(
            """
            [DovetailExport]
            public class Needy : ConstructedJoint
            {
                public Needy(IWidget widget) : base(widget, null, "label", 0, WidgetMode.Slow, null) { }
            }

            [DovetailExport]
            public class Plain : ConstructedJoint
            {
                public Plain() : base(null, null, "label", 0, WidgetMode.Slow, null) { }
            }
            """
        );

        var builder = DovetailContextBuilder.Create(Rivet(result), new Dictionary<object, object>(), []);
        var create = () => builder.CreateAsync(TestContext.CancellationToken).AsTask();

        var exception = await create.ShouldThrowAsync<InvalidOperationException>();
        exception.Message.ShouldNotContain(typeof(IWidget).FullName!);
        var inner = exception.InnerException.ShouldBeOfType<InvalidOperationException>();
        inner.Message.ShouldContain(typeof(IWidget).FullName!);
        inner.Message.ShouldContain("widget");
        inner.Message.ShouldContain("Sample.Needy");
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
    [Arguments("protected")]
    [Arguments("private")]
    [Arguments("private protected")]
    public async Task A_Joint_Whose_Only_Constructor_Cannot_Be_Called_Is_Reported_And_Left_Out(string modifier)
    {
        var result = await Generate(
            $$"""
              [DovetailExport]
              public class Constructed : ConstructedJoint
              {
                  {{modifier}} Constructed() : base(null, null, "label", 0, WidgetMode.Slow, null) { }
              }
              """
        );

        // Reported once, on the joint, and nothing else: the joint is left out of the exports instead of generating code that
        // can't compile, and an exports class with no joint left in it is still valid.
        var diagnostic = result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe("DT0017");
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Error);
        diagnostic.GetMessage().ShouldBe("Joint 'Sample.Constructed' has no public or internal constructor, so it cannot be created");
        var location = diagnostic.Location;
        location.IsInSource.ShouldBeTrue();
        location.SourceTree!.GetText().ToString(location.SourceSpan).ShouldBe("Constructed");
        location.SourceTree.GetText().Lines[location.GetLineSpan().StartLinePosition.Line].ToString().Trim().ShouldBe("public class Constructed : ConstructedJoint");

        CompileErrors(result).Where(z => z.Id != "DT0017").ShouldBeEmpty();
        ExportedSource(result).ShouldNotContain("Constructed");
    }

    [Test]
    [Arguments("public", "private")]
    [Arguments("internal", "protected")]
    [Arguments("protected internal", "private protected")]
    public async Task A_Constructor_It_Can_Be_Created_With_Next_To_One_It_Cannot_Is_Not_Reported(string callable, string uncallable)
    {
        var result = await Generate(
            $$"""
              [DovetailExport]
              public class Constructed : ConstructedJoint
              {
                  {{callable}} Constructed(WidgetOptions? options = null) : base(null, options, "label", 0, WidgetMode.Slow, null) { }
                  {{uncallable}} Constructed(int other) : base(null, null, "label", other, WidgetMode.Slow, null) { }
              }
              """
        );

        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.ShouldBeEmpty();
        CompileErrors(result).ShouldBeEmpty();
        var options = new WidgetOptions();
        var joint = await CreateJoint(DovetailContextBuilder.Create(Rivet(result), new Dictionary<object, object>(), []).Set(options));
        joint.Arguments[1].ShouldBeSameAs(options);
    }

    [Test]
    public async Task A_Static_Constructor_Does_Not_Count_As_A_Second_Constructor()
    {
        var result = await Generate(
            """
            [DovetailExport]
            public class Constructed : ConstructedJoint
            {
                static Constructed() { }

                public Constructed() : base(null, null, "label", 0, WidgetMode.Slow, null) { }
            }
            """
        );

        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.ShouldBeEmpty();
        CompileErrors(result).ShouldBeEmpty();
        Rivet(result).Single().Joint.ShouldBeOfType(result.Assembly!.GetType("Sample.Constructed")!);
    }

    [Test]
    [Arguments("public", "internal")]
    [Arguments("internal", "protected internal")]
    public async Task Two_Constructors_It_Can_Be_Created_With_Are_Still_Reported_As_DT0001(string first, string second)
    {
        var result = await Generate(
            $$"""
              [DovetailExport]
              public class Constructed : ConstructedJoint
              {
                  {{first}} Constructed() : base(null, null, "label", 0, WidgetMode.Slow, null) { }
                  {{second}} Constructed(int other) : base(null, null, "label", other, WidgetMode.Slow, null) { }
              }
              """
        );

        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.ShouldHaveSingleItem().Id.ShouldBe("DT0001");
        CompileErrors(result).Where(z => z.Id != "DT0001").ShouldBeEmpty();
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
