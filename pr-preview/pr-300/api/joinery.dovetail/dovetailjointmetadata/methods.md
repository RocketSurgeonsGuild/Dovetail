# DovetailJointMetadata Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-300/api/joinery.dovetail.md)
- Type: [DovetailJointMetadata](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailjointmetadata.md)
- Kind: `Methods`
- Members: `3`

Container for conventions

<a id="fromfactory"></a>
<a id="fromfactory-func-iserviceprovider-idovetailjoint-dovetailhosttype-dovetailcategory"></a>

## FromFactory(Func<IServiceProvider, IDovetailJoint>, DovetailHostType, DovetailCategory)

- Name: `FromFactory(Func<IServiceProvider, IDovetailJoint>, DovetailHostType, DovetailCategory)`
- Modifiers: `static`
- Returns: [DovetailJointMetadata](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailjointmetadata.md)

Describes a joint that is created the first time [DovetailJointMetadata.Joint](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailjointmetadata/properties.md#joint) is read, rather than up front.

```csharp
public sealed class DovetailJointMetadata
{
    public static DovetailJointMetadata FromFactory(
        Func<IServiceProvider, IDovetailJoint> factory,
        DovetailHostType hostType,
        DovetailCategory category)
    {
        // ...
    }
}
```

## Parameters

- `factory` (`Func<IServiceProvider, IDovetailJoint>`)
  Creates the joint from the properties of the context builder.
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailhosttype.md))
  The host type the convention applies to.
- `category` ([DovetailCategory](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcategory.md))
  The category the convention belongs to.

## Returns

[DovetailJointMetadata](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailjointmetadata.md) -- The metadata of the joint `factory` creates.

## Exceptions

- `ArgumentNullException` -- `factory` is `null`.

## Remarks

This is how the code generated for an exported joint with constructor parameters creates it: the factory resolves each parameter from the properties of the [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) the metadata was given to, so a value set on the builder after the metadata was created is still seen. A parameter with a default value falls back to it when the properties hold nothing of that type, and a parameter without one makes the factory throw `InvalidOperationException`. Read before a builder has the metadata, [DovetailJointMetadata.Joint](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailjointmetadata/properties.md#joint) resolves against empty properties. The joint is created once.

<a id="gethashcode"></a>

## GetHashCode

- Name: `GetHashCode`
- Modifiers: `override`
- Returns: `int`

Serves as the default hash function.

```csharp
public sealed class DovetailJointMetadata
{
    public override int GetHashCode()
    {
        // ...
    }
}
```

## Returns

`int` -- A hash code for the current object.

<a id="withdependency"></a>
<a id="withdependency-dependencydirection-type"></a>

## WithDependency(DependencyDirection, Type)

- Name: `WithDependency(DependencyDirection, Type)`
- Returns: [DovetailJointMetadata](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailjointmetadata.md)

Adds a new dependency to the list

```csharp
public sealed class DovetailJointMetadata
{
    public DovetailJointMetadata WithDependency(
        DependencyDirection direction,
        Type type)
    {
        // ...
    }
}
```

## Parameters

- `direction` ([DependencyDirection](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dependencydirection.md))
  The direction of the dependency.
- `type` (`Type`)
  The [IDovetailJoint](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/idovetailjoint.md) type that this dependency relates to.

## Returns

[DovetailJointMetadata](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailjointmetadata.md) -- This [DovetailJointMetadata](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailjointmetadata.md), for chaining.
