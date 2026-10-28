# DovetailHostBuilderExtensions Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-237/api/dovetail.md)
- Type: [DovetailHostBuilderExtensions](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailhostbuilderextensions.md)
- Kind: `Methods`
- Members: `20`

Base convention extensions

<a id="addifmissing"></a>
<a id="addifmissing-dovetailcontextbuilder-t"></a>

## AddIfMissing(DovetailContextBuilder, T)

- Name: `AddIfMissing(DovetailContextBuilder, T)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md)

Set key to the value if the type is missing

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailContextBuilder AddIfMissing<T>(
        this DovetailContextBuilder builder,
        T value)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The builder
- `value` (`T`)
  The value to save

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.

<a id="addifmissing-dovetailcontextbuilder-type-object"></a>

## AddIfMissing(DovetailContextBuilder, Type, object)

- Name: `AddIfMissing(DovetailContextBuilder, Type, object)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md)

Set key to the value if the key is missing

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailContextBuilder AddIfMissing(
        this DovetailContextBuilder builder,
        Type key,
        object value)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The builder
- `key` (`Type`)
  The key where the value is saved
- `value` (`object`)
  The value to save

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.

<a id="addifmissing-dovetailcontextbuilder-string-t"></a>

## AddIfMissing(DovetailContextBuilder, string, T)

- Name: `AddIfMissing(DovetailContextBuilder, string, T)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md)

Set key to the value if the key is missing

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailContextBuilder AddIfMissing<T>(
        this DovetailContextBuilder builder,
        string key,
        T value)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The builder
- `key` (`string`)
  The key where the value is saved
- `value` (`T`)
  The value to save

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.

<a id="get"></a>
<a id="get-dovetailcontextbuilder"></a>

## Get(DovetailContextBuilder)

- Name: `Get(DovetailContextBuilder)`
- Modifiers: `extension` `nullable`
- Returns: `T?`

Get a value by type from the context

```csharp
public static class DovetailHostBuilderExtensions
{
    public static T? Get<T>(
        this DovetailContextBuilder context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context

## Returns

`T?` -- T.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="get-dovetailcontextbuilder-string"></a>

## Get(DovetailContextBuilder, string)

- Name: `Get(DovetailContextBuilder, string)`
- Modifiers: `extension` `nullable`
- Returns: `T?`

Get a value by key from the context

```csharp
public static class DovetailHostBuilderExtensions
{
    public static T? Get<T>(
        this DovetailContextBuilder context,
        string key)
    {
        // ...
    }
}
```

## Parameters

- `context` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context
- `key` (`string`)
  The key where the value is saved

## Returns

`T?` -- T.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="gethosttype"></a>
<a id="gethosttype-dovetailcontextbuilder"></a>

## GetHostType(DovetailContextBuilder)

- Name: `GetHostType(DovetailContextBuilder)`
- Modifiers: `extension`
- Returns: [DovetailHostType](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailhosttype.md)

Get the host type the builder is currently configured for

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailHostType GetHostType(
        this DovetailContextBuilder context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context

## Returns

[DovetailHostType](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailhosttype.md) -- The [DovetailHostType](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailhosttype.md) for the builder, or [DovetailHostType.Undefined](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailhosttype/fields.md) if none was registered.

<a id="getoradd"></a>
<a id="getoradd-dovetailcontextbuilder-func-t"></a>

## GetOrAdd(DovetailContextBuilder, Func<T>)

- Name: `GetOrAdd(DovetailContextBuilder, Func<T>)`
- Modifiers: `extension`
- Returns: `T`

Get a value by key from the context

```csharp
public static class DovetailHostBuilderExtensions
{
    public static T GetOrAdd<T>(
        this DovetailContextBuilder builder,
        Func<T> factory)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The builder
- `factory` (`Func<T>`)
  The factory method in the event the type is not found

## Returns

`T` -- T.

## Exceptions

- `ArgumentNullException` -- `builder` or `factory` is `null`.

<a id="getoradd-dovetailcontextbuilder-string-func-t"></a>

## GetOrAdd(DovetailContextBuilder, string, Func<T>)

- Name: `GetOrAdd(DovetailContextBuilder, string, Func<T>)`
- Modifiers: `extension`
- Returns: `T`

Get a value by key from the context

```csharp
public static class DovetailHostBuilderExtensions
{
    public static T GetOrAdd<T>(
        this DovetailContextBuilder builder,
        string key,
        Func<T> factory)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The builder
- `key` (`string`)
  The key where the value is saved
- `factory` (`Func<T>`)
  The factory method in the event the type is not found

## Returns

`T` -- T.

## Exceptions

- `ArgumentNullException` -- `builder` or `factory` is `null`.

<a id="isunittesthost"></a>
<a id="isunittesthost-dovetailcontextbuilder"></a>

## IsUnitTestHost(DovetailContextBuilder)

- Name: `IsUnitTestHost(DovetailContextBuilder)`
- Modifiers: `extension`
- Returns: `bool`

Check if this is a test host (to allow conventions to behave differently during unit tests)

```csharp
public static class DovetailHostBuilderExtensions
{
    public static bool IsUnitTestHost(
        this DovetailContextBuilder context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context

## Returns

`bool` -- `true` if `context` is running under [DovetailHostType.UnitTest](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailhosttype/fields.md); otherwise, `false`.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="require"></a>
<a id="require-dovetailcontextbuilder"></a>

## Require(DovetailContextBuilder)

- Name: `Require(DovetailContextBuilder)`
- Modifiers: `extension`
- Returns: `T`

Get a value by type from the context or throw

```csharp
public static class DovetailHostBuilderExtensions
{
    public static T Require<T>(
        this DovetailContextBuilder context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context

## Returns

`T` -- T.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
- `KeyNotFoundException` -- No value of type `T` was found in the context.

<a id="require-dovetailcontextbuilder-string"></a>

## Require(DovetailContextBuilder, string)

- Name: `Require(DovetailContextBuilder, string)`
- Modifiers: `extension`
- Returns: `T`

Get a value by type from the context or throw

```csharp
public static class DovetailHostBuilderExtensions
{
    public static T Require<T>(
        this DovetailContextBuilder context,
        string key)
    {
        // ...
    }
}
```

## Parameters

- `context` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context
- `key` (`string`)
  The key where the value is saved

## Returns

`T` -- T.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.
- `KeyNotFoundException` -- No value of type `T` was found at `key`.

<a id="set"></a>
<a id="set-dovetailcontextbuilder-t"></a>

## Set(DovetailContextBuilder, T)

- Name: `Set(DovetailContextBuilder, T)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md)

Get a value by type from the context

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailContextBuilder Set<T>(
        this DovetailContextBuilder context,
        T value)
    {
        // ...
    }
}
```

## Parameters

- `context` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context
- `value` (`T`)
  The value to save

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="set-dovetailcontextbuilder-type-object"></a>

## Set(DovetailContextBuilder, Type, object)

- Name: `Set(DovetailContextBuilder, Type, object)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md)

Get a value by type from the context

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailContextBuilder Set(
        this DovetailContextBuilder context,
        Type key,
        object value)
    {
        // ...
    }
}
```

## Parameters

- `context` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context
- `key` (`Type`)
  The key where the value is saved
- `value` (`object`)
  The value to save

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="set-dovetailcontextbuilder-string-t"></a>

## Set(DovetailContextBuilder, string, T)

- Name: `Set(DovetailContextBuilder, string, T)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md)

Get a value by type from the context

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailContextBuilder Set<T>(
        this DovetailContextBuilder context,
        string key,
        T value)
    {
        // ...
    }
}
```

## Parameters

- `context` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context
- `key` (`string`)
  The key where the value is saved
- `value` (`T`)
  The value to save

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="useserviceproviderfactory"></a>
<a id="useserviceproviderfactory-dovetailcontextbuilder-iserviceproviderfactory-tcontainerbuilder"></a>

## UseServiceProviderFactory(DovetailContextBuilder, IServiceProviderFactory<TContainerBuilder>)

- Name: `UseServiceProviderFactory(DovetailContextBuilder, IServiceProviderFactory<TContainerBuilder>)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md)

Set the service provider factory to be used for hosting or other systems.

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailContextBuilder UseServiceProviderFactory<TContainerBuilder>(
        this DovetailContextBuilder builder,
        IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context builder to configure.
- `serviceProviderFactory` (`IServiceProviderFactory<TContainerBuilder>`)
  The service provider factory to use.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md) -- The `builder`, for chaining.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.

<a id="useserviceproviderfactory-dovetailcontextbuilder-func-idovetailcontext-iservicecollection-cancellationtoken-valuetask-iserviceproviderfactory-tcontainerbuilder"></a>

## UseServiceProviderFactory(DovetailContextBuilder, Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)

- Name: `UseServiceProviderFactory(DovetailContextBuilder, Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md)

Set the service provider factory to be used for hosting or other systems.

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailContextBuilder UseServiceProviderFactory<TContainerBuilder>(
        this DovetailContextBuilder builder,
        Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>> serviceProviderFactory)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context builder to configure.
- `serviceProviderFactory` (`Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>`)
  A factory method that asynchronously produces the service provider factory to use.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md) -- The `builder`, for chaining.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.

<a id="useserviceproviderfactory-dovetailcontextbuilder-func-idovetailcontext-iservicecollection-valuetask-iserviceproviderfactory-tcontainerbuilder"></a>

## UseServiceProviderFactory(DovetailContextBuilder, Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)

- Name: `UseServiceProviderFactory(DovetailContextBuilder, Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)`
- Modifiers: `extension`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md)

Set the service provider factory to be used for hosting or other systems.

```csharp
public static class DovetailHostBuilderExtensions
{
    public static DovetailContextBuilder UseServiceProviderFactory<TContainerBuilder>(
        this DovetailContextBuilder builder,
        Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>> serviceProviderFactory)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md))
  The context builder to configure.
- `serviceProviderFactory` (`Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>`)
  A factory method that asynchronously produces the service provider factory to use.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcontextbuilder.md) -- The `builder`, for chaining.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.

<a id="useserviceproviderfactory-idovetailcontext-iserviceproviderfactory-tcontainerbuilder"></a>

## UseServiceProviderFactory(IDovetailContext, IServiceProviderFactory<TContainerBuilder>)

- Name: `UseServiceProviderFactory(IDovetailContext, IServiceProviderFactory<TContainerBuilder>)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md)

Set the service provider factory to be used for hosting or other systems.

```csharp
public static class DovetailHostBuilderExtensions
{
    public static IDovetailContext UseServiceProviderFactory<TContainerBuilder>(
        this IDovetailContext builder,
        IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md))
  The context to configure.
- `serviceProviderFactory` (`IServiceProviderFactory<TContainerBuilder>`)
  The service provider factory to use.

## Returns

[IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md) -- The `builder`, for chaining.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.

<a id="useserviceproviderfactory-idovetailcontext-func-idovetailcontext-iservicecollection-cancellationtoken-valuetask-iserviceproviderfactory-tcontainerbuilder"></a>

## UseServiceProviderFactory(IDovetailContext, Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)

- Name: `UseServiceProviderFactory(IDovetailContext, Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md)

Set the service provider factory to be used for hosting or other systems.

```csharp
public static class DovetailHostBuilderExtensions
{
    public static IDovetailContext UseServiceProviderFactory<TContainerBuilder>(
        this IDovetailContext builder,
        Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>> serviceProviderFactory)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md))
  The context to configure.
- `serviceProviderFactory` (`Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>>`)
  A factory method that asynchronously produces the service provider factory to use.

## Returns

[IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md) -- The `builder`, for chaining.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.

<a id="useserviceproviderfactory-idovetailcontext-func-idovetailcontext-iservicecollection-valuetask-iserviceproviderfactory-tcontainerbuilder"></a>

## UseServiceProviderFactory(IDovetailContext, Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)

- Name: `UseServiceProviderFactory(IDovetailContext, Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>)`
- Modifiers: `extension`
- Returns: [IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md)

Set the service provider factory to be used for hosting or other systems.

```csharp
public static class DovetailHostBuilderExtensions
{
    public static IDovetailContext UseServiceProviderFactory<TContainerBuilder>(
        this IDovetailContext builder,
        Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>> serviceProviderFactory)
    {
        // ...
    }
}
```

## Parameters

- `builder` ([IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md))
  The context to configure.
- `serviceProviderFactory` (`Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>>`)
  A factory method that asynchronously produces the service provider factory to use.

## Returns

[IDovetailContext](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailcontext.md) -- The `builder`, for chaining.

## Exceptions

- `ArgumentNullException` -- `builder` is `null`.
