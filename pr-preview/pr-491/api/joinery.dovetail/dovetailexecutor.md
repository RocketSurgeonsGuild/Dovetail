# DovetailExecutor

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-491/api/joinery.dovetail.md)
- Version: `0.8.2-pr.17`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-491-merge.Sha.81844a51bc8d5dab18a91c60758495a62de5b76e/src/Joinery.Dovetail/Infrastructure/DovetailExecutor.cs)

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

This class uses [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) to handle exceptions

## Constructors

- [DovetailExecutor(IDovetailContext)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor/constructors.md#constructor-idovetailcontext) -- A class to help with executing conventions

## Methods

- [AddHandler(Action<TDovetail>)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor/methods.md#addhandler-action-tdovetail) : [DovetailExecutor](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor.md) -- Add a synchronous convention
- [AddHandler(Func<TDovetail, ValueTask>)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-valuetask) : [DovetailExecutor](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [AddHandler(Func<TDovetail, CancellationToken, ValueTask>)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-cancellationtoken-valuetask) : [DovetailExecutor](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [Execute](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor/methods.md#execute) -- Run all the conventions
- [ExecuteAsync(CancellationToken)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor/methods.md#executeasync-cancellationtoken) : `ValueTask` -- Run all the conventions
- [ExecuteWithContext](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor/methods.md#executewithcontext) : [IDovetailContext](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/idovetailcontext.md) -- Run all the conventions
- [ExecuteWithContextAsync(CancellationToken)](/Dovetail/pr-preview/pr-491/api/joinery.dovetail/dovetailexecutor/methods.md#executewithcontextasync-cancellationtoken) : `ValueTask<IDovetailContext>` -- Run all the conventions
