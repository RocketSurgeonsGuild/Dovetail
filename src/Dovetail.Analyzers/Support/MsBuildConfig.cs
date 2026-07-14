namespace Dovetail.Support;

internal record MsBuildConfig
(
    bool DovetailMetadata,
    bool AssignExternal,
    bool IsTestProject,
    string RootNamespace,
    string HostType,
    string Category,
    DovetailConfigurationData ExportConfiguration,
    DovetailConfigurationData ImportConfiguration
)
{
    public DovetailConfigurationData ImportConfiguration { get; init; } = ImportConfiguration with { Namespace = ImportConfiguration.Namespace is "" ? RootNamespace : ImportConfiguration.Namespace };
    public DovetailConfigurationData ExportConfiguration { get; init; } = ExportConfiguration with { Namespace = ExportConfiguration.Namespace is "" ? RootNamespace : ExportConfiguration.Namespace };
}
