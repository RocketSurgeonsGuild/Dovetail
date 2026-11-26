# DryIocOptions Properties

- Package: [Joinery.Dovetail.DryIoc](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.dryioc.md)
- Type: [DryIocOptions](/Dovetail/pr-preview/pr-492/api/joinery.dovetail.dryioc/dryiocoptions.md)
- Kind: `Properties`
- Members: `1`

Options for building the DryIoc Container

<a id="nomoreregistrationallowed"></a>

## NoMoreRegistrationAllowed

- Name: `NoMoreRegistrationAllowed`
- Modifiers: `get; set`
- Returns: `bool`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-492-merge.Sha.b704f8de5a56fc20916b1423a64b3b3874050871/src/Joinery.Dovetail.DryIoc/DryIocOptions.cs)

Prevents additional container registrations (defaults to true)

```csharp
public bool NoMoreRegistrationAllowed { get; set; }
```
