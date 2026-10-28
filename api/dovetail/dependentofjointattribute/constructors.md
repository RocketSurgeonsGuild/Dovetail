# DependentOfJointAttribute Constructors

- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Type: [DependentOfJointAttribute](/Dovetail/api/dovetail/dependentofjointattribute.md)
- Kind: `Constructors`
- Members: `1`

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

<a id="constructor"></a>
<a id="constructor-type"></a>

## DependentOfJointAttribute(Type)

- Name: `Constructor(Type)`

An attribute that ensures the convention runs before the given [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

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

- `NotSupportedException` -- Type must inherit from [IDovetailJoint](/Dovetail/api/dovetail/idovetailjoint.md)

## Remarks

The type to be used with the convention type
