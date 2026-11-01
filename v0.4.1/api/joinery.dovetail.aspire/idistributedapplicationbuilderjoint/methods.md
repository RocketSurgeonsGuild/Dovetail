# IDistributedApplicationBuilderJoint Methods

- Package: [Joinery.Dovetail.Aspire](/Dovetail/v0.4.1/api/joinery.dovetail.aspire.md)
- Type: [IDistributedApplicationBuilderJoint](/Dovetail/v0.4.1/api/joinery.dovetail.aspire/idistributedapplicationbuilderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for DistributedApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-idistributedapplicationbuilder"></a>

## Register(IDovetailContext, IDistributedApplicationBuilder)

- Name: `Register(IDovetailContext, IDistributedApplicationBuilder)`
- Modifiers: `abstract`

Register the DistributedApplicationBuilder part.

```csharp
public interface IDistributedApplicationBuilderJoint
{
    public abstract void Register(
        IDovetailContext context,
        IDistributedApplicationBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationBuilder`)
