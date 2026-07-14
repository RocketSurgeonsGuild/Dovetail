using Dovetail.Infrastructure;
using Dovetail.Joints;
using FakeItEasy;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Rocket.Surgery.Extensions.Testing;

using Serilog.Events;



namespace Dovetail.Tests;

public class DovetailContextTests
    () : AutoFakeTest<TestRecord>(TestRecord.Create(LogEventLevel.Information))
{
    [Test]
    public async Task GetAStronglyTypedValue()
    {
        var builder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var container = await DovetailContext.FromAsync(builder);
        container.Set("abc");
        container.Get<string>().ShouldBe("abc");
    }

    [Test]
    public async Task SetAStronglyTypedValue()
    {
        var builder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var container = await DovetailContext.FromAsync(builder);
        container.Set("abc");
        container.Get<string>().ShouldBe("abc");
    }

    [Test]
    public async Task AddDovetails()
    {
        var contextBuilder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var convention = A.Fake<IServiceJoint>(z => z.Named("Dovetail"));
        contextBuilder.PrependJoint(convention);
        var conventions = await DovetailContext.FromAsync(contextBuilder);
        await new ServiceCollection().ApplyService(conventions);
        A.CallTo(() => convention.Register(conventions, A<IServiceCollection>._)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Setups()
    {
        var contextBuilder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var convention = A.Fake<ISetupJoint>();
        contextBuilder.PrependJoint(convention);

        var context = await DovetailContext.FromAsync(contextBuilder);
        A.CallTo(() => convention.Register(context)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Setups_With_Delegate()
    {
        var contextBuilder = DovetailContextBuilder.Create([], new DovetailDictionary(), []);
        var convention = A.Fake<SetupJoint>();
        contextBuilder.ConfigureSetup(convention);

        var context = await DovetailContext.FromAsync(contextBuilder);
        A.CallTo(() => convention(context)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task ConstructTheContainerAndRegisterWithCore_ServiceProvider()
    {
        var contextBuilder = DovetailContextBuilder
                            .Create([], new DovetailDictionary(), [])
                            .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = await DovetailContext.FromAsync(contextBuilder);
        var servicesCollection = new ServiceCollection()
                                      .AddSingleton(A.Fake<IAbc>())
                                      .AddSingleton(A.Fake<IAbc2>());

        await servicesCollection.ApplyService(context);

        var sp = servicesCollection.BuildServiceProvider();
        sp.GetService<IAbc>().ShouldNotBeNull();
        sp.GetService<IAbc2>().ShouldNotBeNull();
        sp.GetService<IAbc3>().ShouldBeNull();
        sp.GetService<IAbc4>().ShouldBeNull();
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

        collection.AddSingleton(A.Fake<IAbc>());
        collection.AddSingleton(A.Fake<IAbc2>());
        collection.AddSingleton(A.Fake<IAbc4>());

        var sp = collection.BuildServiceProvider();
        sp.GetService<IAbc>().ShouldNotBeNull();
        sp.GetService<IAbc2>().ShouldNotBeNull();
        sp.GetService<IAbc3>().ShouldBeNull();
        sp.GetService<IAbc4>().ShouldNotBeNull();
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
        collection.AddSingleton(A.Fake<IAbc3>());
        collection.AddSingleton(A.Fake<IAbc4>());

        var sp = collection.BuildServiceProvider();
        sp.GetService<IAbc>().ShouldBeNull();
        sp.GetService<IAbc2>().ShouldBeNull();
        sp.GetService<IAbc3>().ShouldNotBeNull();
        sp.GetService<IAbc4>().ShouldNotBeNull();
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
        items.GetService<IAbc>().ShouldNotBeNull();
        items.GetService<IAbc2>().ShouldNotBeNull();
        items.GetService<IAbc3>().ShouldBeNull();
        items.GetService<IAbc4>().ShouldBeNull();
    }

    [Test]
    public async Task ShouldConstructTheDovetailInjectingTheValues()
    {
        var data = A.Fake<IInjectData>();
        var builder = DovetailContextBuilder
                     .Create([], new DovetailDictionary(), [])
                     .AppendJoint<InjectableDovetail>()
                     .Set(data)
                     .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = await DovetailContext.FromAsync(builder);
        var collection = new ServiceCollection();
        await collection.ApplyService(context);
        collection.ShouldContain(z => z.ServiceType == typeof(IInjectData));
    }

    [Test]
    public async Task ShouldConstructTheDovetailInjectingTheValuesIfOptional()
    {
        AutoFake.Provide<IDictionary<object, object?>>(new DovetailDictionary());
        var data = A.Fake<IInjectData>();
        var builder = DovetailContextBuilder
                     .Create([], new DovetailDictionary(), [])
                     .AppendJoint<OptionalInjectableDovetail>()
                     .Set(data)
                     .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = ( await DovetailContext.FromAsync(builder) ).Set(data);
        var collection = new ServiceCollection();
        await collection.ApplyService(context);
        collection.ShouldContain(z => z.ServiceType == typeof(IInjectData));
    }

    [Test]
    public async Task ShouldFailToConstructTheDovetailInjectingTheValuesIfMissing()
    {
        var builder = DovetailContextBuilder
                     .Create([], new DovetailDictionary(), [])
                     .AppendJoint<InjectableDovetail>()
                     .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var a = () => DovetailContext.FromAsync(builder).AsTask();
        await a.ShouldThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task ShouldNotFailToConstructTheDovetailInjectingTheValuesIfOptional()
    {
        var builder = DovetailContextBuilder
                     .Create([], new DovetailDictionary(), [])
                     .AppendJoint<OptionalInjectableDovetail>()
                     .Set<IConfiguration>(new ConfigurationBuilder().Build());
        var context = await DovetailContext.FromAsync(builder);
        var a = () => new ServiceCollection().ApplyService(context).AsTask();
        await a.ShouldNotThrowAsync();
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

            services.AddSingleton(A.Fake<IAbc>());
            services.AddSingleton(A.Fake<IAbc2>());
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
