# ILoggingJoint

- Kind: `interface`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-497/api/joinery.dovetail.md)
- Version: `0.8.2-pr.22`
- Namespace: `Joinery.Dovetail.Joints`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-497-merge.Sha.bfc73c35707fd5b9e0214762920da20b16340f99/src/Joinery.Dovetail/obj/Release/net10.0/ILoggingJoint.cs)
- Implements: [IDovetailJoint](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/idovetailjoint.md)

Joint interface for Logging. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/idovetailjoint.md)

## Definition

```csharp
namespace Joinery.Dovetail.Joints;

public interface ILoggingJoint
    : Joinery.Dovetail.Infrastructure.IDovetailJoint
{
    // ...
}
```

## Methods

- [Register(IDovetailContext, ILoggingBuilder)](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/iloggingjoint/methods.md#register-idovetailcontext-iloggingbuilder) `abstract` -- Register the Logging part.

## See Also

- [IDovetailJoint](/Dovetail/pr-preview/pr-497/api/joinery.dovetail/idovetailjoint.md)
