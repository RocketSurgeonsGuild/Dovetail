using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Diagnostics;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;

#pragma warning disable CA1034, CA1040

namespace Joinery.Dovetail.Tests;

/// <summary>
///     The <c>categories</c> passed to <c>DovetailContextBuilder.Create(...)</c> are the categories the context is limited to: only
///     the joints whose category is in the set are loaded, and an empty set loads every joint
///     (RocketSurgeonsGuild/Dovetail#286).
/// </summary>
public class DovetailContextCategoriesTests
{
    private static readonly DovetailCategory Core = DovetailCategory.Core;
    private static readonly DovetailCategory Application = DovetailCategory.Application;
    private static readonly DovetailCategory Other = "Other";

    [Test]
    public async Task An_Empty_Set_Loads_Every_Joint()
    {
        var context = await Create([]);

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(ApplicationJoint), nameof(OtherJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task A_Set_Of_One_Category_Loads_Only_The_Joints_Of_That_Category()
    {
        var context = await Create([Core]);

        Resolved(context).ShouldBe([nameof(CoreJoint)]);
    }

    [Test]
    public async Task The_Application_Category_Loads_Only_The_Application_Joints()
    {
        var context = await Create([Application]);

        Resolved(context).ShouldBe([nameof(ApplicationJoint)]);
    }

    [Test]
    public async Task A_Set_Of_Two_Categories_Loads_The_Joints_Of_Both_And_No_Others()
    {
        var context = await Create([Core, Application]);

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(ApplicationJoint)], ignoreOrder: true);
    }

    /// <summary>
    ///     What LaunchPad ran into: a helper that passed <c>Core</c> and <c>Application</c>, expecting a context of the Core and
    ///     Application joints, got a context without any of them.
    /// </summary>
    [Test]
    public async Task Passing_Core_And_Application_Loads_Every_Core_And_Application_Joint()
    {
        var context = await DovetailContextBuilder
                           .Create(Metadata().Take(2), new Dictionary<object, object>(), [Core, Application])
                           .CreateAsync();

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(ApplicationJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task A_Set_Of_Every_Category_Loads_Every_Joint()
    {
        var context = await Create([Core, Application, Other]);

        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(ApplicationJoint), nameof(OtherJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task A_Category_That_No_Joint_Has_Loads_Nothing()
    {
        var context = await Create(["Nobody"]);

        context.Joints.ShouldBeEmpty();
    }

    [Test]
    public async Task Categories_Are_Matched_By_Value_Not_By_Instance()
    {
        // A different instance with the same name, as one built from a string would be.
        var context = await Create([new DovetailCategory("Core")]);

        Resolved(context).ShouldBe([nameof(CoreJoint)]);
    }

    [Test]
    public async Task The_Overload_With_A_Host_Type_Loads_The_Same_Joints()
    {
        var context = await DovetailContextBuilder
                           .Create(Metadata(), new Dictionary<object, object>(), DovetailHostType.Live, [Core])
                           .CreateAsync();

        Resolved(context).ShouldBe([nameof(CoreJoint)]);
    }

    [Test]
    public async Task Only_The_Joints_Of_The_Set_Take_Part_In_Setup()
    {
        var core = new SetupJoint();
        var application = new SetupJoint();

        await DovetailContextBuilder
             .Create(
                  [
                      new DovetailJointMetadata(core, DovetailHostType.Undefined, Core),
                      new DovetailJointMetadata(application, DovetailHostType.Undefined, Application),
                  ],
                  new Dictionary<object, object>(),
                  [Core]
              )
             .CreateAsync();

        core.Registered.ShouldBeTrue();
        application.Registered.ShouldBeFalse();
    }

    [Test]
    public async Task The_Builder_And_The_Context_Report_The_Set_That_Was_Given()
    {
        var builder = DovetailContextBuilder.Create(Metadata(), new Dictionary<object, object>(), [Core, Core, "Other"]);
        var context = await builder.CreateAsync();

        builder.Categories.Select(z => z.ToString()).ShouldBe(["Core", "Other"], ignoreOrder: true);
        context.Categories.Select(z => z.ToString()).ShouldBe(["Core", "Other"], ignoreOrder: true);
        Resolved(context).ShouldBe([nameof(CoreJoint), nameof(OtherJoint)], ignoreOrder: true);
    }

    [Test]
    public async Task A_Joint_Without_Metadata_Is_An_Application_Joint_Unless_It_Says_Otherwise()
    {
        var application = DovetailContextBuilder
                         .Create([], new Dictionary<object, object>(), [Application])
                         .AppendJoint(new UndeclaredJoint(), new DeclaredCoreJoint());

        Resolved(await application.CreateAsync()).ShouldBe([nameof(UndeclaredJoint)]);

        var core = DovetailContextBuilder
                  .Create([], new Dictionary<object, object>(), [Core])
                  .AppendJoint(new UndeclaredJoint(), new DeclaredCoreJoint());

        Resolved(await core.CreateAsync()).ShouldBe([nameof(DeclaredCoreJoint)]);
    }

    /// <summary>
    ///     The diagnostics (and the Mermaid diagrams built on them) resolve the joints the same way.
    /// </summary>
    [Test]
    public void The_Diagnostic_Metadata_Is_Limited_To_The_Categories_Given()
    {
        var builder = DovetailContextBuilder.Create(Metadata(), new Dictionary<object, object>(), []);

        builder.GetDiagnosticMetadata().Select(z => z.Category).ShouldBe(["Core", "Application", "Other"], ignoreOrder: true);
        builder.GetDiagnosticMetadata(Core).Select(z => z.Category).ShouldBe(["Core"]);
        builder.GetDiagnosticMetadata(Core, Other).Select(z => z.Category).ShouldBe(["Core", "Other"], ignoreOrder: true);
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
