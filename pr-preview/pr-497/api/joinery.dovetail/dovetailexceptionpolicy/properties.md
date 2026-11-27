# DovetailExceptionPolicy Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-497/api/joinery.dovetail.md)
- Type: [DovetailExceptionPolicy](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/dovetailexceptionpolicy.md)
- Kind: `Properties`
- Members: `1`

A policy that determines how to handle exceptions thrown by conventions

<a id="ignorenotsupported"></a>

## IgnoreNotSupported

- Name: `IgnoreNotSupported`
- Modifiers: `static` `get`
- Returns: [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/dovetailexceptionpolicydelegate.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-497-merge.Sha.bfc73c35707fd5b9e0214762920da20b16340f99/src/Joinery.Dovetail/DovetailExceptionPolicy.cs#L11)

A policy that ignores `NotSupportedException` exceptions

```csharp
public static DovetailExceptionPolicyDelegate IgnoreNotSupported { get; }
```
