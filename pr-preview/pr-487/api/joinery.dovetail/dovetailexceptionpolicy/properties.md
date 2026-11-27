# DovetailExceptionPolicy Properties

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-487/api/joinery.dovetail.md)
- Type: [DovetailExceptionPolicy](/Dovetail/pr-preview/pr-487/api/joinery.dovetail/dovetailexceptionpolicy.md)
- Kind: `Properties`
- Members: `1`

A policy that determines how to handle exceptions thrown by conventions

<a id="ignorenotsupported"></a>

## IgnoreNotSupported

- Name: `IgnoreNotSupported`
- Modifiers: `static` `get`
- Returns: [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-487/api/joinery.dovetail/dovetailexceptionpolicydelegate.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-487-merge.Sha.d2fe0860408e23901125d26601527b824db451a0/src/Joinery.Dovetail/DovetailExceptionPolicy.cs#L11)

A policy that ignores `NotSupportedException` exceptions

```csharp
public static DovetailExceptionPolicyDelegate IgnoreNotSupported { get; }
```
