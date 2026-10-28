# HostJointDelegate<THost>

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/api/joinery.dovetail.hosting.md)
- Version: `0.2.4-beta.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Host part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void HostJointDelegate<THost>(
    Joinery.Dovetail.IDovetailContext context,
    THost host)
    where THost : IHost
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`THost`)

## Constructors

- [HostJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.hosting/hostjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.hosting/hostjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.hosting/hostjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
