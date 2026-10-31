using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Diagnostics;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;

#pragma warning disable CA1034, CA1040

namespace Joinery.Dovetail.Tests;

/// <summary>
///     The <c>categories</c> passed to <c>DovetailContextBuilder.Create(...)</c> choose which joints the context loads: the joints of
///     those categories, plus the <c>Core</c> joints, which are always loaded. An empty set loads the <c>Application</c> joints and the
///     <c>Core</c> joints (RocketSurgeonsGuild/Dovetail#286).
/// </summary>
public class DovetailContextCategoriesTests
{
    private static readonly DovetailCategory Core = DovetailCategory.Core;
    private static readonly DovetailCategory Application = DovetailCategory.Application;
    private static readonly DovetailCategory Other = "Other";

    [Test]
    public async Task An_Empty_Set_Loads_The_Application_And_Core_Joints_But_Not_The_Joints_Of_Other_Categories()
    {
        var context = await Create([]);

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(ApplicationJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task Core_Joints_Are_Always_Loaded_Whatever_The_Set_Is()
    {
        foreach (var categories in new DovetailCategory[][] { [], [Core], [Application], [Other], ["Nobody"], [Application, Other] })
        {
            Resolved(await Create(categories)).ShouldContain(nameof(CoreJoint), $"for [{string.Join(", ", categories.Select(z => z.ToString()))}]");
        }
    }

    [Test]
    public async Task A_Set_Of_Core_Alone_Loads_Only_The_Core_Joints()
    {
        var context = await Create([Core]);

        Resolved(context).ShouldBe([nameof(CoreJoint)]);
    }

    [Test]
    public async Task The_Application_Category_Loads_The_Application_Joints_And_The_Core_Joints()
    {
        var context = await Create([Application]);

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(ApplicationJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task A_Category_Of_Its_Own_Loads_Its_Joints_And_The_Core_Joints_But_Not_The_Application_Joints()
    {
        var context = await Create([Other]);

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(OtherJoint)], ignoreOrder: true);
    }

    /// <summary>
    ///     What LaunchPad ran into: a helper that passed <c>Core</c> and <c>Application</c>, expecting a context of the Core and
    ///     Application joints, got a context without any of them.
    /// </summary>
    [Test]
    public async Task Passing_Core_And_Application_Loads_Every_Core_And_Application_Joint()
    {
        var context = await Create([Core, Application]);

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(ApplicationJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task A_Set_Of_Every_Category_Loads_Every_Joint()
    {
        var context = await Create([Core, Application, Other]);

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(ApplicationJoint), nameof(OtherJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task A_Category_That_No_Joint_Has_Still_Loads_The_Core_Joints()
    {
        var context = await Create(["Nobody"]);

        Resolved(context).ShouldBe([nameof(CoreJoint)]);
    }

    [Test]
    public async Task Categories_Are_Matched_By_Value_Not_By_Instance()
    {
        // A different instance with the same name, as one built from a string would be.
        var context = await Create([new DovetailCategory("Other")]);

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(OtherJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task The_Overload_With_A_Host_Type_Loads_The_Same_Joints()
    {
        var context = await DovetailContextBuilder
                           .Create(Metadata(), new Dictionary<object, object>(), DovetailHostType.Live, [Application])
                           .CreateAsync();

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(ApplicationJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task Only_The_Joints_Of_The_Set_And_The_Core_Joints_Take_Part_In_Setup()
    {
        var core = new SetupJoint();
        var application = new SetupJoint();
        var other = new SetupJoint();

        await DovetailContextBuilder
             .Create(
                  [
                      new DovetailJointMetadata(core, DovetailHostType.Undefined, Core),
                      new DovetailJointMetadata(application, DovetailHostType.Undefined, Application),
                      new DovetailJointMetadata(other, DovetailHostType.Undefined, Other),
                  ],
                  new Dictionary<object, object>(),
                  [Other]
              )
             .CreateAsync();

        core.Registered.ShouldBeTrue();
        application.Registered.ShouldBeFalse();
        other.Registered.ShouldBeTrue();
    }

    [Test]
    public async Task The_Builder_And_The_Context_Report_The_Categories_That_Are_Loaded()
    {
        await AssertReported([], ["Application", "Core"]);
        await AssertReported([Core], ["Core"]);
        await AssertReported([Application], ["Application", "Core"]);
        await AssertReported([Core, Core, Other], ["Core", "Other"]);
        await AssertReported([Other], ["Core", "Other"]);

        static async Task AssertReported(DovetailCategory[] given, string[] reported)
        {
            var builder = DovetailContextBuilder.Create(Metadata(), new Dictionary<object, object>(), given);
            var context = await builder.CreateAsync();

            builder.Categories.Select(z => z.ToString()).ShouldBe(reported, ignoreOrder: true);
            context.Categories.Select(z => z.ToString()).ShouldBe(reported, ignoreOrder: true);
        }
    }

    [Test]
    public async Task A_Joint_Without_Metadata_Is_An_Application_Joint_Unless_It_Says_Otherwise()
    {
        var application = DovetailContextBuilder
                         .Create([], new Dictionary<object, object>(), [Application])
                         .AppendJoint(new UndeclaredJoint(), new DeclaredCoreJoint());

        Resolved(await application.CreateAsync()).ShouldBe([nameof(UndeclaredJoint), nameof(DeclaredCoreJoint)], ignoreOrder: true);

        var other = DovetailContextBuilder
                   .Create([], new Dictionary<object, object>(), [Other])
                   .AppendJoint(new UndeclaredJoint(), new DeclaredCoreJoint());

        Resolved(await other.CreateAsync()).ShouldBe([nameof(DeclaredCoreJoint)]);
    }

    /// <summary>
    ///     The diagnostics (and the Mermaid diagrams built on them) resolve the joints the same way.
    /// </summary>
    [Test]
    public void The_Diagnostic_Metadata_Loads_The_Same_Joints_As_The_Context()
    {
        var builder = DovetailContextBuilder.Create(Metadata(), new Dictionary<object, object>(), []);

        builder.GetDiagnosticMetadata().Select(z => z.Category).ShouldBe(["Core", "Application"], ignoreOrder: true);
        builder.GetDiagnosticMetadata(Core).Select(z => z.Category).ShouldBe(["Core"]);
        builder.GetDiagnosticMetadata(Other).Select(z => z.Category).ShouldBe(["Core", "Other"], ignoreOrder: true);
        builder.GetDiagnosticMetadata(Application, Other).Select(z => z.Category).ShouldBe(["Core", "Application", "Other"], ignoreOrder: true);
    }

    private static ValueTask<IDovetailContext> Create(IEnumerable<DovetailCategory> categories) =>
        DovetailContextBuilder.Create(Metadata(), new Dictionary<object, object>(), categories).CreateAsync();

    private static string[] Resolved(IDovetailContext context) => [.. context.Joints.Select(z => z.GetType().Name)];

    private static IDovetailJointMetadata[] Metadata() =>
    [
        new DovetailJointMetadata(new CoreJoint(), DovetailHostType.Undefined, Core),
        new DovetailJointMetadata(new ApplicationJoint(), DovetailHostType.Undefined, Application),
        new DovetailJointMetadata(new OtherJoint(), DovetailHostType.Undefined, Other),
    ];

    public sealed class CoreJoint : IDovetailJoint;

    public sealed class ApplicationJoint : IDovetailJoint;

    public sealed class OtherJoint : IDovetailJoint;

    public sealed class UndeclaredJoint : IDovetailJoint;

    [DovetailCategory(DovetailCategory.Core)]
    public sealed class DeclaredCoreJoint : IDovetailJoint;

    public sealed class SetupJoint : ISetupJoint
    {
        public bool Registered { get; private set; }

        public void Register(IDovetailContext context) => Registered = true;
    }
}
