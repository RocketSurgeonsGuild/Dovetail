# ILoggingJoint Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-238/api/dovetail.md)
- Type: [ILoggingJoint](/Dovetail/pr-preview/pr-238/api/dovetail/iloggingjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for Logging. Implements the [IDovetailJoint](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailjoint.md)

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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
- `builder` (`ILoggingBuilder`)
