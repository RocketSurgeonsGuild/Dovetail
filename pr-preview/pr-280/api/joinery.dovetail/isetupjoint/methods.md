# ISetupJoint Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-280/api/joinery.dovetail.md)
- Type: [ISetupJoint](/Dovetail/pr-preview/pr-280/api/joinery.dovetail/isetupjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Setup. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-280/api/joinery.dovetail/idovetailjoint.md)

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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-280/api/joinery.dovetail/idovetailcontext.md))
