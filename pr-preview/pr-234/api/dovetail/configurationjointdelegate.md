# ConfigurationJointDelegate

- Kind: `delegate`
- Package: [Dovetail](/Dovetail/pr-preview/pr-234/api/dovetail.md)
- Version: `0.2.2-pr.2`
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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-234/api/dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)

## Constructors

- [ConfigurationJointDelegate(object, nint)](/Dovetail/pr-preview/pr-234/api/dovetail/configurationjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IConfigurationBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-234/api/dovetail/configurationjointdelegate/methods.md#begininvoke-idovetailcontext-iconfigurationbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-234/api/dovetail/configurationjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
