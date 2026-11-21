# BeforeJointAttribute Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-480/api/joinery.dovetail.md)
- Type: [BeforeJointAttribute](/Dovetail/pr-preview/pr-480/api/joinery.dovetail/beforejointattribute.md)
- Kind: `Constructors`
- Members: `1`

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-480/api/joinery.dovetail/idovetailjoint.md)

<a id="constructor"></a>
<a id="constructor-type"></a>

## BeforeJointAttribute(Type)

- Name: `Constructor(Type)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-480-merge.Sha.bbd00571859e9fa257a29d29502064fe14badc48/src/Joinery.Dovetail/Attributes/BeforeJointAttribute.cs#L15-L21)

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-480/api/joinery.dovetail/idovetailjoint.md)

```csharp
public sealed class BeforeJointAttribute
{
    public BeforeJointAttribute(
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

- `NotSupportedException` -- Type must inherit from [IDovetailJoint](/Dovetail/pr-preview/pr-480/api/joinery.dovetail/idovetailjoint.md)

## Remarks

The type to be used with the convention type
