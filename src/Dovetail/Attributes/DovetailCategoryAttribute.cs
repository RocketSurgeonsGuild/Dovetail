namespace Dovetail.Attributes;
/// <summary>
///     Defines the category of a given convention
/// </summary>
[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class DovetailCategoryAttribute(string category) : Attribute
{
    /// <summary>
    ///     The category of a given convention
    /// </summary>
    public DovetailCategory Category { get; } = category;
}
