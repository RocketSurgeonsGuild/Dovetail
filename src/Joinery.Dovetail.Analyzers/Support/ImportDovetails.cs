using Microsoft.CodeAnalysis;
using static Joinery.Dovetail.Helpers;

namespace Joinery.Dovetail.Support;

internal static class ImportDovetails
{
    /// <summary>
    ///     What the imports need from the compilation: the export methods of the referenced assemblies, and whether Specular's
    ///     provider interface is there to seed. Read on every compilation, but cheap, and equal from one edit to the next unless
    ///     the references change, so the imports file isn't generated again.
    /// </summary>
    public sealed record ReferencedExports(EquatableArray<string> ExportMethods, bool HasSpecularProviderInterface)
    {
        /// <summary>
        ///     Reads the references in one pass. It runs on every compilation, which is every edit, so an assembly without Dovetail's
        ///     metadata (nearly all of them) costs a look at its attributes and nothing else.
        /// </summary>
        public static ReferencedExports From(Compilation compilation)
        {
            List<DovetailConfigurationData>? exports = null;
            var hasSpecularProviderInterface = false;
            foreach (var reference in compilation.References)
            {
                if (compilation.GetAssemblyOrModuleSymbol(reference) is not IAssemblySymbol assembly) continue;
                if (!hasSpecularProviderInterface && SpecularProviderSeed.DefinesProviderInterface(assembly)) hasSpecularProviderInterface = true;
                if (DovetailConfigurationData.FromExportMetadata(assembly) is { } export) ( exports ??= [] ).Add(export);
            }

            if (exports is null) return new(default, hasSpecularProviderInterface);

            exports.Sort();
            var methods = new string[exports.Count];
            for (var i = 0; i < methods.Length; i++)
            {
                methods[i] = exports[i].ToString();
            }

            return new(new([.. methods]), hasSpecularProviderInterface);
        }
    }

    public static void HandleDovetailImports(SourceProductionContext context, Request request)
    {
        var buildConfig = request.BuildConfig;
        if (!buildConfig.ImportAssembly) return;

        var exportConfig = buildConfig.ExportConfiguration;
        var referenceMethods = request
                              .References.ExportMethods
                              .Concat(request.HasExports || request.HasConfigurationOptions ? [$"{( exportConfig.Namespace is { Length: > 0 } ? $"{exportConfig.Namespace}." : "" )}{exportConfig.ClassName}.{exportConfig.MethodName}"] : [])
                              .ToList();

        // Only pin the host type when the build says what it is: an explicit DovetailHostType first, then test projects.
        // Pinning it unconditionally (it used to always be UnitTest) wins over the hosting helpers'
        // AddIfMissing(DovetailHostType.Live), so real applications ran UnitTest-only joints and skipped Live-only ones.
        var hostTypeName = PinnableHostTypes.FirstOrDefault(z => string.Equals(z, buildConfig.HostType.Trim(), StringComparison.OrdinalIgnoreCase))
         ?? ( buildConfig.IsTestProject ? "UnitTest" : null );

        // An application that references Specular gets its provider in the context without setting it by hand.
        var seedsSpecularProvider = buildConfig.SpecularEmitsProvider && request.References.HasSpecularProviderInterface;
        var importConfiguration = buildConfig.ImportConfiguration;

        var writer = new SourceWriter();
        if (seedsSpecularProvider) writer.Line($"#pragma warning disable {SpecularProviderSeed.NameConflictWarning}");
        writer
           .WriteSharedHeader()
           .Line("using System;")
           .Line("using System.Collections.Generic;")
           .Line("using System.Runtime.Loader;")
           .Line("using Microsoft.Extensions.DependencyInjection;")
           .Line("using Joinery.Dovetail;")
           .Line("using Joinery.Dovetail.Infrastructure;")
           .BlankLine();
        importConfiguration.WriteAttributes(writer);

        var inNamespace = importConfiguration.Namespace is { Length: > 0 };
        if (inNamespace) writer.Line($"namespace {importConfiguration.Namespace}").OpenBlock();

        writer
           .Summary("The class defined for importing Dovetail parts into this assembly")
           .Line(CompilerGeneratedAttributes)
           .Line($"internal static partial class {importConfiguration.ClassName}")
           .OpenBlock()
           .Line($"internal static DovetailContextBuilderFactory {importConfiguration.MethodName} = {ContextBuilderFactoryName};")
           // The parameterless factory delegate assigned to ImportHelpers.Rivets. It seeds a builder to
           // resolve any convention dependencies, then materializes the final builder from the imported parts.
           .Summary("Creates the context builder populated with the Dovetail parts imported into this assembly")
           .StartLine()
           .Append("private static DovetailContextBuilder ")
           .Append(ContextBuilderFactoryName)
           .Append("(IDictionary<object, object>? properties = null, IEnumerable<DovetailCategory>? categories = null) => ")
           .Append("DovetailContextBuilder.Create(LoadDovetailJointsMethod(), properties ?? new Dictionary<object, object>(), ")
           .Append(hostTypeName is null ? "" : $"DovetailHostType.{hostTypeName}, ")
           .Append("categories ?? [])")
           .Append(seedsSpecularProvider ? SpecularProviderSeed.SeedCall : "")
           .Append(";")
           .EndLine()
           .Summary("The Dovetail parts imported into this assembly")
           .Line("private static IEnumerable<IDovetailJointMetadata> LoadDovetailJointsMethod()")
           .OpenBlock();
        if (referenceMethods.Count == 0) writer.Line("yield break;");
        foreach (var reference in referenceMethods)
        {
            writer.Line($"foreach (var part in {reference}())").Indented("yield return part;");
        }

        writer.CloseBlock();

        if (buildConfig.IsTestProject || buildConfig.AssignExternal)
        {
            writer
               .BlankLine()
               .Line("[System.Runtime.CompilerServices.ModuleInitializer, System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]")
               .Line("internal static void Init()")
               .OpenBlock()
               .Line($"ImportHelpers.Rivets = {importConfiguration.MethodName};")
               .CloseBlock();
        }

        writer.CloseBlock();
        if (inNamespace) writer.CloseBlock();

        if (seedsSpecularProvider) writer.Line($"#pragma warning restore {SpecularProviderSeed.NameConflictWarning}");
        context.AddSource("Imported_Assembly_Dovetails.g.cs", writer.WriteSharedFooter().ToSourceText());
    }

    /// <summary>The name of the generated method that creates the context builder.</summary>
    internal const string ContextBuilderFactoryName = "CreateDovetailContextBuilder";

    /// <summary>The <c>DovetailHostType</c> values a build may pin; anything else (e.g. <c>Undefined</c>) leaves it to the host.</summary>
    private static readonly string[] PinnableHostTypes = ["Live", "UnitTest"];

    /// <summary>What the imports file is generated from.</summary>
    /// <param name="BuildConfig">The build properties.</param>
    /// <param name="References">What the imports need from the compilation.</param>
    /// <param name="HasExports">Whether this assembly exports any joint, so its own exports are imported too.</param>
    /// <param name="HasConfigurationOptions">Whether this assembly declares Optionary options, which its exports register.</param>
    public record Request(MsBuildConfig BuildConfig, ReferencedExports References, bool HasExports, bool HasConfigurationOptions);
}
