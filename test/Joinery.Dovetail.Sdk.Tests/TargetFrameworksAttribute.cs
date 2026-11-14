namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// Runs every test in the class once per target framework the repo supports. Snapshots scrub the target framework,
/// so both runs share one verified file.
/// </summary>
/// <remarks>
/// <c>DOVETAIL_SDK_TESTS_TARGET_FRAMEWORK</c> narrows the run to one of them: CI runs each in its own job.
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
internal sealed class TargetFrameworksAttribute : DataSourceGeneratorAttribute<string>
{
    private static readonly string[] TargetFrameworks = ["net10.0", "net11.0"];

    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        var only = Environment.GetEnvironmentVariable("DOVETAIL_SDK_TESTS_TARGET_FRAMEWORK");
        if (!string.IsNullOrEmpty(only) && !TargetFrameworks.Contains(only, StringComparer.Ordinal))
        {
            throw new InvalidOperationException(
                $"DOVETAIL_SDK_TESTS_TARGET_FRAMEWORK is '{only}', which isn't one of {string.Join(", ", TargetFrameworks)}."
            );
        }

        foreach (var targetFramework in TargetFrameworks)
        {
            if (string.IsNullOrEmpty(only) || targetFramework == only)
            {
                yield return () => targetFramework;
            }
        }
    }
}
