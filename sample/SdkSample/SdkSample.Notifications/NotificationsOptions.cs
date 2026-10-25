using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dovetail.Attributes;

namespace SdkSample.Notifications;

/// <summary>
///     Options for the notifications this library sends. Bound from the <c>Notifications</c> section (the type name without its
///     <c>Options</c> suffix). It shows how the schema describes enums, durations, URIs, lists, dictionaries of nested objects
///     and the annotations Dovetail understands.
/// </summary>
[DovetailConfiguration]
public sealed class NotificationsOptions
{
    /// <summary>
    ///     Turns every notification off without removing the configuration.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///     The least serious level that is still delivered; anything below it is dropped.
    /// </summary>
    public NotificationLevel MinimumLevel { get; set; } = NotificationLevel.Warning;

    /// <summary>
    ///     The channels used when a notification doesn't pick its own. Combine several with commas.
    /// </summary>
    public NotificationChannels DefaultChannels { get; set; } = NotificationChannels.Email;

    /// <summary>
    ///     How long to wait for a delivery before giving up, as a duration such as <c>00:00:30</c>.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    ///     The notification service's address. Unset means notifications are only written to the log.
    /// </summary>
    public Uri? ServiceUri { get; set; }

    /// <summary>
    ///     The people who receive notifications, one address per entry.
    /// </summary>
    public List<string> Recipients { get; set; } = [];

    /// <summary>
    ///     Per-channel settings, keyed by channel name (<c>Email</c>, <c>Sms</c>, <c>Push</c>).
    /// </summary>
    public Dictionary<string, ChannelOptions> Channels { get; set; } = [];

    /// <summary>
    ///     The address notifications come from.
    /// </summary>
    [Description("The sender address shown on email notifications, e.g. \"alerts@example.com\".")]
    [EmailAddress]
    public string? Sender { get; set; }

    /// <summary>
    ///     The address notifications used to come from.
    /// </summary>
    [Obsolete("Use Sender instead.")]
    public string? From { get; set; }
}
