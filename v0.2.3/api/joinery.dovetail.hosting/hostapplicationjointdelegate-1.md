# HostApplicationJointDelegate<TBuilder>

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the HostApplication part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void HostApplicationJointDelegate<TBuilder>(
    Joinery.Dovetail.IDovetailContext context,
    TBuilder builder)
    where TBuilder : IHostApplicationBuilder
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`TBuilder`)

## Constructors

- [HostApplicationJointDelegate(object, nint)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TBuilder, AsyncCallback, object)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/methods.md#begininvoke-idovetailcontext-tbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
