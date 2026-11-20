# DependsOnJointAttribute Constructors

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [DependsOnJointAttribute](/Dovetail/api/joinery.dovetail/dependsonjointattribute.md)
- Kind: `Constructors`
- Members: `1`

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

<a id="constructor"></a>
<a id="constructor-type"></a>

## DependsOnJointAttribute(Type)

- Name: `Constructor(Type)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.0c7afbae1c7765837a6f03dc78ee974990bf9b3c/src/Joinery.Dovetail/Attributes/DependsOnJointAttribute.cs#L15-L21)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

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

- `NotSupportedException` -- Type must inherit from [IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md)

## Remarks

The type to be used with the convention type
