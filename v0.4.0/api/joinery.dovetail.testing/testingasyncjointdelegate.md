# TestingAsyncJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Testing](/Dovetail/v0.4.0/api/joinery.dovetail.testing.md)
- Version: `0.4.0`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
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

- [TestingAsyncJointDelegate(object, nint)](/Dovetail/v0.4.0/api/joinery.dovetail.testing/testingasyncjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, CancellationToken, AsyncCallback, object)](/Dovetail/v0.4.0/api/joinery.dovetail.testing/testingasyncjointdelegate/methods.md#begininvoke-idovetailcontext-cancellationtoken-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.4.0/api/joinery.dovetail.testing/testingasyncjointdelegate/methods.md#endinvoke-iasyncresult) : `ValueTask` `virtual`
