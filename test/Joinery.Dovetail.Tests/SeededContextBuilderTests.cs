using Joinery.Dovetail.Joints;

#pragma warning disable CA1040, CA1034

namespace Joinery.Dovetail.Tests;

/// <summary>
///     When the application references Specular, the generated <c>Imports</c> ends its context builder with
///     <c>.AddIfMissing&lt;ISpecularProvider&gt;(SpecularProvider.Instance)</c> (see the generator's SpecularProviderSeedingTests).
///     These are the behaviours that line relies on, with a stand-in for the provider: a Joint reads the seeded value, and one the
///     application set itself is never overwritten, whether it set it on the builder or passed it in with the properties.
/// </summary>
public class SeededContextBuilderTests
{
    [Test]
    public async Task A_Joint_Reads_The_Seeded_Provider_When_The_Application_Set_Nothing()
    {
        var joint = new ReadsTheProvider();

        await Seed(DovetailContextBuilder.Create([], new Dictionary<object, object>(), [])).AppendJoint(joint).CreateAsync();

        joint.Seen.ShouldBeSameAs(Generated);
    }

    [Test]
    public async Task A_Provider_Set_On_The_Builder_Wins()
    {
        var explicitProvider = new SeededProvider("explicit");
        var joint = new ReadsTheProvider();

        await Seed(DovetailContextBuilder.Create([], new Dictionary<object, object>(), []))
             .Set<ISeededProvider>(explicitProvider)
             .AppendJoint(joint)
             .CreateAsync();

        joint.Seen.ShouldBeSameAs(explicitProvider);
    }

    [Test]
    public async Task A_Provider_Passed_In_With_The_Properties_Wins()
    {
        var explicitProvider = new SeededProvider("explicit");
        var joint = new ReadsTheProvider();

        await Seed(DovetailContextBuilder.Create([], new Dictionary<object, object> { [typeof(ISeededProvider)] = explicitProvider, }, []))
             .AppendJoint(joint)
             .CreateAsync();

        joint.Seen.ShouldBeSameAs(explicitProvider);
    }

    [Test]
    public async Task A_Provider_Set_By_A_Joint_Replaces_The_Seeded_One_For_The_Joints_After_It()
    {
        var explicitProvider = new SeededProvider("explicit");
        var joint = new ReadsTheProvider();

        await Seed(DovetailContextBuilder.Create([], new Dictionary<object, object>(), []))
             .AppendJoint(new SetsTheProvider(explicitProvider), joint)
             .CreateAsync();

        joint.Seen.ShouldBeSameAs(explicitProvider);
    }

    private static readonly SeededProvider Generated = new("generated");

    /// <summary>The line the generator appends to the builder it creates.</summary>
    private static DovetailContextBuilder Seed(DovetailContextBuilder builder) => builder.AddIfMissing<ISeededProvider>(Generated);

    public interface ISeededProvider
    {
        string Name { get; }
    }

    public sealed class SeededProvider(string name) : ISeededProvider
    {
        public string Name { get; } = name;
    }

    private sealed class ReadsTheProvider : ISetupJoint
    {
        public ISeededProvider? Seen { get; private set; }

        public void Register(IDovetailContext context) => Seen = context.Get<ISeededProvider>();
    }

    private sealed class SetsTheProvider(ISeededProvider provider) : ISetupJoint
    {
        public void Register(IDovetailContext context) => context.Set(provider);
    }
}
