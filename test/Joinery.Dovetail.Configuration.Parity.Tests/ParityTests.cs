using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Joinery.Dovetail.Configuration.Parity.Tests;

/// <summary>
///     Dovetail's generated binder against Microsoft's configuration binding source generator, the oracle: on net10.0 both follow
///     version 10 of its rules, on net11.0 version 11. Where Dovetail differs on purpose (see
///     <c>plans/configuration-json-schema-p3.md</c>), the reflection binder or the documented behaviour is the expectation instead.
/// </summary>
public class ParityTests
{
    public static IEnumerable<Func<ParityCase>> Cases() => ParityCases.All().Select(z => (Func<ParityCase>)( () => z ));

    [Test]
    [MethodDataSource(nameof(Cases))]
    public async Task Binds_like_Microsofts_generator(ParityCase parity)
    {
        var configuration = parity.Build();

        var microsoft = Render.Outcome(() => parity.Microsoft(configuration));
        var dovetail = Render.Outcome(() => parity.Dovetail(configuration));

        dovetail.ShouldBe(microsoft);
        await Task.CompletedTask;
    }

    [Test]
    public async Task Rebinds_when_configuration_reloads()
    {
        var configuration = Build(("Reload:Name", "first"), ("Reload:Count", "1"));
        await using var provider = Binders.DovetailServices(configuration).BuildServiceProvider();
        var monitor = provider.GetRequiredService<IOptionsMonitor<ReloadOptions>>();
        monitor.CurrentValue.Name.ShouldBe("first");

        var memory = (MemoryConfigurationProvider)configuration.Providers.Single();
        memory.Set("Reload:Name", "second");
        memory.Set("Reload:Count", "2");
        configuration.Reload();

        monitor.CurrentValue.Name.ShouldBe("second");
        monitor.CurrentValue.Count.ShouldBe(2);
        provider.GetRequiredService<IOptionsSnapshot<ReloadOptions>>().Value.Name.ShouldBe("second");
    }

    [Test]
    public async Task Creates_constructor_types_without_values_and_reports_missing_parameters()
    {
        // Get<T>() returns null for a section without values; IOptions<T>.Value can't, so Dovetail calls the constructor and a
        // missing parameter without a default throws, as it does for a section that has other values.
        var configuration = Build(("Other", "x"));

        var dovetail = Render.Outcome(() => Binders.Dovetail<PosRecord>(configuration));
        var microsoftWithOtherValues = Render.Outcome(() => Build(("S:Unrelated", "x")).GetSection("S").Get<PosRecord>());

        dovetail.ShouldBe(microsoftWithOtherValues);
        dovetail.ShouldContain("has no matching config");
        await Task.CompletedTask;
    }

    [Test]
    [Arguments("present", true)]
    [Arguments("missing", false)]
    public async Task Sets_init_only_members_like_the_reflection_binder(string label, bool present)
    {
        var configuration = present ? Build(("S:Name", "n"), ("S:Port", "80"), ("S:Items:0", "x")) : Build(("S:Other", "x"));

        var reflection = Render.Outcome(() => Binders.Reflection<InitOnlyClass>(configuration.GetSection("S")));
        var dovetail = Render.Outcome(() => Binders.Dovetail<InitOnlyClass>(configuration));

        // The reflection binder appends to the initial list; Dovetail binds into it too, so both keep "init" first.
        dovetail.ShouldBe(reflection, label);
        await Task.CompletedTask;
    }

    [Test]
    [Arguments("present", true)]
    [Arguments("missing", false)]
    public async Task Creates_types_with_required_members_like_the_reflection_binder(string label, bool present)
    {
        var configuration = present ? Build(("S:Name", "n"), ("S:Port", "80")) : Build(("S:Port", "80"));

        var reflection = Render.Outcome(() => Binders.Reflection<RequiredClass>(configuration.GetSection("S")));
        var dovetail = Render.Outcome(() => Binders.Dovetail<RequiredClass>(configuration));

        dovetail.ShouldBe(reflection, label);
        await Task.CompletedTask;
    }

    [Test]
    public async Task Sets_init_only_members_of_collection_elements()
    {
        var configuration = Build(("S:InitOnlyList:0:Name", "a"), ("S:InitOnlyList:0:Port", "2"));

        var reflection = Render.Outcome(() => Binders.Reflection<CollTarget>(configuration.GetSection("S"))!.InitOnlyList);
        var dovetail = Render.Outcome(() => Binders.Dovetail<CollTarget>(configuration).InitOnlyList);

        dovetail.ShouldBe(reflection);
        await Task.CompletedTask;
    }

    private static IConfigurationRoot Build(params (string Key, string? Value)[] pairs) =>
        new ConfigurationBuilder().AddInMemoryCollection(pairs.Select(z => KeyValuePair.Create(z.Key, z.Value))).Build();
}
