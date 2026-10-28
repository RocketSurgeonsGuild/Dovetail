# HostJointDelegate<THost>

- Kind: `delegate`
- Package: [Dovetail.Hosting](/Dovetail/v0.2.2/api/dovetail.hosting.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Host part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void HostJointDelegate<THost>(
    Dovetail.IDovetailContext context,
    THost host)
    where THost : IHost
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `host` (`THost`)

## Constructors

- [HostJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail.hosting/hostjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, THost, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail.hosting/hostjointdelegate-1/methods.md#begininvoke-idovetailcontext-thost-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail.hosting/hostjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
