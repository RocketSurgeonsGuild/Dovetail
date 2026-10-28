namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Runs every test in the class once per target framework the repo supports. Snapshots scrub the target framework,
/// so both runs share one verified file.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
internal sealed class TargetFrameworksAttribute : DataSourceGeneratorAttribute<string>
{
    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => "net10.0";
        yield return () => "net11.0";
    }
}
