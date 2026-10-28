# DovetailContextBuilder

- Kind: `class`
- Package: [Dovetail](/Dovetail/api/dovetail.md)
- Version: `0.2.2-beta.2`
- Namespace: `Dovetail`
- Target framework: `net10.0`

Builder that can be used to create a context.

## Definition

```csharp
namespace Dovetail;

public sealed class DovetailContextBuilder
{
    // ...
}
```

## Properties

- [Categories](/Dovetail/api/dovetail/dovetailcontextbuilder/properties.md#categories) : `ImmutableHashSet<DovetailCategory>` `get` -- The categories of the convention context

## Methods

- [AppendJoint(IEnumerable<IDovetailJointMetadata>)](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#appendjoint-ienumerable-idovetailjointmetadata) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [AppendJoint(IEnumerable<IDovetailJoint>)](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#appendjoint-ienumerable-idovetailjoint) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [AppendJoint](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#appendjoint) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, DovetailHostType, IEnumerable<DovetailCategory>)](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#create-ienumerable-idovetailjointmetadata-idictionary-object-object-dovetailhosttype-ienumerable-dovetailcategory) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) `static` -- Create a default context builder
- [Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, IEnumerable<DovetailCategory>)](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#create-ienumerable-idovetailjointmetadata-idictionary-object-object-ienumerable-dovetailcategory) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) `static` -- Create a default context builder
- [CreateAsync(CancellationToken)](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#createasync-cancellationtoken) : `ValueTask<IDovetailContext>` -- Create a new [IDovetailContext](/Dovetail/api/dovetail/idovetailcontext.md)
- [ExceptJoint(IEnumerable<Assembly>)](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#exceptjoint-ienumerable-assembly) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) -- Adds an exception to the scanner to exclude a specific convention
- [ExceptJoint(IEnumerable<Type>)](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#exceptjoint-ienumerable-type) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) -- Adds an exception to the scanner to exclude a specific convention
- [PrependJoint(IEnumerable<IDovetailJointMetadata>)](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#prependjoint-ienumerable-idovetailjointmetadata) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [PrependJoint(IEnumerable<IDovetailJoint>)](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#prependjoint-ienumerable-idovetailjoint) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
- [PrependJoint](/Dovetail/api/dovetail/dovetailcontextbuilder/methods.md#prependjoint) : [DovetailContextBuilder](/Dovetail/api/dovetail/dovetailcontextbuilder.md) -- Adds a set of conventions to the scanner
