# DovetailContextBuilder

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Version: `0.4.2-beta.3`
- Namespace: `Joinery.Dovetail`
- Target framework: `net10.0`

Builder that can be used to create a context.

## Definition

```csharp
namespace Joinery.Dovetail;

public sealed class DovetailContextBuilder
{
    // ...
}
```

## Properties

- [Categories](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `get` -- The categories of joints the context loads.

## Methods

- [AppendJoint(IEnumerable<IDovetailJointMetadata>)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#appendjoint-ienumerable-idovetailjointmetadata) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [AppendJoint(IEnumerable<IDovetailJoint>)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#appendjoint-ienumerable-idovetailjoint) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [AppendJoint](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#appendjoint) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, DovetailHostType, IEnumerable<DovetailCategory>)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#create-ienumerable-idovetailjointmetadata-idictionary-object-object-dovetailhosttype-ienumerable-dovetailcategory) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `static` -- Create a default context builder
- [Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, IEnumerable<DovetailCategory>)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#create-ienumerable-idovetailjointmetadata-idictionary-object-object-ienumerable-dovetailcategory) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) `static` -- Create a default context builder
- [CreateAsync(CancellationToken)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#createasync-cancellationtoken) : `ValueTask<IDovetailContext>` -- Create a new [IDovetailContext](/Dovetail/api/joinery.dovetail/idovetailcontext.md)
- [ExceptJoint(IEnumerable<Assembly>)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#exceptjoint-ienumerable-assembly) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds an exception to the scanner to exclude a specific convention
- [ExceptJoint(IEnumerable<Type>)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#exceptjoint-ienumerable-type) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds an exception to the scanner to exclude a specific convention
- [PrependJoint(IEnumerable<IDovetailJointMetadata>)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#prependjoint-ienumerable-idovetailjointmetadata) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [PrependJoint(IEnumerable<IDovetailJoint>)](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#prependjoint-ienumerable-idovetailjoint) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [PrependJoint](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder/methods.md#prependjoint) : [DovetailContextBuilder](/Dovetail/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
