using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.CodeAnalysis;
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
