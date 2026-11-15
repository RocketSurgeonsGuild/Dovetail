using Joinery.Dovetail.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Joinery.Dovetail.Configuration.Aot.Tests;

/// <summary>
///     The registration method and binder generated for these types (<c>AddOptionaryConfigurationOptions</c>) compile under the
///     trimming and AOT analyzers (with IL2026, IL3050 and IL2091 as errors), and bind through the options pipeline.
/// </summary>
public sealed class GeneratedRegistrationTests
{
    [Test]
    public async Task Generated_registration_binds_without_reflection()
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
        services.AddOptionaryConfigurationOptions();

        await using var provider = services.BuildServiceProvider();

        var mutable = provider.GetRequiredService<IOptions<GeneratedMutableOptions>>().Value;
        await mutable.Name.Should().BeEqualTo("m");
        await ( await mutable.Ports.Should().HaveSingleItem() ).Should().BeEqualTo(1);
        await mutable.Limits.Should().HaveCount(1);
        await mutable.Limits.Should().ContainKeyWithValue("a", 2);
        await mutable.Child.Value.Should().BeEqualTo("c");
        await provider.GetRequiredService<IOptions<GeneratedPositionalOptions>>().Value.Should().BeEqualTo(new GeneratedPositionalOptions("example.org"));
        var initialized = provider.GetRequiredService<IOptions<GeneratedInitializedOptions>>().Value;
        await initialized.Name.Should().BeEqualTo("i");
        await initialized.Size.Should().BeEqualTo(3);
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
