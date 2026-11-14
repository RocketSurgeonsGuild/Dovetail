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
    public async Task Does_Not_Create_The_Joint_Until_It_Is_Read_And_Creates_It_Once()
    {
        var created = 0;
        var metadata = DovetailJointMetadata.FromFactory(
            _ =>
            {
                created++;
                return new FactoryJoint();
            },
            DovetailHostType.Undefined,
            DovetailCategory.Application
        );

        await created.Should().BeEqualTo(0);
        await metadata.Joint.Should().BeSameReferenceAs(metadata.Joint);
        await created.Should().BeEqualTo(1);
    }

    [Test]
    public async Task Creates_The_Joint_From_The_Properties_Of_The_Builder_It_Was_Given_To()
    {
        var metadata = Describe();
        var builder = DovetailContextBuilder.Create([metadata], new Dictionary<object, object> { [typeof(string)] = "from the properties" }, []);

        await ( await builder.CreateAsync() ).Joints.OfType<FactoryJoint>().Single().Value.Should().BeEqualTo("from the properties");
    }

    [Test]
    public async Task Sees_What_Is_Set_On_The_Builder_After_It_Was_Given_The_Metadata()
    {
        var builder = DovetailContextBuilder.Create([Describe()], new Dictionary<object, object>(), []).Set("set later");

        await ( await builder.CreateAsync() ).Joints.OfType<FactoryJoint>().Single().Value.Should().BeEqualTo("set later");
    }

    [Test]
    public async Task Is_Given_The_Properties_When_Appended_Or_Prepended()
    {
        var appended = Describe();
        var prepended = Describe();
        var builder = DovetailContextBuilder.Create([], new Dictionary<object, object>(), []).AppendJoint(appended).PrependJoint(prepended).Set("set later");

        await builder.CreateAsync();

        await ( (FactoryJoint)appended.Joint ).Value.Should().BeEqualTo("set later");
        await ( (FactoryJoint)prepended.Joint ).Value.Should().BeEqualTo("set later");
    }

    [Test]
    public async Task Resolves_Against_Empty_Properties_When_Read_Outside_A_Builder() => await ( (FactoryJoint)Describe().Joint ).Value.Should().BeEqualTo("default");

    [Test]
    public async Task Keeps_The_Values_The_Joint_Was_Created_With_When_It_Is_Given_To_Another_Builder()
    {
        var metadata = Describe();
        DovetailContextBuilder.Create([metadata], new Dictionary<object, object> { [typeof(string)] = "first" }, []);
        await ( (FactoryJoint)metadata.Joint ).Value.Should().BeEqualTo("first");

        DovetailContextBuilder.Create([metadata], new Dictionary<object, object> { [typeof(string)] = "second" }, []);

        await ( (FactoryJoint)metadata.Joint ).Value.Should().BeEqualTo("first");
    }

    [Test]
    public async Task Fails_Each_Time_It_Is_Read_When_The_Factory_Throws()
    {
        var metadata = DovetailJointMetadata.FromFactory(_ => throw new InvalidOperationException("nothing to create it from"), DovetailHostType.Undefined, DovetailCategory.Application);

        await ( (Func<object?>)( () => metadata.Joint ) ).Should().Throw<InvalidOperationException>();
        await ( (Func<object?>)( () => metadata.Joint ) ).Should().Throw<InvalidOperationException>();
    }

    [Test]
    public async Task Requires_A_Factory() =>
        await ( (Action)( () => DovetailJointMetadata.FromFactory(null!, DovetailHostType.Undefined, DovetailCategory.Application) ) ).Should().Throw<ArgumentNullException>();

    private static DovetailJointMetadata Describe() => DovetailJointMetadata.FromFactory(
        services => new FactoryJoint(services.GetService(typeof(string)) as string ?? "default"),
        DovetailHostType.Undefined,
        DovetailCategory.Application
    );

    public sealed class FactoryJoint(string value = "default") : IDovetailJoint
    {
        public string Value { get; } = value;
    }
}
