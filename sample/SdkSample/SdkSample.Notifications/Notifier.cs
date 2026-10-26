using Microsoft.Extensions.Options;

namespace SdkSample.Notifications;

internal sealed class Notifier(IOptions<NotificationsOptions> notifications) : INotifier
{
    public string Notify(string message, NotificationLevel level)
    {
        var options = notifications.Value;
        if (!options.Enabled) return $"Notifications are disabled; dropped '{message}'.";
        if (level < options.MinimumLevel) return $"Dropped '{message}': {level} is below the minimum level {options.MinimumLevel}.";

        // Nothing is really sent; the point is that every value below came from configuration.
        var channels = new List<string>();
        foreach (var channel in Enum.GetValues<NotificationChannels>())
        {
            if (channel == NotificationChannels.None || !options.DefaultChannels.HasFlag(channel)) continue;
            if (!options.Channels.TryGetValue(channel.ToString(), out var settings)) channels.Add(channel.ToString());
            else if (settings.Enabled) channels.Add($"{channel} ({settings.RateLimitPerMinute}/min)");
        }

        var recipients = options.Recipients.Count == 0 ? "nobody" : string.Join(", ", options.Recipients);
        return $"Sent '{message}' as {level} from {options.Sender ?? "(no sender)"} via {string.Join(" + ", channels)} to {recipients} (timeout {options.Timeout.TotalSeconds:0}s).";
    }
}
