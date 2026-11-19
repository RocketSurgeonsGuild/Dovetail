# TestingAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.testing.md)
- Version: `0.7.2-pr.12`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-459-merge.Sha.aab685a60ccddd799f0b5db0f9ac63cb44227cbd/src/Joinery.Dovetail.Testing/obj/Release/net10.0/TestingAsyncJointDelegate.cs)
- Inherits: `MulticastDelegate`

Async delegate for the Testing part.

## Definition

```csharp
namespace Joinery.Dovetail.Testing.Joints;

public sealed delegate System.Threading.Tasks.ValueTask TestingAsyncJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [TestingAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.testing/testingasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.testing/testingasyncjointdelegate/methods.md#begininvoke-idovetailcontext-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-459/api/joinery.dovetail.testing/testingasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
