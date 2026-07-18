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
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Import.ClassName", "Imports")]
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Import.MethodName", "Rivet")]
/// <summary>
/// The class defined for importing Dovetail parts into this assembly
/// </summary>
/// <remarks>
/// <code>
/// ```mermaid
/// flowchart TD
///     Imports["Imports"]
///     Exports_Rivet["Exports.Rivet"] --&gt; Imports
///     subgraph Joints["Imports joints"]
///         Dovetail_Tests_A["A"]
///         Dovetail_Tests_B["B"]
///         Dovetail_Tests_B --&gt; Dovetail_Tests_A
///         Dovetail_Tests_A --&gt; Dovetail_Tests_B
///     end
/// ```
/// </code>
/// </remarks>
[System.CodeDom.Compiler.GeneratedCode("Dovetail.Analyzers", "version"), System.Runtime.CompilerServices.CompilerGenerated, System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal static partial class Imports
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
        foreach (var part in Exports.Rivet())
            yield return part;
    }

    /// <summary>
    /// The Mermaid diagram of this assembly's Dovetail import and joint dependency graph, embedded in the class documentation above
    /// </summary>
    internal static string GetMermaidDiagram() => "flowchart TD\n    Imports[\"Imports\"]\n    Exports_Rivet[\"Exports.Rivet\"] --> Imports\n    subgraph Joints[\"Imports joints\"]\n        Dovetail_Tests_A[\"A\"]\n        Dovetail_Tests_B[\"B\"]\n        Dovetail_Tests_B --> Dovetail_Tests_A\n        Dovetail_Tests_A --> Dovetail_Tests_B\n    end";
};
#pragma warning restore CA1002, CA1034, CA1822, CS0105, CS1573, CS8602, CS8603, CS8618, CS8669
#nullable restore
