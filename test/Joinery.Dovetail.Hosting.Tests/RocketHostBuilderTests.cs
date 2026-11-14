using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;



namespace Joinery.Dovetail.Hosting.Tests;

public partial class RocketHostBuilderTests() : LoggerTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task Should_UseRocketBooster_With_Dovetails()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail();

        await host.Services.Should().NotBeNull();
    }

    [Test]
    public async Task Should_ConfigureServices()
    {
        var convention = Mock.OfDelegate<ServiceJointDelegate>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureServices(convention.Object));

        convention.Invoke(Any(), Any()).WasCalled();
    }

    [Test]
    public async Task Should_ConfigureConfiguration()
    {
        var convention = Mock.OfDelegate<ConfigurationJointDelegate>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureConfiguration(convention.Object));

        convention.Invoke(Any(), Any()).WasCalled();
    }

    [Test]
    public async Task Should_Chain_Dovetail_Configuration_Into_The_Host()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(
                                   rb => rb.ConfigureConfiguration(
                                       static (_, builder) => builder.AddInMemoryCollection(new Dictionary<string, string?> { ["Dovetail:Chained"] = "yes", })
                                   )
                               );

        await host.Services.GetRequiredService<IConfiguration>()["Dovetail:Chained"].Should().BeEqualTo("yes");
    }

    [Test]
    public async Task Should_ConfigureHosting()
    {
        var convention = Mock.OfDelegate<HostApplicationJointDelegate<IHostApplicationBuilder>>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureHostApplication(convention.Object));

        convention.Invoke(Any(), Any()).WasCalled();
    }

    [Test]
    public async Task Should_ConfigureHosting_HostApplication()
    {
        var convention = Mock.OfDelegate<HostApplicationJointDelegate<HostApplicationBuilder>>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureHostApplication(convention.Object));

        convention.Invoke(Any(), Any()).WasCalled();
    }

    [Test]
    public async Task Should_ConfigureLogging()
    {
        var convention = Mock.OfDelegate<LoggingJointDelegate>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureLogging(convention.Object));

        convention.Invoke(Any(), Any()).WasCalled();
    }

    [Test]
    public async Task Should_Build_The_Host_Correctly()
    {
        var @delegate = Mock.OfDelegate<HostCreatedJointDelegate<IHost>>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(z => z.ConfigureHostCreated(@delegate.Object));

        @delegate.Invoke(Any(), Any()).WasCalled();
        await host.Services.Should().NotBeNull();
    }

    //    [Test]
    //    public async Task Should_Run_Rocket_CommandLine()
    //    {
    //        using var host = Host.CreateApplicationBuilder(Array.Empty<string>())
    //                          .ConfigureDovetail(
    //                               rb => rb
    //                                  .AppendDelegate(
    //                                       new CommandLineDovetail((a, c) => c.OnRun(state => 1337)),
    //                                       new CommandLineDovetail((a, c) => c.OnRun(state => 1337))
    //                                   )
    //                           );
    //
    //        ( await builder.RunCli() ).ShouldBe(1337);
    //    }
    //
    //    [Test]
    //    public async Task Should_Inject_WebHost_Into_Command()
    //    {
    //        using var host = Host.CreateApplicationBuilder(new[] { "myself" })
    //                          .ConfigureDovetail(
    //                               rb => rb
    //                                    .AppendDelegate(new CommandLineDovetail((a, c) => c.OnRun(state => 1337)))
    //                                    .AppendDelegate(new CommandLineDovetail((a, context) => context.AddCommand<MyCommand>("myself")))
    //                           );
    //
    //        ( await builder.RunCli() ).ShouldBe(1234);
    //    }
}
