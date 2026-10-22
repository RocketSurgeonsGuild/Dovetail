using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;
using Stashbox;
using Stashbox.Exceptions;

#pragma warning disable CA1812

namespace Dovetail.Stashbox.Tests;

public class StashboxOptionsTests : AutoFakeTest<TestRecord>
{
    [Test]
    public async Task ConfigureContainer_IsApplied()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.Set(new StashboxOptions { ConfigureContainer = config => config.WithUnknownTypeResolution() }));

        host.Services.GetService<Unregistered>().ShouldNotBeNull();
    }

    [Test]
    public async Task ConfigureContainer_NotSet_DoesNotResolveUnknownTypes()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail();

        host.Services.GetService<Unregistered>().ShouldBeNull();
    }

    [Test]
    public async Task ValidateOnBuild_ValidContainer_Builds()
    {
        using var host = await Host
                              .CreateApplicationBuilder()
                              .ConfigureDovetail(rb => rb.Set(new StashboxOptions { ValidateOnBuild = true, }));

        host.Services.GetService<IStashboxContainer>().ShouldNotBeNull();
    }

    [Test]
    public async Task ValidateOnBuild_UnresolvableDependency_Throws()
    {
        ( await Should.ThrowAsync<AggregateException>(
            async () =>
            {
                using var host = await Host
                                      .CreateApplicationBuilder()
                                      .ConfigureDovetail(
                                           rb => rb
                                                .Set(new StashboxOptions { ValidateOnBuild = true, })
                                                .ConfigureStashbox((_, container) => container.Register<NeedsUnregistered>())
                                       );
            }
        ) ).Message.ShouldContain(nameof(NeedsUnregistered));
    }

    [Test]
    public async Task LifetimeValidation_Development_ThrowsOnScopedDependencyOfSingleton()
    {
        using var host = await CreateBuilder(Environments.Development)
                              .ConfigureDovetail(rb => rb.ConfigureServices((_, services) => RegisterCaptiveDependency(services)));

        Should.Throw<LifetimeValidationFailedException>(host.Services.GetRequiredService<SingletonService>);
    }

    [Test]
    public async Task LifetimeValidation_Production_AllowsScopedDependencyOfSingleton()
    {
        using var host = await CreateBuilder(Environments.Production)
                              .ConfigureDovetail(rb => rb.ConfigureServices((_, services) => RegisterCaptiveDependency(services)));

        host.Services.GetRequiredService<SingletonService>().ShouldNotBeNull();
    }

    [Test]
    public async Task LifetimeValidation_DevelopmentOptedOut_AllowsScopedDependencyOfSingleton()
    {
        using var host = await CreateBuilder(Environments.Development)
                              .ConfigureDovetail(
                                   rb => rb
                                        .Set(new StashboxOptions { ValidateLifetimesInDevelopment = false, })
                                        .ConfigureServices((_, services) => RegisterCaptiveDependency(services))
                               );

        host.Services.GetRequiredService<SingletonService>().ShouldNotBeNull();
    }

    public StashboxOptionsTests() : base(TestRecord.Create(LogEventLevel.Information)) { }

    private static HostApplicationBuilder CreateBuilder(string environmentName) =>
        Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { EnvironmentName = environmentName, });

    private static void RegisterCaptiveDependency(IServiceCollection services) =>
        services
           .AddScoped<ScopedService>()
           .AddSingleton<SingletonService>();

    public interface IUnregisteredDependency;

    public sealed class Unregistered;

    public sealed class NeedsUnregistered(IUnregisteredDependency dependency)
    {
        public IUnregisteredDependency Dependency { get; } = dependency;
    }

    public sealed class ScopedService;

    public sealed class SingletonService(ScopedService scoped)
    {
        public ScopedService Scoped { get; } = scoped;
    }
}
