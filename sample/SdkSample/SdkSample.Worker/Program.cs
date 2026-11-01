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
