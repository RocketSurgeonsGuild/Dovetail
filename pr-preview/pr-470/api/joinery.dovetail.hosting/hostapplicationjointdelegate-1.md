# HostApplicationJointDelegate<TBuilder>

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.hosting.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-470-merge.Sha.1f45da18d9f78a77f1393ca7c4c966285183843c/src/Joinery.Dovetail.Hosting/obj/Release/net10.0/HostApplicationJointDelegate.cs)
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

- [HostApplicationJointDelegate(object, nint)](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/methods.md#begininvoke-idovetailcontext-tbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
