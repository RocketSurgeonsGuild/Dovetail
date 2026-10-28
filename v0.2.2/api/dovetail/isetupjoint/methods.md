# ISetupJoint Methods

- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Type: [ISetupJoint](/Dovetail/v0.2.2/api/dovetail/isetupjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Setup. Implements the [IDovetailJoint](/Dovetail/v0.2.2/api/dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext"></a>

## Register(IDovetailContext)

- Name: `Register(IDovetailContext)`
- Modifiers: `abstract`

Register the Setup part.

```csharp
public interface ISetupJoint
{
    public abstract void Register(
        IDovetailContext context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md))
