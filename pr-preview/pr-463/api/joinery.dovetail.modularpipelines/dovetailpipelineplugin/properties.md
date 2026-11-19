# DovetailPipelinePlugin Properties

- Package: [Joinery.Dovetail.ModularPipelines](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.modularpipelines.md)
- Type: [DovetailPipelinePlugin](/Dovetail/pr-preview/pr-463/api/joinery.dovetail.modularpipelines/dovetailpipelineplugin.md)
- Kind: `Properties`
- Members: `2`

A ModularPipelines plugin that applies Dovetail's joints to the pipeline builder when it is added, for an application that builds and runs the pipeline itself: `builder.AddPlugin(new DovetailPipelinePlugin(DovetailContextBuilder.Create()))`.

<a id="context"></a>

## Context

- Name: `Context`
- Modifiers: `nullable` `get`
- Returns: `IDovetailContext?`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-463-merge.Sha.3b938bb1f685921a3be3b59b60dfaa31fc2a520c/src/Joinery.Dovetail.ModularPipelines/DovetailPipelinePlugin.cs)

The context the joints ran with, once the plugin has been added to a builder.

```csharp
public IDovetailContext? Context { get; }
```

<a id="name"></a>

## Name

- Name: `Name`
- Modifiers: `get`
- Returns: `string`
- Source: [GitHub](https://github.com/RocketSurgeonsGuild/Dovetail/blob/Branch.pull-463-merge.Sha.3b938bb1f685921a3be3b59b60dfaa31fc2a520c/src/Joinery.Dovetail.ModularPipelines/DovetailPipelinePlugin.cs#L26)

Gets the unique name identifying this plugin. A pipeline can add each plugin name once.

```csharp
public string Name { get; }
```
