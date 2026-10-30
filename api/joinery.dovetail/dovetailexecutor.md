# DovetailExecutor

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.3.1-beta.6`
- Namespace: `Joinery.Dovetail.Infrastructure`
- Target framework: `net10.0`

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

This class uses [DovetailExceptionPolicyDelegate](/Dovetail/api/joinery.dovetail/dovetailexceptionpolicydelegate.md) to handle exceptions

## Constructors

- [DovetailExecutor(IDovetailContext)](/Dovetail/api/joinery.dovetail/dovetailexecutor/constructors.md#constructor-idovetailcontext) -- A class to help with executing conventions

## Methods

- [AddHandler(Action<TDovetail>)](/Dovetail/api/joinery.dovetail/dovetailexecutor/methods.md#addhandler-action-tdovetail) : [DovetailExecutor](/Dovetail/api/joinery.dovetail/dovetailexecutor.md) -- Add a synchronous convention
- [AddHandler(Func<TDovetail, ValueTask>)](/Dovetail/api/joinery.dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-valuetask) : [DovetailExecutor](/Dovetail/api/joinery.dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [AddHandler(Func<TDovetail, CancellationToken, ValueTask>)](/Dovetail/api/joinery.dovetail/dovetailexecutor/methods.md#addhandler-func-tdovetail-cancellationtoken-valuetask) : [DovetailExecutor](/Dovetail/api/joinery.dovetail/dovetailexecutor.md) -- Add an asynchronous convention
- [Execute](/Dovetail/api/joinery.dovetail/dovetailexecutor/methods.md#execute) -- Run all the conventions
- [ExecuteAsync(CancellationToken)](/Dovetail/api/joinery.dovetail/dovetailexecutor/methods.md#executeasync-cancellationtoken) : `ValueTask` -- Run all the conventions
- [ExecuteWithContext](/Dovetail/api/joinery.dovetail/dovetailexecutor/methods.md#executewithcontext) : [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md) -- Run all the conventions
- [ExecuteWithContextAsync(CancellationToken)](/Dovetail/api/joinery.dovetail/dovetailexecutor/methods.md#executewithcontextasync-cancellationtoken) : `ValueTask<IDovetailContext>` -- Run all the conventions
