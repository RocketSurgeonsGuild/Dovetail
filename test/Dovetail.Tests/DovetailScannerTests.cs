using Dovetail.Joints;
using FakeItEasy;

using Rocket.Surgery.Extensions.Testing;



namespace Dovetail.Tests;

public class DovetailScannerTests() : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task ShouldConstruct()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);
        scanner.ShouldNotBeNull();

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldBuildAProvider()
    {
        var builder = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []).AppendJoint(new Contrib());

        var context = await DovetailContext.FromAsync(builder);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldScanAddedContributions()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);

        var contribution = A.Fake<IServiceJoint>(z => z.Named("contribution"));
        var contribution2 = A.Fake<IServiceJoint>(z => z.Named("contribution2"));

        scanner.PrependJoint(contribution);
        scanner.AppendJoint(contribution2);

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldIncludeAddedDelegates()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);

        var d1 = A.Fake<ServiceJointDelegate>(z => z.Named("d1"));
        var d2 = A.Fake<ServiceJointDelegate>(z => z.Named("d2"));

        var @delegate = scanner.ConfigureServices(d1, default, null);
        var delegate2 = scanner.ConfigureServices(d2, default, null);

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldScanExcludeContributionTypes()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);

        var contribution = A.Fake<IServiceJoint>(z => z.Named("contribution"));
        var contribution2 = A.Fake<IServiceJoint>(z => z.Named("contribution2"));

        scanner.AppendJoint(contribution);
        scanner.PrependJoint(contribution2);
        scanner.ExceptJoint(typeof(Contrib));

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldScanExcludeContributionAssemblies()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);

        var contribution = A.Fake<IServiceJoint>(z => z.Named("contribution"));

        scanner.PrependJoint(contribution);
        scanner.ExceptJoint(contribution.GetType().Assembly);

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }
}
