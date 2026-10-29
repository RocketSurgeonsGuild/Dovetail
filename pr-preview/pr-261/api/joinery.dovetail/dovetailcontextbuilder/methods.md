# DovetailContextBuilder Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-261/api/joinery.dovetail.md)
- Type: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)
- Kind: `Methods`
- Members: `11`

Builder that can be used to create a context.

<a id="appendjoint-ienumerable-idovetailjointmetadata"></a>

## AppendJoint(IEnumerable<IDovetailJointMetadata>)

- Name: `AppendJoint(IEnumerable<IDovetailJointMetadata>)`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Adds a set of conventions to the scanner

```csharp
public sealed class DovetailContextBuilder
{
    public DovetailContextBuilder AppendJoint(
        params IEnumerable<IDovetailJointMetadata> conventions)
    {
        // ...
    }
}
```

## Parameters

- `conventions` ([IEnumerable<IDovetailJointMetadata>](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/idovetailjointmetadata.md))
  The conventions.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- IDovetailScanner.

<a id="appendjoint-ienumerable-idovetailjoint"></a>

## AppendJoint(IEnumerable<IDovetailJoint>)

- Name: `AppendJoint(IEnumerable<IDovetailJoint>)`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Adds a set of conventions to the scanner

```csharp
public sealed class DovetailContextBuilder
{
    public DovetailContextBuilder AppendJoint(
        params IEnumerable<IDovetailJoint> conventions)
    {
        // ...
    }
}
```

## Parameters

- `conventions` ([IEnumerable<IDovetailJoint>](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/idovetailjoint.md))
  The conventions.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- IDovetailScanner.

<a id="appendjoint"></a>

## AppendJoint

- Name: `AppendJoint`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Adds a set of conventions to the scanner

```csharp
public sealed class DovetailContextBuilder
{
    public DovetailContextBuilder AppendJoint<T>()
    {
        // ...
    }
}
```

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="create"></a>
<a id="create-ienumerable-idovetailjointmetadata-idictionary-object-object-dovetailhosttype-ienumerable-dovetailcategory"></a>

## Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, DovetailHostType, IEnumerable<DovetailCategory>)

- Name: `Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, DovetailHostType, IEnumerable<DovetailCategory>)`
- Modifiers: `static`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Create a default context builder

```csharp
public sealed class DovetailContextBuilder
{
    public static DovetailContextBuilder Create(
        IEnumerable<IDovetailJointMetadata> conventions,
        IDictionary<object, object> properties,
        DovetailHostType hostType,
        IEnumerable<DovetailCategory> categories)
    {
        // ...
    }
}
```

## Parameters

- `conventions` ([IEnumerable<IDovetailJointMetadata>](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/idovetailjointmetadata.md))
  The joints to seed the builder with.
- `properties` (`IDictionary<object, object>`)
  The initial set of properties to populate the context with.
- `hostType` ([DovetailHostType](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailhosttype.md))
  The host type the resulting context should be scoped to.
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcategory.md))
  The categories the resulting context should be scoped to.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- A new [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="create-ienumerable-idovetailjointmetadata-idictionary-object-object-ienumerable-dovetailcategory"></a>

## Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, IEnumerable<DovetailCategory>)

- Name: `Create(IEnumerable<IDovetailJointMetadata>, IDictionary<object, object>, IEnumerable<DovetailCategory>)`
- Modifiers: `static`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Create a default context builder

```csharp
public sealed class DovetailContextBuilder
{
    public static DovetailContextBuilder Create(
        IEnumerable<IDovetailJointMetadata> conventions,
        IDictionary<object, object> properties,
        IEnumerable<DovetailCategory> categories)
    {
        // ...
    }
}
```

## Parameters

- `conventions` ([IEnumerable<IDovetailJointMetadata>](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/idovetailjointmetadata.md))
  The joints to seed the builder with.
- `properties` (`IDictionary<object, object>`)
  The initial set of properties to populate the context with.
- `categories` ([IEnumerable<DovetailCategory>](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcategory.md))
  The categories the resulting context should be scoped to.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- A new [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="createasync"></a>
<a id="createasync-cancellationtoken"></a>

## CreateAsync(CancellationToken)

- Name: `CreateAsync(CancellationToken)`
- Returns: `ValueTask<IDovetailContext>`

Create a new [IDovetailContext](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/idovetailcontext.md)

```csharp
public sealed class DovetailContextBuilder
{
    public ValueTask<IDovetailContext> CreateAsync(
        CancellationToken cancellationToken = default(CancellationToken))
    {
        // ...
    }
}
```

## Parameters

- `cancellationToken` (`CancellationToken`) `optional`
  The cancellation token used during setup.

## Returns

`ValueTask<IDovetailContext>` -- The newly created and set up [IDovetailContext](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/idovetailcontext.md).

<a id="exceptjoint"></a>
<a id="exceptjoint-ienumerable-assembly"></a>

## ExceptJoint(IEnumerable<Assembly>)

- Name: `ExceptJoint(IEnumerable<Assembly>)`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Adds an exception to the scanner to exclude a specific convention

```csharp
public sealed class DovetailContextBuilder
{
    public DovetailContextBuilder ExceptJoint(
        params IEnumerable<Assembly> assemblies)
    {
        // ...
    }
}
```

## Parameters

- `assemblies` (`IEnumerable<Assembly>`)
  The additional types to exclude.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="exceptjoint-ienumerable-type"></a>

## ExceptJoint(IEnumerable<Type>)

- Name: `ExceptJoint(IEnumerable<Type>)`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Adds an exception to the scanner to exclude a specific convention

```csharp
public sealed class DovetailContextBuilder
{
    public DovetailContextBuilder ExceptJoint(
        params IEnumerable<Type> types)
    {
        // ...
    }
}
```

## Parameters

- `types` (`IEnumerable<Type>`)
  The additional types to exclude.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="prependjoint-ienumerable-idovetailjointmetadata"></a>

## PrependJoint(IEnumerable<IDovetailJointMetadata>)

- Name: `PrependJoint(IEnumerable<IDovetailJointMetadata>)`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Adds a set of conventions to the scanner

```csharp
public sealed class DovetailContextBuilder
{
    public DovetailContextBuilder PrependJoint(
        params IEnumerable<IDovetailJointMetadata> conventions)
    {
        // ...
    }
}
```

## Parameters

- `conventions` ([IEnumerable<IDovetailJointMetadata>](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/idovetailjointmetadata.md))
  The conventions.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="prependjoint-ienumerable-idovetailjoint"></a>

## PrependJoint(IEnumerable<IDovetailJoint>)

- Name: `PrependJoint(IEnumerable<IDovetailJoint>)`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Adds a set of conventions to the scanner

```csharp
public sealed class DovetailContextBuilder
{
    public DovetailContextBuilder PrependJoint(
        params IEnumerable<IDovetailJoint> conventions)
    {
        // ...
    }
}
```

## Parameters

- `conventions` ([IEnumerable<IDovetailJoint>](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/idovetailjoint.md))
  The conventions.

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md).

<a id="prependjoint"></a>

## PrependJoint

- Name: `PrependJoint`
- Returns: [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md)

Adds a set of conventions to the scanner

```csharp
public sealed class DovetailContextBuilder
{
    public DovetailContextBuilder PrependJoint<T>()
    {
        // ...
    }
}
```

## Returns

[DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md) -- [DovetailContextBuilder](/Dovetail/pr-preview/pr-261/api/joinery.dovetail/dovetailcontextbuilder.md).
