# StashboxOptions Properties

- Package: [Joinery.Dovetail.Stashbox](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.stashbox.md)
- Type: [StashboxOptions](/Dovetail/pr-preview/pr-483/api/joinery.dovetail.stashbox/stashboxoptions.md)
- Kind: `Properties`
- Members: `3`

Options for building the Stashbox Container

<a id="configurecontainer"></a>

## ConfigureContainer

- Name: `ConfigureContainer`
- Modifiers: `nullable` `get; set`
- Returns: `Action<ContainerConfigurator>`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail.Stashbox/StashboxOptions.cs)

Configures the container (registration behavior, disposable transient tracking, etc.). Applied when the container is created and again once the service collection has been registered, so these settings win over the defaults set by the Microsoft.Extensions.DependencyInjection adapter.

```csharp
public Action<ContainerConfigurator>? ConfigureContainer { get; set; }
```

<a id="validatelifetimesindevelopment"></a>

## ValidateLifetimesInDevelopment

- Name: `ValidateLifetimesInDevelopment`
- Modifiers: `get; set`
- Returns: `bool`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail.Stashbox/StashboxOptions.cs)

Enables lifetime validation when the environment is Development (defaults to true)

```csharp
public bool ValidateLifetimesInDevelopment { get; set; }
```

<a id="validateonbuild"></a>

## ValidateOnBuild

- Name: `ValidateOnBuild`
- Modifiers: `get; set`
- Returns: `bool`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-483-merge.Sha.8119971ebcf60df480f6621ffc0d71c56f95028b/src/Joinery.Dovetail.Stashbox/StashboxOptions.cs)

Validates the whole container once the service provider has been built (defaults to false)

```csharp
public bool ValidateOnBuild { get; set; }
```
