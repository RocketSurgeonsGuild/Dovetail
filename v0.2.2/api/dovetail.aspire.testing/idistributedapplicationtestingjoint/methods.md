# IDistributedApplicationTestingJoint Methods

- Package: [Dovetail.Aspire.Testing](/Dovetail/v0.2.2/api/dovetail.aspire.testing.md)
- Type: [IDistributedApplicationTestingJoint](/Dovetail/v0.2.2/api/dovetail.aspire.testing/idistributedapplicationtestingjoint.md)
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
