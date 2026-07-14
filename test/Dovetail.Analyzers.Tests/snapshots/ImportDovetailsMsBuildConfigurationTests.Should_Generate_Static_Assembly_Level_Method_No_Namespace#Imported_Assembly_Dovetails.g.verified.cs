//HintName: Dovetail.Analyzers/Dovetail.DovetailAttributesGenerator/Imported_Assembly_Dovetails.g.cs
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Dovetail;
using Dovetail.Infrastructure;

#nullable enable
#pragma warning disable CA1002, CA1034, CA1822, CS0105, CS1573, CS8602, CS8603, CS8618, CS8669
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Import.Property", "Import")]
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Import.Namespace", "")]
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Import.ClassName", "MyImports")]
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Import.MethodName", "Rivet")]
/// <summary>
/// The class defined for importing Dovetail parts into this assembly
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("Dovetail.Analyzers", "version"), System.Runtime.CompilerServices.CompilerGenerated, System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal static partial class MyImports
{
    internal static DovetailContextBuilderFactory Rivet = CreateDovetailContextBuilder;
    /// <summary>
    /// Creates the context builder populated with the Dovetail parts imported into this assembly
    /// </summary>
    private static DovetailContextBuilder CreateDovetailContextBuilder(IDictionary<object, object>? properties = null, IEnumerable<DovetailCategory>? categories = null) => DovetailContextBuilder.Create(LoadDovetailJointsMethod(), properties ?? new Dictionary<object, object>(), categories ?? []);
    /// <summary>
    /// The Dovetail parts imported into this assembly
    /// </summary>
    private static IEnumerable<IDovetailJointMetadata> LoadDovetailJointsMethod()
    {
        foreach (var part in Dep2Exports.Rivet())
            yield return part;
        foreach (var part in Dep1.Dep1Exports.Rivet())
            yield return part;
        foreach (var part in SampleDependencyThree.Dovetails.Exports.Rivet())
            yield return part;
    }
};
#pragma warning restore CA1002, CA1034, CA1822, CS0105, CS1573, CS8602, CS8603, CS8618, CS8669
#nullable restore
