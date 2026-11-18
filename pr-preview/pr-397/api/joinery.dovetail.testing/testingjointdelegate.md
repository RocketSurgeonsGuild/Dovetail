# TestingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Testing](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.testing.md)
- Version: `0.7.2-pr.6`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail.Testing/obj/Release/net10.0/TestingJointDelegate.cs)
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

- [TestingJointDelegate(object, nint)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.testing/testingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.testing/testingjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.testing/testingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
