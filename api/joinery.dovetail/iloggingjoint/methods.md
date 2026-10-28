# ILoggingJoint Methods

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [ILoggingJoint](/Dovetail/api/joinery.dovetail/iloggingjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Logging. Implements the [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

<a id="register"></a>
<a id="register-idovetailcontext-iloggingbuilder"></a>

## Register(IDovetailContext, ILoggingBuilder)

- Name: `Register(IDovetailContext, ILoggingBuilder)`
- Modifiers: `abstract`

Register the Logging part.

```csharp
public interface ILoggingJoint
{
    public abstract void Register(
        IDovetailContext context,
        ILoggingBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)
