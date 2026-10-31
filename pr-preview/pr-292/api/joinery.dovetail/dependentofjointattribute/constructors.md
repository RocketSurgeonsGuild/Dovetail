# DependentOfJointAttribute Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-292/api/joinery.dovetail.md)
- Type: [DependentOfJointAttribute](/Dovetail/pr-preview/pr-292/api/joinery.dovetail/dependentofjointattribute.md)
- Kind: `Constructors`
- Members: `1`

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-292/api/joinery.dovetail/idovetailjoint.md)

<a id="constructor"></a>
<a id="constructor-type"></a>

## DependentOfJointAttribute(Type)

- Name: `Constructor(Type)`

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/pr-preview/pr-292/api/joinery.dovetail/idovetailjoint.md)

```csharp
public sealed class DependentOfJointAttribute
{
    public DependentOfJointAttribute(
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

- `NotSupportedException` -- Type must inherit from [IDovetailJoint](/Dovetail/pr-preview/pr-292/api/joinery.dovetail/idovetailjoint.md)

## Remarks

The type to be used with the convention type
