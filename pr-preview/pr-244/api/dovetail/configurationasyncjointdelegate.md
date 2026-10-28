# ConfigurationAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-244/api/dovetail.md)
- Version: `0.2.3-pr.5`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the Configuration part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate System.Threading.Tasks.ValueTask ConfigurationAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Extensions.Configuration.IConfigurationBuilder builder,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-244/api/dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [ConfigurationAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-244/api/dovetail/configurationasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IConfigurationBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-244/api/dovetail/configurationasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iconfigurationbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-244/api/dovetail/configurationasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
