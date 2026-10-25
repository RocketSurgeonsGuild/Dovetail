using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SdkSample.Greetings;
using SdkSample.Worker;

var builder = Host.CreateApplicationBuilder(args);
using var host = await builder.ConfigureDovetail();

var greeter = host.Services.GetRequiredService<IGreeter>();
Console.WriteLine(greeter.Greet("SdkSample.Worker"));
Console.WriteLine(greeter.Farewell("SdkSample.Worker"));
