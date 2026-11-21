# HostApplicationJointDelegate<TBuilder>

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/api/joinery.dovetail.hosting.md)
- Version: `0.8.2-beta.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.e5590664261ce458679e619c245619797437f750/src/Joinery.Dovetail.Hosting/obj/Release/net10.0/HostApplicationJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the HostApplication part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void HostApplicationJointDelegate<TBuilder>(
    Joinery.Dovetail.IDovetailContext context,
    TBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`TBuilder`)

## Constructors

- [HostApplicationJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TBuilder, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/methods.md#begininvoke-idovetailcontext-tbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
