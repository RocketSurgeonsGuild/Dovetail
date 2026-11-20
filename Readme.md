# Dovetail

Joint-driven application wiring for .NET. A library declares the configuration, services, logging and hosting it needs as
joints, and a Roslyn source generator finds them at build time. Referencing a package is enough to wire it up: there is no
extension method to remember, no assembly scanning and no runtime reflection, so Dovetail apps work with Native AOT and
trimming.

[![github-release-badge]][github-release]
[![github-license-badge]][github-license]
[![github-build-badge]][github-build]
[![nuget-Joinery.Dovetail-badge]][nuget-Joinery.Dovetail]

Documentation: <https://rocketsurgeonsguild.github.io/Dovetail/>

## Getting started

The shortest path is to swap your project's Microsoft SDK for its Dovetail counterpart. The SDK detects the kind of host,
adds the Dovetail packages it needs and writes the bootstrapping code:

```xml
<!-- before -->
<Project Sdk="Microsoft.NET.Sdk.Web">

<!-- after -->
<Project Sdk="Joinery.Dovetail.Sdk.Web">
```

Pin the version in `global.json` under `msbuild-sdks`. See [Dovetail SDKs for each Microsoft SDK](https://rocketsurgeonsguild.github.io/Dovetail/guides/microsoft-sdks/).

A joint is a class that implements one of the joint interfaces. Mark it with `[DovetailExport]` and any application that
references your library picks it up:

```csharp
[DovetailExport]
public class TimeJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
    }
}
```

See [Defining joints](https://rocketsurgeonsguild.github.io/Dovetail/concepts/defining-joints/) for the full set of joint types, host types and categories.

## Packages

| Package                                                                          | NuGet                                                                                           | What it does                                                                                               |
| -------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------- |
| [Joinery.Dovetail][nuget-Joinery.Dovetail]                                       | [![nuget-Joinery.Dovetail-badge]][nuget-Joinery.Dovetail]                                       | The joint interfaces, `DovetailContextBuilder` and the source generator                                    |
| [Joinery.Dovetail.Hosting][nuget-Joinery.Dovetail.Hosting]                       | [![nuget-Joinery.Dovetail.Hosting-badge]][nuget-Joinery.Dovetail.Hosting]                       | Bootstraps the generic host, ASP.NET Core, .NET MAUI, Uno Platform and Blazor WebAssembly from your joints |
| [Joinery.Dovetail.Configuration.Json][nuget-Joinery.Dovetail.Configuration.Json] | [![nuget-Joinery.Dovetail.Configuration.Json-badge]][nuget-Joinery.Dovetail.Configuration.Json] | Loads JSON configuration files, such as `appsettings.json`                                                 |
| [Joinery.Dovetail.Configuration.Yaml][nuget-Joinery.Dovetail.Configuration.Yaml] | [![nuget-Joinery.Dovetail.Configuration.Yaml-badge]][nuget-Joinery.Dovetail.Configuration.Yaml] | Loads YAML configuration files, such as `appsettings.yaml`                                                 |
| [Joinery.Dovetail.Configuration.Toml][nuget-Joinery.Dovetail.Configuration.Toml] | [![nuget-Joinery.Dovetail.Configuration.Toml-badge]][nuget-Joinery.Dovetail.Configuration.Toml] | Loads TOML configuration files, such as `appsettings.toml`                                                 |
| [Joinery.Dovetail.Serilog][nuget-Joinery.Dovetail.Serilog]                       | [![nuget-Joinery.Dovetail.Serilog-badge]][nuget-Joinery.Dovetail.Serilog]                       | Serilog joint for `LoggerConfiguration`                                                                    |
| [Joinery.Dovetail.OpenTelemetry][nuget-Joinery.Dovetail.OpenTelemetry]           | [![nuget-Joinery.Dovetail.OpenTelemetry-badge]][nuget-Joinery.Dovetail.OpenTelemetry]           | OpenTelemetry joint for `IOpenTelemetryBuilder`                                                            |
| [Joinery.Dovetail.Autofac][nuget-Joinery.Dovetail.Autofac]                       | [![nuget-Joinery.Dovetail.Autofac-badge]][nuget-Joinery.Dovetail.Autofac]                       | Autofac joint for `ContainerBuilder`                                                                       |
| [Joinery.Dovetail.DryIoc][nuget-Joinery.Dovetail.DryIoc]                         | [![nuget-Joinery.Dovetail.DryIoc-badge]][nuget-Joinery.Dovetail.DryIoc]                         | DryIoc joint for `IContainer`                                                                              |
| [Joinery.Dovetail.Stashbox][nuget-Joinery.Dovetail.Stashbox]                     | [![nuget-Joinery.Dovetail.Stashbox-badge]][nuget-Joinery.Dovetail.Stashbox]                     | Stashbox joint for `IStashboxContainer`                                                                    |
| [Joinery.Dovetail.Aspire][nuget-Joinery.Dovetail.Aspire]                         | [![nuget-Joinery.Dovetail.Aspire-badge]][nuget-Joinery.Dovetail.Aspire]                         | Applies joints to an Aspire `IDistributedApplicationBuilder`                                               |
| [Joinery.Dovetail.Aspire.Testing][nuget-Joinery.Dovetail.Aspire.Testing]         | [![nuget-Joinery.Dovetail.Aspire.Testing-badge]][nuget-Joinery.Dovetail.Aspire.Testing]         | Applies joints to an Aspire `IDistributedApplicationTestingBuilder`                                        |
| [Joinery.Dovetail.Testing][nuget-Joinery.Dovetail.Testing]                       | [![nuget-Joinery.Dovetail.Testing-badge]][nuget-Joinery.Dovetail.Testing]                       | Runs shared test setup joints when the host is a unit test host                                            |
| [Joinery.Dovetail.ModularPipelines][nuget-Joinery.Dovetail.ModularPipelines]     | [![nuget-Joinery.Dovetail.ModularPipelines-badge]][nuget-Joinery.Dovetail.ModularPipelines]     | Applies joints to a ModularPipelines `PipelineBuilder`                                                     |
| [Joinery.Dovetail.Mermaid][nuget-Joinery.Dovetail.Mermaid]                       | [![nuget-Joinery.Dovetail.Mermaid-badge]][nuget-Joinery.Dovetail.Mermaid]                       | Renders a context's resolved joints as a Mermaid diagram                                                   |

### MSBuild SDKs

Each SDK is a drop-in replacement for the Microsoft SDK it names.

| Package                                                                                | NuGet                                                                                                 | Replaces                                                                          |
| -------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| [Joinery.Dovetail.Sdk][nuget-Joinery.Dovetail.Sdk]                                     | [![nuget-Joinery.Dovetail.Sdk-badge]][nuget-Joinery.Dovetail.Sdk]                                     | `Microsoft.NET.Sdk`: console apps, libraries, test projects and file-based apps   |
| [Joinery.Dovetail.Sdk.Web][nuget-Joinery.Dovetail.Sdk.Web]                             | [![nuget-Joinery.Dovetail.Sdk.Web-badge]][nuget-Joinery.Dovetail.Sdk.Web]                             | `Microsoft.NET.Sdk.Web`: ASP.NET Core applications                                |
| [Joinery.Dovetail.Sdk.Worker][nuget-Joinery.Dovetail.Sdk.Worker]                       | [![nuget-Joinery.Dovetail.Sdk.Worker-badge]][nuget-Joinery.Dovetail.Sdk.Worker]                       | `Microsoft.NET.Sdk.Worker`: worker services                                       |
| [Joinery.Dovetail.Sdk.Razor][nuget-Joinery.Dovetail.Sdk.Razor]                         | [![nuget-Joinery.Dovetail.Sdk.Razor-badge]][nuget-Joinery.Dovetail.Sdk.Razor]                         | `Microsoft.NET.Sdk.Razor`: Razor class libraries                                  |
| [Joinery.Dovetail.Sdk.BlazorWebAssembly][nuget-Joinery.Dovetail.Sdk.BlazorWebAssembly] | [![nuget-Joinery.Dovetail.Sdk.BlazorWebAssembly-badge]][nuget-Joinery.Dovetail.Sdk.BlazorWebAssembly] | `Microsoft.NET.Sdk.BlazorWebAssembly`: Blazor WebAssembly applications            |
| [Joinery.Dovetail.Sdk.WebAssembly][nuget-Joinery.Dovetail.Sdk.WebAssembly]             | [![nuget-Joinery.Dovetail.Sdk.WebAssembly-badge]][nuget-Joinery.Dovetail.Sdk.WebAssembly]             | `Microsoft.NET.Sdk.WebAssembly`: WebAssembly applications                         |
| [Joinery.Dovetail.Sdk.WindowsDesktop][nuget-Joinery.Dovetail.Sdk.WindowsDesktop]       | [![nuget-Joinery.Dovetail.Sdk.WindowsDesktop-badge]][nuget-Joinery.Dovetail.Sdk.WindowsDesktop]       | `Microsoft.NET.Sdk.WindowsDesktop`: Windows Forms and WPF                         |
| [Joinery.Dovetail.Sdk.Azure.Functions][nuget-Joinery.Dovetail.Sdk.Azure.Functions]     | [![nuget-Joinery.Dovetail.Sdk.Azure.Functions-badge]][nuget-Joinery.Dovetail.Sdk.Azure.Functions]     | `Azure.Functions.Sdk`: Azure Functions isolated worker applications               |
| [Joinery.Dovetail.Sdk.Console][nuget-Joinery.Dovetail.Sdk.Console]                     | [![nuget-Joinery.Dovetail.Sdk.Console-badge]][nuget-Joinery.Dovetail.Sdk.Console]                     | `Joinery.Dovetail.Sdk` plus a generated `Program.cs` for ConsoleAppFramework apps |
| [Joinery.Dovetail.Sdk.ModularPipelines][nuget-Joinery.Dovetail.Sdk.ModularPipelines]   | [![nuget-Joinery.Dovetail.Sdk.ModularPipelines-badge]][nuget-Joinery.Dovetail.Sdk.ModularPipelines]   | `Joinery.Dovetail.Sdk` plus ModularPipelines, for build pipelines                 |

`Joinery.Dovetail.Analyzers` holds the source generator. `Joinery.Dovetail` brings it in, so you don't reference it
yourself.

## Configuration

Options binding and configuration schemas come from [Optionary](https://github.com/RocketSurgeonsGuild/Optionary), which
Dovetail doesn't depend on. Add the `Optionary` package and put `[Optionary]` on an options class: Optionary generates a
reflection-free binder for `IOptions<T>` and a JSON Schema, and Dovetail exports a joint that registers the options in every host
that references the library. A library's `appsettings` files in JSON, YAML or TOML are still shipped by the Dovetail SDK. See
[Configuration with Optionary](https://rocketsurgeonsguild.github.io/Dovetail/concepts/configuration-with-optionary/) and
[Configuration Files](https://rocketsurgeonsguild.github.io/Dovetail/concepts/configuration-files/).

## Contributing

Run `mise install` before building: it installs the .NET SDK and the extra .NET 10 runtime that the net10.0 tests run on (`.config/mise.toml`). `global.json` pins the same SDK, so a `dotnet` outside mise fails fast unless that SDK is installed.

> [!NOTE]
> On macOS, the first `mise install` of a new .NET SDK can overwrite the shared `~/.local/share/mise/dotnet-root/dotnet` host in place, and macOS then kills every `dotnet` command (SIGKILL). Replace the host with a fresh copy to fix it:
>
> ```bash
> cd ~/.local/share/mise/dotnet-root && cp -p dotnet dotnet.fresh && mv -f dotnet.fresh dotnet
> ```

<!-- references -->

[github-release]: https://github.com/RocketSurgeonsGuild/Dovetail/releases/latest
[github-release-badge]: https://img.shields.io/github/release/RocketSurgeonsGuild/Dovetail.svg?logo=github&style=flat "Latest Release"
[github-license]: https://github.com/RocketSurgeonsGuild/Dovetail/blob/main/LICENSE
[github-license-badge]: https://img.shields.io/github/license/RocketSurgeonsGuild/Dovetail.svg?style=flat "License"
[github-build]: https://github.com/RocketSurgeonsGuild/Dovetail/actions/workflows/build.yml
[github-build-badge]: https://github.com/RocketSurgeonsGuild/Dovetail/actions/workflows/build.yml/badge.svg?branch=main "Build"
[nuget-Joinery.Dovetail]: https://www.nuget.org/packages/Joinery.Dovetail/
[nuget-Joinery.Dovetail-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Hosting]: https://www.nuget.org/packages/Joinery.Dovetail.Hosting/
[nuget-Joinery.Dovetail.Hosting-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Hosting.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Configuration.Json]: https://www.nuget.org/packages/Joinery.Dovetail.Configuration.Json/
[nuget-Joinery.Dovetail.Configuration.Json-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Configuration.Json.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Configuration.Yaml]: https://www.nuget.org/packages/Joinery.Dovetail.Configuration.Yaml/
[nuget-Joinery.Dovetail.Configuration.Yaml-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Configuration.Yaml.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Configuration.Toml]: https://www.nuget.org/packages/Joinery.Dovetail.Configuration.Toml/
[nuget-Joinery.Dovetail.Configuration.Toml-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Configuration.Toml.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Serilog]: https://www.nuget.org/packages/Joinery.Dovetail.Serilog/
[nuget-Joinery.Dovetail.Serilog-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Serilog.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.OpenTelemetry]: https://www.nuget.org/packages/Joinery.Dovetail.OpenTelemetry/
[nuget-Joinery.Dovetail.OpenTelemetry-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.OpenTelemetry.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Autofac]: https://www.nuget.org/packages/Joinery.Dovetail.Autofac/
[nuget-Joinery.Dovetail.Autofac-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Autofac.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.DryIoc]: https://www.nuget.org/packages/Joinery.Dovetail.DryIoc/
[nuget-Joinery.Dovetail.DryIoc-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.DryIoc.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Stashbox]: https://www.nuget.org/packages/Joinery.Dovetail.Stashbox/
[nuget-Joinery.Dovetail.Stashbox-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Stashbox.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Aspire]: https://www.nuget.org/packages/Joinery.Dovetail.Aspire/
[nuget-Joinery.Dovetail.Aspire-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Aspire.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Aspire.Testing]: https://www.nuget.org/packages/Joinery.Dovetail.Aspire.Testing/
[nuget-Joinery.Dovetail.Aspire.Testing-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Aspire.Testing.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Testing]: https://www.nuget.org/packages/Joinery.Dovetail.Testing/
[nuget-Joinery.Dovetail.Testing-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Testing.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.ModularPipelines]: https://www.nuget.org/packages/Joinery.Dovetail.ModularPipelines/
[nuget-Joinery.Dovetail.ModularPipelines-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.ModularPipelines.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Mermaid]: https://www.nuget.org/packages/Joinery.Dovetail.Mermaid/
[nuget-Joinery.Dovetail.Mermaid-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Mermaid.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk/
[nuget-Joinery.Dovetail.Sdk-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk.Web]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk.Web/
[nuget-Joinery.Dovetail.Sdk.Web-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.Web.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk.Worker]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk.Worker/
[nuget-Joinery.Dovetail.Sdk.Worker-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.Worker.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk.Razor]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk.Razor/
[nuget-Joinery.Dovetail.Sdk.Razor-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.Razor.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk.BlazorWebAssembly]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk.BlazorWebAssembly/
[nuget-Joinery.Dovetail.Sdk.BlazorWebAssembly-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.BlazorWebAssembly.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk.WebAssembly]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk.WebAssembly/
[nuget-Joinery.Dovetail.Sdk.WebAssembly-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.WebAssembly.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk.WindowsDesktop]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk.WindowsDesktop/
[nuget-Joinery.Dovetail.Sdk.WindowsDesktop-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.WindowsDesktop.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk.Azure.Functions]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk.Azure.Functions/
[nuget-Joinery.Dovetail.Sdk.Azure.Functions-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.Azure.Functions.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk.Console]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk.Console/
[nuget-Joinery.Dovetail.Sdk.Console-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.Console.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Sdk.ModularPipelines]: https://www.nuget.org/packages/Joinery.Dovetail.Sdk.ModularPipelines/
[nuget-Joinery.Dovetail.Sdk.ModularPipelines-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Sdk.ModularPipelines.svg?logo=nuget&style=flat
[nuget-Joinery.Dovetail.Analyzers]: https://www.nuget.org/packages/Joinery.Dovetail.Analyzers/
[nuget-Joinery.Dovetail.Analyzers-badge]: https://img.shields.io/nuget/v/Joinery.Dovetail.Analyzers.svg?logo=nuget&style=flat
