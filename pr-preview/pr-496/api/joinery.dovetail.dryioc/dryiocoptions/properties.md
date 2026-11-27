# DryIocOptions Properties

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.dryioc.md)
- Type: [DryIocOptions](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.dryioc/dryiocoptions.md)
- Kind: `Properties`
- Members: `1`

Options for building the DryIoc Container

<a id="nomoreregistrationallowed"></a>

## NoMoreRegistrationAllowed

- Name: `NoMoreRegistrationAllowed`
- Modifiers: `get; set`
- Returns: `bool`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-496-merge.Sha.0fe35cacc9dd234d2586c91a500ab0e3ff52d304/src/Joinery.Dovetail.DryIoc/DryIocOptions.cs)

Prevents additional container registrations (defaults to true)

```csharp
public bool NoMoreRegistrationAllowed { get; set; }
```
