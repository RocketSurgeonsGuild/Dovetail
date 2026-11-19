using System.Reflection;
using Joinery.Dovetail.Joints;

namespace Joinery.Dovetail.ModularPipelines.Tests;

/// <summary>
///     What the build generates for ModularPipelines: the PipelineBuilder joint kind (in the package), the ConfigureDovetail entry
///     points the package's DovetailHost writes into an application (this project), and the helper Joinery.Dovetail.Hosting
///     emits there. A change to any of them shows up here.
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

    /// <summary>
    ///     The helper the entry points call is a Joinery.Dovetail.Hosting template (DovetailPipelineHelpers.cs.template), emitted into
    ///     the application rather than compiled into the package.
    /// </summary>
    [Test]
    public async Task PipelineHelpers_AreEmittedIntoTheApplication()
    {
        await typeof(DovetailPipelineHelpers).Assembly.Should().BeSameReferenceAs(typeof(GeneratedSurfaceTests).Assembly);
        await Verify(Describe(typeof(DovetailPipelineHelpers)));
    }

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
