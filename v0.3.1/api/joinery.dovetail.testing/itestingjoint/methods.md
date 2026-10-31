# ITestingJoint Methods

- Package: [Joinery.Dovetail.Testing](/Dovetail/v0.3.1/api/joinery.dovetail.testing.md)
- Type: [ITestingJoint](/Dovetail/v0.3.1/api/joinery.dovetail.testing/itestingjoint.md)
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
