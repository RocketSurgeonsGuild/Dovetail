# ConfigurationJointDelegate

- Kind: `delegate`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-496/api/joinery.dovetail.md)
- Version: `0.8.2-pr.19`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-496-merge.Sha.0fe35cacc9dd234d2586c91a500ab0e3ff52d304/src/Joinery.Dovetail/obj/Release/net10.0/ConfigurationJointDelegate.cs)
- Inherits: `MulticastDelegate`

Delegate for the Configuration part.

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public sealed delegate void ConfigurationJointDelegate(
    Joinery.Dovetail.IDovetailContext context,
    Microsoft.Extensions.Configuration.IConfigurationBuilder builder)
```

## Delegate Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`IConfigurationBuilder`)

## Constructors

- [ConfigurationJointDelegate(object, nint)](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/configurationjointdelegate/constructors.md#constructor-object-nint)

## Methods

- [BeginInvoke(IDovetailContext, IConfigurationBuilder, AsyncCallback, object)](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/configurationjointdelegate/methods.md#begininvoke-idovetailcontext-iconfigurationbuilder-asynccallback-object) : `IAsyncResult` `virtual`
- [EndInvoke(IAsyncResult)](/Dovetail/pr-preview/pr-496/api/joinery.dovetail/configurationjointdelegate/methods.md#endinvoke-iasyncresult) `virtual`
