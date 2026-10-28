# ConfigurationJointDelegate

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Version: `0.2.2`
- Namespace: `Dovetail.Joints`
- Target framework: `net10.0`
- Inherits: `MulticastDelegate`

Delegate for the Configuration part.

## Definition

```csharp
namespace Dovetail.Joints;

public sealed delegate void ConfigurationJointDelegate(
    Dovetail.IDovetailContext context,
    Microsoft.Extensions.Configuration.IConfigurationBuilder builder)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)

## Constructors

- [ConfigurationJointDelegate(object, nint)](/Dovetail/v0.2.2/api/dovetail/configurationjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IConfigurationBuilder, AsyncCallback, object)](/Dovetail/v0.2.2/api/dovetail/configurationjointdelegate/methods.md#begininvoke-idovetailcontext-iconfigurationbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/v0.2.2/api/dovetail/configurationjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
