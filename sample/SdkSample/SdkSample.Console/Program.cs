using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SdkSample.Console;
using SdkSample.Greetings;

using var host = await Host.CreateApplicationBuilder(args).ConfigureDovetail();

var greeter = host.Services.GetRequiredService<IGreeter>();
Console.WriteLine(greeter.Greet("SdkSample.Console"));
Console.WriteLine(greeter.Farewell("SdkSample.Console"));
