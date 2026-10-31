using Joinery.Dovetail.Infrastructure;

#pragma warning disable CA1034, CA1040

namespace Joinery.Dovetail.Tests;

/// <summary>
///     <see cref="DovetailJointMetadata" /> built from a factory is what the code generated for an exported joint with constructor
///     parameters returns: the joint is created from the properties of the context builder that holds the metadata, when it is first
///     needed, which is the same moment and the same source as for a joint added with <c>AppendJoint&lt;T&gt;()</c>.
/// </summary>
public class DovetailJointMetadataTests
{
    [Test]
    public void Does_Not_Create_The_Joint_Until_It_Is_Read_And_Creates_It_Once()
    {
        var created = 0;
        var metadata = new DovetailJointMetadata(
            _ =>
            {
                created++;
                return new FactoryJoint();
            },
            DovetailHostType.Undefined,
            DovetailCategory.Application
        );

        created.ShouldBe(0);
        metadata.Joint.ShouldBeSameAs(metadata.Joint);
        created.ShouldBe(1);
    }

    [Test]
    public async Task Creates_The_Joint_From_The_Properties_Of_The_Builder_It_Was_Given_To()
    {
        var metadata = Describe();
        var builder = DovetailContextBuilder.Create([metadata], new Dictionary<object, object> { [typeof(string)] = "from the properties" }, []);

        ( await builder.CreateAsync() ).Joints.OfType<FactoryJoint>().Single().Value.ShouldBe("from the properties");
    }

    [Test]
    public async Task Sees_What_Is_Set_On_The_Builder_After_It_Was_Given_The_Metadata()
    {
        var builder = DovetailContextBuilder.Create([Describe()], new Dictionary<object, object>(), []).Set("set later");

        ( await builder.CreateAsync() ).Joints.OfType<FactoryJoint>().Single().Value.ShouldBe("set later");
    }

    [Test]
    public async Task Is_Given_The_Properties_When_Appended_Or_Prepended()
    {
        var appended = Describe();
        var prepended = Describe();
        var builder = DovetailContextBuilder.Create([], new Dictionary<object, object>(), []).AppendJoint(appended).PrependJoint(prepended).Set("set later");

        await builder.CreateAsync();

        ( (FactoryJoint)appended.Joint ).Value.ShouldBe("set later");
        ( (FactoryJoint)prepended.Joint ).Value.ShouldBe("set later");
    }

    [Test]
    public void Resolves_Against_Empty_Properties_When_Read_Outside_A_Builder() => ( (FactoryJoint)Describe().Joint ).Value.ShouldBe("default");

    [Test]
    public void Keeps_The_Values_The_Joint_Was_Created_With_When_It_Is_Given_To_Another_Builder()
    {
        var metadata = Describe();
        DovetailContextBuilder.Create([metadata], new Dictionary<object, object> { [typeof(string)] = "first" }, []);
        ( (FactoryJoint)metadata.Joint ).Value.ShouldBe("first");

        DovetailContextBuilder.Create([metadata], new Dictionary<object, object> { [typeof(string)] = "second" }, []);

        ( (FactoryJoint)metadata.Joint ).Value.ShouldBe("first");
    }

    [Test]
    public void Fails_Each_Time_It_Is_Read_When_The_Factory_Throws()
    {
        var metadata = new DovetailJointMetadata(_ => throw new InvalidOperationException("nothing to create it from"), DovetailHostType.Undefined, DovetailCategory.Application);

        Should.Throw<InvalidOperationException>(() => metadata.Joint);
        Should.Throw<InvalidOperationException>(() => metadata.Joint);
    }

    [Test]
    public void Requires_A_Factory() =>
        Should.Throw<ArgumentNullException>(() => new DovetailJointMetadata((Func<IServiceProvider, IDovetailJoint>)null!, DovetailHostType.Undefined, DovetailCategory.Application));

    private static DovetailJointMetadata Describe() => new(
        services => new FactoryJoint(services.GetService(typeof(string)) as string ?? "default"),
        DovetailHostType.Undefined,
        DovetailCategory.Application
    );

    public sealed class FactoryJoint(string value = "default") : IDovetailJoint
    {
        public string Value { get; } = value;
    }
}
