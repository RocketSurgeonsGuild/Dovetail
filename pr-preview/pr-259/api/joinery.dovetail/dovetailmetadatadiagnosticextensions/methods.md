# DovetailMetadataDiagnosticExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-259/api/joinery.dovetail.md)
- Type: [DovetailMetadataDiagnosticExtensions](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/dovetailmetadatadiagnosticextensions.md)
- Kind: `Methods`
- Members: `5`

Extension methods for [IDovetailJointMetadata](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/idovetailjointmetadata.md) to provide additional diagnostic information.

<a id="get-assembly"></a>
<a id="get-assembly-idovetailjointmetadata"></a>

## get_Assembly(IDovetailJointMetadata)

- Name: `get_Assembly(IDovetailJointMetadata)`
- Modifiers: `static`
- Returns: `Assembly`

```csharp
public static class DovetailMetadataDiagnosticExtensions
{
    public static Assembly get_Assembly(
        IDovetailJointMetadata metadata)
    {
        // ...
    }
}
```

## Parameters

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/idovetailjointmetadata.md))

<a id="get-jointname"></a>
<a id="get-jointname-idovetailjointmetadata"></a>

## get_JointName(IDovetailJointMetadata)

- Name: `get_JointName(IDovetailJointMetadata)`
- Modifiers: `static`
- Returns: `string`

```csharp
public static class DovetailMetadataDiagnosticExtensions
{
    public static string get_JointName(
        IDovetailJointMetadata metadata)
    {
        // ...
    }
}
```

## Parameters

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/idovetailjointmetadata.md))

<a id="getconnectedto"></a>
<a id="getconnectedto-idovetailjointmetadata"></a>

## GetConnectedTo(IDovetailJointMetadata)

- Name: `GetConnectedTo(IDovetailJointMetadata)`
- Modifiers: `extension`
- Returns: `IEnumerable<Type>`

```csharp
public static class DovetailMetadataDiagnosticExtensions
{
    public static IEnumerable<Type> GetConnectedTo(
        this IDovetailJointMetadata metadata)
    {
        // ...
    }
}
```

## Parameters

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/idovetailjointmetadata.md))

<a id="getconnectedtostepnames"></a>
<a id="getconnectedtostepnames-idovetailjointmetadata"></a>

## GetConnectedToStepNames(IDovetailJointMetadata)

- Name: `GetConnectedToStepNames(IDovetailJointMetadata)`
- Modifiers: `extension`
- Returns: `IEnumerable<string>`

```csharp
public static class DovetailMetadataDiagnosticExtensions
{
    public static IEnumerable<string> GetConnectedToStepNames(
        this IDovetailJointMetadata metadata)
    {
        // ...
    }
}
```

## Parameters

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/idovetailjointmetadata.md))

<a id="getstepnames"></a>
<a id="getstepnames-idovetailjointmetadata"></a>

## GetStepNames(IDovetailJointMetadata)

- Name: `GetStepNames(IDovetailJointMetadata)`
- Modifiers: `extension`
- Returns: `IEnumerable<string>`

```csharp
public static class DovetailMetadataDiagnosticExtensions
{
    public static IEnumerable<string> GetStepNames(
        this IDovetailJointMetadata metadata)
    {
        // ...
    }
}
```

## Parameters

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-259/api/joinery.dovetail/idovetailjointmetadata.md))
