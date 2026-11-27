# ImportHelpers Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-490/api/joinery.dovetail.md)
- Type: [ImportHelpers](/Dovetail/pr-preview/pr-490/api/joinery.dovetail/importhelpers.md)
- Kind: `Properties`
- Members: `1`

This class is used by the source generator to allow the conventions provided to be overridden by the caller.

<a id="rivets"></a>

## Rivets

- Name: `Rivets`
- Modifiers: `static` `nullable` `get; set`
- Returns: [DovetailContextBuilderFactory?](/Dovetail/pr-preview/pr-490/api/joinery.dovetail/dovetailcontextbuilderfactory.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-490-merge.Sha.ee8f02fb8407f340a5cec4b5afe7824bb404b428/src/Joinery.Dovetail/ImportHelpers.cs)

The external conventions that have been provided by the caller.

```csharp
public static DovetailContextBuilderFactory? Rivets { get; set; }
```

## Remarks

If the `IsTestProject` property is detected, the generator will inject a `CompilerServices.ModuleInitializerAttribute` the automatically assigns the external conventions.
