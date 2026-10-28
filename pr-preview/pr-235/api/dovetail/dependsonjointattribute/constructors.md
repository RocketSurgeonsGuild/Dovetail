# DependsOnJointAttribute Constructors

- Package: [Dovetail](/Dovetail/pr-preview/pr-235/api/dovetail.md)
- Type: [DependsOnJointAttribute](/Dovetail/pr-preview/pr-235/api/dovetail/dependsonjointattribute.md)
- Kind: `Constructors`
- Members: `1`

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md)

<a id="constructor"></a>
<a id="constructor-type"></a>

## DependsOnJointAttribute(Type)

- Name: `Constructor(Type)`

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md)

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

- `NotSupportedException` -- Type must inherit from [IDovetailJoint](/Dovetail/pr-preview/pr-235/api/dovetail/idovetailjoint.md)

## Remarks

The type to be used with the convention type
