using SampleMermaidDiagram;

var context = await Imports.Joints().CreateAsync();
File.WriteAllText("diagram.md", context.ToMermaidJointFlowDiagramMarkdown());
