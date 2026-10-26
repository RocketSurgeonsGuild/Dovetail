namespace SdkSample.Notifications;

/// <summary>
///     The ways a notification can be delivered. A <see cref="FlagsAttribute" /> enum: in configuration, combine the names
///     with commas, for example <c>Email, Push</c>.
/// </summary>
[Flags]
public enum NotificationChannels
{
    /// <summary>No delivery at all.</summary>
    None = 0,

    /// <summary>Send an email.</summary>
    Email = 1,

    /// <summary>Send a text message.</summary>
    Sms = 2,

    /// <summary>Send a push notification.</summary>
    Push = 4,
}
