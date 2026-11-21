# ImportHelpers Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Type: [ImportHelpers](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/importhelpers.md)
- Kind: `Properties`
- Members: `1`

This class is used by the source generator to allow the conventions provided to be overridden by the caller.

<a id="rivets"></a>

## Rivets

- Name: `Rivets`
- Modifiers: `static` `nullable` `get; set`
- Returns: [DovetailContextBuilderFactory?](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/dovetailcontextbuilderfactory.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail/ImportHelpers.cs)

The external conventions that have been provided by the caller.

```csharp
public static DovetailContextBuilderFactory? Rivets { get; set; }
```

## Remarks

If the `IsTestProject` property is detected, the generator will inject a `CompilerServices.ModuleInitializerAttribute` the automatically assigns the external conventions.
