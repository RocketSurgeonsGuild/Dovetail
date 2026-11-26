# TestingJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail.Testing](/Dovetail/api/joinery.dovetail.testing.md)
- Version: `0.8.2-beta.13`
- Namespace: `Joinery.Dovetail.Testing.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.9116ea12c6002cf1bba224cfb363e5a7ac536a99/src/Joinery.Dovetail.Testing/obj/Release/net10.0/TestingJointDelegate.cs)
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

- [TestingJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail.testing/testingjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/api/joinery.dovetail.testing/testingjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail.testing/testingjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
