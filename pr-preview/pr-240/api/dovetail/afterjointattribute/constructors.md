# AfterJointAttribute Constructors

- Package: [Dovetail](/Dovetail/pr-preview/pr-240/api/dovetail.md)
- Type: [AfterJointAttribute](/Dovetail/pr-preview/pr-240/api/dovetail/afterjointattribute.md)
- Kind: `Constructors`
- Members: `1`

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)

<a id="constructor"></a>
<a id="constructor-type"></a>

## AfterJointAttribute(Type)

- Name: `Constructor(Type)`

An attribute that ensures the convention runs after the given [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)

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

- `NotSupportedException` -- Type must inherit from [IDovetailJoint](/Dovetail/pr-preview/pr-240/api/dovetail/idovetailjoint.md)

## Remarks

The type to be used with the convention type
