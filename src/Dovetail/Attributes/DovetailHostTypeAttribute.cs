namespace Dovetail.Attributes;
/// <summary>
///     Defines the host type of a given convention
/// </summary>
[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class DovetailHostTypeAttribute(DovetailHostType hostType) : Attribute
{
    /// <summary>
    ///     The host type of a given convention
    /// </summary>
    public DovetailHostType HostType { get; } = hostType;
}
