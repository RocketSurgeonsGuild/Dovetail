# HostCreatedJointDelegate<THost>

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-486/api/joinery.dovetail.md)
- Version: `0.8.2-pr.13`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-486-merge.Sha.0dbc318ad08f7f5b82ee2d67aaffd22e5ac9b9a1/src/Joinery.Dovetail/obj/Release/net10.0/HostCreatedJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the HostCreated part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void HostCreatedJointDelegate<THost>(
    Joinery.Dovetail.IDovetailContext context,
    THost host)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/idovetailcontext.md))
- `host` (`THost`)

## Constructors

- [HostCreatedJointDelegate(object, nint)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/hostcreatedjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, AsyncCallback, object)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/hostcreatedjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-486/api/joinery.dovetail/hostcreatedjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
