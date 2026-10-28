# UnoApplicationBuilderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.Uno](/Dovetail/api/dovetail.hosting.uno.md)
- Version: `0.2.3-beta.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the UnoApplicationBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void UnoApplicationBuilderJointDelegate(
    Dovetail.IDovetailContext context,
    Uno.Extensions.Hosting.IApplicationBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IApplicationBuilder`)

## Constructors

- [UnoApplicationBuilderJointDelegate(object, nint)](/Dovetail/api/dovetail.hosting.uno/unoapplicationbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IApplicationBuilder, AsyncCallback, object)](/Dovetail/api/dovetail.hosting.uno/unoapplicationbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-iapplicationbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/dovetail.hosting.uno/unoapplicationbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
