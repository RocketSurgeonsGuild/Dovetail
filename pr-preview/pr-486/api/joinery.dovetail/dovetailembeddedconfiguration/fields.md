# DovetailEmbeddedConfiguration Fields

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.md)
- Type: [DovetailEmbeddedConfiguration](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/dovetailembeddedconfiguration.md)
- Kind: `Fields`
- Members: `1`

Loads configuration files that the Dovetail SDK embedded into an assembly as manifest resources, for hosts that have no file system to read them from (mobile and other app-package hosts).

<a id="assemblykey"></a>

## AssemblyKey

- Name: `AssemblyKey`
- Modifiers: `static` `const`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-486-merge.Sha.0dbc318ad08f7f5b82ee2d67aaffd22e5ac9b9a1/src/Joinery.Dovetail/DovetailEmbeddedConfiguration.cs)

The context key that holds the `Reflection.Assembly` the application's configuration files are embedded in.

```csharp
public const static string AssemblyKey
```
