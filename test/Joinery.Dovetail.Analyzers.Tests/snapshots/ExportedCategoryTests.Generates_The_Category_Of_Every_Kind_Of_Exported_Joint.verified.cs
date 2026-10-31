#nullable enable
#pragma warning disable CA1002, CA1034, CA1822, CS0105, CS1573, CS8602, CS8603, CS8618, CS8669
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Joinery.Dovetail;
using Joinery.Dovetail.Infrastructure;

[assembly: System.Reflection.AssemblyMetadata("Dovetail.Export.Property", "Export")]
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Export.Namespace", "")]
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Export.ClassName", "Exports")]
[assembly: System.Reflection.AssemblyMetadata("Dovetail.Export.MethodName", "Rivet")]
/// <summary>
/// The class defined for exporting conventions from this assembly
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("Joinery.Dovetail.Analyzers", "version"), System.Runtime.CompilerServices.CompilerGenerated, System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public static partial class Exports
{
    /// <summary>
    /// The conventions exports from this assembly
    /// </summary>
    public static IEnumerable<IDovetailJointMetadata> Rivet()
    {
        yield return DovetailJointMetadata.FromFactory(static services => new Sample.CoreFactory(Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService<Joinery.Dovetail.Analyzers.Tests.WidgetOptions>(services)), DovetailHostType.Undefined, DovetailCategory.Core);
        yield return new DovetailJointMetadata(new Sample.CoreJoint(), DovetailHostType.Undefined, DovetailCategory.Core);
        yield return new DovetailJointMetadata(new Sample.CustomJoint(), DovetailHostType.Undefined, "Custom");
        yield return new DovetailJointMetadata(new Sample.Plain(), DovetailHostType.Undefined, DovetailCategory.Application);
        yield return DovetailJointMetadata.FromFactory(static services => new Sample.PlainFactory(Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService<Joinery.Dovetail.Analyzers.Tests.WidgetOptions>(services)), DovetailHostType.Undefined, DovetailCategory.Application);
    }
}
#pragma warning restore CA1002, CA1034, CA1822, CS0105, CS1573, CS8602, CS8603, CS8618, CS8669
#nullable restore
