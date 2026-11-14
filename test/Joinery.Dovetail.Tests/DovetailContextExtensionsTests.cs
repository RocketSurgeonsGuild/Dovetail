
#pragma warning disable CA1040, CA1034

namespace Joinery.Dovetail.Tests;

public class DovetailContextExtensionsTests
{
    [Test]
    public async Task Should_Get_Item_By_Type()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType = IMyType.Mock().Object;
        context.Set(myType);

        await context.Get<IMyType>().Should().BeSameReferenceAs(myType);
    }

    [Test]
    public async Task Should_Get_Item_By_Name()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType = IMyType.Mock().Object;
        context.Set("value", myType);
        await context.Get<IMyType>("value").Should().BeSameReferenceAs(myType);
    }

    [Test]
    public async Task Should_Require_Item_By_Type()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType = IMyType.Mock().Object;
        context.Set(myType);

        await context.Require<IMyType>().Should().BeSameReferenceAs(myType);
    }

    [Test]
    public async Task Should_Require_Item_By_Name()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType = IMyType.Mock().Object;
        context.Set("value", myType);
        await context.Require<IMyType>("value").Should().BeSameReferenceAs(myType);
    }

    [Test]
    public async Task Should_Fail_To_Require_Item_By_Type()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType = IMyType.Mock().Object;
        Action a = () => context.Require<IMyType>();
        await a.Should().Throw<KeyNotFoundException>();
    }

    [Test]
    public async Task Should_Fail_To_Require_Item_By_Name()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType = IMyType.Mock().Object;
        Action a = () => context.Require<IMyType>("value");
        await a.Should().Throw<KeyNotFoundException>();
    }

    [Test]
    public async Task Should_Get_IsUnitTestHost()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        context.Set(DovetailHostType.UnitTest);

        await context.IsUnitTestHost().Should().BeTrue();
    }

    [Test]
    public async Task Should_Not_IsUnitTestHost()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        context.Set(DovetailHostType.Live);

        await context.IsUnitTestHost().Should().BeFalse();
    }

    [Test]
    public async Task Should_GetOrAdd_Item_By_Type_Get()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType1 = IMyType.Mock().Object;
        var myType2 = IMyType.Mock().Object;
        context.Set(myType1);
        await context.GetOrAdd(() => myType2).Should().NotBeSameReferenceAs(myType2);
    }

    [Test]
    public async Task Should_GetOrAdd_Item_By_Name_Get()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType1 = IMyType.Mock().Object;
        var myType2 = IMyType.Mock().Object;
        context.Set("value", myType1);
        await context.GetOrAdd("value", () => myType2).Should().NotBeSameReferenceAs(myType2);
    }

    [Test]
    public async Task Should_GetOrAdd_Item_By_Type_Add()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType2 = IMyType.Mock().Object;
        await context.GetOrAdd(() => myType2).Should().BeSameReferenceAs(myType2);
    }

    [Test]
    public async Task Should_GetOrAdd_Item_By_Name_Add()
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        var myType2 = IMyType.Mock().Object;
        await context.GetOrAdd("value", () => myType2).Should().BeSameReferenceAs(myType2);
    }

    [Test]
    [Arguments(DovetailHostType.Undefined)]
    [Arguments(DovetailHostType.Live)]
    [Arguments(DovetailHostType.UnitTest)]
    public async Task Should_Get_HostType(DovetailHostType hostType)
    {
        var context = await DovetailContext.FromAsync(DovetailContextBuilder.Create([], new Dictionary<object, object?>(), []));
        context.Set(hostType);
        await context.GetHostType().Should().BeEqualTo(hostType);
    }

    public interface IMyType;
}
