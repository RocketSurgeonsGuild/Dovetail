# IResourceBuilderJoint Methods

- Package: [Dovetail.OpenTelemetry](/Dovetail/pr-preview/pr-237/api/dovetail.opentelemetry.md)
- Type: [IResourceBuilderJoint](/Dovetail/pr-preview/pr-237/api/dovetail.opentelemetry/iresourcebuilderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for `Resources.ResourceBuilder`. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-resourcebuilder"></a>

## Register(IDovetailContext, ResourceBuilder)

- Name: `Register(IDovetailContext, ResourceBuilder)`
- Modifiers: `abstract`

Register the `Resources.ResourceBuilder` part.

```csharp
public interface IResourceBuilderJoint
{
    public abstract void Register(
        IDovetailContext context,
        ResourceBuilder resourceBuilder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `resourceBuilder` (`ResourceBuilder`)
