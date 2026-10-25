using Dovetail.Attributes;
using Dovetail.Configuration.Aot.Tests.Rivets;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dovetail.Configuration.Aot.Tests;

/// <summary>
///     The joint and binder Dovetail's generator emits for these types compile under the trimming and AOT analyzers (with IL2026,
///     IL3050 and IL2091 as errors), and bind through the options pipeline.
/// </summary>
public sealed class GeneratedJointTests
{
    [Test]
    public async Task Generated_joint_binds_without_reflection()
    {
        var services = new ServiceCollection()
           .AddSingleton<IConfiguration>(
                new ConfigurationBuilder()
                   .AddInMemoryCollection(
                        [
                            new("Mutable:Name", "m"),
                            new("Mutable:Ports:0", "1"),
                            new("Mutable:Limits:a", "2"),
                            new("Mutable:Child:Value", "c"),
                            new("Positional:Host", "example.org"),
                            new("Initialized:Name", "i"),
                            new("Initialized:Size", "3"),
                        ]
                    )
                   .Build()
            );
        foreach (var joint in Exports.Joints().Select(z => z.Joint).OfType<IServiceJoint>())
        {
            joint.Register(null!, services);
        }

        await using var provider = services.BuildServiceProvider();

        var mutable = provider.GetRequiredService<IOptions<GeneratedMutableOptions>>().Value;
        mutable.Name.ShouldBe("m");
        mutable.Ports.ShouldBe([1]);
        mutable.Limits.ShouldBe(new Dictionary<string, int> { ["a"] = 2 });
        mutable.Child.Value.ShouldBe("c");
        provider.GetRequiredService<IOptions<GeneratedPositionalOptions>>().Value.ShouldBe(new GeneratedPositionalOptions("example.org"));
        var initialized = provider.GetRequiredService<IOptions<GeneratedInitializedOptions>>().Value;
        initialized.Name.ShouldBe("i");
        initialized.Size.ShouldBe(3);
    }
}

public class GeneratedChild
{
    public string? Value { get; set; }
}

[DovetailConfiguration("Mutable")]
public class GeneratedMutableOptions
{
    public string? Name { get; set; }
    public List<int> Ports { get; set; } = [];
    public Dictionary<string, int> Limits { get; set; } = [];
    public GeneratedChild Child { get; set; } = new();
}

[DovetailConfiguration("Positional")]
public record GeneratedPositionalOptions(string Host, int Port = 80);

[DovetailConfiguration("Initialized")]
public class GeneratedInitializedOptions
{
    public string Name { get; init; } = "none";
    public required int Size { get; set; }
}
