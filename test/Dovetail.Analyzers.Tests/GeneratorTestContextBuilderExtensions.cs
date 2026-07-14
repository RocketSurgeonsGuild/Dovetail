using FluentValidation;

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Dovetail.Analyzers.Tests;

internal static class GeneratorTestContextBuilderExtensions
{
    public static GeneratorTestContextBuilder AddSharedDeps(this GeneratorTestContextBuilder builder, CancellationToken cancellationToken) => builder.AddCompilationReferences(GenerationHelpers.CreateDeps(builder, cancellationToken).GetAwaiter().GetResult());

    public static GeneratorTestContextBuilder AddSharedGenericDeps(this GeneratorTestContextBuilder builder, CancellationToken cancellationToken) => builder.AddCompilationReferences(GenerationHelpers.CreateGenericDeps(builder, cancellationToken).GetAwaiter().GetResult());

    /// <summary>
    ///     Supplies the Dovetail import/export configuration build properties that the Dovetail MSBuild targets
    ///     (<c>Dovetail.targets</c>) provide during a real build. The generator reads these directly, so the test
    ///     harness must mirror them or the generated code resolves to <c>##??NOT DEFINED??##</c> and fails to compile.
    /// </summary>
    public static GeneratorTestContextBuilder AddDovetailConfiguration(
        this GeneratorTestContextBuilder builder,
        string importNamespace,
        string exportNamespace,
        string importClassName = "Imports",
        string importMethodName = "Rivet",
        string exportClassName = "Exports",
        string exportMethodName = "Rivet"
    ) => builder
        .AddGlobalOption("build_property.DovetailImportNamespace", importNamespace)
        .AddGlobalOption("build_property.DovetailImportClassName", importClassName)
        .AddGlobalOption("build_property.DovetailImportMethodName", importMethodName)
        .AddGlobalOption("build_property.DovetailExportNamespace", exportNamespace)
        .AddGlobalOption("build_property.DovetailExportClassName", exportClassName)
        .AddGlobalOption("build_property.DovetailExportMethodName", exportMethodName);

    public static GeneratorTestContextBuilder AddCommonReferences(this GeneratorTestContextBuilder builder) => builder.AddReferences(
        typeof(ActivatorUtilities),
        typeof(DovetailContext),
        typeof(IServiceProvider),
        typeof(IConfiguration),
        typeof(IValidator)
        );

    public static GeneratorTestContextBuilder AddCommonGenerators(this GeneratorTestContextBuilder builder)
    {
        foreach (var generator in GetAllGenerators(typeof(GeneratorTestContextBuilderExtensions).Assembly.GetIndagoProvider()))
        {
            builder = builder.WithGenerator(generator);
        }

        return builder;
    }

    private static IEnumerable<Type> GetAllGenerators(IIndagoProvider provider) => provider.GetTypes(s => s
                                                                                              .FromAssemblyOf<DovetailAttributesGenerator>()
                                                                                              .GetTypes(f => f.WithAttribute<GeneratorAttribute>().AssignableTo<IIncrementalGenerator>())
        );
}
