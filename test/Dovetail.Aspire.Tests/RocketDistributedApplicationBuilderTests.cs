using Aspire.Hosting;
using Dovetail.Joints;
using FakeItEasy;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;

namespace Dovetail.Aspire.Tests;

public partial class RocketDistributedApplicationBuilderTests
    () : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task Should_UseRocketBooster()
    {
        await using var host = await DistributedApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail();

        host.Services.ShouldNotBeNull();
    }

    [Test]
    public async Task Should_ConfigureHosting()
    {
        var convention = A.Fake<DistributedApplicationBuilderJoint>();
        await using var host = await DistributedApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(rb => rb.ConfigureDistributedApplicationBuilder(convention));

        A.CallTo(() => convention.Invoke(A<IDovetailContext>._, A<IDistributedApplicationBuilder>._)).MustHaveHappened();
    }

    [Test]
    public async Task Should_Build_The_Host_Correctly()
    {
        var @delegate = A.Fake<HostCreatedAsyncJoint<IHost>>();
        var delegate2 = A.Fake<HostCreatedJoint<DistributedApplication>>();
        await using var host = await DistributedApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(z => z.ConfigureHostCreated(@delegate).ConfigureHostCreated(delegate2));

        A.CallTo(() => @delegate.Invoke(A<IDovetailContext>._, A<IHost>._, A<CancellationToken>._)).MustHaveHappened();
        A.CallTo(() => delegate2.Invoke(A<IDovetailContext>._, A<DistributedApplication>._)).MustHaveHappened();
        host.Services.ShouldNotBeNull();
    }
}
