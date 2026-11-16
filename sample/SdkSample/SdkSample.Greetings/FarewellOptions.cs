using System.ComponentModel.DataAnnotations;
using Optionary;

namespace SdkSample.Greetings;

/// <summary>
///     Options for the sign-off. An immutable positional record: Optionary binds it through a generated options factory, which
///     calls the constructor with the values under the <c>Farewell</c> section.
/// </summary>
/// <param name="Message">What the sign-off says before the name.</param>
/// <param name="Exclamations">How many exclamation marks end it.</param>
[Optionary]
public sealed record FarewellOptions(string Message, [property: Range(0, 5)] int Exclamations);
