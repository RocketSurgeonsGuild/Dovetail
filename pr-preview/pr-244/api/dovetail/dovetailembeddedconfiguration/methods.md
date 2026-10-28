# DovetailEmbeddedConfiguration Methods

- Package: [Dovetail](/Dovetail/pr-preview/pr-244/api/dovetail.md)
- Type: [DovetailEmbeddedConfiguration](/Dovetail/pr-preview/pr-244/api/dovetail/dovetailembeddedconfiguration.md)
- Kind: `Methods`
- Members: `3`

Loads configuration files that the Dovetail SDK embedded into an assembly as manifest resources, for hosts that have no file system to read them from (mobile and other app-package hosts).

<a id="get-embeddedconfigurationassembly"></a>
<a id="get-embeddedconfigurationassembly-idovetailcontext"></a>

## get_EmbeddedConfigurationAssembly(IDovetailContext)

- Name: `get_EmbeddedConfigurationAssembly(IDovetailContext)`
- Modifiers: `static` `nullable`
- Returns: `Assembly?`

```csharp
public static class DovetailEmbeddedConfiguration
{
    public static Assembly? get_EmbeddedConfigurationAssembly(
        IDovetailContext context)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-244/api/dovetail/idovetailcontext.md))

<a id="getconventionalfilenames"></a>
<a id="getconventionalfilenames-idovetailcontext-readonlyspan-string"></a>

## GetConventionalFileNames(IDovetailContext, ReadOnlySpan<string>)

- Name: `GetConventionalFileNames(IDovetailContext, ReadOnlySpan<string>)`
- Modifiers: `static`
- Returns: `IReadOnlyList<string>`

Gets the conventional configuration file names, lowest precedence first: `appsettings`, the application name, each of those qualified by the environment name, and each of those qualified by `local`.

```csharp
public static class DovetailEmbeddedConfiguration
{
    public static IReadOnlyList<string> GetConventionalFileNames(
        IDovetailContext context,
        params ReadOnlySpan<string> extensions)
    {
        // ...
    }
}
```

## Parameters

- `context` ([IDovetailContext](/Dovetail/pr-preview/pr-244/api/dovetail/idovetailcontext.md))
  The context to read the `ApplicationName` and `EnvironmentName` from.
- `extensions` (`ReadOnlySpan<string>`)
  The file extensions to produce names for (without the leading dot), in order at each position.

## Returns

`IReadOnlyList<string>` -- The file names, lowest precedence first.

## Exceptions

- `ArgumentNullException` -- `context` is `null`.

<a id="openresource"></a>
<a id="openresource-assembly-readonlyspan-string"></a>

## OpenResource(Assembly, ReadOnlySpan<string>)

- Name: `OpenResource(Assembly, ReadOnlySpan<string>)`
- Modifiers: `static` `nullable`
- Returns: `Stream?`

Opens the first embedded resource that matches one of `fileNames`, in order. A resource matches when its name is the file name itself (the Dovetail SDK's naming) or ends with `.{fileName}` (the default manifest naming of other tools).

```csharp
public static class DovetailEmbeddedConfiguration
{
    public static Stream? OpenResource(
        Assembly assembly,
        params ReadOnlySpan<string> fileNames)
    {
        // ...
    }
}
```

## Parameters

- `assembly` (`Assembly`)
  The assembly to search.
- `fileNames` (`ReadOnlySpan<string>`)
  The candidate file names, most preferred first.

## Returns

`Stream?` -- The resource stream, or `null` if no resource matches.

## Exceptions

- `ArgumentNullException` -- `assembly` is `null`.
