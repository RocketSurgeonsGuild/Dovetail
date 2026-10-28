# UnoHostBuilderJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-241/api/dovetail.hosting.uno.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the UnoHostBuilder part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void UnoHostBuilderJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Extensions.Hosting.IHostBuilder builder)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `builder` (`IHostBuilder`)

## Constructors

- [UnoHostBuilderJointDelegate(object, nint)](/Dovetail/pr-preview/pr-241/api/dovetail.hosting.uno/unohostbuilderjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IHostBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-241/api/dovetail.hosting.uno/unohostbuilderjointdelegate/methods.md#begininvoke-idovetailcontext-ihostbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-241/api/dovetail.hosting.uno/unohostbuilderjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
