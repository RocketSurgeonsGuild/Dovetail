using Aspire.Hosting;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;

namespace Joinery.Dovetail.Aspire.Tests;

public partial class RocketDistributedApplicationBuilderTests
    () : LoggerTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task Should_UseRocketBooster()
    {
        await using var host = await DistributedApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail();

        await host.Services.Should().NotBeNull();
    }

    [Test]
    public async Task Should_ConfigureHosting()
    {
        var convention = Mock.OfDelegate<DistributedApplicationBuilderJointDelegate>();
        await using var host = await DistributedApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(rb => rb.ConfigureDistributedApplicationBuilder(convention.Object));

        convention.Invoke(Any(), Any()).WasCalled();
    }

    [Test]
    public async Task Should_Build_The_Host_Correctly()
    {
        var @delegate = Mock.OfDelegate<HostCreatedAsyncJointDelegate<IHost>>();
        var delegate2 = Mock.OfDelegate<HostCreatedJointDelegate<DistributedApplication>>();
        await using var host = await DistributedApplication
                                    .CreateBuilder()
                                    .ConfigureDovetail(z => z.ConfigureHostCreated(@delegate.Object).ConfigureHostCreated(delegate2.Object));

        @delegate.Invoke(Any(), Any(), Any()).WasCalled();
        delegate2.Invoke(Any(), Any()).WasCalled();
        await host.Services.Should().NotBeNull();
    }
}
