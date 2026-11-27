# DovetailCategory Fields

- Package: [Joinery.Dovetail](/Dovetail/pr-preview/pr-498/api/joinery.dovetail.md)
- Type: [DovetailCategory](/Dovetail/pr-preview/pr-498/api/joinery.dovetail/dovetailcategory.md)
- Kind: `Fields`
- Members: `2`

The category of a given convention

<a id="application"></a>

## Application

- Name: `Application`
- Modifiers: `static` `const`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-498-merge.Sha.564cb3970fab4def9c0887052d46386c596bc161/src/Joinery.Dovetail/DovetailCategory.cs)

This convention is loaded for any application that might be starting

```csharp
public const static string Application
```

## Remarks

Application is the default category for a convention

<a id="core"></a>

## Core

- Name: `Core`
- Modifiers: `static` `const`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-498-merge.Sha.564cb3970fab4def9c0887052d46386c596bc161/src/Joinery.Dovetail/DovetailCategory.cs)

This convention is to load for any infrastructure bits (serializer, logging, etc)

```csharp
public const static string Core
```
