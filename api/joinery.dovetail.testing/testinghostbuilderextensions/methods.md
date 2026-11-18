# TestingHostBuilderExtensions Methods

- Package: [Joinery.Dovetail.Testing](/Dovetail/api/joinery.dovetail.testing.md)
- Type: [TestingHostBuilderExtensions](/Dovetail/api/joinery.dovetail.testing/testinghostbuilderextensions.md)
- Kind: `Methods`
- Members: `2`

Builder-method extensions for applying Testing parts to a `Dovetail.DovetailContextBuilder`.

<a id="configuretesting"></a>
<a id="configuretesting-dovetailcontextbuilder-testingjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureTesting(DovetailContextBuilder, TestingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureTesting(DovetailContextBuilder, TestingJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.d6b0f8d96f8417678d91819460b242bed026e72e/src/Joinery.Dovetail.Testing/obj/Release/net10.0/DovetailJoint.Testing.g.cs)

```csharp
public static class TestingHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureTesting(
        this DovetailContextBuilder container,
        TestingJointDelegate @delegate,
        int priority = 0,
        DovetailCategory? category = null,
        DovetailHostType hostType = DovetailHostType.Undefined,
        string? expression = null)
    {
        // ...
    }
}
```

## Parameters

- `container` (`DovetailContextBuilder`)
- `delegate` ([TestingJointDelegate](/Dovetail/api/joinery.dovetail.testing/testingjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`

<a id="configuretesting-dovetailcontextbuilder-testingasyncjointdelegate-int-dovetailcategory-dovetailhosttype-string"></a>

## ConfigureTesting(DovetailContextBuilder, TestingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)

- Name: `ConfigureTesting(DovetailContextBuilder, TestingAsyncJointDelegate, int, DovetailCategory?, DovetailHostType, string?)`
- Modifiers: `extension`
- Returns: `DovetailContextBuilder`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.d6b0f8d96f8417678d91819460b242bed026e72e/src/Joinery.Dovetail.Testing/obj/Release/net10.0/DovetailJoint.Testing.g.cs)

```csharp
public static class TestingHostBuilderExtensions
{
    public static DovetailContextBuilder ConfigureTesting(
        this DovetailContextBuilder container,
        TestingAsyncJointDelegate @delegate,
        int priority = 0,
        DovetailCategory? category = null,
        DovetailHostType hostType = DovetailHostType.Undefined,
        string? expression = null)
    {
        // ...
    }
}
```

## Parameters

- `container` (`DovetailContextBuilder`)
- `delegate` ([TestingAsyncJointDelegate](/Dovetail/api/joinery.dovetail.testing/testingasyncjointdelegate.md))
- `priority` (`int`) `optional`
- `category` (`DovetailCategory?`) `optional`
- `hostType` (`DovetailHostType`) `optional`
- `expression` (`string?`) `optional`
