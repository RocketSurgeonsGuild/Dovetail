namespace Dovetail;

/// <summary>
///     A policy that determines how to handle exceptions thrown by conventions
/// </summary>
public static class DovetailExceptionPolicy
{
    /// <summary>
    ///    A policy that ignores <see cref="NotSupportedException" /> exceptions
    /// </summary>
    public static DovetailExceptionPolicyDelegate IgnoreNotSupported { get; } = exception => exception is NotSupportedException;
}
