# AfterJointAttribute Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-470/api/joinery.dovetail.md)
- Type: [AfterJointAttribute](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/afterjointattribute.md)
- Kind: `Constructors`
- Members: `1`

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/idovetailjoint.md)

<a id="constructor"></a>
<a id="constructor-type"></a>

## AfterJointAttribute(Type)

- Name: `Constructor(Type)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-470-merge.Sha.1f45da18d9f78a77f1393ca7c4c966285183843c/src/Joinery.Dovetail/Attributes/AfterJointAttribute.cs#L15-L21)

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/idovetailjoint.md)

```csharp
public sealed class AfterJointAttribute
{
    public AfterJointAttribute(
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

- `NotSupportedException` -- Type must inherit from [IDovetailJoint](/Dovetail/pr-preview/pr-470/api/joinery.dovetail/idovetailjoint.md)

## Remarks

The type to be used with the convention type
