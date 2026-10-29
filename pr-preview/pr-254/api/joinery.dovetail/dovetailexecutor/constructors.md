# DovetailExecutor Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-254/api/joinery.dovetail.md)
- Type: [DovetailExecutor](/Dovetail/pr-preview/pr-254/api/joinery.dovetail/dovetailexecutor.md)
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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-254/api/joinery.dovetail/idovetailcontext.md))
  The context whose joints will be executed.

## Remarks

This class uses [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-254/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) to handle exceptions
