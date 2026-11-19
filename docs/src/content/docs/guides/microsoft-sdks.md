---
title: Dovetail SDKs for each Microsoft SDK
description: Swap a project's Microsoft SDK for its Dovetail counterpart, such as Microsoft.NET.Sdk.Web for Joinery.Dovetail.Sdk.Web, and the project gets Dovetail with nothing else to change.
---

# Dovetail SDKs for each Microsoft SDK

Each Microsoft project SDK has a Dovetail SDK of the same name. To give a project Dovetail, change its SDK name and nothing else:

```xml
<!-- before -->
<Project Sdk="Microsoft.NET.Sdk.Web">

<!-- after -->
<Project Sdk="Joinery.Dovetail.Sdk.Web">
```

Pin the version in `global.json` under `msbuild-sdks`, or put it in the attribute (`Sdk="Joinery.Dovetail.Sdk.Web/1.2.3"`):

```json
{
    "msbuild-sdks": {
        "Joinery.Dovetail.Sdk.Web": "1.2.3"
    }
}
```

| Microsoft SDK                         | Dovetail SDK                             | What it is for                                        |
| ------------------------------------- | ---------------------------------------- | ----------------------------------------------------- |
| `Microsoft.NET.Sdk`                   | `Joinery.Dovetail.Sdk`                   | Console apps, libraries and test projects             |
| `Microsoft.NET.Sdk.Web`               | `Joinery.Dovetail.Sdk.Web`               | ASP.NET Core applications                             |
| `Microsoft.NET.Sdk.Worker`            | `Joinery.Dovetail.Sdk.Worker`            | Worker services                                       |
| `Microsoft.NET.Sdk.Razor`             | `Joinery.Dovetail.Sdk.Razor`             | Razor class libraries                                 |
| `Microsoft.NET.Sdk.BlazorWebAssembly` | `Joinery.Dovetail.Sdk.BlazorWebAssembly` | Blazor WebAssembly applications                       |
| `Microsoft.NET.Sdk.WebAssembly`       | `Joinery.Dovetail.Sdk.WebAssembly`       | WebAssembly applications (the `wasmbrowser` template) |
| `Microsoft.NET.Sdk.WindowsDesktop`    | `Joinery.Dovetail.Sdk.WindowsDesktop`    | Windows Forms and WPF projects that still name it     |
| `Aspire.AppHost.Sdk`                  | `Joinery.Dovetail.Sdk.Aspire.AppHost`    | Aspire AppHost projects                               |

`Joinery.Dovetail.Sdk.Console` is not a mirror: it is `Joinery.Dovetail.Sdk` plus a generated `Program.cs` for
[console applications](../../guides/console-apps/). Nor is `Joinery.Dovetail.Sdk.ModularPipelines`: it is `Joinery.Dovetail.Sdk` plus
ModularPipelines, for [build pipelines](../../guides/modular-pipelines/) (`#:sdk Joinery.Dovetail.Sdk.ModularPipelines`).

## What a Dovetail SDK does

`Joinery.Dovetail.Sdk.Web` imports `Microsoft.NET.Sdk.Web`, then chains `Joinery.Dovetail.Sdk`. That is the same thing as naming
both yourself:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <Sdk Name="Joinery.Dovetail.Sdk" Version="1.2.3" />
</Project>
```

So the project builds exactly as it did with the Microsoft SDK. On top of that, `Joinery.Dovetail.Sdk` detects the host from what the
Microsoft SDK set (see [DovetailHost](../../reference/dovetail-host/)), adds the Dovetail packages the project needs, and emits the
bootstrapping.

The others work the same way. `Microsoft.NET.Sdk.WindowsDesktop` has been unnecessary since .NET 5 (the SDK warns `NETSDK1137`):
`Microsoft.NET.Sdk` with `UseWPF` or `UseWindowsForms` is enough, and so is `Joinery.Dovetail.Sdk`. Its mirror is for projects that
have not moved off it yet.

## Aspire AppHosts

`Aspire.AppHost.Sdk` comes from NuGet rather than with the .NET SDK, so `Joinery.Dovetail.Sdk.Aspire.AppHost` imports it at a
version: the one it was built against, unless `DovetailAspireAppHostSdkVersion` says otherwise. The Aspire SDK is imported before any
of the project's files, so set that property where MSBuild sees it first, as an environment variable or on the command line:

```bash
dotnet build -p:DovetailAspireAppHostSdkVersion=13.6.5
```

An `Aspire.AppHost.Sdk` entry in `global.json` doesn't change it. Keep `Aspire.Hosting.AppHost`, which the Aspire SDK adds for you, at
the same version.

Dovetail's host detection has no Aspire host yet, so an AppHost is detected as a generic host (`HostApplication`), exactly as with
`Aspire.AppHost.Sdk` and `Joinery.Dovetail.Sdk` named side by side.

## Choosing the version of Joinery.Dovetail.Sdk

A Dovetail SDK chains the `Joinery.Dovetail.Sdk` of its own version: every Dovetail package ships at one version. To chain another, set
`DovetailSdkVersion` in a `Directory.Build.props`. The Microsoft SDK imports that file before the Dovetail SDK reads the property.

## With Joinery.Dovetail.Sdk in a Directory.Build.props

A repository that applies `Joinery.Dovetail.Sdk` to every project from a `Directory.Build.props` can still switch a project to a
Dovetail SDK. The Dovetail SDK sees that `Joinery.Dovetail.Sdk` is already there and does not apply it a second time.
