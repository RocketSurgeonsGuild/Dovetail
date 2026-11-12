using System.Reflection;
using Joinery.Dovetail.Joints;

namespace Joinery.Dovetail.ModularPipelines.Tests;

/// <summary>
///     What the build generates for ModularPipelines: the PipelineBuilder joint kind (in the package) and the ConfigureDovetail entry
///     points the package's DovetailHost writes into an application (this project). A change to either shows up here.
/// </summary>
public class GeneratedSurfaceTests
{
    [Test]
    public Task PipelineBuilderJoint_Surface() =>
        Verify(
            Describe(
                typeof(IPipelineBuilderJoint),
                typeof(IPipelineBuilderAsyncJoint),
                typeof(PipelineBuilderJointDelegate),
                typeof(PipelineBuilderAsyncJointDelegate),
                typeof(PipelineBuilderJointExtensions),
                typeof(PipelineBuilderHostBuilderExtensions)
            )
        );

    [Test]
    public Task ConfigureDovetail_EntryPoints() => Verify(Describe(typeof(GeneratedDovetailModularPipelinesExtensions)));

    private static IEnumerable<string> Describe(params Type[] types) =>
        types.SelectMany(
            type => type
                   .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                   .Where(method => !method.IsSpecialName && method.Name is not ( "BeginInvoke" or "EndInvoke" ))
                   .Select(method => $"{type.Name}.{method.Name}({string.Join(", ", method.GetParameters().Select(Parameter))}) : {Name(method.ReturnType)}")
                   .Order(StringComparer.Ordinal)
        );

    private static string Parameter(ParameterInfo parameter) => $"{Name(parameter.ParameterType)} {parameter.Name}";

    private static string Name(Type type) =>
        type.IsGenericType
            ? $"{type.Name[..type.Name.IndexOf('`', StringComparison.Ordinal)]}<{string.Join(", ", type.GetGenericArguments().Select(Name))}>"
            : type.Name;
}
