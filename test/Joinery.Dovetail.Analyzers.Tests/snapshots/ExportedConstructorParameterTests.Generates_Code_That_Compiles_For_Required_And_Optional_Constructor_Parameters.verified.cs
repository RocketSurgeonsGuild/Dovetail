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
        yield return new DovetailJointMetadata(static services => new Sample.Constructed(Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService<Joinery.Dovetail.Analyzers.Tests.IWidget>(services) ?? throw new System.InvalidOperationException("Unable to resolve a value of type 'Joinery.Dovetail.Analyzers.Tests.IWidget' for parameter 'widget' of joint 'Sample.Constructed' from the Dovetail context. Set it on the context builder before the joint is created, or give the parameter a default value."), Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService<Joinery.Dovetail.Analyzers.Tests.WidgetOptions>(services), Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService<string>(services) ?? "fallback", (System.Nullable<int>)services.GetService(typeof(int)) ?? (int)3, (System.Nullable<Joinery.Dovetail.Analyzers.Tests.WidgetMode>)services.GetService(typeof(Joinery.Dovetail.Analyzers.Tests.WidgetMode)) ?? (Joinery.Dovetail.Analyzers.Tests.WidgetMode)0, Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService<System.Nullable<int>>(services)), DovetailHostType.Undefined, DovetailCategory.Application);
    }
}
#pragma warning restore CA1002, CA1034, CA1822, CS0105, CS1573, CS8602, CS8603, CS8618, CS8669
#nullable restore
