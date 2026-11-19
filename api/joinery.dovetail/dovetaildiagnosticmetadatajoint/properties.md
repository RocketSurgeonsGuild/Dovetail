# DovetailDiagnosticMetadataJoint Properties

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [DovetailDiagnosticMetadataJoint](/Dovetail/api/joinery.dovetail/dovetaildiagnosticmetadatajoint.md)
- Kind: `Properties`
- Members: `7`

Represents a metadata item for a Dovetail joint, including its name, host type, category, and dependencies.

<a id="assemblyname"></a>

## AssemblyName

- Name: `AssemblyName`
- Modifiers: `get; init`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJoint.cs)

The assembly name of the joint.

```csharp
public string AssemblyName { get; init; }
```

<a id="category"></a>

## Category

- Name: `Category`
- Modifiers: `get; init`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJoint.cs)

The category of the joint.

```csharp
public string Category { get; init; }
```

<a id="connectstojointtypes"></a>

## ConnectsToJointTypes

- Name: `ConnectsToJointTypes`
- Modifiers: `get; init`
- Returns: `ImmutableList<string>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJoint.cs)

The types of the joint that this joint connects to.

```csharp
public ImmutableList<string> ConnectsToJointTypes { get; init; }
```

<a id="dependencies"></a>

## Dependencies

- Name: `Dependencies`
- Modifiers: `get; init`
- Returns: `ImmutableList<DovetailDiagnosticMetadataJointDependency>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJoint.cs)

The dependencies of the joint.

```csharp
public ImmutableList<DovetailDiagnosticMetadataJointDependency> Dependencies { get; init; }
```

<a id="hosttype"></a>

## HostType

- Name: `HostType`
- Modifiers: `get; init`
- Returns: [DovetailHostType](/Dovetail/api/joinery.dovetail/dovetailhosttype.md)
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJoint.cs)

The host type of the joint.

```csharp
public DovetailHostType HostType { get; init; }
```

<a id="implementsjointtypes"></a>

## ImplementsJointTypes

- Name: `ImplementsJointTypes`
- Modifiers: `get; init`
- Returns: `ImmutableList<string>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJoint.cs)

The types of the joint.

```csharp
public ImmutableList<string> ImplementsJointTypes { get; init; }
```

<a id="jointname"></a>

## JointName

- Name: `JointName`
- Modifiers: `get; init`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.c5bb6c9e8f86c7f4f65638c0127e84cdbf96b00b/src/Joinery.Dovetail/Diagnostics/DovetailDiagnosticMetadataJoint.cs)

The name of the joint.

```csharp
public string JointName { get; init; }
```
