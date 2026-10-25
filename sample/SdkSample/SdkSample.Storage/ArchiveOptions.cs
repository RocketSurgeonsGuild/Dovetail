using System.ComponentModel.DataAnnotations;
using Dovetail.Attributes;
using Microsoft.Extensions.Configuration;

namespace SdkSample.Storage;

/// <summary>
///     How files are archived. A class with a <see langword="required" /> member and <see langword="init" />-only properties,
///     which the options pipeline can't create on its own: Dovetail builds it with an object initializer and then binds the
///     rest. Its keys are in dash case, like <see cref="StorageOptions" />: <c>allowed-extensions</c>, and the section is
///     <c>archive</c>.
/// </summary>
[DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Dash)]
public sealed class ArchiveOptions
{
    /// <summary>
    ///     The archive's name: 3 to 31 lowercase letters, digits or dashes, starting with a letter. The section must set it.
    /// </summary>
    [RegularExpression("^[a-z][a-z0-9-]{2,30}$")]
    public required string Name { get; init; }

    /// <summary>
    ///     How many days an archived file is kept. Read from the key <c>keep-days</c>: an explicit
    ///     <c>[ConfigurationKeyName]</c> wins over the <c>retention-days</c> the dash style would give.
    /// </summary>
    [ConfigurationKeyName("keep-days")]
    [Range(1, 3650)]
    public int RetentionDays { get; init; } = 30;

    /// <summary>
    ///     The file extensions the archive accepts, including the dot. Empty means every extension.
    /// </summary>
    public string[] AllowedExtensions { get; init; } = [];
}
