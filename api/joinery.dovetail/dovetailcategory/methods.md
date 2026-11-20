# DovetailCategory Methods

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [DovetailCategory](/Dovetail/api/joinery.dovetail/dovetailcategory.md)
- Kind: `Methods`
- Members: `5`

The category of a given convention

<a id="equals"></a>
<a id="equals-object"></a>

## Equals(object?)

- Name: `Equals(object?)`
- Modifiers: `override`
- Returns: `bool`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail/DovetailCategory.cs#L17)

Determines whether the specified object is equal to the current object.

```csharp
public sealed class DovetailCategory
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

<a id="gethashcode"></a>

## GetHashCode

- Name: `GetHashCode`
- Modifiers: `override`
- Returns: `int`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail/DovetailCategory.cs#L20)

Serves as the default hash function.

```csharp
public sealed class DovetailCategory
{
    public override int GetHashCode()
    {
        // ...
    }
}
```

## Returns

`int` -- A hash code for the current object.

<a id="op-implicit"></a>
<a id="op-implicit-dovetailcategory"></a>

## op_Implicit(DovetailCategory)

- Name: `op_Implicit(DovetailCategory)`
- Modifiers: `static`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail/DovetailCategory.cs)

Implicitly convert to a string

```csharp
public sealed class DovetailCategory
{
    public static implicit operator string(
        DovetailCategory category)
    {
        // ...
    }
}
```

## Parameters

- `category` ([DovetailCategory](/Dovetail/api/joinery.dovetail/dovetailcategory.md))
  The category to convert.

## Returns

`string` -- The underlying string value of the category.

<a id="op-implicit-string"></a>

## op_Implicit(string)

- Name: `op_Implicit(string)`
- Modifiers: `static`
- Returns: [DovetailCategory](/Dovetail/api/joinery.dovetail/dovetailcategory.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail/DovetailCategory.cs)

Implicitly convert from a string

```csharp
public sealed class DovetailCategory
{
    public static implicit operator DovetailCategory(
        string category)
    {
        // ...
    }
}
```

## Parameters

- `category` (`string`)
  The string value to wrap.

## Returns

[DovetailCategory](/Dovetail/api/joinery.dovetail/dovetailcategory.md) -- A new [DovetailCategory](/Dovetail/api/joinery.dovetail/dovetailcategory.md) wrapping the given value.

<a id="tostring"></a>

## ToString

- Name: `ToString`
- Modifiers: `override`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail/DovetailCategory.cs#L23)

Returns a string that represents the current object.

```csharp
public sealed class DovetailCategory
{
    public override string ToString()
    {
        // ...
    }
}
```

## Returns

`string` -- A string that represents the current object.
