# HostCreatedJointDelegate<THost>

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/v0.8.0/api/joinery.dovetail.md)
- Version: `0.8.0`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.dd91ad69021d707105f71e00730b2dfe6a92f5ae/src/Joinery.Dovetail/obj/Release/net10.0/HostCreatedJointDelegate.cs)
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

- `context` ([IDovetailContext](/Dovetail/v0.8.0/api/joinery.dovetail/idovetailcontext.md))
- `host` (`THost`)

## Constructors

- [HostCreatedJointDelegate(object, nint)](/Dovetail/v0.8.0/api/joinery.dovetail/hostcreatedjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, AsyncCallback, object)](/Dovetail/v0.8.0/api/joinery.dovetail/hostcreatedjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.8.0/api/joinery.dovetail/hostcreatedjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
