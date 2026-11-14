using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Joinery.Dovetail.Joints;
using Joinery.Dovetail.Testing.Joints;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;

namespace Joinery.Dovetail.Aspire.Tests;

public partial class RocketDistributedApplicationTestingBuilderTests
    () : LoggerTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task Should_UseRocketBooster()
    {
        await using var host = await DistributedApplicationTestingBuilder
                                    .CreateAsync<Anchor>()
                                    .ConfigureDovetail();

        await host.Services.Should().NotBeNull();
    }

    [Test]
    public async Task Should_ConfigureHosting()
    {
        var convention = Mock.OfDelegate<DistributedApplicationTestingJointDelegate>();
        await using var host = await DistributedApplicationTestingBuilder
                                    .CreateAsync<Anchor>()
                                    .ConfigureDovetail(rb => rb.ConfigureDistributedApplicationTesting(convention.Object));

        convention.Invoke(Any(), Any()).WasCalled();
    }

    [Test]
    public async Task Should_Build_The_Host_Correctly()
    {
        var @delegate = Mock.OfDelegate<HostCreatedAsyncJointDelegate<IHost>>();
        var delegate2 = Mock.OfDelegate<HostCreatedJointDelegate<DistributedApplication>>();
        await using var host = await DistributedApplicationTestingBuilder
                                    .CreateAsync<Anchor>()
                                    .ConfigureDovetail(z => z.ConfigureHostCreated(@delegate.Object).ConfigureHostCreated(delegate2.Object));

        @delegate.Invoke(Any(), Any(), Any()).WasCalled();
        delegate2.Invoke(Any(), Any()).WasCalled();
        await host.Services.Should().NotBeNull();
    }
}
