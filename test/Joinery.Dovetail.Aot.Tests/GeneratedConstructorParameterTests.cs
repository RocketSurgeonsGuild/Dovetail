using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;

// Exports is generated: a formatter that runs without the generator can neither resolve it nor suggest simplifying its name, so the
// fully qualified reference would flip between IDE0002 and a removed using on every automatic lint run.
#pragma warning disable IDE0002

namespace Joinery.Dovetail.Aot.Tests;

/// <summary>
///     An exported joint with constructor parameters is created by the code Dovetail's generator emits, and that code compiles in a
///     real build under the trimming and AOT analyzers (with IL2026, IL3050 and IL2091 as errors): the parameters are resolved from
///     the context builder's properties by type, and an optional one falls back to its default.
/// </summary>
public sealed class GeneratedConstructorParameterTests
{
    [Test]
    public async Task Exported_joint_is_created_from_the_values_set_on_the_context_builder()
    {
        var builder = DovetailContextBuilder
                     .Create(global::Joinery.Dovetail.Aot.Tests.Rivets.Exports.Joints(), new Dictionary<object, object>(), DovetailHostType.UnitTest, [])
                     .Set(new GeneratedGreetingOptions("hello"))
                     .Set(3);

        var joint = ( await builder.CreateAsync() ).Joints.OfType<GeneratedGreetingJoint>().Single();

        await joint.Greeting.Should().BeEqualTo("hello");
        await joint.Repeat.Should().BeEqualTo(3);
    }

    [Test]
    public async Task Exported_joint_falls_back_to_the_defaults_of_the_parameters_the_context_builder_has_no_value_for()
    {
        var builder = DovetailContextBuilder.Create(global::Joinery.Dovetail.Aot.Tests.Rivets.Exports.Joints(), new Dictionary<object, object>(), DovetailHostType.UnitTest, []);

        var joint = ( await builder.CreateAsync() ).Joints.OfType<GeneratedGreetingJoint>().Single();

        await joint.Greeting.Should().BeEqualTo("none");
        await joint.Repeat.Should().BeEqualTo(1);
    }
}

public sealed record GeneratedGreetingOptions(string Greeting);

/// <summary>
///     Every parameter is optional, so the joint can also be created without a context builder value.
/// </summary>
[DovetailExport]
public sealed class GeneratedGreetingJoint(GeneratedGreetingOptions? options = null, int repeat = 1) : IDovetailJoint
{
    public string Greeting { get; } = options?.Greeting ?? "none";

    public int Repeat { get; } = repeat;
}
