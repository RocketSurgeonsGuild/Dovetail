using Dovetail.Infrastructure;
using FakeItEasy;

#pragma warning disable CA1034, CA1040

namespace Dovetail.Tests;

public class ServiceProviderDictionaryExtensionsTests
{
    [Test]
    public void Should_Get_Item_By_Type()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = A.Fake<IMyType>();
        context[typeof(IMyType)] = myType;

        context.Get<IMyType>().ShouldBeSameAs(myType);
    }

    [Test]
    public void Should_Get_Item_By_Name()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = A.Fake<IMyType>();
        context["value"] = myType;
        context.Get<IMyType>("value").ShouldBeSameAs(myType);
    }

    [Test]
    public void Should_Require_Item_By_Type()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = A.Fake<IMyType>();
        context[typeof(IMyType)] = myType;

        context.Require<IMyType>().ShouldBeSameAs(myType);
    }

    [Test]
    public void Should_Require_Item_By_Name()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = A.Fake<IMyType>();
        context["value"] = myType;

        context.Require<IMyType>("value").ShouldBeSameAs(myType);
    }

    [Test]
    public void Should_Fail_To_Require_Item_By_Type()
    {
        IDovetailDictionary context = new DovetailDictionary();
        Action a = () => context.Require<IMyType>();
        a.ShouldThrow<KeyNotFoundException>();
    }

    [Test]
    public void Should_Fail_To_Require_Item_By_Name()
    {
        IDovetailDictionary context = new DovetailDictionary();
        Action a = () => context.Require<IMyType>("value");
        a.ShouldThrow<KeyNotFoundException>();
    }

    [Test]
    public void Should_Set_Item_By_Type()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = A.Fake<IMyType>();

        context.Set(myType);
    }

    [Test]
    public void Should_Set_Item_By_Name()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType = A.Fake<IMyType>();

        context.Set("value", myType);
    }

    [Test]
    public void Should_GetOrAdd_Item_By_Type_Get()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType1 = A.Fake<IMyType>();
        var myType2 = A.Fake<IMyType>();
        context.GetOrAdd(() => myType2).ShouldBeSameAs(myType2);
    }

    [Test]
    public void Should_GetOrAdd_Item_By_Name_Get()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType1 = A.Fake<IMyType>();
        var myType2 = A.Fake<IMyType>();
        context["value"] = myType1;
        context.GetOrAdd("value", () => myType2).ShouldNotBeSameAs(myType2);
    }

    [Test]
    public void Should_GetOrAdd_Item_By_Type_Add()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType2 = A.Fake<IMyType>();
        context.GetOrAdd(() => myType2).ShouldBeSameAs(myType2);
    }

    [Test]
    public void Should_GetOrAdd_Item_By_Name_Add()
    {
        IDovetailDictionary context = new DovetailDictionary();
        var myType2 = A.Fake<IMyType>();
        context.GetOrAdd("value", () => myType2).ShouldBeSameAs(myType2);
    }

    public interface IMyType;
}
