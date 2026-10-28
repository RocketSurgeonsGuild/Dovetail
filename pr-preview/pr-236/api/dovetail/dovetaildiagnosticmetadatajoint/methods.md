# DovetailDiagnosticMetadataJoint Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-236/api/dovetail.md)
- Type: [DovetailDiagnosticMetadataJoint](/Dovetail/pr-preview/pr-236/api/dovetail/dovetaildiagnosticmetadatajoint.md)
- Kind: `Methods`
- Members: `8`

Represents a metadata item for a Dovetail joint, including its name, host type, category, and dependencies.

<a id="clone"></a>

## <Clone>$

- Name: `<Clone>$`
- Modifiers: `virtual`
- Returns: [DovetailDiagnosticMetadataJoint](/Dovetail/pr-preview/pr-236/api/dovetail/dovetaildiagnosticmetadatajoint.md)

```csharp
public record DovetailDiagnosticMetadataJoint
{
    public virtual DovetailDiagnosticMetadataJoint <Clone>$()
    {
        // ...
    }
}
```

<a id="deconstruct"></a>
<a id="deconstruct-string-string-immutablelist-string-immutablelist-string-dovetailhosttype-string-immutablelist-dovetaildiagnosticmetadatajointdependency"></a>

## Deconstruct(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>)

- Name: `Deconstruct(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>)`

```csharp
public record DovetailDiagnosticMetadataJoint
{
    public void Deconstruct(
        out string JointName,
        out string AssemblyName,
        out ImmutableList<string> ImplementsJointTypes,
        out ImmutableList<string> ConnectsToJointTypes,
        out DovetailHostType HostType,
        out string Category,
        out ImmutableList<DovetailDiagnosticMetadataJointDependency> Dependencies)
    {
        // ...
    }
}
```

## Parameters

- `JointName` (`string`)
- `AssemblyName` (`string`)
- `ImplementsJointTypes` (`ImmutableList<string>`)
- `ConnectsToJointTypes` (`ImmutableList<string>`)
- `HostType` ([DovetailHostType](/Dovetail/pr-preview/pr-236/api/dovetail/dovetailhosttype.md))
- `Category` (`string`)
- `Dependencies` (`ImmutableList<DovetailDiagnosticMetadataJointDependency>`)

<a id="equals"></a>
<a id="equals-object"></a>

## Equals(object?)

- Name: `Equals(object?)`
- Modifiers: `override`
- Returns: `bool`

Determines whether the specified object is equal to the current object.

```csharp
public record DovetailDiagnosticMetadataJoint
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

<a id="equals-dovetaildiagnosticmetadatajoint"></a>

## Equals(DovetailDiagnosticMetadataJoint?)

- Name: `Equals(DovetailDiagnosticMetadataJoint?)`
- Modifiers: `virtual`
- Returns: `bool`

Indicates whether the current object is equal to another object of the same type.

```csharp
public record DovetailDiagnosticMetadataJoint
{
    public virtual bool Equals(
        DovetailDiagnosticMetadataJoint? other)
    {
        // ...
    }
}
```

## Parameters

- `other` ([DovetailDiagnosticMetadataJoint?](/Dovetail/pr-preview/pr-236/api/dovetail/dovetaildiagnosticmetadatajoint.md))
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
public record DovetailDiagnosticMetadataJoint
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
<a id="op-equality-dovetaildiagnosticmetadatajoint-dovetaildiagnosticmetadatajoint"></a>

## op_Equality(DovetailDiagnosticMetadataJoint?, DovetailDiagnosticMetadataJoint?)

- Name: `op_Equality(DovetailDiagnosticMetadataJoint?, DovetailDiagnosticMetadataJoint?)`
- Modifiers: `static`
- Returns: `bool`

```csharp
public record DovetailDiagnosticMetadataJoint
{
    public static bool operator ==(
        DovetailDiagnosticMetadataJoint? left,
        DovetailDiagnosticMetadataJoint? right)
    {
        // ...
    }
}
```

## Parameters

- `left` ([DovetailDiagnosticMetadataJoint?](/Dovetail/pr-preview/pr-236/api/dovetail/dovetaildiagnosticmetadatajoint.md))
- `right` ([DovetailDiagnosticMetadataJoint?](/Dovetail/pr-preview/pr-236/api/dovetail/dovetaildiagnosticmetadatajoint.md))

<a id="op-inequality"></a>
<a id="op-inequality-dovetaildiagnosticmetadatajoint-dovetaildiagnosticmetadatajoint"></a>

## op_Inequality(DovetailDiagnosticMetadataJoint?, DovetailDiagnosticMetadataJoint?)

- Name: `op_Inequality(DovetailDiagnosticMetadataJoint?, DovetailDiagnosticMetadataJoint?)`
- Modifiers: `static`
- Returns: `bool`

```csharp
public record DovetailDiagnosticMetadataJoint
{
    public static bool operator !=(
        DovetailDiagnosticMetadataJoint? left,
        DovetailDiagnosticMetadataJoint? right)
    {
        // ...
    }
}
```

## Parameters

- `left` ([DovetailDiagnosticMetadataJoint?](/Dovetail/pr-preview/pr-236/api/dovetail/dovetaildiagnosticmetadatajoint.md))
- `right` ([DovetailDiagnosticMetadataJoint?](/Dovetail/pr-preview/pr-236/api/dovetail/dovetaildiagnosticmetadatajoint.md))

<a id="tostring"></a>

## ToString

- Name: `ToString`
- Modifiers: `override`
- Returns: `string`

Returns a string that represents the current object.

```csharp
public record DovetailDiagnosticMetadataJoint
{
    public override string ToString()
    {
        // ...
    }
}
```

## Returns

`string` -- A string that represents the current object.
