using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Serilog.Events;



namespace Joinery.Dovetail.Tests;

public class DovetailContextTests
    () : LoggerTest<TestRecord>(TestRecord.Create(LogEventLevel.Information))
{
    [Test]
    public async Task GetAStronglyTypedValue()
    {
        var builder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var container = await DovetailContext.FromAsync(builder);
        container.Set("abc");
        await container.Get<string>().Should().BeEqualTo("abc");
    }

    [Test]
    public async Task SetAStronglyTypedValue()
    {
        var builder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var container = await DovetailContext.FromAsync(builder);
        container.Set("abc");
        await container.Get<string>().Should().BeEqualTo("abc");
    }

    [Test]
    public async Task AddDovetails()
    {
        var contextBuilder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var convention = IServiceJoint.Mock();
        contextBuilder.PrependJoint(convention.Object);
        var conventions = await DovetailContext.FromAsync(contextBuilder);
        await new ServiceCollection().ApplyService(conventions);
        convention.Register(Is(conventions), Any()).WasCalled(Times.Once);
    }

    [Test]
    public async Task Setups()
    {
        var contextBuilder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var convention = ISetupJoint.Mock();
        contextBuilder.PrependJoint(convention.Object);

        var context = await DovetailContext.FromAsync(contextBuilder);
        convention.Register(Is(context)).WasCalled(Times.Once);
    }

    [Test]
    public async Task Setups_With_Delegate()
    {
        var contextBuilder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var convention = Mock.OfDelegate<SetupJointDelegate>();
        contextBuilder.ConfigureSetup(convention.Object);

        var context = await DovetailContext.FromAsync(contextBuilder);
        convention.Invoke(Is(context)).WasCalled(Times.Once);
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore_ServiceProvider()
    {
        var contextBuilder = DovetailContextBuilder
                            .Create([], new DovetailDictionary(), [])
                            .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = await DovetailContext.FromAsync(contextBuilder);
        var servicesCollection = new ServiceCollection()
                                      .AddSingleton(IAbc.Mock().Object)
                                      .AddSingleton(IAbc2.Mock().Object);

        await servicesCollection.ApplyService(context);

        var sp = servicesCollection.BuildServiceProvider();
        await sp.GetService<IAbc>().Should().NotBeNull();
        await sp.GetService<IAbc2>().Should().NotBeNull();
        await sp.GetService<IAbc3>().Should().BeNull();
        await sp.GetService<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithApplication_ServiceProvider()
    {
        var contextBuilder = DovetailContextBuilder
                            .Create([], new DovetailDictionary(), [])
                            .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = await DovetailContext.FromAsync(contextBuilder);

        var collection = new ServiceCollection();
        await collection.ApplyService(context);

        collection.AddSingleton(IAbc.Mock().Object);
        collection.AddSingleton(IAbc2.Mock().Object);
        collection.AddSingleton(IAbc4.Mock().Object);

        var sp = collection.BuildServiceProvider();
        await sp.GetService<IAbc>().Should().NotBeNull();
        await sp.GetService<IAbc2>().Should().NotBeNull();
        await sp.GetService<IAbc3>().Should().BeNull();
        await sp.GetService<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_ServiceProvider()
    {
        var contextBuilder = DovetailContextBuilder
                            .Create([], new DovetailDictionary(), [])
                            .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = await DovetailContext.FromAsync(contextBuilder);

        var collection = new ServiceCollection();
        await collection.ApplyService(context);
        collection.AddSingleton(IAbc3.Mock().Object);
        collection.AddSingleton(IAbc4.Mock().Object);

        var sp = collection.BuildServiceProvider();
        await sp.GetService<IAbc>().Should().BeNull();
        await sp.GetService<IAbc2>().Should().BeNull();
        await sp.GetService<IAbc3>().Should().NotBeNull();
        await sp.GetService<IAbc4>().Should().NotBeNull();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithSystem_UsingDovetail()
    {
        var builder = DovetailContextBuilder
                     .Create([], new DovetailDictionary(), [])
                     .AppendJoint(new AbcDovetail());
        builder.Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = await DovetailContext.FromAsync(builder);

        var servicesCollection = new ServiceCollection();
        await servicesCollection.ApplyService(context);

        var items = servicesCollection.BuildServiceProvider();
        await items.GetService<IAbc>().Should().NotBeNull();
        await items.GetService<IAbc2>().Should().NotBeNull();
        await items.GetService<IAbc3>().Should().BeNull();
        await items.GetService<IAbc4>().Should().BeNull();
    }

    [Test]
    public async Task ShouldConstructTheDovetailInjectingTheValues()
    {
        var data = IInjectData.Mock().Object;
        var builder = DovetailContextBuilder
                     .Create([], new DovetailDictionary(), [])
                     .AppendJoint<InjectableDovetail>()
                     .Set(data)
                     .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = await DovetailContext.FromAsync(builder);
        var collection = new ServiceCollection();
        await collection.ApplyService(context);
        await collection.Should().Contain(z => z.ServiceType == typeof(IInjectData));
    }

    [Test]
    public async Task ShouldConstructTheDovetailInjectingTheValuesIfOptional()
    {
        var data = IInjectData.Mock().Object;
        var builder = DovetailContextBuilder
                     .Create([], new DovetailDictionary(), [])
                     .AppendJoint<OptionalInjectableDovetail>()
                     .Set(data)
                     .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = ( await DovetailContext.FromAsync(builder) ).Set(data);
        var collection = new ServiceCollection();
        await collection.ApplyService(context);
        await collection.Should().Contain(z => z.ServiceType == typeof(IInjectData));
    }

    [Test]
    public async Task ShouldFailToConstructTheDovetailInjectingTheValuesIfMissing()
    {
        var builder = DovetailContextBuilder
                     .Create([], new DovetailDictionary(), [])
                     .AppendJoint<InjectableDovetail>()
                     .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var a = () => DovetailContext.FromAsync(builder).AsTask();
        await a.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public async Task ShouldNotFailToConstructTheDovetailInjectingTheValuesIfOptional()
    {
        var builder = DovetailContextBuilder
                     .Create([], new DovetailDictionary(), [])
                     .AppendJoint<OptionalInjectableDovetail>()
                     .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = await DovetailContext.FromAsync(builder);
        Task a() => new ServiceCollection().ApplyService(context).AsTask();
        await Assert.That(a).ThrowsNothing();
    }

    public interface IAbc;

    public interface IAbc2;

    public interface IAbc3;

    public interface IAbc4;

    public interface IInjectData;

    public class InjectData;

    public class AbcDovetail : IServiceJoint
    {
        public void Register(IDovetailContext context, IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(context);

            services.AddSingleton(IAbc.Mock().Object);
            services.AddSingleton(IAbc2.Mock().Object);
        }
    }

    public class InjectableDovetail(IInjectData convention) : IServiceJoint
    {
        public void Register(IDovetailContext context, IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(context);

            services.AddSingleton(convention);
        }
    }

    public class OptionalInjectableDovetail(IInjectData? convention = null) : IServiceAsyncJoint
    {
        public ValueTask Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (convention is { })
                services.AddSingleton(convention);
            return ValueTask.CompletedTask;
        }
    }
}
