using System.Reflection;
using Dovetail.Infrastructure;

namespace Dovetail.Diagnostics;

/// <summary>
///  Extension methods for <see cref="IDovetailJointMetadata" /> to provide additional diagnostic information.
/// </summary>
public static class DovetailMetadataDiagnosticExtensions
{

    extension(IDovetailJointMetadata metadata)
    {
        /// <summary>
        ///    The assembly that is executing the convention
        /// </summary>
        public Assembly Assembly => metadata.Joint is IDovetailJointDelegate jointDelegate ? jointDelegate.Assembly : metadata.Joint.GetType().Assembly;

        /// <summary>
        ///    The name of the joint
        /// </summary>
        public string JointName => metadata.Joint is IDovetailJointDelegate jointDelegate ? jointDelegate.Expression : metadata.Joint.GetType().FullName!;

        /// <summary>
        ///   Gets the names of the steps that this joint implements
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Trimming", "IL2075:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.", Justification = "This is a diagnostics helper that reflects over already-instantiated joints; their attributes are not trimmable away.")]
        public IEnumerable<string> GetStepNames() => metadata.Joint.GetType().GetInterfaces()
                       .Where(z => z.IsAssignableTo(typeof(IDovetailJoint)) && z != typeof(IDovetailJoint))
                       .Select(t => t.GetCustomAttribute<JointNameAttribute>()?.Name!)
                       .Where(z => !string.IsNullOrWhiteSpace(z));

        /// <summary>
        ///   Gets the types of the steps that this joint is connected to
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetConnectedTo() => metadata.Joint.GetType()
               .GetCustomAttributesData()
               .Where(z => z.AttributeType.IsGenericType && z.AttributeType.GetGenericTypeDefinition() == typeof(ConnectsJointAttribute<>))
               .Select(z => z.AttributeType.GetGenericArguments()[0]);

        /// <summary>
        ///    Gets the names of the steps that this joint is connected to
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetConnectedToStepNames() => metadata.GetConnectedTo()
               .Select(z => z.GetCustomAttribute<JointNameAttribute>()?.Name!)
               .Where(z => !string.IsNullOrWhiteSpace(z));

    }
}
