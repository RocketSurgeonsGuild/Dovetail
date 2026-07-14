using Dovetail.Joints;
using FakeItEasy;

using Rocket.Surgery.Extensions.Testing;



namespace Dovetail.Tests;

public class GenericTypedDovetailScannerTests() : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public void ShouldConstruct()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);
        scanner.ShouldNotBeNull();
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

        var contribution = A.Fake<IServiceJoint>(c => c.Named("contribution"));
        var contribution2 = A.Fake<IServiceJoint>(c => c.Named("contribution2"));

        scanner.PrependJoint(contribution);
        scanner.AppendJoint(contribution2);

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldIncludeAddedDelegates()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);

        var @delegate = A.Fake<ServiceJoint>(c => c.Named("delegate"));
        var delegate2 = A.Fake<ServiceJoint>(c => c.Named("delegate2"));

        scanner.ConfigureServices(delegate2, default, null);
        scanner.ConfigureServices(@delegate, default, null);

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }

    [Test]
    public async Task ShouldScanExcludeContributionTypes()
    {
        var scanner = DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []);
        var contribution = A.Fake<IServiceJoint>(c => c.Named("contribution"));
        var contribution2 = A.Fake<IServiceJoint>(c => c.Named("contribution2"));

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

        var contribution = A.Fake<IServiceJoint>(c => c.Named("contribution"));

        scanner.PrependJoint(contribution);
        scanner.ExceptJoint(contribution.GetType().Assembly);

        var context = await DovetailContext.FromAsync(scanner);
        await Verify(context.Joints.Select(z => z.ToString()));
    }
}
