# HostCreatedJointDelegate<THost>

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-235/api/dovetail.md)
- Version: `0.2.2-pr.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the HostCreated part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void HostCreatedJointDelegate<THost>(
    Dovetail.IDovetailContext context,
    THost host)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailcontext.md))
- `host` (`THost`)

## Constructors

- [HostCreatedJointDelegate(object, nint)](/Dovetail/pr-preview/pr-235/api/dovetail/hostcreatedjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, AsyncCallback, object)](/Dovetail/pr-preview/pr-235/api/dovetail/hostcreatedjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-235/api/dovetail/hostcreatedjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
