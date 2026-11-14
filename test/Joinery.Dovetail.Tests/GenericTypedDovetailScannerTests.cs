using Joinery.Dovetail.Joints;
using Joinery.Dovetail.Tests.Fixtures;
using Rocket.Surgery.Extensions.Testing;

namespace Joinery.Dovetail.Tests;

public class GenericTypedDovetailScannerTests() : LoggerTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task ShouldConstruct()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);
        await scanner.Should().NotBeNull();
    }

    [Test]
    public async Task ShouldBuildAProvider()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []).AppendJoint(new Contrib());

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldScanAddedContributions()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);

        var contribution = new NamedServiceJoint("contribution");
        var contribution2 = new NamedServiceJoint("contribution2");

        scanner.PrependJoint(contribution);
        scanner.AppendJoint(contribution2);

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldIncludeAddedDelegates()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);

        var @delegate = Mock.OfDelegate<ServiceJointDelegate>().Object;
        var delegate2 = Mock.OfDelegate<ServiceJointDelegate>().Object;

        scanner.ConfigureServices(delegate2, default, null);
        scanner.ConfigureServices(@delegate, default, null);

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldScanExcludeContributionTypes()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);
        var contribution = new NamedServiceJoint("contribution");
        var contribution2 = new NamedServiceJoint("contribution2");

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

        var contribution = new NamedServiceJoint("contribution");

        scanner.PrependJoint(contribution);
        scanner.ExceptJoint(contribution.GetType().Assembly);

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }
}
