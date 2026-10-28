# TestingJointDelegate

- Kind: `delegate`
- Package: [Dovetail.Testing](/Dovetail/v0.2.2/api/dovetail.testing.md)
- Version: `0.2.2`
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

- [TestingJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail.testing/testingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail.testing/testingjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail.testing/testingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
