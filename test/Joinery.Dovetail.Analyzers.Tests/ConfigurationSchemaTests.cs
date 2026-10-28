using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

/// <summary>
///     The JSON Schema fragments <c>[DovetailConfiguration]</c> produces. Each snapshot shows every fragment the compilation
///     carries (read back from its <c>AssemblyMetadata</c>, pretty-printed) followed by the generator's diagnostics.
/// </summary>
public class ConfigurationSchemaTests() : GeneratorTest()
{
    private const string Usings = """
        using System;
        using System.Collections.Generic;
        using System.ComponentModel;
        using System.ComponentModel.DataAnnotations;
        using Joinery.Dovetail.Attributes;
        using Microsoft.Extensions.Configuration;

        """;

    private GeneratorTestContextBuilder WithAnnotations() =>
        Builder.AddConfigurationOptionsReferences().AddReferences(typeof(RangeAttribute), typeof(DescriptionAttribute), typeof(TypeConverterAttribute), typeof(Uri));

    [Test]
    public async Task Describes_an_options_class()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               public enum GreetingMode { Polite, Casual, Shouty }

                               [Flags]
                               public enum Channels { None = 0, Console = 1, Email = 2 }

                               /// <summary>Settings for the greeting service.</summary>
                               [DovetailConfiguration]
                               public class GreetingsOptions
                               {
                                   /// <summary>Text placed before the name.</summary>
                                   public string Message { get; set; } = "Hello";

                                   /// <summary>How many times to greet.</summary>
                                   [Range(1, 10)]
                                   public int Count { get; set; } = 1;

                                   [Description("Turns greetings off.")]
                                   public bool Enabled { get; set; } = true;

                                   public GreetingMode Mode { get; set; } = GreetingMode.Polite;

                                   public Channels Channels { get; set; }

                                   public TimeSpan Timeout { get; set; }

                                   public Uri? Endpoint { get; set; }

                                   [EmailAddress]
                                   public string? ReplyTo { get; set; }

                                   [RegularExpression("[a-z]{2}-[A-Z]{2}")]
                                   public string Culture { get; set; } = "en-US";

                                   [Obsolete("Use Message.")]
                                   public string? OldGreeting { get; set; }

                                   [ConfigurationKeyName("custom_key")]
                                   public string? Custom { get; set; }

                                   public byte Level { get; set; }

                                   public double? Ratio { get; set; }

                                   /// <summary>Who to greet.</summary>
                                   [MaxLength(5)]
                                   public List<string> Names { get; } = [];

                                   public Dictionary<string, int> Weights { get; set; } = [];

                                   public Dictionary<GreetingMode, string> Templates { get; set; } = [];

                                   public RetryOptions Retry { get; set; } = new();

                                   public int Total => Count * 2;
                               }

                               /// <summary>How to retry.</summary>
                               public class RetryOptions
                               {
                                   /// <summary>Attempts before giving up.</summary>
                                   [Range(0, 5)]
                                   public int Attempts { get; set; } = 3;

                                   public TimeSpan[] Delays { get; set; } = [];
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Names_sections()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration]
                               public class HostingSettings { public string? Name { get; set; } }

                               [DovetailConfiguration("Http:Retry")]
                               public class RetryPolicy { public int Count { get; set; } }

                               [DovetailConfiguration("Shared:A")]
                               public class SharedAOptions { public string? Value { get; set; } }

                               [DovetailConfiguration("Shared:B")]
                               public class SharedBOptions { public string? Value { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Describes_constructor_bound_types()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               /// <summary>A server to connect to.</summary>
                               /// <param name="Host">The host name.</param>
                               /// <param name="Port">The port to connect on.</param>
                               [DovetailConfiguration]
                               public record ServerOptions(string Host, int Port = 8080)
                               {
                                   /// <summary>Whether to use TLS.</summary>
                                   public bool Secure { get; init; }
                               }

                               [DovetailConfiguration]
                               public class ConnectionOptions(string server)
                               {
                                   /// <summary>The server's address.</summary>
                                   public string Server { get; } = server;

                                   public int Timeout { get; set; } = 30;
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Reports_problems()
    {
        var result = await WithAnnotations()
                          .WithDiagnosticSeverity(DiagnosticSeverity.Warning)
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration]
                               public abstract class AbstractOptions { }

                               [DovetailConfiguration]
                               public class GenericOptions<T> { }

                               [DovetailConfiguration]
                               public class Options { }

                               [DovetailConfiguration("A::B")]
                               public class BadPathOptions { }

                               [DovetailConfiguration]
                               public class UnsupportedOptions
                               {
                                   public object? Anything { get; set; }
                                   public System.Globalization.CultureInfo? Culture { get; set; }
                                   public Action? Callback { get; set; }
                                   public Dictionary<int, string> ByNumber { get; set; } = [];
                                   public string? Kept { get; set; }
                               }

                               [DovetailConfiguration]
                               public class CollidingOptions
                               {
                                   public string? Name { get; set; }

                                   [ConfigurationKeyName("name")]
                                   public string? Other { get; set; }
                               }

                               [DovetailConfiguration]
                               public class AmbiguousOptions
                               {
                                   public AmbiguousOptions(int a) { }
                                   public AmbiguousOptions(string b) { }
                                   public int A { get; set; }
                                   public string? B { get; set; }
                               }

                               [DovetailConfiguration]
                               public class OrphanParameterOptions(string unmatched)
                               {
                                   public string? Name { get; set; }
                               }

                               [DovetailConfiguration]
                               public class TreeOptions
                               {
                                   public string? Name { get; set; }
                                   public TreeOptions? Child { get; set; }
                               }

                               [DovetailConfiguration]
                               public class AnnotationOptions
                               {
                                   [Range(1, 10)]
                                   public string? Text { get; set; }

                                   [RegularExpression("(?i)abc")]
                                   public string? Pattern { get; set; }

                                   [RegularExpression("[a-z-[aeiou]]+")]
                                   public string? Consonants { get; set; }

                                   [MaxLength(3)]
                                   public int Number { get; set; }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Describes_types_from_referenced_assemblies_with_their_xml_docs()
    {
        var thirdParty = await Builder
                              .WithProjectName("ThirdParty")
                              .AddSources(
                                   """
                                   namespace ThirdParty;

                                   public class ClientOptions
                                   {
                                       public string? BaseAddress { get; set; }
                                       public int Retries { get; set; }
                                       public ClientLimits Limits { get; set; } = new();
                                   }

                                   public class ClientLimits
                                   {
                                       public int MaxConnections { get; set; }
                                   }
                                   """
                               )
                              .Build()
                              .GenerateAsync(TestContext.CancellationToken);

        const string documentationPath = "/packages/thirdparty/1.0.0/lib/net10.0/ThirdParty.xml";
        var result = await Builder
                          .AddConfigurationOptionsReferences()
                          .AddCompilationReferences(thirdParty)
                          .AddSources("[assembly: Joinery.Dovetail.Attributes.DovetailConfiguration<ThirdParty.ClientOptions>(\"Client\")]")
                          .AddAdditionalText(
                               documentationPath,
                               """
                               <?xml version="1.0"?>
                               <doc>
                                 <assembly><name>ThirdParty</name></assembly>
                                 <members>
                                   <member name="T:ThirdParty.ClientOptions"><summary>Options for the <see cref="T:ThirdParty.ClientOptions"/> client.</summary></member>
                                   <member name="P:ThirdParty.ClientOptions.BaseAddress"><summary>Where requests go.</summary></member>
                                   <member name="P:ThirdParty.ClientOptions.Retries"><summary>Retries per request.</summary></member>
                                   <member name="P:ThirdParty.ClientLimits.MaxConnections"><summary>Connections per server.</summary></member>
                                 </members>
                               </doc>
                               """
                           )
                          .AddOption(documentationPath, "build_metadata.AdditionalFiles.DovetailReferenceDocumentation", "true")
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Lists_required_members_when_asked()
    {
        var result = await WithAnnotations()
                          .AddGlobalOption("build_property.DovetailConfigurationSchemaRequired", "true")
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration]
                               public record DatabaseOptions(string ConnectionString, int Port, int? Timeout = null)
                               {
                                   [Required]
                                   public string? Schema { get; init; }

                                   public required string Provider { get; init; }

                                   public bool Pooling { get; init; }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Describes_framework_and_collection_types()
    {
        var result = await WithAnnotations()
                          .AddReferences(typeof(System.Net.IPAddress), typeof(SortedDictionary<string, int>), typeof(Queue<int>), typeof(TypeConverter))
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               public class Tags : List<string> { }

                               [TypeConverter(typeof(TypeConverter))]
                               public class Money { public decimal Amount { get; set; } public string? Currency { get; set; } }

                               [DovetailConfiguration]
                               public class TypesOptions
                               {
                                   public System.Globalization.CultureInfo? Culture { get; set; }
                                   public Half Ratio { get; set; }
                                   public Int128 Big { get; set; }
                                   public UInt128 BigUnsigned { get; set; }
                                   public object? Anything { get; set; }
                                   public IConfigurationSection? Raw { get; set; }
                                   public Tags Labels { get; set; } = new();
                                   public System.Collections.Generic.SortedDictionary<string, int> Sorted { get; set; } = new();
                                   public Dictionary<int, string> ByNumber { get; set; } = [];
                                   public Dictionary<uint, string> ByUnsigned { get; set; } = [];
                                   public Money Price { get; set; } = new();
                                   public DateTime At { get; set; }
                                   public DateOnly Day { get; set; }
                                   public TimeOnly Time { get; set; }
                                   public System.Net.IPAddress? Address { get; set; }
                                   public Queue<int> Pending { get; set; } = new();
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Describes_nested_objects_without_requiring_their_constructors()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               public class Limits
                               {
                                   internal Limits() { }
                                   public int MaxConnections { get; set; }
                               }

                               public class Seeded(int seed)
                               {
                                   public int Value { get; set; } = seed;
                               }

                               [DovetailConfiguration]
                               public class ClientOptions
                               {
                                   public Limits Limits { get; } = new();
                                   public Seeded Seeded { get; set; } = new(1);
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Reports_invalid_declarations()
    {
        var result = await WithAnnotations()
                          .WithDiagnosticSeverity(DiagnosticSeverity.Warning)
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration]
                               public class Tags : List<string> { }

                               public class Outer<T>
                               {
                                   [DovetailConfiguration]
                                   public class InnerOptions { public string? Value { get; set; } }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Skips_empty_string_defaults()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration]
                               public class BlankOptions
                               {
                                   /// <summary>Who to greet.</summary>
                                   public string Name { get; set; } = "";

                                   public string Greeting { get; set; } = "Hi";
                               }

                               [DovetailConfiguration]
                               public record BlankRecordOptions(string Name = "", string Greeting = "Hi");
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        foreach (var section in new[] { "Blank", "BlankRecord" })
        {
            using var document = JsonDocument.Parse(Fragments(result)[section]);
            var properties = document.RootElement.GetProperty("properties");
            var name = properties.GetProperty("Name");
            name.TryGetProperty("default", out _).ShouldBeFalse(section);
            ( name.TryGetProperty("description", out var description) ? description.GetString() ?? "" : "" ).ShouldNotContain("Default:", Case.Sensitive, section);
            properties.GetProperty("Greeting").GetProperty("default").GetString().ShouldBe("Hi", section);
            properties.GetProperty("Greeting").GetProperty("description").GetString()!.ShouldContain("Default: Hi.", Case.Sensitive, section);
        }
    }

    [Test]
    public async Task Rejects_a_type_configured_by_both_the_class_and_the_assembly_form()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               [assembly: DovetailConfiguration<Sample.TwiceOptions>("Second")]
                               [assembly: DovetailConfiguration<Sample.AssemblyOnlyOptions>("Elsewhere")]

                               namespace Sample;

                               [DovetailConfiguration("First")]
                               public class TwiceOptions { public string? Value { get; set; } }

                               public class AssemblyOnlyOptions { public string? Value { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        // Two assembly attributes for different types must compile (the attribute has to allow several uses on the assembly).
        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();
        var diagnostics = result.Results[typeof(DovetailAttributesGenerator)].Diagnostics;
        diagnostics.Select(z => z.Id).ShouldBe(["DT0015"]);
        diagnostics[0].GetMessage(CultureInfo.InvariantCulture).ShouldBe("'Sample.TwiceOptions' is configured more than once; a type can have only one [DovetailConfiguration]");
        // The assembly attribute is the one reported, and only the class declaration produces a fragment.
        diagnostics[0].Location.SourceTree!.ToString().Substring(diagnostics[0].Location.SourceSpan.Start, diagnostics[0].Location.SourceSpan.Length).ShouldContain("\"Second\"");
        Fragments(result).Keys.ShouldBe(["First", "Elsewhere"], ignoreOrder: true);
    }

    [Test]
    public async Task Rejects_a_type_named_twice_in_the_assembly_form()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               [assembly: DovetailConfiguration<Sample.RepeatedOptions>("First")]
                               [assembly: DovetailConfiguration<Sample.RepeatedOptions>("Second")]

                               namespace Sample;

                               public class RepeatedOptions { public string? Value { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();
        var diagnostics = result.Results[typeof(DovetailAttributesGenerator)].Diagnostics;
        diagnostics.Select(z => z.Id).ShouldBe(["DT0015"]);
        // The first declaration wins and the repeat produces nothing.
        Fragments(result).Keys.ShouldBe(["First"]);
    }

    [Test]
    public async Task Inherits_key_names_and_accepts_escaped_patterns()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               public class BaseOptions
                               {
                                   [ConfigurationKeyName("base_key")]
                                   public virtual string? Name { get; set; }
                               }

                               [DovetailConfiguration]
                               public class DerivedOptions : BaseOptions
                               {
                                   public override string? Name { get; set; }

                                   [RegularExpression(@"C:\\Apps\\.*")]
                                   public string? Folder { get; set; }

                                   [RegularExpression(@"\(?id\)?")]
                                   public string? Parenthesized { get; set; }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Describes_the_declared_members_of_interfaces_and_abstract_classes()
    {
        // The binder can't create these, but fills an existing instance's declared properties, so the keys are valid.
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               public interface IShape { int Sides { get; set; } }
                               public class Square : IShape { public int Sides { get; set; } = 4; public string? Fill { get; set; } }
                               public abstract class Base { public string? Name { get; set; } }
                               public class Derived : Base { public string? Extra { get; set; } }

                               [DovetailConfiguration]
                               public class ShapesOptions
                               {
                                   public IShape Shape { get; set; } = new Square();
                                   public Base Item { get; } = new Derived();
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.ShouldBeEmpty();
        using var shapes = JsonDocument.Parse(Fragments(result)["Shapes"]);
        var definitions = shapes.RootElement.GetProperty("definitions");
        definitions.GetProperty("Sample.IShape").GetProperty("properties").EnumerateObject().Select(z => z.Name).ShouldBe(["Sides"]);
        definitions.GetProperty("Sample.Base").GetProperty("properties").EnumerateObject().Select(z => z.Name).ShouldBe(["Name"]);
        shapes.RootElement.GetProperty("properties").EnumerateObject().Select(z => z.Name).ShouldBe(["Shape", "Item"], ignoreOrder: true);
    }

    [Test]
    public async Task Survives_empty_comments()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               /**/
                               [DovetailConfiguration]
                               public class QuietOptions
                               {
                                   /**/
                                   public string? Name { get; set; }
                               }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        var text = Describe(result);
        text.ShouldContain("## Quiet");
        text.ShouldNotContain("DT0014");
    }

    [Test]
    public async Task Locates_diagnostics_in_source_so_pragmas_apply()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration]
                               public class CallbackOptions { public Action? Callback { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        var diagnostics = result.Results[typeof(DovetailAttributesGenerator)].Diagnostics;
        diagnostics.ShouldContain(z => z.Id == "DT0009");
        diagnostics.ShouldAllBe(z => z.Location.SourceTree != null);
    }

    /// <summary>
    ///     Each object type is described once and referenced, so a deep graph where every level uses the next type twice stays
    ///     linear (with inlined copies, and every key's case-insensitive twin, it grew 4x per level).
    /// </summary>
    [Test]
    public async Task Keeps_deep_type_graphs_linear()
    {
        var levels = string.Join(
            "\n",
            Enumerable.Range(1, 12).Select(z => $"public class Level{z} {{ public Level{z + 1} First {{ get; set; }} = new(); public Level{z + 1} Second {{ get; set; }} = new(); }}")
        );
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + "namespace Sample;\n"
                             + "[DovetailConfiguration] public class DeepOptions { public Level1 Root { get; set; } = new(); }\n"
                             + levels
                             + "\npublic class Level13 { public int Value { get; set; } }\npublic class Node<T> { public Node<Node<T>>? Next { get; set; } }\n"
                             + "[DovetailConfiguration] public class EndlessOptions { public Node<int>? Start { get; set; } }\n"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        // The binder stops at a generic nesting depth as well, and what it generates still compiles.
        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        var fragments = Fragments(result);
        using var deep = JsonDocument.Parse(fragments["Deep"]);
        deep.RootElement.GetProperty("definitions").EnumerateObject().Count().ShouldBe(13);
        fragments["Deep"].Length.ShouldBeLessThan(10_000);
        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.Select(z => z.Id).ShouldBe(["DT0012"]);
    }

    /// <summary>
    ///     The pipeline's values compare by content, so an unrelated edit leaves the sections cached instead of rebuilding and
    ///     re-emitting them (the reason the model holds no symbols or locations).
    /// </summary>
    [Test]
    public async Task Keeps_sections_cached_when_unrelated_code_changes()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               /// <summary>Settings for the greeting service.</summary>
                               [DovetailConfiguration]
                               public class GreetingsOptions
                               {
                                   public string Message { get; set; } = "Hello";
                                   public RetryOptions Retry { get; set; } = new();
                               }

                               public class RetryOptions { public int Attempts { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

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
        foreach (var name in new[] { "dovetail:configuration_classes", "dovetail:configuration_sections" })
        {
            steps[name].SelectMany(z => z.Outputs).Select(z => z.Reason).ShouldAllBe(z => z == IncrementalStepRunReason.Cached || z == IncrementalStepRunReason.Unchanged, name);
        }
    }

    [Test]
    [Arguments("Camel")]
    [Arguments("Dash")]
    [Arguments("Underscore")]
    public async Task Spells_keys_in_the_declared_key_style(string style)
    {
        var result = await WithAnnotations()
                          .AddSources(ConfigurationKeyStyleSources.Styled(style))
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Derives_section_names_in_each_key_style()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration]
                               public class GreetingsOptions { public string? Message { get; set; } }

                               [DovetailConfiguration]
                               public class HttpClientOptions { public string? BaseAddress { get; set; } }

                               [DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Camel)]
                               public class HttpClientSettings { public string? BaseAddress { get; set; } }

                               [DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Camel)]
                               public class OpenAIConfig { public string? ApiKey { get; set; } }

                               [DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Dash)]
                               public class HttpClientConfiguration { public string? BaseAddress { get; set; } }

                               [DovetailConfiguration("Http:RetryPolicy", KeyStyle = ConfigurationKeyStyle.Dash)]
                               public class ExplicitSectionOptions { public int MaxAttempts { get; set; } }

                               [DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Dash)]
                               public class OpenAIOptions { public string? ApiKey { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Styles_the_assembly_form_like_the_class_form()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               [assembly: DovetailConfiguration<Sample.ThirdPartyOptions>(KeyStyle = ConfigurationKeyStyle.Dash)]
                               [assembly: DovetailConfiguration<Sample.CamelThirdPartyOptions>("Third:Camel", KeyStyle = ConfigurationKeyStyle.Camel)]

                               namespace Sample;

                               public class ThirdPartyOptions
                               {
                                   public string ConnectionString { get; set; } = "";
                                   public ThirdPartyPool PoolSettings { get; set; } = new();
                               }

                               public class ThirdPartyPool { public int MaxSize { get; set; } }

                               public class CamelThirdPartyOptions { public int RetryCount { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Treats_the_undefined_and_pascal_styles_alike_for_ordinary_names()
    {
        const string members = """
            public string Message { get; set; } = "";
            public int RetryCount { get; set; }
            public Nested Inner { get; set; } = new();
            """;
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + $$"""
                                 namespace Sample;

                                 [DovetailConfiguration("Default", KeyStyle = ConfigurationKeyStyle.Undefined)]
                                 public class DefaultOptions { {{members}} }

                                 [DovetailConfiguration("Explicit", KeyStyle = ConfigurationKeyStyle.Pascal)]
                                 public class ExplicitOptions { {{members}} }

                                 public class Nested { public int WaitTime { get; set; } }
                                 """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();
        var fragments = Fragments(result);
        // Ordinary C# names are Pascal case already, so Undefined and Pascal spell the same keys. That is also why the Nested type
        // they share is not a conflict: the binder reads the same keys either way.
        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.ShouldBeEmpty();
        fragments["Explicit"].ShouldBe(fragments["Default"].Replace("\"title\":\"Default\"", "\"title\":\"Explicit\"", StringComparison.Ordinal));
        fragments["Default"].ShouldContain("\"RetryCount\"");
        fragments["Default"].ShouldContain("\"WaitTime\"");
    }

    [Test]
    public async Task Respells_only_for_pascal_when_the_declared_name_starts_in_lower_case()
    {
        var result = await WithAnnotations()
                          .AddSources(ConfigurationKeyStyleSources.LowerCaseNames)
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(Describe(result));
    }

    [Test]
    public async Task Rejects_a_type_that_undefined_and_pascal_spell_differently()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration("First")]
                               public class FirstOptions { public LowerNested Inner { get; set; } = new(); }

                               [DovetailConfiguration("Second", KeyStyle = ConfigurationKeyStyle.Pascal)]
                               public class SecondOptions { public LowerNested Inner { get; set; } = new(); }

                               public class LowerNested { public int waitTime { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();
        var diagnostics = result.Results[typeof(DovetailAttributesGenerator)].Diagnostics;
        diagnostics.Select(z => z.Id).ShouldBe(["DT0016"]);
        Fragments(result).Keys.ShouldBe(["First"]);
    }

    [Test]
    public async Task Derives_underscore_section_names()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Underscore)]
                               public class HttpClientOptions { public int MaxConnections { get; set; } }

                               [DovetailConfiguration("Explicit:Section", KeyStyle = ConfigurationKeyStyle.Underscore)]
                               public class ExplicitOptions { public int MaxConnections { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();
        Fragments(result).Keys.ShouldBe(["http_client", "Explicit:Section"], ignoreOrder: true);
        Fragments(result)["http_client"].ShouldContain("\"max_connections\"");
    }

    [Test]
    public async Task Rejects_a_nested_type_bound_with_two_key_styles()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration("First", KeyStyle = ConfigurationKeyStyle.Camel)]
                               public class FirstOptions { public RetryOptions Retry { get; set; } = new(); }

                               [DovetailConfiguration("Second", KeyStyle = ConfigurationKeyStyle.Dash)]
                               public class SecondOptions { public RetryOptions Retry { get; set; } = new(); }

                               [DovetailConfiguration("Third", KeyStyle = ConfigurationKeyStyle.Camel)]
                               public class ThirdOptions { public RetryOptions Retry { get; set; } = new(); }

                               public class RetryOptions { public int MaxAttempts { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();
        var diagnostics = result.Results[typeof(DovetailAttributesGenerator)].Diagnostics;
        diagnostics.Select(z => z.Id).ShouldBe(["DT0016"]);
        diagnostics[0].Severity.ShouldBe(DiagnosticSeverity.Error);
        diagnostics[0].GetMessage(CultureInfo.InvariantCulture)
                      .ShouldBe(
                           "'Sample.RetryOptions' is bound with more than one key style; use the same KeyStyle for every section that reaches it, or give the shared type [ConfigurationKeyName] attributes"
                       );
        // The later declaration is the one reported, and it produces nothing; the earlier ones, and a later one in the same style, still do.
        diagnostics[0].Location.SourceTree!.ToString().Substring(diagnostics[0].Location.SourceSpan.Start, diagnostics[0].Location.SourceSpan.Length).ShouldContain("\"Second\"");
        Fragments(result).Keys.ShouldBe(["First", "Third"], ignoreOrder: true);
        result.Results[typeof(DovetailAttributesGenerator)].SyntaxTrees.Select(z => Path.GetFileName(z.FilePath)).ShouldContain("Dovetail_ConfigurationOptions.g.cs");
    }

    [Test]
    public async Task Allows_a_shared_type_when_its_keys_do_not_depend_on_the_style()
    {
        var result = await WithAnnotations()
                          .AddSources(
                               Usings
                             + """
                               namespace Sample;

                               [DovetailConfiguration("A", KeyStyle = ConfigurationKeyStyle.Camel)]
                               public class AOptions { public NamedRetry Retry { get; set; } = new(); public SharedRetry Shared { get; set; } = new(); }

                               [DovetailConfiguration("B", KeyStyle = ConfigurationKeyStyle.Dash)]
                               public class BOptions { public NamedRetry Retry { get; set; } = new(); }

                               // Described only: nothing is bound, so there is no binder to disagree with.
                               [DovetailConfiguration("C", KeyStyle = ConfigurationKeyStyle.Dash, RegisterOptions = false)]
                               public class COptions { public SharedRetry Shared { get; set; } = new(); }

                               public class NamedRetry
                               {
                                   [ConfigurationKeyName("max_attempts")]
                                   public int MaxAttempts { get; set; }
                               }

                               public class SharedRetry { public int MaxAttempts { get; set; } }
                               """
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
        result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.ShouldBeEmpty();
        Fragments(result).Keys.ShouldBe(["A", "B", "C"], ignoreOrder: true);
    }

    /// <summary>The raw schema fragments in the compilation, by section.</summary>
    private static Dictionary<string, string> Fragments(GeneratorTestResults result)
    {
        const string prefix = "Dovetail.ConfigurationSchema.v1:";
        return result.FinalCompilation.Assembly.GetAttributes()
                     .Select(z => z is { AttributeClass.Name: "AssemblyMetadataAttribute", ConstructorArguments: [{ Value: string key }, { Value: string value }] } ? (key, value) : default)
                     .Where(z => z.key?.StartsWith(prefix, StringComparison.Ordinal) == true)
                     .ToDictionary(z => z.key[prefix.Length..], z => z.value);
    }

    /// <summary>Every schema fragment (pretty-printed JSON), then the generator's diagnostics; the generated code must compile.</summary>
    private static string Describe(GeneratorTestResults result)
    {
        // The generator's own DT errors are part of what's verified below; only compiler errors mean broken generated code.
        result.FinalDiagnostics.Where(z => z.Severity == DiagnosticSeverity.Error && z.Id.StartsWith("CS", StringComparison.Ordinal)).ShouldBeEmpty();

        const string prefix = "Dovetail.ConfigurationSchema.v1:";
        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        var text = new StringBuilder();
        foreach (var attribute in result.FinalCompilation.Assembly.GetAttributes())
        {
            if (attribute is { AttributeClass.Name: "AssemblyMetadataAttribute", ConstructorArguments: [{ Value: string key }, { Value: string value }] }
             && key.StartsWith(prefix, StringComparison.Ordinal))
            {
                using var document = JsonDocument.Parse(value);
                text.AppendLine("## " + key[prefix.Length..]);
                text.AppendLine(JsonSerializer.Serialize(document.RootElement, options));
            }
        }

        text.AppendLine("## Diagnostics");
        foreach (var diagnostic in result.Results[typeof(DovetailAttributesGenerator)].Diagnostics.OrderBy(z => z.Location.SourceSpan.Start).ThenBy(z => z.Id, StringComparer.Ordinal))
        {
            text.AppendLine($"{diagnostic.Id} {diagnostic.Severity} line {diagnostic.Location.GetLineSpan().StartLinePosition.Line + 1}: {diagnostic.GetMessage(CultureInfo.InvariantCulture)}");
        }

        return text.ToString();
    }
}
