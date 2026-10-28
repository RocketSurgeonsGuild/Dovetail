# DovetailExecutor

- Kind: `class`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.3-beta.1`
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

This class uses [DovetailExceptionPolicyDelegate](/Dovetail/api/dovetail/dovetailexceptionpolicydelegate.md) to handle exceptions

## Constructors

- [DovetailExecutor(IDovetailContext)](/Dovetail/api/dovetail/dovetailexecutor/constructors.md#constructor-idovetailcontext) -- A class to help with executing conventions

## Methods

- [AddHandler(Action<TDovetail>)](/Dovetail/api/dovetail/dovetailexecutor/methods.md#addhandler-action-tdovetail) : [DovetailExecutor](/Dovetail/api/dovetail/dovetailexecutor.md) -- Add a synchronous convention
- [AddHandler(Func<TDovetail, ValueTask>)](/Dovetail/api/dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-valuetask) : [DovetailExecutor](/Dovetail/api/dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [AddHandler(Func<TDovetail, CancellationToken, ValueTask>)](/Dovetail/api/dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-cancellationtoken-valuetask) : [DovetailExecutor](/Dovetail/api/dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [Execute](/Dovetail/api/dovetail/dovetailexecutor/methods.md#execute) -- Run all the conventions
- [ExecuteAsync(CancellationToken)](/Dovetail/api/dovetail/dovetailexecutor/methods.md#executeasync-cancellationtoken) : `ValueTask` -- Run all the conventions
- [ExecuteWithContext](/Dovetail/api/dovetail/dovetailexecutor/methods.md#executewithcontext) : [IDovetailContext](/Dovetail/api/dovetail/idovetailcontext.md) -- Run all the conventions
- [ExecuteWithContextAsync(CancellationToken)](/Dovetail/api/dovetail/dovetailexecutor/methods.md#executewithcontextasync-cancellationtoken) : `ValueTask<IDovetailContext>` -- Run all the conventions
