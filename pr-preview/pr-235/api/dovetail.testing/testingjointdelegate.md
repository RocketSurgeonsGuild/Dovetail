# TestingJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Testing](/Dovetail/pr-preview/pr-235/api/dovetail.testing.md)
- Version: `0.2.2-pr.4`
- Namespace: `Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Testing part.

## Definition

```csharp
namespace Dovetail.Testing.Joints;

public sealed delegate void TestingJointDelegate(
    Dovetail.IDovetailContext context)
```

## Delegate Parameters

- `context` (`IDovetailContext`)

## Constructors

- [TestingJointDelegate(object, nint)](/Dovetail/pr-preview/pr-235/api/dovetail.testing/testingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/pr-preview/pr-235/api/dovetail.testing/testingjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-235/api/dovetail.testing/testingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
