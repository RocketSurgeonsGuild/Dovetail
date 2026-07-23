using System.Collections.Immutable;
using System.Reflection;
using Dovetail.Attributes;
using Dovetail.Infrastructure;

namespace Dovetail.Diagnostics;


/// <summary>
///  Provides extension methods for retrieving metadata from Dovetail context builders and contexts.
/// </summary>
public static class DiagnosticExtensions
{
    private static string GetJointName(Type jointType) => jointType.FullName!;

    /// <summary>
    ///   Gets the names of the steps that this joint implements
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<string> GetStepNames([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type jointType) => jointType.GetInterfaces()
                   .Where(z => z.IsAssignableTo(typeof(IDovetailJoint)) && z != typeof(IDovetailJoint))
                   .Select(t => t.GetCustomAttribute<JointNameAttribute>()?.Name!)
                   .Where(z => !string.IsNullOrWhiteSpace(z));

    /// <summary>
    ///   Gets the types of the steps that this joint is connected to
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Type> GetConnectedTo(Type jointType) => jointType.GetCustomAttributesData()
           .Where(z => z.AttributeType.IsGenericType && z.AttributeType.GetGenericTypeDefinition() == typeof(ConnectsJointAttribute<>))
           .Select(z => z.AttributeType.GetGenericArguments()[0]);

    /// <summary>
    ///    Gets the names of the steps that this joint is connected to
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<string> GetConnectedToStepNames(Type jointType) => GetConnectedTo(jointType)
           .Select(z => z.GetCustomAttribute<JointNameAttribute>()?.Name!)
           .Where(z => !string.IsNullOrWhiteSpace(z));

    private static DovetailDiagnosticMetadataJoint Create(IReadOnlyList<IDovetailJointMetadata> allMetadata, IDovetailJointMetadata metadata)
    {
        var jointName = metadata.JointName;
        var deps = allMetadata.Join(metadata.Dependencies, x => x.Joint.GetType(), y => y.Type, (x, y) => Create(allMetadata, y)!)
        .Where(x => x != null)
        .OrderBy(x => x.JointName)
        .ToImmutableList();
        return new DovetailDiagnosticMetadataJoint(
            jointName,
metadata.Assembly.GetName().Name!,
            [.. metadata.GetStepNames()],
            [.. metadata.GetConnectedToStepNames()],
             metadata.HostType,
              metadata.Category,
               deps
            );
    }
    private static DovetailDiagnosticMetadataJointDependency? Create(IReadOnlyList<IDovetailJointMetadata> allMetadata, IDovetailDependency dependency)
    {
        var metadata = allMetadata.FirstOrDefault(x => x.Joint.GetType() == dependency.Type);
        var jointName = GetJointName(dependency.Type);
        var deps = allMetadata
        .Join(metadata?.Dependencies ?? [], x => x.Joint.GetType(), y => y.Type, (x, y) => Create(allMetadata, y)!)
        .Where(x => x != null)
        .OrderBy(x => x.JointName)
        .ToImmutableList();
        return new DovetailDiagnosticMetadataJointDependency(
            metadata?.JointName ?? GetJointName(dependency.Type),
metadata?.Assembly.GetName().Name ?? dependency.Type.Assembly.GetName().Name!,
             [.. metadata?.GetStepNames() ?? GetStepNames(dependency.Type)],
[.. metadata?.GetConnectedToStepNames() ?? GetConnectedToStepNames(dependency.Type)],
            metadata?.HostType ?? dependency.Type.GetCustomAttribute<DovetailHostTypeAttribute>()?.HostType ?? DovetailHostType.Undefined,
            metadata?.Category ?? dependency.Type.GetCustomAttribute<DovetailCategoryAttribute>()?.Category ?? "Undefined",
             deps,
             dependency.Direction
        );
    }

    extension(DovetailContextBuilder builder)
    {
        /// <summary>
        ///   Renders the resolved joints of the context as a Mermaid flowchart diagram.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DovetailDiagnosticMetadataJoint> GetDiagnosticMetadata(params IEnumerable<DovetailCategory> categories) =>
        DovetailResolver.ResolveMetadata([.. categories], builder.Metadata)
        .Select(item => Create(builder.Metadata, item))
        .ToImmutableList();
    }

    extension(IDovetailContext builder)
    {
        /// <summary>
        ///   Renders the resolved joints of the context as a Mermaid flowchart diagram.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DovetailDiagnosticMetadataJoint> GetDiagnosticMetadata(params IEnumerable<DovetailCategory> categories) =>
        DovetailResolver.ResolveMetadata([.. categories], builder.Metadata)
        .Select(item => Create(builder.Metadata, item))
        .ToImmutableList();
    }
}
