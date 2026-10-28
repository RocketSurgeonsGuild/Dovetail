# UnoHostBuilderJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the UnoHostBuilder part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void UnoHostBuilderJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.Hosting.IHostBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IHostBuilder`)

## Constructors

- [UnoHostBuilderJointDelegate(object, nint)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unohostbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IHostBuilder, AsyncCallback, object)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unohostbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-ihostbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/unohostbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
