# DryIocOptions Properties

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.dryioc.md)
- Type: [DryIocOptions](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.dryioc/dryiocoptions.md)
- Kind: `Properties`
- Members: `1`

Options for building the DryIoc Container

<a id="nomoreregistrationallowed"></a>

## NoMoreRegistrationAllowed

- Name: `NoMoreRegistrationAllowed`
- Modifiers: `get; set`
- Returns: `bool`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-486-merge.Sha.1faa3d8c1fb998da38c062158c5a17b8390b25fa/src/Joinery.Dovetail.DryIoc/DryIocOptions.cs)

Prevents additional container registrations (defaults to true)

```csharp
public bool NoMoreRegistrationAllowed { get; set; }
```
