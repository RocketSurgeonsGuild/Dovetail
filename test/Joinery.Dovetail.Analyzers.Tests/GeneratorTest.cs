using Rocket.Surgery.Extensions.Testing;
using Rocket.Surgery.Extensions.Testing.SourceGenerators;

namespace Joinery.Dovetail.Analyzers.Tests;

public abstract class GeneratorTest() : LoggerTest<TestRecord>(TestRecord.Create())
{
    // Raw builder without the default Dovetail configuration. Used to build the shared dependency
    // compilations (which supply their own configuration) and by configuration-override tests.
    protected GeneratorTestContextBuilder Builder { get; } = GeneratorTestContextBuilder
                                                                         .Create()
                                                                         .AddCommonReferences()
                                                                         .AddCommonGenerators()
                                                                         .AddDovetailConfiguration("", "");

    /// <summary>
    ///     Runs the configuration generator alone, without the joint generator: what moves to Optionary is tested without the
    ///     exports the joint generator writes around it.
    /// </summary>
    protected GeneratorTestContextBuilder ConfigurationBuilder { get; } = GeneratorTestContextBuilder
                                                                         .Create()
                                                                         .AddCommonReferences()
                                                                         .WithGenerator<Support.Configuration.ConfigurationGenerator>();

    protected GeneratorTestContextBuilder WithSharedDeps() => Builder.AddSharedDeps(TestContext.CancellationToken);

    protected GeneratorTestContextBuilder WithGenericSharedDeps() => Builder.AddSharedGenericDeps(TestContext.CancellationToken);

    protected GeneratorTestContextBuilder Configure(Func<GeneratorTestContextBuilder, GeneratorTestContextBuilder> builder) => builder(Builder);
}
