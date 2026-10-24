using System.Diagnostics;

namespace Dovetail.Attributes;

/// <summary>
///     Declares a configuration section for an options type. Dovetail's source generator describes the type as JSON Schema,
///     which is merged into the application's <c>appsettings.schema.json</c> for editor completion and validation.
/// </summary>
/// <remarks>
///     <para>
///         On a class, the class is the options type. On the assembly, pass the options type, for types you don't own:
///         <c>[assembly: DovetailConfiguration("Hosting", typeof(HostOptions))]</c>.
///     </para>
///     <para>
///         Without a section path the section is the type name with an <c>Options</c>, <c>Settings</c>, <c>Configuration</c>
///         or <c>Config</c> suffix removed, so <c>GreetingsOptions</c> becomes <c>Greetings</c>. Nested sections use
///         <c>:</c>, as in <c>"Http:Retry"</c>. A type may be declared for several sections.
///     </para>
///     <para>Only works with source generators enabled.</para>
/// </remarks>
[PublicAPI]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
[Conditional("CodeGeneration")]
public sealed class DovetailConfigurationAttribute : Attribute
{
    /// <summary>
    ///     Declares the attributed class under the section derived from its name.
    /// </summary>
    public DovetailConfigurationAttribute() { }

    /// <summary>
    ///     Declares the attributed class under <paramref name="section" />.
    /// </summary>
    /// <param name="section">The section path, e.g. <c>"Greetings"</c> or <c>"Http:Retry"</c>.</param>
    public DovetailConfigurationAttribute(string section) => Section = section;

    /// <summary>
    ///     Declares <paramref name="type" /> under the section derived from its name. Use this form on the assembly.
    /// </summary>
    /// <param name="type">The options type.</param>
    public DovetailConfigurationAttribute(Type type) => Type = type;

    /// <summary>
    ///     Declares <paramref name="type" /> under <paramref name="section" />. Use this form on the assembly.
    /// </summary>
    /// <param name="section">The section path, e.g. <c>"Greetings"</c> or <c>"Http:Retry"</c>.</param>
    /// <param name="type">The options type.</param>
    public DovetailConfigurationAttribute(string section, Type type)
    {
        Section = section;
        Type = type;
    }

    /// <summary>
    ///     The section path, or <see langword="null" /> for the one derived from the type name.
    /// </summary>
    public string? Section { get; }

    /// <summary>
    ///     The options type when declared on the assembly; <see langword="null" /> on a class.
    /// </summary>
    public Type? Type { get; }

    /// <summary>
    ///     When several sections bind the same type, the order they are applied in: lower first, so later sections override
    ///     earlier keys. Ties are broken by section path. The default is 0.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    ///     Whether to register <c>IOptions&lt;T&gt;</c> for the type. <see langword="false" /> only describes the section in the
    ///     schema. The default is <see langword="true" />.
    /// </summary>
    public bool RegisterOptions { get; set; } = true;

    /// <summary>
    ///     Whether to validate the options when the host starts. The default is <see langword="false" />.
    /// </summary>
    public bool ValidateOnStart { get; set; }
}
