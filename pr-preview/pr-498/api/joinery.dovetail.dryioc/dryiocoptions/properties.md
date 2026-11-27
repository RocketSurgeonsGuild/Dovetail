# DryIocOptions Properties

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-498/api/joinery.dovetail.dryioc.md)
- Type: [DryIocOptions](/Dovetail/pr-preview/pr-498/api/joinery.dovetail.dryioc/dryiocoptions.md)
- Kind: `Properties`
- Members: `1`

Options for building the DryIoc Container

<a id="nomoreregistrationallowed"></a>

## NoMoreRegistrationAllowed

- Name: `NoMoreRegistrationAllowed`
- Modifiers: `get; set`
- Returns: `bool`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-498-merge.Sha.564cb3970fab4def9c0887052d46386c596bc161/src/Joinery.Dovetail.DryIoc/DryIocOptions.cs)

Prevents additional container registrations (defaults to true)

```csharp
public bool NoMoreRegistrationAllowed { get; set; }
```
