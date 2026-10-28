# UnoApplicationBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.4-beta.6`
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

- [UnoApplicationBuilderJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.hosting.uno/unoapplicationbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IApplicationBuilder, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.hosting.uno/unoapplicationbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-iapplicationbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.hosting.uno/unoapplicationbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
