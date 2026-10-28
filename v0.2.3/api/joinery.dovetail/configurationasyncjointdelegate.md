# ConfigurationAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/v0.2.3/api/joinery.dovetail.md)
- Version: `0.2.3`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the Configuration part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask ConfigurationAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.Configuration.IConfigurationBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/v0.2.3/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [ConfigurationAsyncJointDelegate(object, nint)](/Dovetail/v0.2.3/api/joinery.dovetail/configurationasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IConfigurationBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/v0.2.3/api/joinery.dovetail/configurationasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iconfigurationbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.3/api/joinery.dovetail/configurationasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
