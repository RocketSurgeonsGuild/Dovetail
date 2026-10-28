# SetupJointDelegate

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-244/api/dovetail.md)
- Version: `0.2.3-pr.5`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Setup part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void SetupJointDelegate(
    Dovetail.IDovetailContext context)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-244/api/dovetail/idovetailcontext.md))

## Constructors

- [SetupJointDelegate(object, nint)](/Dovetail/pr-preview/pr-244/api/dovetail/setupjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/pr-preview/pr-244/api/dovetail/setupjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-244/api/dovetail/setupjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
