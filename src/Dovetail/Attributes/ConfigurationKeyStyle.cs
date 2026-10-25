namespace Dovetail.Attributes;

/// <summary>
///     How <see cref="DovetailConfigurationAttribute" /> spells the configuration keys of an options type and of every object type
///     nested in it.
/// </summary>
/// <remarks>
///     <para>
///         The style decides the canonical key of every bound property and constructor parameter, in both the generated binder
///         and the configuration schema. It also decides the default section path derived from the type name. A
///         <c>[ConfigurationKeyName]</c> on a member is always used as written, and an explicit section path is never changed.
///         Dictionary keys, enum values and other data are never restyled.
///     </para>
///     <para>
///         Word splitting matches <see cref="System.Text.Json.JsonNamingPolicy" />: <see cref="Camel" /> is
///         <see cref="System.Text.Json.JsonNamingPolicy.CamelCase" />, <see cref="Dash" /> is
///         <see cref="System.Text.Json.JsonNamingPolicy.KebabCaseLower" /> and <see cref="Underscore" /> is
///         <see cref="System.Text.Json.JsonNamingPolicy.SnakeCaseLower" />.
///     </para>
///     <para>
///         The generated binder exists once for each type, so a type reached from sections of styles that spell its keys
///         differently is reported as DT0016.
///     </para>
/// </remarks>
[PublicAPI]
public enum ConfigurationKeyStyle
{
    /// <summary>
    ///     Nothing is respelled: the canonical key is the declared name exactly as written, and the derived section path is the
    ///     type name without its suffix. Configuration keys ignore case, so data in any casing binds, as long as the letters are
    ///     right. This is the default.
    /// </summary>
    Undefined = 0,

    /// <summary>
    ///     The declared name with its first letter upper-cased: <c>maxFileSizeMb</c> becomes <c>MaxFileSizeMb</c>, and
    ///     <c>GreetingsOptions</c> is the section <c>Greetings</c>. For the usual C# property name this is the same as
    ///     <see cref="Undefined" />.
    /// </summary>
    Pascal = 1,

    /// <summary>
    ///     Lower camel case: <c>MaxFileSizeMb</c> becomes <c>maxFileSizeMb</c>, <c>HTTPServer</c> becomes <c>httpServer</c>, and
    ///     <c>HttpClientOptions</c> is the section <c>httpClient</c>. Configuration keys ignore case, so this only changes the
    ///     spelling the schema suggests.
    /// </summary>
    Camel = 2,

    /// <summary>
    ///     Lower-case words joined with <c>-</c>: <c>MaxFileSizeMb</c> becomes <c>max-file-size-mb</c>, and
    ///     <c>HttpClientOptions</c> is the section <c>http-client</c>. The binder reads exactly these keys, so a value written as
    ///     <c>MaxFileSizeMb</c> no longer binds.
    /// </summary>
    Dash = 3,

    /// <summary>
    ///     Lower-case words joined with <c>_</c>: <c>MaxFileSizeMb</c> becomes <c>max_file_size_mb</c>, and
    ///     <c>HttpClientOptions</c> is the section <c>http_client</c>. The binder reads exactly these keys, so a value written as
    ///     <c>MaxFileSizeMb</c> no longer binds.
    /// </summary>
    Underscore = 4,
}
