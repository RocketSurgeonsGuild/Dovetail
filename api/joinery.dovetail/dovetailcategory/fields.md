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

This convention is to load for any infrastructure bits (serializer, logging, etc)

```csharp
public const static string Core
```
