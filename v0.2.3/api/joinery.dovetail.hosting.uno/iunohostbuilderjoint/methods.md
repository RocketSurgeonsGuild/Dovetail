# IUnoHostBuilderJoint Methods

- Package: [Joinery.Dovetail.Hosting.Uno](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno.md)
- Type: [IUnoHostBuilderJoint](/Dovetail/v0.2.3/api/joinery.dovetail.hosting.uno/iunohostbuilderjoint.md)
- Kind: `Methods`
- Members: `1`

Joint interface for UnoHostBuilder. Implements the `Infrastructure.IDovetailJoint`

<a id="register"></a>
<a id="register-idovetailcontext-ihostbuilder"></a>

## Register(IDovetailContext, IHostBuilder)

- Name: `Register(IDovetailContext, IHostBuilder)`
- Modifiers: `abstract`

Register the UnoHostBuilder part.

```csharp
public interface IUnoHostBuilderJoint
{
    public abstract void Register(
        IDovetailContext context,
        IHostBuilder builder)
    {
        // ...
    }
}
```

## Parameters

- `context` (`IDovetailContext`)
- `builder` (`IHostBuilder`)
