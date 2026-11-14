namespace Joinery.Dovetail.Sdk.Tests;

/// <summary>
/// For a test class without <see cref="TargetFrameworksAttribute" />: when <c>DOVETAIL_SDK_TESTS_TARGET_FRAMEWORK</c> splits the
/// run by target framework (CI runs each in its own job), its tests run only in the net11.0 run rather than once per run.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
internal sealed class SingleTargetFrameworkJobAttribute() : SkipAttribute("Runs in the net11.0 run of the SDK tests")
{
    public override Task<bool> ShouldSkip(TestRegisteredContext context) =>
        Task.FromResult(
            Environment.GetEnvironmentVariable("DOVETAIL_SDK_TESTS_TARGET_FRAMEWORK") is { Length: > 0 } only && only != "net11.0"
        );
}
