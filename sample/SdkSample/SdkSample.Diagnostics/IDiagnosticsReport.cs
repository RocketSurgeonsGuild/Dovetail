namespace SdkSample.Diagnostics;

public interface IDiagnosticsReport
{
    /// <summary>
    ///     Describes the diagnostics settings, including the host settings this library configures.
    /// </summary>
    string Describe();
}
