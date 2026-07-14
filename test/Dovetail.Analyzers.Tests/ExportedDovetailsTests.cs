using Microsoft.Extensions.DependencyInjection;

namespace Dovetail.Analyzers.Tests;

public class ExportedDovetailsTests() : GeneratorTest()
{
    [Test]
    public async Task Should_Pull_Through_A_Dovetail()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;

namespace Dovetail.Tests
{
    [DovetailExport]
    internal class Contrib : IDovetailJoint { }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Pull_Through_A_Dovetail_With_Custom_Namespace()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;


namespace Dovetail.Tests
{
    [DovetailExport]
    internal class Contrib : IDovetailJoint { }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Pull_Through_A_Dovetail_With_No_Namespace()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;


namespace Dovetail.Tests
{
    [DovetailExport]
    internal class Contrib : IDovetailJoint { }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }


    [Test]
    public async Task Should_Pull_Through_A_Dovetail_With_Custom_MethodName()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;


namespace Dovetail.Tests
{
    [DovetailExport]
    internal class Contrib : IDovetailJoint { }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Pull_Through_A_Dovetail_With_ExportAttribute()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;

namespace Dovetail.Tests
{
    [DovetailExportAttribute]
    internal class Contrib : IDovetailJoint { }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Pull_Through_All_Dovetails()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;


[DovetailExport]
internal class Contrib1 : IDovetailJoint { }
",
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;

[DovetailExportAttribute]
internal class Contrib2 : IDovetailJoint { }
[DovetailExport]
internal class Contrib3 : IDovetailJoint { }
",
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;

[DovetailExport]
internal class Contrib4 : IDovetailJoint { }
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Handle_Dovetails_With_One_Constructor()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;

namespace Dovetail.Tests
{
    interface IService {}
    interface IServiceB {}
    interface IServiceC {}
    internal class ParentContrib {
        [DovetailExport]
        internal class Contrib : IDovetailJoint { public Contrib(IService service, IServiceB serviceB, IServiceC? serviceC = null) {} }
    }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Handle_Nested_Dovetails()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;

namespace Dovetail.Tests
{
    internal class ParentContrib {
        [DovetailExport]
        internal class Contrib : IDovetailJoint { }
    }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Handle_Nested_Static_Dovetails()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;

namespace Dovetail.Tests
{
    internal static class ParentContrib {
        [DovetailExport]
        internal class Contrib : IDovetailJoint { }
    }
}
"
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result);
    }

    [Test]
    public async Task Should_Handle_Dovetails_With_Nullable_Constructor_Parameters()
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Rocket.Surgery.LaunchPad.Mapping;

namespace Rocket.Surgery.LaunchPad.Mapping;

/// <summary>
///     AutoMapperDovetail.
///     Implements the <see cref=""IServiceJoint"" />
/// </summary>
/// <seealso cref=""IServiceJoint"" />
[DovetailExport]
public class AutoMapperDovetail : IServiceJoint
{
    private readonly AutoMapperOptions _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref=""AutoMapperDovetail"" /> class.
    /// </summary>
    /// <param name=""options"">The options.</param>
    public AutoMapperDovetail(AutoMapperOptions? options = null)
    {
        _options = options ?? new AutoMapperOptions();
    }

    /// <summary>
    ///     Registers the specified context.
    /// </summary>
    /// <param name=""context"">The context.</param>
    /// <param name=""configuration""></param>
    /// <param name=""services""></param>
    public void Register(IDovetailContext context, IConfiguration configuration, IServiceCollection services)
    {
    }
}

/// <summary>
///     Class AutoMapperOptions.
/// </summary>
public class AutoMapperOptions
{
    /// <summary>
    ///     Gets or sets the service lifetime.
    /// </summary>
    /// <value>The service lifetime.</value>
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Transient;
}
"
                           )
                          .AddReferences(typeof(ServiceLifetime))
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);
        await Verify(result);
    }

    [Test]
    [Arguments(DovetailHostType.Live)]
    [Arguments(DovetailHostType.UnitTest)]
    public async Task Should_Support_HostType_Dovetails(DovetailHostType hostType)
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;

namespace Dovetail.Tests
{
    [DovetailExport]
    [{HostType}Dovetail]
    internal class Contrib : IDovetailJoint { }
}
".Replace("{HostType}", hostType.ToString(), StringComparison.OrdinalIgnoreCase)
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result).UseTextForParameters(hostType.ToString());
    }

    [Test]
    [Arguments("Custom")]
    [Arguments("Infrastructure")]
    [Arguments("Application")]
    public async Task Should_Support_Category_Dovetails(string category)
    {
        var result = await WithGenericSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;

namespace Dovetail.Tests
{
    [DovetailExport]
    [DovetailCategory(""{Category}"")]
    internal class Contrib : IDovetailJoint { }
}
".Replace("{Category}", category, StringComparison.OrdinalIgnoreCase)
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result).UseTextForParameters(category);
    }

    [Test]
    [Arguments("AfterJointAttribute")]
    [Arguments("DependsOnJointAttribute")]
    [Arguments("BeforeJointAttribute")]
    [Arguments("DependentOfJointAttribute")]
    public async Task Should_Support_DependencyDirection_Dovetails(string attributeName)
    {
        var result = await WithSharedDeps()
                          .AddSources(
                               @"
using Dovetail;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Tests;

namespace Dovetail.Tests
{
    [DovetailExport]
    [{AttributeName}(typeof(D))]
    [LiveJoint, System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class Contrib : IDovetailJoint { }

    internal class D : IDovetailJoint { }
}
".Replace("{AttributeName}", attributeName, StringComparison.OrdinalIgnoreCase)
                           )
                          .Build()
                          .GenerateAsync(TestContext.CancellationToken);

        await Verify(result).UseTextForParameters(attributeName);
    }

    [Before(Test)]
    public Task InitializeAsync()
    {
        Configure(b => b.IgnoreOutputFile("Imported_Assembly_Dovetails.cs"));
        return Task.CompletedTask;
    }
}
