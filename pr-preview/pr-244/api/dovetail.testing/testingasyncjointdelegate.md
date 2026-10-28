# TestingAsyncJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Testing](/Dovetail/pr-preview/pr-244/api/dovetail.testing.md)
- Version: `0.2.3-pr.5`
- Namespace: `Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Async delegate for the Testing part.

## Definition

```csharp
namespace Dovetail.Testing.Joints;

public sealed delegate System.Threading.Tasks.ValueTask TestingAsyncJointDelegate(
    Dovetail.IDovetailContext context,
    System.Threading.CancellationToken cancellationToken)
```

## Delegate Parameters

- `context` (`IDovetailContext`)
- `cancellationToken` (`CancellationToken`) `optional`

## Constructors

- [TestingAsyncJointDelegate(object, nint)](/Dovetail/pr-preview/pr-244/api/dovetail.testing/testingasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, CancellationToken, AsyncCallback, object)](/Dovetail/pr-preview/pr-244/api/dovetail.testing/testingasyncjointdelegate/methods.md#begininvoke-idovetailcontext-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-244/api/dovetail.testing/testingasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
