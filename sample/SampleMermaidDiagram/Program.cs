using SampleMermaidDiagram;

var builder = Imports.Joints();
File.WriteAllText("diagram.md", builder.ToMermaidJointFlowDiagramMarkdown());
