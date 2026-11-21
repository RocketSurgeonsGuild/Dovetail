# DependsOnJointAttribute Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-478/api/joinery.dovetail.md)
- Type: [DependsOnJointAttribute](/Dovetail/pr-preview/pr-478/api/joinery.dovetail/dependsonjointattribute.md)
- Kind: `Constructors`
- Members: `1`

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-478/api/joinery.dovetail/idovetailjoint.md)

<a id="constructor"></a>
<a id="constructor-type"></a>

## DependsOnJointAttribute(Type)

- Name: `Constructor(Type)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-478-merge.Sha.3cefccc51138c5f35edba1dddeb09aae10973315/src/Joinery.Dovetail/Attributes/DependsOnJointAttribute.cs#L15-L21)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-478/api/joinery.dovetail/idovetailjoint.md)

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

- `NotSupportedException` -- Type must inherit from [IDovetailJoint](/Dovetail/pr-preview/pr-478/api/joinery.dovetail/idovetailjoint.md)

## Remarks

The type to be used with the convention type
