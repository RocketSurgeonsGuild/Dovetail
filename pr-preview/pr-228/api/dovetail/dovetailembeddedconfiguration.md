# DovetailEmbeddedConfiguration

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-228/api/dovetail.md)
- Version: `0.2.1-pr.19`
- Namespace: `Dovetail`
- Target framework: `net10.0`

Loads configuration files that the Dovetail SDK embedded into an assembly as manifest resources, for hosts that have no file system to read them from (mobile and other app-package hosts).

## Definition

```csharp
namespace Dovetail;

public static class DovetailEmbeddedConfiguration
{
    // ...
}
```

## Remarks

A host turns this on by setting [DovetailEmbeddedConfiguration.AssemblyKey](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailembeddedconfiguration/fields.md#assemblykey) on the context to the assembly that carries the application's configuration. The configuration joints then read the embedded copies instead of files on disk.

## Methods

- [get_EmbeddedConfigurationAssembly(IDovetailContext)](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailembeddedconfiguration/methods.md#get-embeddedconfigurationassembly-idovetailcontext) : `Assembly?` `static`
- [GetConventionalFileNames(IDovetailContext, ReadOnlySpan<string>)](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailembeddedconfiguration/methods.md#getconventionalfilenames-idovetailcontext-readonlyspan-string) : `IReadOnlyList<string>` `static` -- Gets the conventional configuration file names, lowest precedence first: `appsettings`, the application name, each of those qualified by the environment name, and each of those qualified by `local`.
- [OpenResource(Assembly, ReadOnlySpan<string>)](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailembeddedconfiguration/methods.md#openresource-assembly-readonlyspan-string) : `Stream?` `static` -- Opens the first embedded resource that matches one of `fileNames`, in order. A resource matches when its name is the file name itself (the Dovetail SDK's naming) or ends with `.{fileName}` (the default manifest naming of other tools).

## Fields

- [AssemblyKey](/Dovetail/pr-preview/pr-228/api/dovetail/dovetailembeddedconfiguration/fields.md#assemblykey) : `string` `static` -- The context key that holds the `Reflection.Assembly` the application's configuration files are embedded in.
