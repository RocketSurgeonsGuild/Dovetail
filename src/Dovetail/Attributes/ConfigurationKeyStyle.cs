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
///         <see cref="System.Text.Json.JsonNamingPolicy.CamelCase" /> and <see cref="Dash" /> is
///         <see cref="System.Text.Json.JsonNamingPolicy.KebabCaseLower" />.
///     </para>
///     <para>
///         The generated binder exists once for each type, so a type reached from sections of different styles is reported as
///         DT0016.
///     </para>
/// </remarks>
[PublicAPI]
public enum ConfigurationKeyStyle
{
    /// <summary>
    ///     The declared name with its first letter upper-cased: <c>RetryCount</c> stays <c>RetryCount</c>, and
    ///     <c>GreetingsOptions</c> is the section <c>Greetings</c>. This is the default.
    /// </summary>
    Pascal = 0,

    /// <summary>
    ///     Lower camel case: <c>RetryCount</c> becomes <c>retryCount</c>, <c>HTTPServer</c> becomes <c>httpServer</c>, and
    ///     <c>HttpClientOptions</c> is the section <c>httpClient</c>. Configuration keys ignore case, so this only changes the
    ///     spelling the schema suggests.
    /// </summary>
    Camel = 1,

    /// <summary>
    ///     Lower-case words joined with <c>-</c>: <c>RetryCount</c> becomes <c>retry-count</c>, and <c>HttpClientOptions</c> is the
    ///     section <c>http-client</c>. The binder reads exactly these keys, so a value written as <c>RetryCount</c> no longer binds.
    /// </summary>
    Dash = 2,
}
