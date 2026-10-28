# DovetailDiagnosticMetadataJointDependency Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-242/api/joinery.dovetail.md)
- Type: [DovetailDiagnosticMetadataJointDependency](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md)
- Kind: `Methods`
- Members: `9`

Represents a metadata dependency for a Dovetail joint, including its name, host type, category, dependencies, and direction.

<a id="clone"></a>

## <Clone>$

- Name: `<Clone>$`
- Modifiers: `override`
- Returns: [DovetailDiagnosticMetadataJointDependency](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md)

```csharp
public record DovetailDiagnosticMetadataJointDependency
{
    public override DovetailDiagnosticMetadataJointDependency <Clone>$()
    {
        // ...
    }
}
```

<a id="deconstruct"></a>
<a id="deconstruct-string-string-immutablelist-string-immutablelist-string-dovetailhosttype-string-immutablelist-dovetaildiagnosticmetadatajointdependency-dependencydirection"></a>

## Deconstruct(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>, DependencyDirection)

- Name: `Deconstruct(string, string, ImmutableList<string>, ImmutableList<string>, DovetailHostType, string, ImmutableList<DovetailDiagnosticMetadataJointDependency>, DependencyDirection)`

```csharp
public record DovetailDiagnosticMetadataJointDependency
{
    public void Deconstruct(
        out string JointName,
        out string AssemblyName,
        out ImmutableList<string> ImplementsJointTypes,
        out ImmutableList<string> ConnectsToJointTypes,
        out DovetailHostType HostType,
        out string Category,
        out ImmutableList<DovetailDiagnosticMetadataJointDependency> Dependencies,
        out DependencyDirection Direction)
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
- `HostType` ([DovetailHostType](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dovetailhosttype.md))
- `Category` (`string`)
- `Dependencies` (`ImmutableList<DovetailDiagnosticMetadataJointDependency>`)
- `Direction` ([DependencyDirection](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dependencydirection.md))

<a id="equals"></a>
<a id="equals-object"></a>

## Equals(object?)

- Name: `Equals(object?)`
- Modifiers: `override`
- Returns: `bool`

Determines whether the specified object is equal to the current object.

```csharp
public record DovetailDiagnosticMetadataJointDependency
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
- Modifiers: `override`
- Returns: `bool`

Indicates whether the current object is equal to another object of the same type.

```csharp
public record DovetailDiagnosticMetadataJointDependency
{
    public sealed override bool Equals(
        DovetailDiagnosticMetadataJoint? other)
    {
        // ...
    }
}
```

## Parameters

- `other` ([DovetailDiagnosticMetadataJoint?](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md))
  An object to compare with this object.

## Returns

`bool` -- `true` if the current object is equal to the `other` parameter; otherwise, `false`.

<a id="equals-dovetaildiagnosticmetadatajointdependency"></a>

## Equals(DovetailDiagnosticMetadataJointDependency?)

- Name: `Equals(DovetailDiagnosticMetadataJointDependency?)`
- Returns: `bool`

Indicates whether the current object is equal to another object of the same type.

```csharp
public record DovetailDiagnosticMetadataJointDependency
{
    public bool Equals(
        DovetailDiagnosticMetadataJointDependency? other)
    {
        // ...
    }
}
```

## Parameters

- `other` ([DovetailDiagnosticMetadataJointDependency?](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md))
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
public record DovetailDiagnosticMetadataJointDependency
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
<a id="op-equality-dovetaildiagnosticmetadatajointdependency-dovetaildiagnosticmetadatajointdependency"></a>

## op_Equality(DovetailDiagnosticMetadataJointDependency?, DovetailDiagnosticMetadataJointDependency?)

- Name: `op_Equality(DovetailDiagnosticMetadataJointDependency?, DovetailDiagnosticMetadataJointDependency?)`
- Modifiers: `static`
- Returns: `bool`

```csharp
public record DovetailDiagnosticMetadataJointDependency
{
    public static bool operator ==(
        DovetailDiagnosticMetadataJointDependency? left,
        DovetailDiagnosticMetadataJointDependency? right)
    {
        // ...
    }
}
```

## Parameters

- `left` ([DovetailDiagnosticMetadataJointDependency?](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md))
- `right` ([DovetailDiagnosticMetadataJointDependency?](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md))

<a id="op-inequality"></a>
<a id="op-inequality-dovetaildiagnosticmetadatajointdependency-dovetaildiagnosticmetadatajointdependency"></a>

## op_Inequality(DovetailDiagnosticMetadataJointDependency?, DovetailDiagnosticMetadataJointDependency?)

- Name: `op_Inequality(DovetailDiagnosticMetadataJointDependency?, DovetailDiagnosticMetadataJointDependency?)`
- Modifiers: `static`
- Returns: `bool`

```csharp
public record DovetailDiagnosticMetadataJointDependency
{
    public static bool operator !=(
        DovetailDiagnosticMetadataJointDependency? left,
        DovetailDiagnosticMetadataJointDependency? right)
    {
        // ...
    }
}
```

## Parameters

- `left` ([DovetailDiagnosticMetadataJointDependency?](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md))
- `right` ([DovetailDiagnosticMetadataJointDependency?](/Dovetail/pr-preview/pr-242/api/joinery.dovetail/dovetaildiagnosticmetadatajointdependency.md))

<a id="tostring"></a>

## ToString

- Name: `ToString`
- Modifiers: `override`
- Returns: `string`

Returns a string that represents the current object.

```csharp
public record DovetailDiagnosticMetadataJointDependency
{
    public override string ToString()
    {
        // ...
    }
}
```

## Returns

`string` -- A string that represents the current object.
