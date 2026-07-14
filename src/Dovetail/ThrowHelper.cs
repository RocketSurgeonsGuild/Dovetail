using System.Reflection;
using Dovetail.Infrastructure;

namespace Dovetail;

internal static class ThrowHelper
{
    public static Type EnsureTypeIsDovetail(Type type)
    {
        return !typeof(IDovetailJoint).IsAssignableFrom(type)
            ? throw new NotSupportedException("Type must inherit from " + nameof(IDovetailJoint))
            : type;
    }

    public static TypeInfo EnsureTypeIsDovetail(TypeInfo type)
    {
        return !typeof(IDovetailJoint).IsAssignableFrom(type)
            ? throw new NotSupportedException("Type must inherit from " + nameof(IDovetailJoint))
            : type;
    }
}
