# HostCreatedJointDelegate<THost>

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
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

- `context` ([IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md))
- `host` (`THost`)

## Constructors

- [HostCreatedJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail/hostcreatedjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail/hostcreatedjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail/hostcreatedjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
