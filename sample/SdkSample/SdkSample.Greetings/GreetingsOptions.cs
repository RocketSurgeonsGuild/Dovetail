using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dovetail.Attributes;

namespace SdkSample.Greetings;

/// <summary>
///     Options for the greetings this library prints. Bound from the <c>Greetings</c> section (the type name without its
///     <c>Options</c> suffix) and described in the application's <c>appsettings.schema.json</c>.
/// </summary>
[DovetailConfiguration]
public sealed class GreetingsOptions
{
    /// <summary>
    ///     What the greeting says before the name. Unset means the library's configuration file didn't load.
    /// </summary>
    [Description("The text that comes before the name, e.g. \"Hello\".")]
    public string? Message { get; set; }

    /// <summary>
    ///     How many times the greeting is said.
    /// </summary>
    [Range(1, 5)]
    public int Repeat { get; set; } = 1;
}
