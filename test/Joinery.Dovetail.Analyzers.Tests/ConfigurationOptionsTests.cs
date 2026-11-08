using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Options;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
///     The options registration <c>[DovetailConfiguration]</c> produces: a joint the assembly exports, and the reflection-free
///     binder it uses, generated with the rules of the configuration binding source generator version the compilation's
///     Microsoft.Extensions.Configuration.Binder reference asks for. Each snapshot shows the generated file and how the
///     exports list the joint; the generated code must compile.
/// </summary>
public class ConfigurationOptionsTests() : GeneratorTest()
{
    private const string Usings = """
        using System;
        using System.Collections.Generic;
        using Joinery.Dovetail.Attributes;
        using Microsoft.Extensions.Configuration;
        using Microsoft.Extensions.Options;

        """;

    private const string OptionsClass = Usings
      + """
        namespace Sample;

        public enum Mode { Off, On }

        [DovetailConfiguration]
        public class GreetingsOptions
        {
            public string Message { get; set; } = "Hello";
            public int Count { get; set; } = 1;
            public int? Limit { get; set; }
            public bool Enabled { get; set; } = true;
            public Mode Mode { get; set; }
            public TimeSpan Timeout { get; set; }
            public Uri? Endpoint { get; set; }
            public byte[]? Key { get; set; }
            public List<string> Names { get; } = [];
            public string[] Tags { get; set; } = [];
            public IEnumerable<int> Ports { get; set; } = [];
            public Dictionary<string, int> Weights { get; set; } = [];
            public IReadOnlyDictionary<Mode, string> Templates { get; set; } = new Dictionary<Mode, string>();
            public RetryOptions Retry { get; set; } = new();
            public List<RetryOptions> Fallbacks { get; set; } = [];
            public IConfigurationSection? Raw { get; set; }

            [ConfigurationKeyName("custom_key")]
            public string? Custom { get; set; }

            public int Total => Count * 2;
        }

        public class RetryOptions
        {
            public int Attempts { get; set; } = 3;
            public TimeSpan[] Delays { get; set; } = [];
        }
        """;

    private const string ImmutableTypes = Usings
      + """
        namespace Sample;

        [DovetailConfiguration]
        public record ServerOptions(string Host, int Port = 80, int? Weight = null)
        {
            public List<string> Aliases { get; init; } = [];
        }

        [DovetailConfiguration]
        public class InitOptions
        {
            public string Name { get; init; } = "fallback";
            public required int Size { get; set; }
            public List<string> Items { get; init; } = [];
            public string? Other { get; set; }
            public required Action? Callback { get; set; }
        }

        [DovetailConfiguration]
        public class CtorOptions
        {
            public CtorOptions(string name, int port)
            {
                Name = name;
                Port = port;
            }

            public string Name { get; }
            public int Port { get; }
            public string? Extra { get; set; }
        }
        """;

    [Test]
    [Arguments(10)]
    [Arguments(11)]
    public async Task Binds_an_options_class(int binder)
    {
        var result = await WithBinder(binder).AddSources(OptionsClass).Build().GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    [Arguments(10)]
    [Arguments(11)]
    public async Task Creates_records_and_init_only_types(int binder)
    {
        var result = await WithBinder(binder).AddSources(ImmutableTypes).Build().GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    [Arguments("Camel")]
    [Arguments("Dash")]
    [Arguments("Underscore")]
    public async Task Reads_the_keys_of_the_declared_key_style(string style)
    {
        var result = await WithBinder(11).AddSources(ConfigurationKeyStyleSources.Styled(style)).Build().GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Reads_declared_names_when_undefined_and_upper_cased_first_letters_for_pascal()
    {
        var result = await WithBinder(11).AddSources(ConfigurationKeyStyleSources.LowerCaseNames).Build().GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Keeps_styled_options_cached_when_unrelated_code_changes()
    {
        var result = await WithBinder(11).AddSources(ConfigurationKeyStyleSources.Styled("Dash")).Build().GenerateAsync(TestContext.CancellationToken);

        var compilation = result.InputCompilation;
        var parseOptions = (CSharpParseOptions)compilation.SyntaxTrees.First().Options;
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new DovetailAttributesGenerator().AsSourceGenerator()],
            parseOptions: parseOptions,
            driverOptions: new(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true)
        );
        driver = driver.RunGenerators(compilation, TestContext.CancellationToken);
        driver = driver.RunGenerators(
            compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText("namespace Sample; public class Unrelated { }", parseOptions, cancellationToken: TestContext.CancellationToken)),
            TestContext.CancellationToken
        );

        var steps = driver.GetRunResult().Results.Single().TrackedSteps;
        foreach (var name in new[] { "dovetail:configuration_classes", "dovetail:configuration_options", "dovetail:configuration_sections" })
        {
            steps[name].SelectMany(z => z.Outputs).Select(z => z.Reason).ShouldAllBe(z => z == IncrementalStepRunReason.Cached || z == IncrementalStepRunReason.Unchanged, name);
        }
    }

    [Test]
    [Arguments(10)]
    [Arguments(11)]
    public async Task Compiles_every_supported_shape(int binder)
    {
        var result = await WithBinder(binder)
                          .AddSources(
                               Usings
                             + """
                               using System.Globalization;

                               namespace Sample;

                               [Flags]
                               public enum Channels { None = 0, Console = 1, Email = 2 }

                               public struct Point { public int X { get; set; } public int Y { get; set; } }

                               public readonly record struct Size(int Width, int Height);

                               public record Endpoint(string Host, int Port = 80);

                               public class Labelled { public string Label { get; init; } = "none"; public required string Id { get; set; } }

                               public class Tree { public string? Name { get; set; } public Dictionary<string, Tree> Children { get; set; } = []; }

                               [DovetailConfiguration]
                               public class KitchenSinkOptions
                               {
                                   public sbyte A { get; set; }
                                   public short B { get; set; }
                                   public uint C { get; set; }
                                   public ulong D { get; set; }
                                   public float E { get; set; }
                                   public decimal F { get; set; }
                                   public char G { get; set; }
                                   public Half H { get; set; }
                                   public Int128 I { get; set; }
                                   public UInt128 J { get; set; }
                                   public DateTime K { get; set; }
                                   public DateTimeOffset L { get; set; }
                                   public DateOnly M { get; set; }
                                   public TimeOnly N { get; set; }
                                   public Guid O { get; set; }
                                   public Version? P { get; set; }
                                   public CultureInfo? Q { get; set; }
                                   public Channels R { get; set; }
                                   public Channels? S { get; set; }
                                   public object? T { get; set; }
                                   public Point U { get; set; }
                                   public Point? V { get; set; }
                                   public Size W { get; set; }
                                   public Endpoint? X { get; set; }
                                   public Labelled? Y { get; set; }
                                   public Tree Z { get; set; } = new();
                                   public int[][] Jagged { get; set; } = [];
                                   public List<int?> NullableInts { get; set; } = [];
                                   public HashSet<string> Set { get; set; } = [];
                                   public ISet<string> ISet { get; set; } = new HashSet<string>();
                                   public IReadOnlySet<string> ReadOnlySet { get; set; } = new HashSet<string>();
                                   public IList<Endpoint> Endpoints { get; set; } = [];
                                   public IReadOnlyList<Labelled> Labels { get; set; } = [];
                                   public IDictionary<string, string> Map { get; set; } = new Dictionary<string, string>();
                                   public Dictionary<int, Endpoint> ByPort { get; set; } = [];
                                   public Dictionary<Channels, Labelled> ByChannel { get; } = [];
                                   public Dictionary<string, List<string>> Groups { get; set; } = [];
                                   public SortedDictionary<string, int> Sorted { get; set; } = [];
                                   public IConfigurationSection? Raw { get; set; }
                                   public static int Shared { get; set; }
                                   public string? @class { get; set; }
                               }

                               [DovetailConfiguration]
                               public record ShapedRecord(List<string> Names, Endpoint Primary, Point Origin, Dictionary<string, int> Weights, string[] Tags, Size? Box = null);
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        result.Results[typeof(DovetailAttributesGenerator)].SyntaxTrees.Select(z => Path.GetFileName(z.FilePath)).ShouldContain("Dovetail_ConfigurationOptions.g.cs");
    }

    [Test]
    public async Task Registers_sections_in_order_with_validators()
    {
        var result = await WithBinder(11)
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration("Shared:B", ValidateOnStart = true)]
                               public class SharedOptions
                               {
                                   public string? Value { get; set; }
                               }

                               [DovetailConfiguration("Shared:A")]
                               public class FirstOptions
                               {
                                   public string? Value { get; set; }
                               }

                               [DovetailConfiguration("Shared:C", RegisterOptions = false)]
                               public class DescribedOnlyOptions
                               {
                                   public string? Value { get; set; }
                               }

                               [OptionsValidator]
                               public partial class SharedOptionsValidator : IValidateOptions<SharedOptions>
                               {
                                   public ValidateOptionsResult Validate(string? name, SharedOptions options) => ValidateOptionsResult.Success;
                               }

                               [OptionsValidator]
                               public partial class UnrelatedValidator : IValidateOptions<Unrelated>
                               {
                                   public ValidateOptionsResult Validate(string? name, Unrelated options) => ValidateOptionsResult.Success;
                               }

                               public class Unrelated { }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    [Arguments(10, false)]
    [Arguments(11, true)]
    public async Task Honours_ConfigurationIgnore_from_binder_version_11(int binder, bool ignored)
    {
        // [ConfigurationIgnore] ships with .NET 11's Microsoft.Extensions.Configuration.Abstractions; declared here so the test
        // also runs on .NET 10.
        var result = await WithBinder(binder)
                          .AddSources(
                               Usings
                             + """
                               namespace Microsoft.Extensions.Configuration
                               {
                                   [AttributeUsage(AttributeTargets.Property)]
                                   public sealed class ConfigurationIgnoreAttribute : Attribute;
                               }

                               namespace Sample
                               {
                                   [DovetailConfiguration]
                                   public class SecretOptions
                                   {
                                       public string? Name { get; set; }

                                       [ConfigurationIgnore]
                                       public string? Hidden { get; set; }
                                   }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        var binderSource = result.Results[typeof(DovetailAttributesGenerator)]
                                 .SyntaxTrees.Single(z => Path.GetFileName(z.FilePath) == "Dovetail_ConfigurationOptions.g.cs")
                                 .ToString();
        var schema = result.FinalCompilation.Assembly.GetAttributes()
                           .Single(z => z.ConstructorArguments is [{ Value: "Dovetail.ConfigurationSchema.v1:Secret" }, _])
                           .ConstructorArguments[1]
                           .Value as string;
        binderSource.Contains("key: \"Hidden\"", StringComparison.Ordinal).ShouldBe(!ignored);
        schema!.Contains("\"Hidden\"", StringComparison.Ordinal).ShouldBe(!ignored);
        binderSource.ShouldContain("key: \"Name\"");
    }

    [Test]
    public async Task Reports_types_the_binder_cannot_create()
    {
        var result = await WithBinder(11)
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration]
                               public class RefOptions
                               {
                                   public RefOptions(ref int value) => Value = value;

                                   public int Value { get; }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.Results[typeof(DovetailAttributesGenerator)]
              .Diagnostics.Select(z => $"{z.Id}: {z.GetMessage(System.Globalization.CultureInfo.InvariantCulture)}")
              .ShouldBe(["DT0011: 'Sample.RefOptions' can't be created from configuration because its constructor parameters can't be in, out or ref ('value')"]);
        result.Results[typeof(DovetailAttributesGenerator)].SyntaxTrees.Select(z => Path.GetFileName(z.FilePath)).ShouldNotContain("Dovetail_ConfigurationOptions.g.cs");
    }

    [Test]
    public async Task Registers_nothing_without_options()
    {
        var result = await WithBinder(11)
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration(RegisterOptions = false)]
                               public class SchemaOnlyOptions
                               {
                                   public string? Value { get; set; }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        result.Results[typeof(DovetailAttributesGenerator)].SyntaxTrees.Select(z => Path.GetFileName(z.FilePath)).ShouldNotContain("Dovetail_ConfigurationOptions.g.cs");
    }

    [Test]
    public async Task Keeps_options_cached_when_unrelated_code_changes()
    {
        var result = await WithBinder(11).AddSources(OptionsClass).Build().GenerateAsync(TestContext.CancellationToken);

        var compilation = result.InputCompilation;
        var parseOptions = (CSharpParseOptions)compilation.SyntaxTrees.First().Options;
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new DovetailAttributesGenerator().AsSourceGenerator()],
            parseOptions: parseOptions,
            driverOptions: new(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true)
        );
        driver = driver.RunGenerators(compilation, TestContext.CancellationToken);
        driver = driver.RunGenerators(
            compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText("namespace Sample; public class Unrelated { }", parseOptions, cancellationToken: TestContext.CancellationToken)),
            TestContext.CancellationToken
        );

        var steps = driver.GetRunResult().Results.Single().TrackedSteps;
        steps["dovetail:configuration_options"]
           .SelectMany(z => z.Outputs)
           .Select(z => z.Reason)
           .ShouldAllBe(z => z == IncrementalStepRunReason.Cached || z == IncrementalStepRunReason.Unchanged);
    }

    /// <summary>
    ///     A <c>[Validate]</c> options type is validated by the ZeroAlloc.Validation.Options overload of its own:
    ///     <c>ValidateWithZeroAlloc()</c> comes before <c>ValidateOnStart()</c>, and types without <c>[Validate]</c> don't call it
    ///     (the stand-in only has overloads for the <c>[Validate]</c> types, so the generated code wouldn't compile otherwise).
    /// </summary>
    [Test]
    public async Task Validates_ZeroAlloc_models_with_their_generated_validator()
    {
        var result = await WithBinder(11)
                          .AddSources(ZeroAllocValidation, ZeroAllocValidationOptions, ZeroAllocOptionsSources, ZeroAllocOverloads("ZeroAllocOptionsValidationExtensions"))
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    [Arguments("Internal", "InternalZeroAllocOptionsValidationExtensions")]
    [Arguments("internal", "InternalZeroAllocOptionsValidationExtensions")]
    [Arguments("Public", "ZeroAllocOptionsValidationExtensions")]
    public async Task Calls_the_overloads_where_ZeroAllocGeneratedAccessibility_puts_them(string accessibility, string extensionsClass)
    {
        var result = await WithBinder(11)
                          .AddGlobalOption("build_property.ZeroAllocGeneratedAccessibility", accessibility)
                          .AddSources(ZeroAllocValidation, ZeroAllocValidationOptions, ZeroAllocOptionsSources, ZeroAllocOverloads(extensionsClass))
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        GeneratedOptions(result).ShouldContain($"global::ZeroAlloc.Validation.Options.{extensionsClass}.ValidateWithZeroAlloc(");
    }

    /// <summary>
    ///     Without ZeroAlloc.Validation.Options there is no overload to call, and <c>DovetailEnableZeroAllocValidation=false</c>
    ///     leaves ZeroAlloc.Validation's registrations to the project.
    /// </summary>
    [Test]
    [Arguments(false, null)]
    [Arguments(true, "false")]
    public async Task Leaves_ZeroAlloc_validation_out(bool optionsPackage, string? enabled)
    {
        var builder = WithBinder(11).AddSources(ZeroAllocValidation, ZeroAllocOptionsSources);
        if (optionsPackage)
        {
            builder = builder.AddSources(ZeroAllocValidationOptions, ZeroAllocOverloads("ZeroAllocOptionsValidationExtensions"));
        }

        if (enabled is not null)
        {
            builder = builder.AddGlobalOption("build_property.DovetailEnableZeroAllocValidation", enabled);
        }

        var result = await builder.Build().GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        GeneratedOptions(result).ShouldNotContain("ValidateWithZeroAlloc");
    }

    /// <summary>
    ///     Options validation is synchronous, so calling <c>ValidateWithZeroAlloc()</c> for a model with an asynchronous rule, its
    ///     own or one of a nested or collection model's, is an error (ZV0034). Dovetail reports DT0018 and leaves the call out.
    /// </summary>
    [Test]
    public async Task Reports_ZeroAlloc_models_with_asynchronous_rules()
    {
        var result = await WithBinder(11)
                          .AddSources(
                               ZeroAllocValidation,
                               ZeroAllocValidationOptions,
                               Usings
                             + """
                               using System.Threading;
                               using System.Threading.Tasks;
                               using ZeroAlloc.Validation;

                               namespace Sample;

                               public sealed class UniqueNameAttribute : AsyncValidationAttribute<string>
                               {
                                   public override ValueTask<bool> IsValidAsync(string value, CancellationToken ct) => new(true);
                               }

                               [DovetailConfiguration, Validate]
                               public class TenantsOptions
                               {
                                   public string? Region { get; set; }
                                   public List<TenantOptions> Tenants { get; set; } = [];
                               }

                               [Validate]
                               public class TenantOptions
                               {
                                   [UniqueName]
                                   public string Name { get; set; } = "";
                               }

                               public class BaseOptions
                               {
                                   [UniqueName]
                                   public string Name { get; set; } = "";
                               }

                               // Only the type's own members are validated, so the base type's asynchronous rule doesn't count.
                               [DovetailConfiguration, Validate(IncludeBaseProperties = false)]
                               public class DerivedOptions : BaseOptions
                               {
                                   public string? Other { get; set; }
                               }
                               """,
                               """
                               using Microsoft.Extensions.Options;

                               namespace ZeroAlloc.Validation.Options;

                               public static class ZeroAllocOptionsValidationExtensions
                               {
                                   public static OptionsBuilder<global::Sample.DerivedOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.DerivedOptions> builder) => builder;
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        result.Results[typeof(DovetailAttributesGenerator)]
              .Diagnostics.Select(z => $"{z.Id}: {z.GetMessage(System.Globalization.CultureInfo.InvariantCulture)}")
              .ShouldBe(
                   [
                       "DT0018: 'Sample.TenantsOptions' isn't validated with ZeroAlloc.Validation when its options are read, because 'Sample.TenantOptions.Name' has an asynchronous rule and options validation is synchronous",
                   ]
               );
        var generated = GeneratedOptions(result);
        generated.ShouldContain("ValidateWithZeroAlloc(global::Joinery.Dovetail.DovetailConfigurationOptionsExtensions.AddDovetailConfigurationOptions<global::Sample.DerivedOptions>");
        generated.ShouldNotContain("ValidateWithZeroAlloc(global::Joinery.Dovetail.DovetailConfigurationOptionsExtensions.AddDovetailConfigurationOptions<global::Sample.TenantsOptions>");
    }

    /// <summary>
    ///     Which <c>[DovetailConfiguration]</c> types call <c>ValidateWithZeroAlloc()</c>: the <c>[Validate]</c> classes (records
    ///     included) of this assembly that the generated code can see, nested or not, whatever their accessibility. A type
    ///     without <c>[Validate]</c> and an options type from another assembly get no overload from ZeroAlloc.Validation.Options,
    ///     so they register without one. The overload stand-ins only cover the types that should call them, so a call the generator
    ///     shouldn't make fails to compile.
    /// </summary>
    [Test]
    public async Task Calls_ValidateWithZeroAlloc_only_for_the_types_ZeroAlloc_writes_an_overload_for()
    {
        // ZeroAlloc.Validation.Options writes overloads per project, so a [Validate] class of a referenced assembly has none here.
        var thirdParty = await Builder
                              .WithProjectName("ThirdParty")
                              .AddReferences(typeof(IValidateOptions<>))
                              .AddSources(
                                   ZeroAllocValidation,
                                   ZeroAllocValidationOptions,
                                   """
                                   namespace ThirdParty;

                                   [ZeroAlloc.Validation.Validate]
                                   public class ExternalOptions
                                   {
                                       public string? Endpoint { get; set; }
                                   }
                                   """
                               )
                              .Build()
                              .GenerateAsync(TestContext.CancellationToken);

        var result = await WithBinder(11)
                          .AddCompilationReferences(thirdParty)
                          .AddSources(
                               Usings
                             + """
                               using ZeroAlloc.Validation;

                               [assembly: DovetailConfiguration<ThirdParty.ExternalOptions>("External")]

                               namespace Sample;

                               [DovetailConfiguration, Validate]
                               public class PublicOptions
                               {
                                   public string? Value { get; set; }
                               }

                               [DovetailConfiguration, Validate]
                               internal class InternalOptions
                               {
                                   public string? Value { get; set; }
                               }

                               public class Outer
                               {
                                   [DovetailConfiguration, Validate]
                                   public class NestedOptions
                                   {
                                       public string? Value { get; set; }
                                   }

                                   [DovetailConfiguration, Validate]
                                   protected internal class ProtectedInternalOptions
                                   {
                                       public string? Value { get; set; }
                                   }
                               }

                               [DovetailConfiguration, Validate]
                               public record RecordOptions
                               {
                                   public string? Value { get; init; }
                               }

                               [DovetailConfiguration]
                               public class UnvalidatedOptions
                               {
                                   public string? Value { get; set; }
                               }
                               """,
                               """
                               using Microsoft.Extensions.Options;

                               namespace ZeroAlloc.Validation.Options;

                               public static class ZeroAllocOptionsValidationExtensions
                               {
                                   public static OptionsBuilder<global::Sample.PublicOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.PublicOptions> builder) => builder;
                                   internal static OptionsBuilder<global::Sample.InternalOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.InternalOptions> builder) => builder;
                                   public static OptionsBuilder<global::Sample.Outer.NestedOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.Outer.NestedOptions> builder) => builder;
                                   internal static OptionsBuilder<global::Sample.Outer.ProtectedInternalOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.Outer.ProtectedInternalOptions> builder) => builder;
                                   public static OptionsBuilder<global::Sample.RecordOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.RecordOptions> builder) => builder;
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();
        await Verify(result);
    }

    /// <summary>
    ///     Every way a <c>[Validate]</c> options type reaches an asynchronous rule, each reported with DT0018 and registered without
    ///     <c>ValidateWithZeroAlloc()</c>: a rule on its own property, on the type itself, on a nested model, on the items of an
    ///     array or a list, or on an inherited property. Rules ZeroAlloc.Validation doesn't run don't count: base properties under
    ///     <c>IncludeBaseProperties = false</c>, and members of a nested type without <c>[Validate]</c>. A model that reaches itself
    ///     is checked once. The overload stand-ins only cover the types that should call them, so a call the generator shouldn't make
    ///     fails to compile.
    /// </summary>
    [Test]
    public async Task Reports_each_way_a_ZeroAlloc_model_reaches_an_asynchronous_rule()
    {
        var result = await WithBinder(11)
                          .WithDiagnosticSeverity(DiagnosticSeverity.Warning)
                          .AddSources(
                               ZeroAllocValidation,
                               ZeroAllocValidationOptions,
                               "#nullable enable\n"
                             + Usings
                             + """
                               using System.Threading;
                               using System.Threading.Tasks;
                               using ZeroAlloc.Validation;

                               namespace Sample;

                               public sealed class UniqueNameAttribute : AsyncValidationAttribute<string>
                               {
                                   public override ValueTask<bool> IsValidAsync(string value, CancellationToken ct) => new(true);
                               }

                               [AttributeUsage(AttributeTargets.Class)]
                               public sealed class ReachableAttribute : AsyncValidationAttribute<object>
                               {
                                   public override ValueTask<bool> IsValidAsync(object value, CancellationToken ct) => new(true);
                               }

                               [Validate]
                               public class TenantOptions
                               {
                                   [UniqueName]
                                   public string Name { get; set; } = "";
                               }

                               public class NamedOptions
                               {
                                   [UniqueName]
                                   public string Name { get; set; } = "";
                               }

                               [DovetailConfiguration, Validate]
                               public class OwnRuleOptions
                               {
                                   [UniqueName]
                                   public string Name { get; set; } = "";
                               }

                               [DovetailConfiguration, Validate, Reachable]
                               public class TypeRuleOptions
                               {
                                   public string? Endpoint { get; set; }
                               }

                               [DovetailConfiguration, Validate]
                               public class NestedRuleOptions
                               {
                                   public TenantOptions Tenant { get; set; } = new();
                               }

                               [DovetailConfiguration, Validate]
                               public class ArrayRuleOptions
                               {
                                   public TenantOptions[] Tenants { get; set; } = [];
                               }

                               [DovetailConfiguration, Validate]
                               public class ListRuleOptions
                               {
                                   public IReadOnlyList<TenantOptions> Tenants { get; set; } = [];
                               }

                               [DovetailConfiguration, Validate]
                               public class InheritedRuleOptions : NamedOptions;

                               [DovetailConfiguration, Validate(IncludeBaseProperties = false)]
                               public class OwnPropertiesOnlyOptions : NamedOptions
                               {
                                   public string? Other { get; set; }
                               }

                               [DovetailConfiguration, Validate]
                               public class UnvalidatedModelOptions
                               {
                                   public NamedOptions Named { get; set; } = new();
                               }

                               [DovetailConfiguration, Validate]
                               public class TreeOptions
                               {
                                   public string? Name { get; set; }
                                   public List<TreeOptions> Children { get; set; } = [];
                               }
                               """,
                               """
                               using Microsoft.Extensions.Options;

                               namespace ZeroAlloc.Validation.Options;

                               public static class ZeroAllocOptionsValidationExtensions
                               {
                                   public static OptionsBuilder<global::Sample.OwnPropertiesOnlyOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.OwnPropertiesOnlyOptions> builder) => builder;
                                   public static OptionsBuilder<global::Sample.UnvalidatedModelOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.UnvalidatedModelOptions> builder) => builder;
                                   public static OptionsBuilder<global::Sample.TreeOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.TreeOptions> builder) => builder;
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();
        // Which assembly versions need unifying (CS1701, CS1702) depends on the runtime the tests run on, so they stay out of the snapshot.
        await Verify(result with { FinalDiagnostics = result.FinalDiagnostics.RemoveAll(z => z.Id is "CS1701" or "CS1702") });
    }

    /// <summary>Stand-ins for the ZeroAlloc.Validation types the generator looks for.</summary>
    private const string ZeroAllocValidation = """
        namespace ZeroAlloc.Validation
        {
            [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
            public sealed class ValidateAttribute : System.Attribute
            {
                public bool IncludeBaseProperties { get; set; } = true;
            }

            [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
            public abstract class AsyncValidationAttribute<T> : System.Attribute
            {
                public abstract System.Threading.Tasks.ValueTask<bool> IsValidAsync(T value, System.Threading.CancellationToken ct);
            }
        }
        """;

    /// <summary>A stand-in for the runtime type of ZeroAlloc.Validation.Options, which tells the generator the package is referenced.</summary>
    private const string ZeroAllocValidationOptions = """
        namespace ZeroAlloc.Validation.Options
        {
            public sealed class ZeroAllocOptionsValidator<T>;
        }
        """;

    private const string ZeroAllocOptionsSources = Usings
      + """
        using ZeroAlloc.Validation;

        namespace Sample;

        [DovetailConfiguration(ValidateOnStart = true), Validate]
        public class DatabaseOptions
        {
            public string ConnectionString { get; set; } = "";
        }

        [DovetailConfiguration, Validate]
        public class CacheOptions
        {
            public int Size { get; set; }
        }

        [DovetailConfiguration]
        public class PlainOptions
        {
            public string? Value { get; set; }
        }
        """;

    /// <summary>What ZeroAlloc.Validation.Options' generator writes for <see cref="ZeroAllocOptionsSources" />, in <paramref name="className" />.</summary>
    private static string ZeroAllocOverloads(string className) => $$"""
        using Microsoft.Extensions.Options;

        namespace ZeroAlloc.Validation.Options;

        public static class {{className}}
        {
            public static OptionsBuilder<global::Sample.DatabaseOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.DatabaseOptions> builder) => builder;
            public static OptionsBuilder<global::Sample.CacheOptions> ValidateWithZeroAlloc(this OptionsBuilder<global::Sample.CacheOptions> builder) => builder;
        }
        """;

    private static string GeneratedOptions(GeneratorTestResults result) =>
        result.Results[typeof(DovetailAttributesGenerator)].SyntaxTrees.Single(z => Path.GetFileName(z.FilePath) == "Dovetail_ConfigurationOptions.g.cs").ToString();

    /// <summary>
    ///     The test compilation, referencing the assemblies the generated code uses, and a stand-in for
    ///     Microsoft.Extensions.Configuration.Binder at <paramref name="binder" />.0.0.0 that selects the binder rules.
    /// </summary>
    private GeneratorTestContextBuilder WithBinder(int binder) =>
        Builder
           .AddConfigurationOptionsReferences()
           .AddReferences(typeof(IValidateOptions<>), typeof(RangeAttribute), typeof(Uri))
           .AddReferences(BinderStandIn(binder));

    private static MetadataReference BinderStandIn(int major) =>
        CSharpCompilation
           .Create(
                "Microsoft.Extensions.Configuration.Binder",
                [CSharpSyntaxTree.ParseText($"[assembly: System.Reflection.AssemblyVersion(\"{major}.0.0.0\")]")],
                [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
                new(OutputKind.DynamicallyLinkedLibrary)
            )
           .ToMetadataReference();

    /// <summary>The generated options file and the exports that list its joint; the generated code must compile.</summary>
    private static string Describe(GeneratorTestResults result)
    {
        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();

        var generated = result.Results[typeof(DovetailAttributesGenerator)];
        var text = new StringBuilder();
        foreach (var name in new[] { "Dovetail_ConfigurationOptions.g.cs", "Exported_Dovetails.g.cs" })
        {
            text.AppendLine("## " + name);
            text.AppendLine(generated.SyntaxTrees.SingleOrDefault(z => Path.GetFileName(z.FilePath) == name)?.ToString() ?? "(none)");
        }

        text.AppendLine("## Diagnostics");
        foreach (var diagnostic in generated.Diagnostics.OrderBy(z => z.Location.SourceSpan.Start).ThenBy(z => z.Id, StringComparer.Ordinal))
        {
            text.AppendLine($"{diagnostic.Id} {diagnostic.Severity}: {diagnostic.GetMessage(System.Globalization.CultureInfo.InvariantCulture)}");
        }

        return text.ToString();
    }
}
