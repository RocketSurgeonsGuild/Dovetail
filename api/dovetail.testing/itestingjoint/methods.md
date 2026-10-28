# ITestingJoint Methods

- Package: [Dovetail.Testing](/Dovetail/api/dovetail.testing.md)
- Type: [ITestingJoint](/Dovetail/api/dovetail.testing/itestingjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Testing. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext"></a>

## Register(IDovetailContext)

- Name: `Register(IDovetailContext)`
- Modifiers: `abstract`

Register the Testing part.

```csharp
public interface ITestingJoint
{
    public abstract void Register(
        IDovetailContext context)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
