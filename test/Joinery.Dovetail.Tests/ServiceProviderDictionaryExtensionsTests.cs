using Joinery.Dovetail.Infrastructure;

#pragma warning disable CA1034, CA1040

namespace Joinery.Dovetail.Tests;

public class ServiceProviderDictionaryExtensionsTests
{
    [Test]
    public async Task Should_Get_Item_By_Type()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = IMyType.Mock().Object;
        context[typeof(IMyType)] = myType;

        await context.Get<IMyType>().Should().BeSameReferenceAs(myType);
    }

    [Test]
    public async Task Should_Get_Item_By_Name()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = IMyType.Mock().Object;
        context["value"] = myType;
        await context.Get<IMyType>("value").Should().BeSameReferenceAs(myType);
    }

    [Test]
    public async Task Should_Require_Item_By_Type()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = IMyType.Mock().Object;
        context[typeof(IMyType)] = myType;

        await context.Require<IMyType>().Should().BeSameReferenceAs(myType);
    }

    [Test]
    public async Task Should_Require_Item_By_Name()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = IMyType.Mock().Object;
        context["value"] = myType;

        await context.Require<IMyType>("value").Should().BeSameReferenceAs(myType);
    }

    [Test]
    public async Task Should_Fail_To_Require_Item_By_Type()
    {
        IDovetailDictionary context = new DovetailDictionary();
        Action a = () => context.Require<IMyType>();
        await a.Should().Throw<KeyNotFoundException>();
    }

    [Test]
    public async Task Should_Fail_To_Require_Item_By_Name()
    {
        IDovetailDictionary context = new DovetailDictionary();
        Action a = () => context.Require<IMyType>("value");
        await a.Should().Throw<KeyNotFoundException>();
    }

    [Test]
    public void Should_Set_Item_By_Type()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = IMyType.Mock().Object;

        context.Set(myType);
    }

    [Test]
    public void Should_Set_Item_By_Name()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = IMyType.Mock().Object;

        context.Set("value", myType);
    }

    [Test]
    public async Task Should_GetOrAdd_Item_By_Type_Get()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType1 = IMyType.Mock().Object;
        var myType2 = IMyType.Mock().Object;
        await context.GetOrAdd(() => myType2).Should().BeSameReferenceAs(myType2);
    }

    [Test]
    public async Task Should_GetOrAdd_Item_By_Name_Get()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType1 = IMyType.Mock().Object;
        var myType2 = IMyType.Mock().Object;
        context["value"] = myType1;
        await context.GetOrAdd("value", () => myType2).Should().NotBeSameReferenceAs(myType2);
    }

    [Test]
    public async Task Should_GetOrAdd_Item_By_Type_Add()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType2 = IMyType.Mock().Object;
        await context.GetOrAdd(() => myType2).Should().BeSameReferenceAs(myType2);
    }

    [Test]
    public async Task Should_GetOrAdd_Item_By_Name_Add()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType2 = IMyType.Mock().Object;
        await context.GetOrAdd("value", () => myType2).Should().BeSameReferenceAs(myType2);
    }

    public interface IMyType;
}
