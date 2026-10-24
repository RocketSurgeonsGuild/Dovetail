using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Dovetail.Analyzers.Tests;

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
        using Dovetail.Attributes;
        using Microsoft.Extensions.Configuration;

        """;

    private GeneratorTestContextBuilder WithAnnotations() => Builder.AddReferences(typeof(RangeAttribute), typeof(DescriptionAttribute), typeof(TypeConverterAttribute), typeof(Uri));

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

                               [DovetailConfiguration("Shared:A", Order = 1)]
                               [DovetailConfiguration("Shared:B")]
                               public class SharedOptions { public string? Value { get; set; } }
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
                          .AddCompilationReferences(thirdParty)
                          .AddSources("[assembly: Dovetail.Attributes.DovetailConfiguration(\"Client\", typeof(ThirdParty.ClientOptions))]")
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
                               [assembly: DovetailConfiguration("Untyped")]

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
