# IDistributedApplicationJoint Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/pr-preview/pr-268/api/joinery.dovetail.aspire.md)
- Type: [IDistributedApplicationJoint](/Dovetail/pr-preview/pr-268/api/joinery.dovetail.aspire/idistributedapplicationjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for DistributedApplication. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-distributedapplication"></a>

## Register(IDovetailContext, DistributedApplication)

- Name: `Register(IDovetailContext, DistributedApplication)`
- Modifiers: `abstract`

Register the DistributedApplication part.

```csharp
public interface IDistributedApplicationJoint
{
    public abstract void Register(
        IDovetailContext context,
        DistributedApplication host)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `host` (`DistributedApplication`)
