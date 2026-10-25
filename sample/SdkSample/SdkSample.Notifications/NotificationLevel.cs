namespace SdkSample.Notifications;

/// <summary>
///     How serious a notification is. A plain enum: the schema lists its names, and the binder accepts them in any casing.
/// </summary>
public enum NotificationLevel
{
    /// <summary>Nothing needs doing.</summary>
    Info,

    /// <summary>Something needs a look soon.</summary>
    Warning,

    /// <summary>Something needs a look now.</summary>
    Error,
}
