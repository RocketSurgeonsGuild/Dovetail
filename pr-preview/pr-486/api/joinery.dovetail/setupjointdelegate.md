# SetupJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.md)
- Version: `0.8.2-pr.13`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-486-merge.Sha.0dbc318ad08f7f5b82ee2d67aaffd22e5ac9b9a1/src/Joinery.Dovetail/obj/Release/net10.0/SetupJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the Setup part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void SetupJointDelegate(
    Joinery.Dovetail.IDovetailContext context)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/idovetailcontext.md))

## Constructors

- [SetupJointDelegate(object, nint)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/setupjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/setupjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/setupjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
