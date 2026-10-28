# SerilogRuntimeConfiguration Methods

- Package: [Dovetail.Serilog](/Dovetail/api/dovetail.serilog.md)
- Type: [SerilogRuntimeConfiguration](/Dovetail/api/dovetail.serilog/serilogruntimeconfiguration.md)
- Kind: `Methods`
- Members: `7`

Hand-authored stand-in for the generator-emitted configuration class that `openspec/changes/dovetail-managed-configuration` will eventually produce for `appsettings.json` 's `Serilog` section (see design.md, Decision 4). Dogfoods the runtime half of the pipeline (task 8.3) ahead of the generator landing.

<a id="clone"></a>

## <Clone>$

- Name: `<Clone>$`
- Returns: [SerilogRuntimeConfiguration](/Dovetail/api/dovetail.serilog/serilogruntimeconfiguration.md)

```csharp
public record SerilogRuntimeConfiguration
{
    public SerilogRuntimeConfiguration <Clone>$()
    {
        // ...
    }
}
```

<a id="equals"></a>
<a id="equals-object"></a>

## Equals(object?)

- Name: `Equals(object?)`
- Modifiers: `override`
- Returns: `bool`

Determines whether the specified object is equal to the current object.

```csharp
public record SerilogRuntimeConfiguration
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

<a id="equals-serilogruntimeconfiguration"></a>

## Equals(SerilogRuntimeConfiguration?)

- Name: `Equals(SerilogRuntimeConfiguration?)`
- Returns: `bool`

Indicates whether the current object is equal to another object of the same type.

```csharp
public record SerilogRuntimeConfiguration
{
    public bool Equals(
        SerilogRuntimeConfiguration? other)
    {
        // ...
    }
}
```

## Parameters

- `other` ([SerilogRuntimeConfiguration?](/Dovetail/api/dovetail.serilog/serilogruntimeconfiguration.md))
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
public record SerilogRuntimeConfiguration
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
<a id="op-equality-serilogruntimeconfiguration-serilogruntimeconfiguration"></a>

## op_Equality(SerilogRuntimeConfiguration?, SerilogRuntimeConfiguration?)

- Name: `op_Equality(SerilogRuntimeConfiguration?, SerilogRuntimeConfiguration?)`
- Modifiers: `static`
- Returns: `bool`

```csharp
public record SerilogRuntimeConfiguration
{
    public static bool operator ==(
        SerilogRuntimeConfiguration? left,
        SerilogRuntimeConfiguration? right)
    {
        // ...
    }
}
```

## Parameters

- `left` ([SerilogRuntimeConfiguration?](/Dovetail/api/dovetail.serilog/serilogruntimeconfiguration.md))
- `right` ([SerilogRuntimeConfiguration?](/Dovetail/api/dovetail.serilog/serilogruntimeconfiguration.md))

<a id="op-inequality"></a>
<a id="op-inequality-serilogruntimeconfiguration-serilogruntimeconfiguration"></a>

## op_Inequality(SerilogRuntimeConfiguration?, SerilogRuntimeConfiguration?)

- Name: `op_Inequality(SerilogRuntimeConfiguration?, SerilogRuntimeConfiguration?)`
- Modifiers: `static`
- Returns: `bool`

```csharp
public record SerilogRuntimeConfiguration
{
    public static bool operator !=(
        SerilogRuntimeConfiguration? left,
        SerilogRuntimeConfiguration? right)
    {
        // ...
    }
}
```

## Parameters

- `left` ([SerilogRuntimeConfiguration?](/Dovetail/api/dovetail.serilog/serilogruntimeconfiguration.md))
- `right` ([SerilogRuntimeConfiguration?](/Dovetail/api/dovetail.serilog/serilogruntimeconfiguration.md))

<a id="tostring"></a>

## ToString

- Name: `ToString`
- Modifiers: `override`
- Returns: `string`

Returns a string that represents the current object.

```csharp
public record SerilogRuntimeConfiguration
{
    public override string ToString()
    {
        // ...
    }
}
```

## Returns

`string` -- A string that represents the current object.
