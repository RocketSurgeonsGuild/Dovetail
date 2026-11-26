# DovetailJointMetadata Constructors

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-488/api/joinery.dovetail.md)
- Type: [DovetailJointMetadata](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailjointmetadata.md)
- Kind: `Constructors`
- Members: `2`

Container for conventions

<a id="constructor"></a>
<a id="constructor-idovetailjoint-dovetailhosttype"></a>

## DovetailJointMetadata(IDovetailJoint, DovetailHostType)

- Name: `Constructor(IDovetailJoint, DovetailHostType)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.a3aef988fbd13ff75690c9f41acdb3e58da6e1a4/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L11-L26)

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

- `joint` ([IDovetailJoint](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/idovetailjoint.md))
  The joint this metadata describes.
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailhosttype.md))
  The host type the convention applies to.

<a id="constructor-idovetailjoint-dovetailhosttype-dovetailcategory"></a>

## DovetailJointMetadata(IDovetailJoint, DovetailHostType, DovetailCategory)

- Name: `Constructor(IDovetailJoint, DovetailHostType, DovetailCategory)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-488-merge.Sha.a3aef988fbd13ff75690c9f41acdb3e58da6e1a4/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L11-L39)

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

- `joint` ([IDovetailJoint](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/idovetailjoint.md))
  The joint this metadata describes.
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailhosttype.md))
  The host type the convention applies to.
- `category` ([DovetailCategory](/Dovetail/pr-preview/pr-488/api/joinery.dovetail/dovetailcategory.md))
  The category the convention belongs to.
