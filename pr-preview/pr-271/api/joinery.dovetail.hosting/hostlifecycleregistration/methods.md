# HostLifecycleRegistration Methods

- Package: [Joinery.Dovetail.Hosting](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting.md)
- Type: [HostLifecycleRegistration](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md)
- Kind: `Methods`
- Members: `8`

A handler to run at one [HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md), added to the service collection by the `OnHostStarting`, `OnHostStarted`, `OnHostStopping` and `OnHostStopped` extension methods that Joinery.Dovetail.Hosting emits into every project that references the generic host (Microsoft.Extensions.Hosting).

<a id="clone"></a>

## <Clone>$

- Name: `<Clone>$`
- Returns: [HostLifecycleRegistration](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md)

```csharp
public record HostLifecycleRegistration
{
    public HostLifecycleRegistration <Clone>$()
    {
        // ...
    }
}
```

<a id="deconstruct"></a>
<a id="deconstruct-hostlifecyclemoment-func-iserviceprovider-cancellationtoken-task"></a>

## Deconstruct(HostLifecycleMoment, Func<IServiceProvider, CancellationToken, Task>)

- Name: `Deconstruct(HostLifecycleMoment, Func<IServiceProvider, CancellationToken, Task>)`

```csharp
public record HostLifecycleRegistration
{
    public void Deconstruct(
        out HostLifecycleMoment Moment,
        out Func<IServiceProvider, CancellationToken, Task> Handler)
    {
        // ...
    }
}
```

## Parameters

- `Moment` ([HostLifecycleMoment](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecyclemoment.md))
- `Handler` (`Func<IServiceProvider, CancellationToken, Task>`)

<a id="equals"></a>
<a id="equals-object"></a>

## Equals(object?)

- Name: `Equals(object?)`
- Modifiers: `override`
- Returns: `bool`

Determines whether the specified object is equal to the current object.

```csharp
public record HostLifecycleRegistration
{
    public override bool Equals(
        object? obj)
    {
        // ...
    }
}
```

## Parameters

- `obj` (`object?`)
  The object to compare with the current object.

## Returns

`bool` -- `true` if the specified object is equal to the current object; otherwise, `false`.

<a id="equals-hostlifecycleregistration"></a>

## Equals(HostLifecycleRegistration?)

- Name: `Equals(HostLifecycleRegistration?)`
- Returns: `bool`

Indicates whether the current object is equal to another object of the same type.

```csharp
public record HostLifecycleRegistration
{
    public bool Equals(
        HostLifecycleRegistration? other)
    {
        // ...
    }
}
```

## Parameters

- `other` ([HostLifecycleRegistration?](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md))
  An object to compare with this object.

## Returns

`bool` -- `true` if the current object is equal to the `other` parameter; otherwise, `false`.

<a id="gethashcode"></a>

## GetHashCode

- Name: `GetHashCode`
- Modifiers: `override`
- Returns: `int`

Serves as the default hash function.

```csharp
public record HostLifecycleRegistration
{
    public override int GetHashCode()
    {
        // ...
    }
}
```

## Returns

`int` -- A hash code for the current object.

<a id="op-equality"></a>
<a id="op-equality-hostlifecycleregistration-hostlifecycleregistration"></a>

## op_Equality(HostLifecycleRegistration?, HostLifecycleRegistration?)

- Name: `op_Equality(HostLifecycleRegistration?, HostLifecycleRegistration?)`
- Modifiers: `static`
- Returns: `bool`

```csharp
public record HostLifecycleRegistration
{
    public static bool operator ==(
        HostLifecycleRegistration? left,
        HostLifecycleRegistration? right)
    {
        // ...
    }
}
```

## Parameters

- `left` ([HostLifecycleRegistration?](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md))
- `right` ([HostLifecycleRegistration?](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md))

<a id="op-inequality"></a>
<a id="op-inequality-hostlifecycleregistration-hostlifecycleregistration"></a>

## op_Inequality(HostLifecycleRegistration?, HostLifecycleRegistration?)

- Name: `op_Inequality(HostLifecycleRegistration?, HostLifecycleRegistration?)`
- Modifiers: `static`
- Returns: `bool`

```csharp
public record HostLifecycleRegistration
{
    public static bool operator !=(
        HostLifecycleRegistration? left,
        HostLifecycleRegistration? right)
    {
        // ...
    }
}
```

## Parameters

- `left` ([HostLifecycleRegistration?](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md))
- `right` ([HostLifecycleRegistration?](/Dovetail/pr-preview/pr-271/api/joinery.dovetail.hosting/hostlifecycleregistration.md))

<a id="tostring"></a>

## ToString

- Name: `ToString`
- Modifiers: `override`
- Returns: `string`

Returns a string that represents the current object.

```csharp
public record HostLifecycleRegistration
{
    public override string ToString()
    {
        // ...
    }
}
```

## Returns

`string` -- A string that represents the current object.
