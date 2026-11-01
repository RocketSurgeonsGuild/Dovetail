# TestingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.testing.md)
- Version: `0.4.2-pr.5`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Testing part.

## Definition

```csharp
namespace Joinery.Dovetail.Testing.Joints;

public sealed delegate void TestingJointDelegate(
    Joinery.Dovetail.IDovetailContext context)
```

## Delegate Parameters

- `context` (`IDovetailContext`)

## Constructors

- [TestingJointDelegate(object, nint)](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.testing/testingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.testing/testingjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-299/api/joinery.dovetail.testing/testingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
