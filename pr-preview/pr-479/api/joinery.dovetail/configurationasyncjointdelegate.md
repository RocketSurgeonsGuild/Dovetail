# ConfigurationAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail/obj/Release/net10.0/ConfigurationAsyncJointDelegate.cs)
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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [ConfigurationAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/configurationasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IConfigurationBuilder, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/configurationasyncjointdelegate/methods.md#begininvoke-idovetailcontext-iconfigurationbuilder-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail/configurationasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
