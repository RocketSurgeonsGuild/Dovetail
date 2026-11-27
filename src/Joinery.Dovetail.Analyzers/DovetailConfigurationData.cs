using Microsoft.CodeAnalysis;

namespace Joinery.Dovetail;

internal record DovetailConfigurationData(string Property, string Namespace, string ClassName, string MethodName) : IComparable<DovetailConfigurationData>, IComparable
{
    public static IncrementalValueProvider<DovetailConfigurationData> Read(
        IncrementalGeneratorInitializationContext context,
        string propertyPrefix
    )
    {
        var prefix = $"Dovetail{propertyPrefix}";
        return context.AnalyzerConfigOptionsProvider.Select((config, _) => new DovetailConfigurationData(
                                                                propertyPrefix,
                                                                config.GlobalOptions.GetBuildProperty(prefix + nameof(Namespace), s => s) ?? "##??NOT DEFINED??##",
                                                                config.GlobalOptions.GetBuildProperty(prefix + nameof(ClassName), s => s) ?? "##??NOT DEFINED??##",
                                                                config.GlobalOptions.GetBuildProperty(prefix + nameof(MethodName), s => s) ?? "##??NOT DEFINED??##"
                                                            )
        );
    }

    /// <summary>
    ///     The export configuration an assembly advertises with its <c>Dovetail.Export.*</c> assembly metadata, or
    ///     <see langword="null" /> when it advertises none. This runs for every reference on every compilation, so it walks the
    ///     attributes once and allocates nothing for an assembly without the metadata, which is nearly all of them.
    /// </summary>
    public static DovetailConfigurationData? FromExportMetadata(IAssemblySymbol assemblySymbol)
    {
        var found = false;
        // AssemblyMetadata allows repeated keys (Dovetail's own configuration schema fragments repeat theirs); the first wins.
        TypedConstant? namespaceValue = null, classNameValue = null, methodNameValue = null;
        foreach (var attribute in assemblySymbol.GetAttributes())
        {
            if (attribute is not { AttributeClass.MetadataName: "AssemblyMetadataAttribute", ConstructorArguments: [{ Value: string { Length: > 0 } key }, var value] })
                continue;
            if (!key.StartsWith(ExportMetadataPrefix, StringComparison.OrdinalIgnoreCase)) continue;

            found = true;
            if (string.Equals(key, ExportNamespaceKey, StringComparison.Ordinal)) namespaceValue ??= value;
            else if (string.Equals(key, ExportClassNameKey, StringComparison.Ordinal)) classNameValue ??= value;
            else if (string.Equals(key, ExportMethodNameKey, StringComparison.Ordinal)) methodNameValue ??= value;
        }

        return found
            ? new("Export", ValueOrUndefined(namespaceValue), ValueOrUndefined(classNameValue), ValueOrUndefined(methodNameValue))
            : null;

        static string ValueOrUndefined(TypedConstant? value) => value is { Value: string text } ? text : NotDefined;
    }

    private const string NotDefined = "##??NOT DEFINED??##";
    private const string ExportMetadataPrefix = "Dovetail.Export.";
    private const string ExportNamespaceKey = ExportMetadataPrefix + nameof(Namespace);
    private const string ExportClassNameKey = ExportMetadataPrefix + nameof(ClassName);
    private const string ExportMethodNameKey = ExportMetadataPrefix + nameof(MethodName);

    /// <summary>Writes the <c>Dovetail.{Property}.*</c> assembly metadata that tells referencing assemblies about this configuration.</summary>
    public SourceWriter WriteAttributes(SourceWriter writer) =>
        writer
           .Line(Helpers.AssemblyMetadataAttribute($"Dovetail.{Property}.{nameof(Property)}", Property))
           .Line(Helpers.AssemblyMetadataAttribute($"Dovetail.{Property}.{nameof(Namespace)}", Namespace))
           .Line(Helpers.AssemblyMetadataAttribute($"Dovetail.{Property}.{nameof(ClassName)}", ClassName))
           .Line(Helpers.AssemblyMetadataAttribute($"Dovetail.{Property}.{nameof(MethodName)}", MethodName));

    public override string ToString() => $"{( Namespace is { Length: > 0 } ? $"{Namespace}." : "" )}{ClassName}.{MethodName}";

    public int CompareTo(DovetailConfigurationData? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var propertyComparison = string.Compare(Property, other.Property, StringComparison.Ordinal);
        if (propertyComparison != 0) return propertyComparison;
        var namespaceComparison = string.Compare(Namespace, other.Namespace, StringComparison.Ordinal);
        if (namespaceComparison != 0) return namespaceComparison;
        var classNameComparison = string.Compare(ClassName, other.ClassName, StringComparison.Ordinal);
        return classNameComparison != 0 ? classNameComparison : string.Compare(MethodName, other.MethodName, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        return obj is null
            ? 1
            : ReferenceEquals(this, obj)
            ? 0
            : obj is DovetailConfigurationData other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(DovetailConfigurationData)}");
    }

    public static bool operator <(DovetailConfigurationData? left, DovetailConfigurationData? right) => Comparer<DovetailConfigurationData>.Default.Compare(left!, right!) < 0;

    public static bool operator >(DovetailConfigurationData? left, DovetailConfigurationData? right) => Comparer<DovetailConfigurationData>.Default.Compare(left!, right!) > 0;

    public static bool operator <=(DovetailConfigurationData? left, DovetailConfigurationData? right) => Comparer<DovetailConfigurationData>.Default.Compare(left!, right!) <= 0;

    public static bool operator >=(DovetailConfigurationData? left, DovetailConfigurationData? right) => Comparer<DovetailConfigurationData>.Default.Compare(left!, right!) >= 0;
}
