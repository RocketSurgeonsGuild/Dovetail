namespace Dovetail;

/// <summary>
///     A policy delegate that can be used to determine if a given exception should be ignored or not
/// </summary>
/// <param name="exception">The exception thrown by a convention.</param>
/// <returns><see langword="true" /> if the exception should be ignored; otherwise, <see langword="false" />.</returns>
public delegate bool DovetailExceptionPolicyDelegate(Exception exception);
