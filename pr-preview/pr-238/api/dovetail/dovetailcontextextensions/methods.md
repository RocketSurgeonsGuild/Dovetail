# DovetailContextExtensions Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-238/api/dovetail.md)
- Type: [DovetailContextExtensions](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailcontextextensions.md)
- Kind: `Methods`
- Members: `18`

Base convention extensions

<a id="addifmissing"></a>
<a id="addifmissing-idovetailcontext-t"></a>

## AddIfMissing(IDovetailContext, T)

- Name: `AddIfMissing(IDovetailContext, T)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md)

Set key to the value if the type is missing

```csharp
public static class DovetailContextExtensions
{
    public static IDovetailContext AddIfMissing<T>(
        this IDovetailContext context,
        T value)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context
- `value` (`T`)
  The value to save

## Returns

[IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md) -- The `context`, for chaining.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="addifmissing-idovetailcontext-type-object"></a>

## AddIfMissing(IDovetailContext, Type, object)

- Name: `AddIfMissing(IDovetailContext, Type, object)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md)

Set key to the value if the key is missing

```csharp
public static class DovetailContextExtensions
{
    public static IDovetailContext AddIfMissing(
        this IDovetailContext context,
        Type key,
        object value)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The properties
- `key` (`Type`)
  The key where the value is saved
- `value` (`object`)
  The value to save

## Returns

[IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md) -- The `context`, for chaining.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="addifmissing-idovetailcontext-string-t"></a>

## AddIfMissing(IDovetailContext, string, T)

- Name: `AddIfMissing(IDovetailContext, string, T)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md)

Set key to the value if the key is missing

```csharp
public static class DovetailContextExtensions
{
    public static IDovetailContext AddIfMissing<T>(
        this IDovetailContext context,
        string key,
        T value)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The properties
- `key` (`string`)
  The key where the value is saved
- `value` (`T`)
  The value to save

## Returns

[IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md) -- The `context`, for chaining.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="get"></a>
<a id="get-idovetailcontext"></a>

## Get(IDovetailContext)

- Name: `Get(IDovetailContext)`
- Modifiers: `extension` `nullable`
- Returns: `T?`

Get a value by type from the context

```csharp
public static class DovetailContextExtensions
{
    public static T? Get<T>(
        this IDovetailContext context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context

## Returns

`T?` -- T.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="get-idovetailcontext-string"></a>

## Get(IDovetailContext, string)

- Name: `Get(IDovetailContext, string)`
- Modifiers: `extension` `nullable`
- Returns: `T?`

Get a value by key from the context

```csharp
public static class DovetailContextExtensions
{
    public static T? Get<T>(
        this IDovetailContext context,
        string key)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context
- `key` (`string`)
  The key where the value is saved

## Returns

`T?` -- T.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="gethosttype"></a>
<a id="gethosttype-idovetailcontext"></a>

## GetHostType(IDovetailContext)

- Name: `GetHostType(IDovetailContext)`
- Modifiers: `extension`
- Returns: [DovetailHostType](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailhosttype.md)

Get the host type the context is currently running under

```csharp
public static class DovetailContextExtensions
{
    public static DovetailHostType GetHostType(
        this IDovetailContext context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context

## Returns

[DovetailHostType](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailhosttype.md) -- The [DovetailHostType](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailhosttype.md) for the context, or [DovetailHostType.Undefined](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailhosttype/fields.md) if none was registered.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="getoradd"></a>
<a id="getoradd-idovetailcontext-func-t"></a>

## GetOrAdd(IDovetailContext, Func<T>)

- Name: `GetOrAdd(IDovetailContext, Func<T>)`
- Modifiers: `extension`
- Returns: `T`

Get a value by key from the context

```csharp
public static class DovetailContextExtensions
{
    public static T GetOrAdd<T>(
        this IDovetailContext context,
        Func<T> factory)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context
- `factory` (`Func<T>`)
  The factory method in the event the type is not found

## Returns

`T` -- T.

## Exceptions

- `ArgumentNullException` -- `context` or `factory` is `null`.

<a id="getoradd-idovetailcontext-string-func-t"></a>

## GetOrAdd(IDovetailContext, string, Func<T>)

- Name: `GetOrAdd(IDovetailContext, string, Func<T>)`
- Modifiers: `extension`
- Returns: `T`

Get a value by key from the context

```csharp
public static class DovetailContextExtensions
{
    public static T GetOrAdd<T>(
        this IDovetailContext context,
        string key,
        Func<T> factory)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context
- `key` (`string`)
  The key where the value is saved
- `factory` (`Func<T>`)
  The factory method in the event the type is not found

## Returns

`T` -- T.

## Exceptions

- `ArgumentNullException` -- `context` or `factory` is `null`.

<a id="isunittesthost"></a>
<a id="isunittesthost-idovetailcontext"></a>

## IsUnitTestHost(IDovetailContext)

- Name: `IsUnitTestHost(IDovetailContext)`
- Modifiers: `extension`
- Returns: `bool`

Check if this is a test host (to allow conventions to behave differently during unit tests)

```csharp
public static class DovetailContextExtensions
{
    public static bool IsUnitTestHost(
        this IDovetailContext context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context

## Returns

`bool` -- `true` if `context` is running under [DovetailHostType.UnitTest](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailhosttype/fields.md); otherwise, `false`.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="registerjoints"></a>
<a id="registerjoints-idovetailcontext-action-dovetailexecutor"></a>

## RegisterJoints(IDovetailContext, Action<DovetailExecutor>)

- Name: `RegisterJoints(IDovetailContext, Action<DovetailExecutor>)`
- Modifiers: `extension`

Register a set of conventions

```csharp
public static class DovetailContextExtensions
{
    public static void RegisterJoints(
        this IDovetailContext context,
        Action<DovetailExecutor> configure)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context to execute the conventions against.
- `configure` (`Action<DovetailExecutor>`)
  A callback used to configure which joints the [DovetailExecutor](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailexecutor.md) should execute.

## Exceptions

- `ArgumentNullException` -- `context` or `configure` is `null`.

<a id="registerjointsasync"></a>
<a id="registerjointsasync-idovetailcontext-action-dovetailexecutor-cancellationtoken"></a>

## RegisterJointsAsync(IDovetailContext, Action<DovetailExecutor>, CancellationToken)

- Name: `RegisterJointsAsync(IDovetailContext, Action<DovetailExecutor>, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask`

Register a set of conventions

```csharp
public static class DovetailContextExtensions
{
    public static ValueTask RegisterJointsAsync(
        this IDovetailContext context,
        Action<DovetailExecutor> configure,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context to execute the conventions against.
- `configure` (`Action<DovetailExecutor>`)
  A callback used to configure which joints the [DovetailExecutor](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailexecutor.md) should execute.
- `cancellationToken` (`CancellationToken`) `optional`
  The cancellation token used during execution.

## Returns

`ValueTask` -- A `Tasks.ValueTask` representing the asynchronous execution of the conventions.

## Exceptions

- `ArgumentNullException` -- `context` or `configure` is `null`.

<a id="registerjointswithcontext"></a>
<a id="registerjointswithcontext-idovetailcontext-action-dovetailexecutor"></a>

## RegisterJointsWithContext(IDovetailContext, Action<DovetailExecutor>)

- Name: `RegisterJointsWithContext(IDovetailContext, Action<DovetailExecutor>)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md)

Register a set of conventions

```csharp
public static class DovetailContextExtensions
{
    public static IDovetailContext RegisterJointsWithContext(
        this IDovetailContext context,
        Action<DovetailExecutor> configure)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context to execute the conventions against.
- `configure` (`Action<DovetailExecutor>`)
  A callback used to configure which joints the [DovetailExecutor](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailexecutor.md) should execute.

## Returns

[IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md) -- The `context` after executing the conventions.

## Exceptions

- `ArgumentNullException` -- `context` or `configure` is `null`.

<a id="registerjointswithcontextasync"></a>
<a id="registerjointswithcontextasync-idovetailcontext-action-dovetailexecutor-cancellationtoken"></a>

## RegisterJointsWithContextAsync(IDovetailContext, Action<DovetailExecutor>, CancellationToken)

- Name: `RegisterJointsWithContextAsync(IDovetailContext, Action<DovetailExecutor>, CancellationToken)`
- Modifiers: `extension`
- Returns: `ValueTask<IDovetailContext>`

Register a set of conventions

```csharp
public static class DovetailContextExtensions
{
    public static ValueTask<IDovetailContext> RegisterJointsWithContextAsync(
        this IDovetailContext context,
        Action<DovetailExecutor> configure,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context to execute the conventions against.
- `configure` (`Action<DovetailExecutor>`)
  A callback used to configure which joints the [DovetailExecutor](/Dovetail/pr-preview/pr-238/api/dovetail/dovetailexecutor.md) should execute.
- `cancellationToken` (`CancellationToken`)
  The cancellation token used during execution.

## Returns

`ValueTask<IDovetailContext>` -- A `Tasks.ValueTask`1` producing the `context` after executing the conventions.

## Exceptions

- `ArgumentNullException` -- `context` or `configure` is `null`.

<a id="require"></a>
<a id="require-idovetailcontext"></a>

## Require(IDovetailContext)

- Name: `Require(IDovetailContext)`
- Modifiers: `extension`
- Returns: `T`

Get a value by type from the context or throw

```csharp
public static class DovetailContextExtensions
{
    public static T Require<T>(
        this IDovetailContext context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context

## Returns

`T` -- T.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
- `KeyNotFoundException` -- No value of type `T` was found in the context.

<a id="require-idovetailcontext-string"></a>

## Require(IDovetailContext, string)

- Name: `Require(IDovetailContext, string)`
- Modifiers: `extension`
- Returns: `T`

Get a value by type from the context or throw

```csharp
public static class DovetailContextExtensions
{
    public static T Require<T>(
        this IDovetailContext context,
        string key)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context
- `key` (`string`)
  The key where the value is saved

## Returns

`T` -- T.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
- `KeyNotFoundException` -- No value of type `T` was found at `key`.

<a id="set"></a>
<a id="set-idovetailcontext-t"></a>

## Set(IDovetailContext, T)

- Name: `Set(IDovetailContext, T)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md)

Get a value by type from the context

```csharp
public static class DovetailContextExtensions
{
    public static IDovetailContext Set<T>(
        this IDovetailContext context,
        T value)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context
- `value` (`T`)
  The value to save

## Exceptions

- `ArgumentNullException` -- `context` or `value` is `null`.

<a id="set-idovetailcontext-type-object"></a>

## Set(IDovetailContext, Type, object)

- Name: `Set(IDovetailContext, Type, object)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md)

Get a value by type from the context

```csharp
public static class DovetailContextExtensions
{
    public static IDovetailContext Set(
        this IDovetailContext context,
        Type key,
        object value)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context
- `key` (`Type`)
  The key where the value is saved
- `value` (`object`)
  The value to save

## Exceptions

- `ArgumentNullException` -- `context` or `key` is `null`.

<a id="set-idovetailcontext-string-t"></a>

## Set(IDovetailContext, string, T)

- Name: `Set(IDovetailContext, string, T)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md)

Get a value by type from the context

```csharp
public static class DovetailContextExtensions
{
    public static IDovetailContext Set<T>(
        this IDovetailContext context,
        string key,
        T value)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-238/api/dovetail/idovetailcontext.md))
  The context
- `key` (`string`)
  The key where the value is saved
- `value` (`T`)
  The value to save

## Exceptions

- `ArgumentNullException` -- `context` is `null`, or `key` is `null`.
- `ArgumentException` -- `key` is empty or consists only of white-space characters.
