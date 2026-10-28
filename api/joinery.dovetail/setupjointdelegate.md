# SetupJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.2.3-beta.4`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Setup part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void SetupJointDelegate(
    Joinery.Dovetail.IDovetailContext context)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))

## Constructors

- [SetupJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail/setupjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/api/joinery.dovetail/setupjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail/setupjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
