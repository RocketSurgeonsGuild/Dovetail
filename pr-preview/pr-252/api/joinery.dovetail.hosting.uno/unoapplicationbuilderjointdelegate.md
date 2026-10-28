# UnoApplicationBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.4-pr.5`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the UnoApplicationBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void UnoApplicationBuilderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Uno.Extensions.Hosting.IApplicationBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IApplicationBuilder`)

## Constructors

- [UnoApplicationBuilderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.hosting.uno/unoapplicationbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IApplicationBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.hosting.uno/unoapplicationbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-iapplicationbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-252/api/joinery.dovetail.hosting.uno/unoapplicationbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
