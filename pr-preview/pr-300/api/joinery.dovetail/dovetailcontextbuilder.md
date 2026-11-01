# DovetailContextBuilder

- Kind: `class`
- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-300/api/joinery.dovetail.md)
- Version: `0.4.2-pr.9`
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

- [Categories](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `get` -- The categories of joints the context loads.

## Methods

- [AppendJoint(IEnumerable<IDovetailJointMetadata>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#appendjoint-ienumerable-idovetailjointmetadata) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [AppendJoint(IEnumerable<IDovetailJoint>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#appendjoint-ienumerable-idovetailjoint) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [AppendJoint](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#appendjoint) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, DovetailHostType, IEnumerable<DovetailCategory>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#create-ienumerable-idovetailjointmetadata-idictionary-object-object-dovetailhosttype-ienumerable-dovetailcategory) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) `static` -- Create a default context builder
- [Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, IEnumerable<DovetailCategory>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#create-ienumerable-idovetailjointmetadata-idictionary-object-object-ienumerable-dovetailcategory) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) `static` -- Create a default context builder
- [CreateAsync(CancellationToken)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#createasync-cancellationtoken) : `ValueTask<IDovetailContext>` -- Create a new [IDovetailContext](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/idovetailcontext.md)
- [ExceptJoint(IEnumerable<Assembly>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#exceptjoint-ienumerable-assembly) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds an exception to the scanner to exclude a specific convention
- [ExceptJoint(IEnumerable<Type>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#exceptjoint-ienumerable-type) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds an exception to the scanner to exclude a specific convention
- [PrependJoint(IEnumerable<IDovetailJointMetadata>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#prependjoint-ienumerable-idovetailjointmetadata) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [PrependJoint(IEnumerable<IDovetailJoint>)](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#prependjoint-ienumerable-idovetailjoint) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [PrependJoint](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder/methods.md#prependjoint) : [DovetailContextBuilder](/Dovetail/pr-preview/pr-300/api/joinery.dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
