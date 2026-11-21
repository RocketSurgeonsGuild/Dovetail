using Microsoft.Extensions.Configuration;
using SdkSample.Diagnostics;
using SdkSample.Greetings;
using SdkSample.Notifications;
using SdkSample.Storage;
using SdkSample.Worker;

var builder = Host.CreateApplicationBuilder(args);
using var host = await builder.ConfigureDovetail();

// One short summary per library. Each service reads its options, which Dovetail bound from the library's own YAML file
// (SdkSample.<Library>.yaml) with this project's appsettings.yaml (SdkSample.Worker.yaml) layered on top.
var greeter = host.Services.GetRequiredService<IGreeter>();
Console.WriteLine(greeter.Greet("SdkSample.Worker"));
Console.WriteLine(greeter.Farewell("SdkSample.Worker"));
Console.WriteLine(host.Services.GetRequiredService<INotifier>().Notify("Worker started", NotificationLevel.Info));
Console.WriteLine(host.Services.GetRequiredService<IStore>().Describe());
Console.WriteLine(host.Services.GetRequiredService<IDiagnosticsReport>().Describe());

// Run by SdkSample.AppHost, the worker references the web application: Aspire hands it web's endpoints as configuration
// (services:web:...), which the service discovery SdkSample.ServiceDefaults registers resolves for https+http://web. The host is started
// so that OpenTelemetry exports the call, and the dashboard shows it in one trace with the request it made to web.
if (host.Services.GetRequiredService<IConfiguration>().GetSection("services:web").Exists())
{
    await host.StartAsync();
    var web = host.Services.GetRequiredService<IHttpClientFactory>().CreateClient();
    Console.WriteLine($"web says: {await web.GetStringAsync("https+http://web/")}");
    await host.StopAsync();
}
