# DependsOnJointAttribute Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-397/api/joinery.dovetail.md)
- Type: [DependsOnJointAttribute](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/dependsonjointattribute.md)
- Kind: `Constructors`
- Members: `1`

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/idovetailjoint.md)

<a id="constructor"></a>
<a id="constructor-type"></a>

## DependsOnJointAttribute(Type)

- Name: `Constructor(Type)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-397-merge.Sha.91944b3b1523dd3e82455c32e7a1561e8b3ad9a0/src/Joinery.Dovetail/Attributes/DependsOnJointAttribute.cs#L15-L21)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/idovetailjoint.md)

```csharp
public sealed class DependsOnJointAttribute
{
    public DependsOnJointAttribute(
        Type type)
    {
        // ...
    }
}
```

## Parameters

- `type` (`Type`)
  The type.

## Exceptions

- `NotSupportedException` -- Type must inherit from [IDovetailJoint](/Dovetail/pr-preview/pr-397/api/joinery.dovetail/idovetailjoint.md)

## Remarks

The type to be used with the convention type
