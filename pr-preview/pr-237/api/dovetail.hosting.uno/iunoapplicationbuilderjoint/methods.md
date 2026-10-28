# IUnoApplicationBuilderJoint Methods

- Package: [Dovetail.Hosting.Uno](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.uno.md)
- Type: [IUnoApplicationBuilderJoint](/Dovetail/pr-preview/pr-237/api/dovetail.hosting.uno/iunoapplicationbuilderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for UnoApplicationBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-iapplicationbuilder"></a>

## Register(IDovetailContext, IApplicationBuilder)

- Name: `Register(IDovetailContext, IApplicationBuilder)`
- Modifiers: `abstract`

Register the UnoApplicationBuilder part.

```csharp
public interface IUnoApplicationBuilderJoint
{
    public abstract void Register(
        IDovetailContext context,
        IApplicationBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IApplicationBuilder`)
