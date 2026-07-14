using System.Diagnostics;

namespace Dovetail.Attributes;
/// <summary>
///     Defines this convention as one that is exported to the Dovetail source generator
/// </summary>
/// <remarks>
///     Only works with source generators enabled.
/// </remarks>
/// <seealso cref="Attribute" />
[PublicAPI]
[AttributeUsage(AttributeTargets.Class)]
[Conditional("CodeGeneration")]
public sealed class DovetailExportAttribute : Attribute;
