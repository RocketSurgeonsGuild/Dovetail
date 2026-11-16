# Console applications

# Console applications

`Joinery.Dovetail.Sdk.Console` makes a project a Dovetail console application. You write commands and joints; the SDK writes the
entry point.

```xml
<Project Sdk="Joinery.Dovetail.Sdk.Console">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>
</Project>
```

Pin the SDK's version in `global.json` under `msbuild-sdks`, as for `Joinery.Dovetail.Sdk`, which it chains. There is no
`OutputType` to set: the SDK makes the project an `Exe`.

## What the SDK does

- Chains `Joinery.Dovetail.Sdk`, so everything Dovetail's SDK detects for an application still applies.
- Sets `DovetailConsoleApp` to `true`.
- Adds [ConsoleAppFramework](https://github.com/Cysharp/ConsoleAppFramework) (a source generator, so nothing ships with your
  application) and `Microsoft.Extensions.Hosting`. A version you reference yourself wins.
- Through `Joinery.Dovetail.Hosting`, writes `Program.cs` to `obj/` and declares the `ConsoleApp` joint.

The generated `Program` creates the generic host's `HostApplicationBuilder`, configures it with Dovetail (services,
configuration, logging, OpenTelemetry and every other joint), builds and starts the host, applies the `ConsoleApp` joints, runs the
command the command line names, and stops the host. The exit code is ConsoleAppFramework's.

## Commands and joints

Dovetail is joints only here: there are no partial methods to implement. A class marked `[DovetailExport]` implements the joints it
needs, and `IConsoleAppJoint` registers commands.

```text
using ConsoleAppFramework;
using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

public class GreetCommands(Greeter greeter)
{
    [Command("greet")]
    public void Greet(string name = "world") => Console.WriteLine(greeter.Greet(name));
}

[DovetailExport]
internal class AppJoint : IServiceJoint, IConsoleAppJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<Greeter>();

    public void Register(IDovetailContext context, ConsoleApp.ConsoleAppBuilder app) => app.Add<GreetCommands>();
}
```

Commands are built from the host's services, so they can inject anything the other joints registered. `IConsoleAppAsyncJoint` and
`ConfigureConsoleApp` on the context builder exist too, as for any joint kind.

> [!NOTE]
> ConsoleAppFramework reads the `Add` calls in your application's own compilation. The joints that add commands therefore have to live
> in the application's assembly: a library cannot contribute commands through a joint.

## Your own entry point

Set `DovetailUseDefaultProgram` to `false` to leave `Program.cs` to you. The `ConsoleApp` joint is still declared, so your entry point
can call `ApplyConsoleApp(context)` itself. Set `DovetailConsoleApp` to `false` to turn all of it off.

| Property                             | Default                     | Effect                                                   |
| ------------------------------------ | --------------------------- | -------------------------------------------------------- |
| `DovetailConsoleApp`                 | `true` with the console SDK | Declares the `ConsoleApp` joint and writes `Program.cs`. |
| `DovetailUseDefaultProgram`          | `true`                      | `false` keeps the generated `Program.cs` out.            |
| `DovetailConsoleAppFrameworkVersion` | the version the SDK pins    | The ConsoleAppFramework version the SDK adds.            |