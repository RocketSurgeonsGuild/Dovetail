using Microsoft.Extensions.DependencyInjection;

var builder = Host
   .CreateApplicationBuilder(args);
builder.Services.Configure<ConsoleLifetimeOptions>(z => z.SuppressStatusMessages = true);
await builder
     .ConfigureDovetail(z => z.ConfigureCommandLine(configurator => configurator.AddCommand<Dump>("dump")))
     .RunConsoleAppAsync();
