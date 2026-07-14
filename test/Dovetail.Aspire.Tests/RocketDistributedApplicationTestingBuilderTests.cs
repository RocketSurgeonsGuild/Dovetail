using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Dovetail.Joints;
using Dovetail.Testing.Joints;
using FakeItEasy;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;

namespace Dovetail.Aspire.Tests;

public partial class RocketDistributedApplicationTestingBuilderTests
    () : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task Should_UseRocketBooster()
    {
        await using var host = await DistributedApplicationTestingBuilder
                                    .CreateAsync<Anchor>()
                                    .ConfigureDovetail();

        host.Services.ShouldNotBeNull();
    }

    [Test]
    public async Task Should_ConfigureHosting()
    {
        var convention = A.Fake<DistributedApplicationTestingJoint>();
        await using var host = await DistributedApplicationTestingBuilder
                                    .CreateAsync<Anchor>()
                                    .ConfigureDovetail(rb => rb.ConfigureDistributedApplicationTesting(convention));

        A.CallTo(() => convention.Invoke(A<IDovetailContext>._, A<IDistributedApplicationTestingBuilder>._)).MustHaveHappened();
    }

    [Test]
    public async Task Should_Build_The_Host_Correctly()
    {
        var @delegate = A.Fake<HostCreatedAsyncJoint<IHost>>();
        var delegate2 = A.Fake<HostCreatedJoint<DistributedApplication>>();
        await using var host = await DistributedApplicationTestingBuilder
                                    .CreateAsync<Anchor>()
                                    .ConfigureDovetail(z => z.ConfigureHostCreated(@delegate).ConfigureHostCreated(delegate2));

        A.CallTo(() => @delegate.Invoke(A<IDovetailContext>._, A<IHost>._, A<CancellationToken>._)).MustHaveHappened();
        A.CallTo(() => delegate2.Invoke(A<IDovetailContext>._, A<DistributedApplication>._)).MustHaveHappened();
        host.Services.ShouldNotBeNull();
    }
}
