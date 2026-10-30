# DovetailExecutor Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-269/api/joinery.dovetail.md)
- Type: [DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md)
- Kind: `Methods`
- Members: `7`

A class to help with executing conventions

<a id="addhandler"></a>
<a id="addhandler-action-tdovetail"></a>

## AddHandler(Action<TDovetail>)

- Name: `AddHandler(Action<TDovetail>)`
- Returns: [DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md)

Add a synchronous convention

```csharp
public class DovetailExecutor
{
    public DovetailExecutor AddHandler<TDovetail>(
        Action<TDovetail> action)
    {
        // ...
    }
}
```

## Parameters

- `action` (`Action<TDovetail>`)
  The action to run against each matching `TDovetail` joint.

## Returns

[DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md) -- This [DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md), for chaining.

<a id="addhandler-func-tdovetail-valuetask"></a>

## AddHandler(Func<TDovetail, ValueTask>)

- Name: `AddHandler(Func<TDovetail, ValueTask>)`
- Returns: [DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md)

Add an asynchronous convention

```csharp
public class DovetailExecutor
{
    public DovetailExecutor AddHandler<TDovetail>(
        Func<TDovetail, ValueTask> action)
    {
        // ...
    }
}
```

## Parameters

- `action` (`Func<TDovetail, ValueTask>`)
  The asynchronous action to run against each matching `TDovetail` joint.

## Returns

[DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md) -- This [DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md), for chaining.

<a id="addhandler-func-tdovetail-cancellationtoken-valuetask"></a>

## AddHandler(Func<TDovetail, CancellationToken, ValueTask>)

- Name: `AddHandler(Func<TDovetail, CancellationToken, ValueTask>)`
- Returns: [DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md)

Add an asynchronous convention

```csharp
public class DovetailExecutor
{
    public DovetailExecutor AddHandler<TDovetail>(
        Func<TDovetail, CancellationToken, ValueTask> action)
    {
        // ...
    }
}
```

## Parameters

- `action` (`Func<TDovetail, CancellationToken, ValueTask>`)
  The asynchronous, cancellable action to run against each matching `TDovetail` joint.

## Returns

[DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md) -- This [DovetailExecutor](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/dovetailexecutor.md), for chaining.

<a id="execute"></a>

## Execute

- Name: `Execute`

Run all the conventions

```csharp
public class DovetailExecutor
{
    public void Execute()
    {
        // ...
    }
}
```

<a id="executeasync"></a>
<a id="executeasync-cancellationtoken"></a>

## ExecuteAsync(CancellationToken)

- Name: `ExecuteAsync(CancellationToken)`
- Returns: `ValueTask`

Run all the conventions

```csharp
public class DovetailExecutor
{
    public ValueTask ExecuteAsync(
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `cancellationToken` (`CancellationToken`)
  The cancellation token used while executing the conventions.

## Returns

`ValueTask` -- A `Tasks.ValueTask` representing the asynchronous execution of the conventions.

<a id="executewithcontext"></a>

## ExecuteWithContext

- Name: `ExecuteWithContext`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/idovetailcontext.md)

Run all the conventions

```csharp
public class DovetailExecutor
{
    public IDovetailContext ExecuteWithContext()
    {
        // ...
    }
}
```

## Returns

[IDovetailContext](/Dovetail/pr-preview/pr-269/api/joinery.dovetail/idovetailcontext.md) -- The context the conventions were executed against.

<a id="executewithcontextasync"></a>
<a id="executewithcontextasync-cancellationtoken"></a>

## ExecuteWithContextAsync(CancellationToken)

- Name: `ExecuteWithContextAsync(CancellationToken)`
- Returns: `ValueTask<IDovetailContext>`

Run all the conventions

```csharp
public class DovetailExecutor
{
    public ValueTask<IDovetailContext> ExecuteWithContextAsync(
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `cancellationToken` (`CancellationToken`)
  The cancellation token used while executing the conventions.

## Returns

`ValueTask<IDovetailContext>` -- A `Tasks.ValueTask`1` producing the context the conventions were executed against.
