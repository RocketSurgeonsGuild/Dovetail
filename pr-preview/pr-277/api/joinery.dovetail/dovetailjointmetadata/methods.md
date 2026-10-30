# DovetailJointMetadata Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-277/api/joinery.dovetail.md)
- Type: [DovetailJointMetadata](/Dovetail/pr-preview/pr-277/api/joinery.dovetail/dovetailjointmetadata.md)
- Kind: `Methods`
- Members: `2`

Container for conventions

<a id="gethashcode"></a>

## GetHashCode

- Name: `GetHashCode`
- Modifiers: `override`
- Returns: `int`

Serves as the default hash function.

```csharp
public sealed class DovetailJointMetadata
{
    public override int GetHashCode()
    {
        // ...
    }
}
```

## Returns

`int` -- A hash code for the current object.

<a id="withdependency"></a>
<a id="withdependency-dependencydirection-type"></a>

## WithDependency(DependencyDirection, Type)

- Name: `WithDependency(DependencyDirection, Type)`
- Returns: [DovetailJointMetadata](/Dovetail/pr-preview/pr-277/api/joinery.dovetail/dovetailjointmetadata.md)

Adds a new dependency to the list

```csharp
public sealed class DovetailJointMetadata
{
    public DovetailJointMetadata WithDependency(
        DependencyDirection direction,
        Type type)
    {
        // ...
    }
}
```

## Parameters

- `direction` ([DependencyDirection](/Dovetail/pr-preview/pr-277/api/joinery.dovetail/dependencydirection.md))
  The direction of the dependency.
- `type` (`Type`)
  The [IDovetailJoint](/Dovetail/pr-preview/pr-277/api/joinery.dovetail/idovetailjoint.md) type that this dependency relates to.

## Returns

[DovetailJointMetadata](/Dovetail/pr-preview/pr-277/api/joinery.dovetail/dovetailjointmetadata.md) -- This [DovetailJointMetadata](/Dovetail/pr-preview/pr-277/api/joinery.dovetail/dovetailjointmetadata.md), for chaining.
