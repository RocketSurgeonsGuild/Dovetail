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
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Import.Namespace", "SampleDependencyThree")]
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Import.ClassName", "Imports")]
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Import.MethodName", "Rivet")]
namespace SampleDependencyThree
{
    /// <summary>
    /// The class defined for importing Dovetail parts into this assembly
    /// </summary>
    /// <remarks>
    /// <code>
    /// ```mermaid
    /// flowchart TD
    ///     Imports["Imports"]
    ///     Dep1_Dep1Exports_Rivet["Dep1.Dep1Exports.Rivet"] --&gt; Imports
    ///     SampleDependencyThree_Dovetails_Exports_Rivet["SampleDependencyThree.Dovetails.Exports.Rivet"] --&gt; Imports
    ///     subgraph Joints["Imports joints"]
    ///         Sample_DependencyThree_Class3["Class3"]
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
            foreach (var part in Dep1.Dep1Exports.Rivet())
                yield return part;
            foreach (var part in SampleDependencyThree.Dovetails.Exports.Rivet())
                yield return part;
        }

        /// <summary>
        /// The Mermaid diagram of this assembly's Dovetail import and joint dependency graph, embedded in the class documentation above
        /// </summary>
        internal static string GetMermaidDiagram() => "flowchart TD\n    Imports[\"Imports\"]\n    Dep1_Dep1Exports_Rivet[\"Dep1.Dep1Exports.Rivet\"] --> Imports\n    SampleDependencyThree_Dovetails_Exports_Rivet[\"SampleDependencyThree.Dovetails.Exports.Rivet\"] --> Imports\n    subgraph Joints[\"Imports joints\"]\n        Sample_DependencyThree_Class3[\"Class3\"]\n    end";
    };
}
#pragma warning restore CA1002, CA1034, CA1822, CS0105, CS1573, CS8602, CS8603, CS8618, CS8669
#nullable restore
