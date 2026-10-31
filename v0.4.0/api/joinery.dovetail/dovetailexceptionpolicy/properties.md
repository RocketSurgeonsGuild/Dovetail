# DovetailExceptionPolicy Properties

- Package: [Joinery.Dovetail](/Dovetail/v0.4.0/api/joinery.dovetail.md)
- Type: [DovetailExceptionPolicy](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailexceptionpolicy.md)
- Kind: `Properties`
- Members: `1`

A policy that determines how to handle exceptions thrown by conventions

<a id="ignorenotsupported"></a>

## IgnoreNotSupported

- Name: `IgnoreNotSupported`
- Modifiers: `static` `get`
- Returns: [DovetailExceptionPolicyDelegate](/Dovetail/v0.4.0/api/joinery.dovetail/dovetailexceptionpolicydelegate.md)

A policy that ignores `NotSupportedException` exceptions

```csharp
public static DovetailExceptionPolicyDelegate IgnoreNotSupported { get; }
```
