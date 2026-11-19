# DovetailExecutor Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-466/api/joinery.dovetail.md)
- Type: [DovetailExecutor](/Dovetail/pr-preview/pr-466/api/joinery.dovetail/dovetailexecutor.md)
- Kind: `Constructors`
- Members: `1`

A class to help with executing conventions

<a id="constructor"></a>
<a id="constructor-idovetailcontext"></a>

## DovetailExecutor(IDovetailContext)

- Name: `Constructor(IDovetailContext)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-466-merge.Sha.8c32fc2383e0ecb5c3325a81237d52253f24a3ca/src/Joinery.Dovetail/Infrastructure/DovetailExecutor.cs#L10-L165)

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

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-466/api/joinery.dovetail/idovetailcontext.md))
  The context whose joints will be executed.

## Remarks

This class uses [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-466/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) to handle exceptions
