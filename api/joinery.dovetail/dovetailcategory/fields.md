# DovetailCategory Fields

- Package: [Joinery.Dovetail](/Dovetail/api/joinery.dovetail.md)
- Type: [DovetailCategory](/Dovetail/api/joinery.dovetail/dovetailcategory.md)
- Kind: `Fields`
- Members: `2`

The category of a given convention

<a id="application"></a>

## Application

- Name: `Application`
- Modifiers: `static` `const`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.8a82cb49b33f2224c727413f73300cc763726aca/src/Joinery.Dovetail/DovetailCategory.cs)

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
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.main.Sha.8a82cb49b33f2224c727413f73300cc763726aca/src/Joinery.Dovetail/DovetailCategory.cs)

This convention is to load for any infrastructure bits (serializer, logging, etc)

```csharp
public const static string Core
```
