# HostApplicationJointDelegate<TBuilder>

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.hosting.md)
- Version: `0.7.2-pr.8`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-463-merge.Sha.3b938bb1f685921a3be3b59b60dfaa31fc2a520c/src/Joinery.Dovetail.Hosting/obj/Release/net10.0/HostApplicationJointDelegate.cs)
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

- [HostApplicationJointDelegate(object, nint)](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, TBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/methods.md#begininvoke-idovetailcontext-tbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.hosting/hostapplicationjointdelegate-1/methods.md#endinvoke-iasyncresult) `virtual`
