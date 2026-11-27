# DryIocOptions Properties

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-494/api/joinery.dovetail.dryioc.md)
- Type: [DryIocOptions](/Dovetail/pr-preview/pr-494/api/joinery.dovetail.dryioc/dryiocoptions.md)
- Kind: `Properties`
- Members: `1`

Options for building the DryIoc Container

<a id="nomoreregistrationallowed"></a>

## NoMoreRegistrationAllowed

- Name: `NoMoreRegistrationAllowed`
- Modifiers: `get; set`
- Returns: `bool`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-494-merge.Sha.60ec96c618c8e073441c11142954fbb8c2ec3264/src/Joinery.Dovetail.DryIoc/DryIocOptions.cs)

Prevents additional container registrations (defaults to true)

```csharp
public bool NoMoreRegistrationAllowed { get; set; }
```
