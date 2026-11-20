# DovetailExecutor

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-468/api/joinery.dovetail.md)
- Version: `0.8.1-pr.5`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-468-merge.Sha.d585f4d77fe77956909200475d8894f0f38a443f/src/Joinery.Dovetail/Infrastructure/DovetailExecutor.cs)

A class to help with executing conventions

## Definition

```csharp
namespace Joinery.Dovetail.Infrastructure;

public class DovetailExecutor
{
    // ...
}
```

## Remarks

This class uses [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) to handle exceptions

## Constructors

- [DovetailExecutor(IDovetailContext)](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor/constructors.md#constructor-idovetailcontext) -- A class to help with executing conventions

## Methods

- [AddHandler(Action<TDovetail>)](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor/methods.md#addhandler-action-tdovetail) : [DovetailExecutor](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor.md) -- Add a synchronous convention
- [AddHandler(Func<TDovetail, ValueTask>)](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-valuetask) : [DovetailExecutor](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [AddHandler(Func<TDovetail, CancellationToken, ValueTask>)](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-cancellationtoken-valuetask) : [DovetailExecutor](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [Execute](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor/methods.md#execute) -- Run all the conventions
- [ExecuteAsync(CancellationToken)](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor/methods.md#executeasync-cancellationtoken) : `ValueTask` -- Run all the conventions
- [ExecuteWithContext](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor/methods.md#executewithcontext) : [IDovetailContext](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/idovetailcontext.md) -- Run all the conventions
- [ExecuteWithContextAsync(CancellationToken)](/Dovetail/pr-preview/pr-468/api/joinery.dovetail/dovetailexecutor/methods.md#executewithcontextasync-cancellationtoken) : `ValueTask<IDovetailContext>` -- Run all the conventions
