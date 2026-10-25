using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SdkSample.Diagnostics;
using SdkSample.Greetings;
using SdkSample.Notifications;
using SdkSample.Storage;
using SdkSample.Web;

var builder = WebApplication.CreateBuilder(args);
// Enums as names, the way they are written in configuration.
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
var app = await builder.ConfigureDovetail();

app.MapGet("/", (IGreeter greeter) => greeter.Greet("SdkSample.Web"));

// The resolved options of every library: each library's defaults, then this project's appsettings.yaml on top.
app.MapGet(
    "/config",
    (
        IOptions<GreetingsOptions> greetings,
        IOptions<FarewellOptions> farewell,
        IOptions<NotificationsOptions> notifications,
        IOptions<StorageOptions> storage,
        IOptions<ArchiveOptions> archive,
        IOptions<DiagnosticsOptions> diagnostics,
        IOptions<HostOptions> host,
        IOptions<ConsoleLifetimeOptions> console
    ) => new
    {
        Greetings = greetings.Value,
        Farewell = farewell.Value,
        Notifications = notifications.Value,
        Storage = storage.Value,
        Archive = archive.Value,
        Diagnostics = diagnostics.Value,
        Host = host.Value,
        Console = console.Value,
    }
);

await app.RunAsync();
