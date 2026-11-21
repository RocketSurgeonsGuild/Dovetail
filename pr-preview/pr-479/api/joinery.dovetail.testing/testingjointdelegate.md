# TestingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.testing.md)
- Version: `0.8.2-pr.7`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-479-merge.Sha.feaeb0c3ab7f343cde2ec1bc8326c12fbbd4fca4/src/Joinery.Dovetail.Testing/obj/Release/net10.0/TestingJointDelegate.cs)
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

- [TestingJointDelegate(object, nint)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.testing/testingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.testing/testingjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-479/api/joinery.dovetail.testing/testingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
