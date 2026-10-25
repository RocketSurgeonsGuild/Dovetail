using System.ComponentModel.DataAnnotations;

namespace SdkSample.Notifications;

/// <summary>
///     Settings for one delivery channel. Used as the value type of <see cref="NotificationsOptions.Channels" />, so the schema
///     describes every entry under <c>Notifications:Channels:&lt;name&gt;</c> with these members.
/// </summary>
public sealed class ChannelOptions
{
    /// <summary>
    ///     Whether this channel delivers anything. A disabled channel is skipped.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///     The most notifications this channel sends in a minute; the rest wait for the next minute.
    /// </summary>
    [Range(1, 600)]
    public int RateLimitPerMinute { get; set; } = 30;

    /// <summary>
    ///     Where the channel's provider listens. Unset means the channel is only simulated.
    /// </summary>
    public Uri? Endpoint { get; set; }
}
