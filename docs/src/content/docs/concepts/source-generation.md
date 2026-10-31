---
title: Source Generation
description: How the Roslyn source generator resolves and exports joints at build time.
tags: [source-generator, aot]
---

# Source Generation and Dovetails [New]

We have built a Source Generator to use with Dovetail that allows us to export joints and import joints with easy, in a way that is statically compiled.

# Exporting Dovetails

Any joint marked with `[assembly: Dovetail(typeof(MyDovetail))]` will automatically be added to a public partial static class named `Exports` that will exist in the root namespace of the assembly being built.

> [!TIP]
> This class is partial if you want to add any custom methods to it for consumers.

Each exported joint carries the host type it declares with `[LiveJoint]`, `[UnitTestJoint]` or `[DovetailHostType]`, the same
as when a joint is added through reflection at runtime. See [which attributes are honoured where](./defining-joints#which-attributes-are-honoured-where).

A joint with constructor parameters is exported as a factory, because the exported method has no context to read from: the context builder
that holds the metadata creates the joint from its properties, and an optional parameter falls back to its default. See
[how constructor parameters are resolved](./defining-joints#how-constructor-parameters-are-resolved).

# Importing Dovetails

The generator creates an internal static partial `Imports` class in the assembly's root namespace. `Imports.Joints` is a
`DovetailContextBuilderFactory` that creates a context builder loaded with the assembly's own joints and those of every
referenced assembly that exports joints. The `DovetailImportNamespace`, `DovetailImportClassName` and
`DovetailImportMethodName` MSBuild properties change those names.

> [!TIP]
> This class is partial if you want to add any custom methods to it for consumers.

You can also import the `GetDovetails` method onto a given class by using the `[ImportDovetails]` attribute on the class.

## Which projects import and export

Two MSBuild properties decide what the generator emits for a project:

| Property                 | When `true`                                                                                                                     |
| ------------------------ | ------------------------------------------------------------------------------------------------------------------------------- |
| `ImportDovetailAssembly` | The `Imports` class is generated.                                                                                               |
| `ExportDovetailAssembly` | Referencing assemblies import this assembly's joints. When `false`, the joints still reach this assembly's own `Imports` class. |

Joinery.Dovetail.Sdk sets both as part of its host detection. Projects that don't use the SDK get these defaults:

- test projects (`IsTestProject`) import but don't export;
- applications (`OutputType` of `Exe` or `WinExe`) import and export;
- libraries export but don't import.

Set either property in a project file to override its default.

The generator runs in every project that references a Dovetail package, including projects that only reference one
indirectly, for example through `Joinery.Dovetail.Hosting`. Its diagnostics (`DT0001` to `DT0005`, such as the error for a joint
with more than one constructor, and `DT0017`, the error for a joint with no constructor it can be created with) apply there too. The code it generates needs C# 12 or later: in a project whose `LangVersion` is older, it
generates nothing and reports warning `DT0006` instead.

## Specular's provider

An application that references [Specular](https://www.nuget.org/packages/Specular) doesn't set its provider by hand. Specular generates
an internal `SpecularProvider` into each project that emits one, and the generated `Imports` seeds that provider into the context
builder it creates, so a joint reads it with `context.Get<ISpecularProvider>()`. Scanning through it covers the assemblies the
application references, including the joints of libraries that don't generate a provider of their own (`SpecularEmitProvider` set to
`false`) and so keep receiving the application's through the context.

```csharp
public sealed class PluginJoint : ISetupJoint
{
    public void Register(IDovetailContext context)
    {
        var provider = context.Get<ISpecularProvider>()!;
        var plugins = provider.GetTypes(z => z.FromAssemblyOf<IPlugin>().GetTypes(f => f.AssignableTo<IPlugin>()));
    }
}
```

Dovetail has no dependency on Specular, so the seeding is generated code that exists only where Specular does:

- The generator adds it when the project's compilation references the assembly that defines `Specular.ISpecularProvider` and the
  `SpecularEmitProvider` MSBuild property isn't `false`, the two things Specular's own generator looks at before it generates a
  `SpecularProvider`. It can't look for `SpecularProvider` itself: a generator doesn't see what another one adds.
- In every other project nothing is generated and no diagnostic is raised.
- A provider you set yourself wins. The generated code only adds the provider when the context doesn't hold one, so
  `builder.ConfigureDovetail(b => b.Set<ISpecularProvider>(provider))`, or a provider passed in with the properties, is never
  overwritten.
- Set `SpecularEmitProvider` to `false` for a project that references Specular without generating a provider (say it excludes
  Specular's analyzers) and Dovetail stops naming one. Otherwise the build fails with CS0103 for `SpecularProvider`.

### Native AOT and trimming

The seeding adds no reflection: it is a generic call to `AddIfMissing<ISpecularProvider>` with the value of a static property, both of
which the trimmer and the AOT compiler can follow. Specular generates `SpecularProvider` without reflection as well. The
Joinery.Dovetail.Sdk.Tests publish an application that uses it trimmed and with Native AOT, without trim or AOT warnings, and the
published application starts with the provider in its context and scans across assemblies.
