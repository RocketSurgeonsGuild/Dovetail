namespace Dovetail;

/// <summary>
///     A policy delegate that can be used to determine if a given exception should be ignored or not
/// </summary>
public delegate bool DovetailExceptionPolicyDelegate(Exception exception);
