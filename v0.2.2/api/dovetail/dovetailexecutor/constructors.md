# DovetailExecutor Constructors

- Package: [Dovetail](/Dovetail/v0.2.2/api/dovetail.md)
- Type: [DovetailExecutor](/Dovetail/v0.2.2/api/dovetail/dovetailexecutor.md)
- Kind: `Constructors`
- Members: `1`

A class to help with executing conventions

<a id="constructor"></a>
<a id="constructor-idovetailcontext"></a>

## DovetailExecutor(IDovetailContext)

- Name: `Constructor(IDovetailContext)`

A class to help with executing conventions

```csharp
public class DovetailExecutor
{
    public DovetailExecutor(
        IDovetailContext context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/v0.2.2/api/dovetail/idovetailcontext.md))
  The context whose joints will be executed.

## Remarks

This class uses [DovetailExceptionPolicyDelegate](/Dovetail/v0.2.2/api/dovetail/dovetailexceptionpolicydelegate.md) to handle exceptions
