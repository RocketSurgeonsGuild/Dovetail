# Source Generation

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