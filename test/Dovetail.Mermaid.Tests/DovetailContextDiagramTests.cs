using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.DependencyOne;

#pragma warning disable CA1040, CA1034

namespace Dovetail.Mermaid.Tests;

public class DovetailContextDiagramTests
{
    [Test]
    public async Task Should_Render_Default_Flowchart()
    {
        var context = await Imports.Joints().CreateAsync();

        await Verify(context.ToMermaidDiagram());
    }

    [Test]
    public async Task Should_Group_Joints_Into_Subgraphs_By_Owning_Assembly()
    {
        var context = await CreateContextAsync(new PlainJoint(), new Class1());

        await Verify(context.ToMermaidDiagram());
    }

    [Test]
    public async Task Should_Tag_Node_With_Discovered_Joint_Types()
    {
        var context = await CreateContextAsync(new TaggedJoint());

        await Verify(context.ToMermaidDiagram());
    }

    [Test]
    public async Task Should_Render_Edge_From_Dependency_To_Dependent_For_DependsOn()
    {
        var context = await CreateContextAsync(new PlainJoint(), new DependsOnPlainJoint());

        await Verify(context.ToMermaidDiagram());
    }

    [Test]
    public async Task Should_Render_Edge_From_Joint_To_Dependency_For_DependentOf()
    {
        var context = await CreateContextAsync(new PlainJoint(), new DependentOfPlainJoint());

        await Verify(context.ToMermaidDiagram());
    }

    [Test]
    public async Task Should_List_Joints_In_Topological_Run_Order_Across_A_Dependency_Chain()
    {
        // Registered out of order on purpose, to prove the diagram reflects resolved run order, not insertion order.
        var context = await CreateContextAsync(new DependsOnDependsOnPlainJoint(), new PlainJoint(), new DependsOnPlainJoint());

        await Verify(context.ToMermaidDiagram());
    }

    [Test]
    public async Task Should_Omit_Edge_When_Dependency_Type_Is_Not_Registered_In_Context()
    {
        var context = await CreateContextAsync(new DanglingDependencyJoint());

        await Verify(context.ToMermaidDiagram());
    }

    [Test]
    public async Task Should_Exclude_Joints_That_Do_Not_Match_The_Context_HostType()
    {
        var context = await CreateContextAsync(new LiveOnlyJoint(), new UnitTestOnlyJoint(), new PlainJoint());
        context.Set(DovetailHostType.Live);

        await Verify(context.ToMermaidDiagram());
    }

    private static async Task<IDovetailContext> CreateContextAsync(params IDovetailJoint[] joints)
    {
        var builder = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);
        builder.AppendJoint(joints);
        return await builder.CreateAsync();
    }
}

[DovetailExport]
public class PlainJoint : ISetupJoint
{
    public void Register(IDovetailContext context) { }
}

[DovetailExport]
public class TaggedJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
}

[DovetailExport]
[DependsOnJoint<PlainJoint>]
public class DependsOnPlainJoint : ILoggingJoint
{
    public void Register(IDovetailContext context, ILoggingBuilder builder) { }
}

[DovetailExport]
[DependsOnJoint<DependsOnPlainJoint>]
public class DependsOnDependsOnPlainJoint : ILoggingJoint
{
    public void Register(IDovetailContext context, ILoggingBuilder builder) { }
}

[DovetailExport]
[DependentOfJoint<PlainJoint>]
public class DependentOfPlainJoint : IConfigurationJoint
{
    public void Register(IDovetailContext context, IConfigurationBuilder builder) { }
};
[DovetailExport]
[LiveJoint]
public class LiveOnlyJoint : IConfigurationJoint
{
    public void Register(IDovetailContext context, IConfigurationBuilder builder) { }
}

[DovetailExport]
[UnitTestJoint]
public class UnitTestOnlyJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
}

[DovetailExport]
public class UnregisteredJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
}
[DovetailExport]
[DependsOnJoint(typeof(UnregisteredJoint))]
public class DanglingDependencyJoint : IDovetailJoint;
