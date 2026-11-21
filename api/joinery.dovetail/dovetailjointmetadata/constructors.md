# DovetailJointMetadata Constructors

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [DovetailJointMetadata](/Dovetail/api/joinery.dovetail/dovetailjointmetadata.md)
- Kind: `Constructors`
- Members: `2`

Container for conventions

<a id="constructor"></a>
<a id="constructor-idovetailjoint-dovetailhosttype"></a>

## DovetailJointMetadata(IDovetailJoint, DovetailHostType)

- Name: `Constructor(IDovetailJoint, DovetailHostType)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.354b445804a95dc447ae84afadceb7b383ae9fe4/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L11-L26)

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

- `joint` ([IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md))
  The joint this metadata describes.
- `hostType` ([DovetailHostType](/Dovetail/api/joinery.dovetail/dovetailhosttype.md))
  The host type the convention applies to.

<a id="constructor-idovetailjoint-dovetailhosttype-dovetailcategory"></a>

## DovetailJointMetadata(IDovetailJoint, DovetailHostType, DovetailCategory)

- Name: `Constructor(IDovetailJoint, DovetailHostType, DovetailCategory)`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.354b445804a95dc447ae84afadceb7b383ae9fe4/src/Joinery.Dovetail/Infrastructure/DovetailJointMetadata.cs#L11-L39)

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

- `joint` ([IDovetailJoint](/Dovetail/api/joinery.dovetail/idovetailjoint.md))
  The joint this metadata describes.
- `hostType` ([DovetailHostType](/Dovetail/api/joinery.dovetail/dovetailhosttype.md))
  The host type the convention applies to.
- `category` ([DovetailCategory](/Dovetail/api/joinery.dovetail/dovetailcategory.md))
  The category the convention belongs to.
