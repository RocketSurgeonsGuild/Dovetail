# IDistributedApplicationTestingJoint Methods

- Package: [Joinery.Dovetail.Aspire.Testing](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.aspire.testing.md)
- Type: [IDistributedApplicationTestingJoint](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.aspire.testing/idistributedapplicationtestingjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for DistributedApplicationTesting. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-idistributedapplicationtestingbuilder"></a>

## Register(IDovetailContext, IDistributedApplicationTestingBuilder)

- Name: `Register(IDovetailContext, IDistributedApplicationTestingBuilder)`
- Modifiers: `abstract`

Register the DistributedApplicationTesting part.

```csharp
public interface IDistributedApplicationTestingJoint
{
    public abstract void Register(
        IDovetailContext context,
        IDistributedApplicationTestingBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IDistributedApplicationTestingBuilder`)
