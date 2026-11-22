# DovetailMetadataDiagnosticExtensions Methods

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-484/api/joinery.dovetail.md)
- Type: [DovetailMetadataDiagnosticExtensions](/Dovetail/pr-preview/pr-484/api/joinery.dovetail/dovetailmetadatadiagnosticextensions.md)
- Kind: `Methods`
- Members: `5`

Extension methods for [IDovetailJointMetadata](/Dovetail/pr-preview/pr-484/api/joinery.dovetail/idovetailjointmetadata.md) to provide additional diagnostic information.

<a id="get-assembly"></a>
<a id="get-assembly-idovetailjointmetadata"></a>

## get_Assembly(IDovetailJointMetadata)

- Name: `get_Assembly(IDovetailJointMetadata)`
- Modifiers: `static`
- Returns: `Assembly`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-484-merge.Sha.0e7ab3e6cc9922c1b2ae61bec2dfad8c9fdd39a2/src/Joinery.Dovetail/Diagnostics/DovetailMetadataDiagnosticExtensions.cs#L17)

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

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-484/api/joinery.dovetail/idovetailjointmetadata.md))

<a id="get-jointname"></a>
<a id="get-jointname-idovetailjointmetadata"></a>

## get_JointName(IDovetailJointMetadata)

- Name: `get_JointName(IDovetailJointMetadata)`
- Modifiers: `static`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-484-merge.Sha.0e7ab3e6cc9922c1b2ae61bec2dfad8c9fdd39a2/src/Joinery.Dovetail/Diagnostics/DovetailMetadataDiagnosticExtensions.cs#L22)

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

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-484/api/joinery.dovetail/idovetailjointmetadata.md))

<a id="getconnectedto"></a>
<a id="getconnectedto-idovetailjointmetadata"></a>

## GetConnectedTo(IDovetailJointMetadata)

- Name: `GetConnectedTo(IDovetailJointMetadata)`
- Modifiers: `extension`
- Returns: `IEnumerable<Type>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-484-merge.Sha.0e7ab3e6cc9922c1b2ae61bec2dfad8c9fdd39a2/src/Joinery.Dovetail/Diagnostics/DovetailMetadataDiagnosticExtensions.cs#L38-L41)

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

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-484/api/joinery.dovetail/idovetailjointmetadata.md))

<a id="getconnectedtostepnames"></a>
<a id="getconnectedtostepnames-idovetailjointmetadata"></a>

## GetConnectedToStepNames(IDovetailJointMetadata)

- Name: `GetConnectedToStepNames(IDovetailJointMetadata)`
- Modifiers: `extension`
- Returns: `IEnumerable<string>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-484-merge.Sha.0e7ab3e6cc9922c1b2ae61bec2dfad8c9fdd39a2/src/Joinery.Dovetail/Diagnostics/DovetailMetadataDiagnosticExtensions.cs#L47-L49)

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

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-484/api/joinery.dovetail/idovetailjointmetadata.md))

<a id="getstepnames"></a>
<a id="getstepnames-idovetailjointmetadata"></a>

## GetStepNames(IDovetailJointMetadata)

- Name: `GetStepNames(IDovetailJointMetadata)`
- Modifiers: `extension`
- Returns: `IEnumerable<string>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-484-merge.Sha.0e7ab3e6cc9922c1b2ae61bec2dfad8c9fdd39a2/src/Joinery.Dovetail/Diagnostics/DovetailMetadataDiagnosticExtensions.cs#L29-L32)

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

- `metadata` ([IDovetailJointMetadata](/Dovetail/pr-preview/pr-484/api/joinery.dovetail/idovetailjointmetadata.md))
