# HostApplicationJointDelegate<TBuilder>

- Kind: `delegate`
- Package: [Dovetail.Hosting](/Dovetail/api/dovetail.hosting.md)
- Version: `0.2.2-beta.1`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the HostApplication part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void HostApplicationJointDelegate<TBuilder>(
    Dovetail.IDovetailContext context,
    TBuilder builder)
    where TBuilder : IHostApplicationBuilder
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`TBuilder`)

## Constructors

- [HostApplicationJointDelegate(object, nint)](/Dovetail/api/dovetail.hosting/hostapplicationjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TBuilder, AsyncCallback, object)](/Dovetail/api/dovetail.hosting/hostapplicationjointdelegate-1/methods.md#begininvoke-idovetailcontext-tbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.hosting/hostapplicationjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
