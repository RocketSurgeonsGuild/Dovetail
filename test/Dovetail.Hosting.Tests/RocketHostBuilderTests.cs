using Dovetail.Joints;
using FakeItEasy;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Extensions.Testing;



namespace Dovetail.Hosting.Tests;

public partial class RocketHostBuilderTests() : AutoFakeTest<TestRecord>(TestRecord.Create())
{
    [Test]
    public async Task Should_UseRocketBooster_With_Dovetails()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail();

        host.Services.ShouldNotBeNull();
    }

    [Test]
    public async Task Should_ConfigureServices()
    {
        var convention = A.Fake<ServiceJoint>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureServices(convention));

        A.CallTo(() => convention.Invoke(A<IDovetailContext>._, A<IServiceCollection>._)).MustHaveHappened();
    }

    [Test]
    public async Task Should_ConfigureConfiguration()
    {
        var convention = A.Fake<ConfigurationJoint>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureConfiguration(convention));

        A.CallTo(() => convention.Invoke(A<IDovetailContext>._, A<IConfigurationBuilder>._)).MustHaveHappened();
    }

    [Test]
    public async Task Should_ConfigureHosting()
    {
        var convention = A.Fake<HostApplicationJoint<IHostApplicationBuilder>>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureHostApplication(convention));

        A.CallTo(() => convention.Invoke(A<IDovetailContext>._, A<IHostApplicationBuilder>._)).MustHaveHappened();
    }

    [Test]
    public async Task Should_ConfigureHosting_HostApplication()
    {
        var convention = A.Fake<HostApplicationJoint<HostApplicationBuilder>>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureHostApplication(convention));

        A.CallTo(() => convention.Invoke(A<IDovetailContext>._, A<HostApplicationBuilder>._)).MustHaveHappened();
    }

    [Test]
    public async Task Should_ConfigureLogging()
    {
        var convention = A.Fake<LoggingJoint>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.ConfigureLogging(convention));

        A.CallTo(() => convention.Invoke(A<IDovetailContext>._, A<ILoggingBuilder>._)).MustHaveHappened();
    }

    [Test]
    public async Task Should_Build_The_Host_Correctly()
    {
        var @delegate = A.Fake<HostCreatedJoint<IHost>>();
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(z => z.ConfigureHostCreated(@delegate));

        A.CallTo(() => @delegate.Invoke(A<IDovetailContext>._, A<IHost>._)).MustHaveHappened();
        host.Services.ShouldNotBeNull();
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
