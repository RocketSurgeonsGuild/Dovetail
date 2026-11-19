# SetupJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.8.1-beta.2`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.cdc3faae2d21ed65b085ade9f74e0b3b6e412754/src/Joinery.Dovetail/obj/Release/net10.0/SetupJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the Setup part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void SetupJointDelegate(
    Joinery.Dovetail.IDovetailContext context)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))

## Constructors

- [SetupJointDelegate(object, nint)](/Dovetail/api/joinery.dovetail/setupjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, AsyncCallback, object)](/Dovetail/api/joinery.dovetail/setupjointdelegate/methods.md#begininvoke-idovetailcontext-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/api/joinery.dovetail/setupjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
