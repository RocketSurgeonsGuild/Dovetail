using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1040, CA1034

namespace Dovetail.Mermaid.Tests;

public class DovetailContextDiagramTests
{
    [Test]
    public async Task Should_Render_Default_Flowchart()
    {
        var context = await Imports.Joints().Set(DovetailHostType.Undefined).CreateAsync();

        await Verify(context.ToMermaidJointFlowDiagramMarkdown(), "md");
    }
    // [Test]
    public async Task Should_Verify_Metadata()
    {
        var context = await Imports.Joints().Set(DovetailHostType.Undefined).CreateAsync();

        // TODO: Fix
        await Verify(context.Metadata);
    }
}


[DovetailExport]
[DovetailHostType(DovetailHostType.Undefined)]
public class MySetupJoint : ISetupJoint
{
    public void Register(IDovetailContext context) { }
}

[DovetailExport]
[DovetailHostType(DovetailHostType.Undefined)]
public class TaggedJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
}

[DovetailExport]
[DovetailHostType(DovetailHostType.Undefined)]
public class MyLoggingJoint : ILoggingJoint
{
    public void Register(IDovetailContext context, ILoggingBuilder builder) { }
}

[DovetailExport]
[DependsOnJoint<MyLoggingJoint>]
[DovetailHostType(DovetailHostType.Undefined)]
public class DependsOnDependsOnPlainJoint : ILoggingJoint
{
    public void Register(IDovetailContext context, ILoggingBuilder builder) { }
}

[DovetailExport]
[DependentOfJoint<LiveConfigurationJoint>]
[DovetailHostType(DovetailHostType.Undefined)]
public class ConfigurationJoint1 : IConfigurationJoint
{
    public void Register(IDovetailContext context, IConfigurationBuilder builder) { }
}

[DovetailExport]
[LiveJoint]
public class LiveConfigurationJoint : IConfigurationJoint
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
public class FirstLoggingBridgeJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
}

[DovetailExport]
public class SecondLoggingBridgeJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
}

[DovetailExport]
public class SpecificStepBridgeJoint : IServiceJoint, ILoggingJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }

    void ILoggingJoint.Register(IDovetailContext context, ILoggingBuilder builder) { }
}

[DovetailExport]
[DovetailCategory(DovetailCategory.Core)]
public class CoreCategoryJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) { }
}
