# DovetailExecutor

- Kind: `class`
- Package: [Dovetail](/Dovetail/pr-preview/pr-241/api/dovetail.md)
- Version: `0.2.3-pr.3`
- Namespace: `Dovetail.Infrastructure`
- Target framework: `net10.0`

A class to help with executing conventions

## Definition

```csharp
namespace Dovetail.Infrastructure;

public class DovetailExecutor
{
    // ...
}
```

## Remarks

This class uses [DovetailExceptionPolicyDelegate](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexceptionpolicydelegate.md) to handle exceptions

## Constructors

- [DovetailExecutor(IDovetailContext)](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor/constructors.md#constructor-idovetailcontext) -- A class to help with executing conventions

## Methods

- [AddHandler(Action<TDovetail>)](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor/methods.md#addhandler-action-tdovetail) : [DovetailExecutor](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor.md) -- Add a synchronous convention
- [AddHandler(Func<TDovetail, ValueTask>)](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-valuetask) : [DovetailExecutor](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [AddHandler(Func<TDovetail, CancellationToken, ValueTask>)](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-cancellationtoken-valuetask) : [DovetailExecutor](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [Execute](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor/methods.md#execute) -- Run all the conventions
- [ExecuteAsync(CancellationToken)](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor/methods.md#executeasync-cancellationtoken) : `ValueTask` -- Run all the conventions
- [ExecuteWithContext](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor/methods.md#executewithcontext) : [IDovetailContext](/Dovetail/pr-preview/pr-241/api/dovetail/idovetailcontext.md) -- Run all the conventions
- [ExecuteWithContextAsync(CancellationToken)](/Dovetail/pr-preview/pr-241/api/dovetail/dovetailexecutor/methods.md#executewithcontextasync-cancellationtoken) : `ValueTask<IDovetailContext>` -- Run all the conventions
