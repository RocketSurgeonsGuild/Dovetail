# DovetailJointMetadata Constructors

- Package: [Dovetail](/Dovetail/pr-preview/pr-237/api/dovetail.md)
- Type: [DovetailJointMetadata](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailjointmetadata.md)
- Kind: `Constructors`
- Members: `2`

Container for conventions

<a id="constructor"></a>
<a id="constructor-idovetailjoint-dovetailhosttype"></a>

## DovetailJointMetadata(IDovetailJoint, DovetailHostType)

- Name: `Constructor(IDovetailJoint, DovetailHostType)`

The default constructor

```csharp
public sealed class DovetailJointMetadata
{
    public DovetailJointMetadata(
        IDovetailJoint joint,
        DovetailHostType hostType)
    {
        // ...
    }
}
```

## Parameters

- `joint` ([IDovetailJoint](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailjoint.md))
  The joint this metadata describes.
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailhosttype.md))
  The host type the convention applies to.

<a id="constructor-idovetailjoint-dovetailhosttype-dovetailcategory"></a>

## DovetailJointMetadata(IDovetailJoint, DovetailHostType, DovetailCategory)

- Name: `Constructor(IDovetailJoint, DovetailHostType, DovetailCategory)`

The default constructor

```csharp
public sealed class DovetailJointMetadata
{
    public DovetailJointMetadata(
        IDovetailJoint joint,
        DovetailHostType hostType,
        DovetailCategory category)
    {
        // ...
    }
}
```

## Parameters

- `joint` ([IDovetailJoint](/Dovetail/pr-preview/pr-237/api/dovetail/idovetailjoint.md))
  The joint this metadata describes.
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailhosttype.md))
  The host type the convention applies to.
- `category` ([DovetailCategory](/Dovetail/pr-preview/pr-237/api/dovetail/dovetailcategory.md))
  The category the convention belongs to.
