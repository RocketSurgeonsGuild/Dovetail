namespace SdkSample.Notifications;

public interface INotifier
{
    /// <summary>
    ///     Sends <paramref name="message" /> the way <see cref="NotificationsOptions" /> says and describes what happened.
    /// </summary>
    string Notify(string message, NotificationLevel level);
}
