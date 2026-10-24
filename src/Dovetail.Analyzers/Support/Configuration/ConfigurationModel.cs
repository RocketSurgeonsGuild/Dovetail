namespace Dovetail.Support.Configuration;

/// <summary>The JSON Schema type a configuration value maps to.</summary>
internal enum SchemaKind
{
    Object,
    String,
    Integer,
    Number,
    Boolean,
    Array,

    /// <summary>A dictionary: an object whose keys are free-form (or enum names) and whose values share one schema.</summary>
    Map,
}

/// <summary>
///     The schema for one use of a type: a property, an array element, a dictionary value or a section root. Member-level
///     annotations (description, default, ranges, …) live on the node of the member that carries them.
/// </summary>
internal sealed record SchemaNode
{
    public SchemaKind Kind { get; init; }

    /// <summary>Whether <c>null</c> is a valid value (nullable value types and nullable reference types).</summary>
    public bool Nullable { get; init; }

    public string? Format { get; init; }

    /// <summary>An ECMA-262 pattern, already anchored.</summary>
    public string? Pattern { get; init; }

    public string? ContentEncoding { get; init; }

    /// <summary>A short hint about the expected text, e.g. for time spans, added to the description.</summary>
    public string? FormatHint { get; init; }

    public string? Description { get; init; }

    /// <summary>
    ///     The documentation comment id (<c>T:…</c>/<c>P:…</c>) whose summary becomes <see cref="Description" />, when the docs
    ///     have to be looked up in a referenced package's XML documentation file.
    /// </summary>
    public string? DocumentationId { get; init; }

    /// <summary>The name of the assembly whose XML documentation file holds <see cref="DocumentationId" />.</summary>
    public string? DocumentationAssembly { get; init; }

    /// <summary>The default as JSON literal text.</summary>
    public string? DefaultJson { get; init; }

    public bool Deprecated { get; init; }
    public string? DeprecationMessage { get; init; }

    /// <summary>JSON number text.</summary>
    public string? Minimum { get; init; }

    public bool ExclusiveMinimum { get; init; }
    public string? Maximum { get; init; }
    public bool ExclusiveMaximum { get; init; }
    public int? MinLength { get; init; }
    public int? MaxLength { get; init; }
    public int? MinItems { get; init; }
    public int? MaxItems { get; init; }

    /// <summary>Allowed names, for enums.</summary>
    public EquatableArray<string> EnumValues { get; init; } = EquatableArray<string>.Empty;

    /// <summary>A <c>[Flags]</c> enum: any comma-separated combination of <see cref="EnumValues" />.</summary>
    public bool FlagsEnum { get; init; }

    /// <summary>The element schema, for arrays.</summary>
    public SchemaNode? Items { get; init; }

    /// <summary>The value schema, for maps.</summary>
    public SchemaNode? Values { get; init; }

    /// <summary>Allowed key names when a map is keyed by an enum.</summary>
    public EquatableArray<string> KeyNames { get; init; } = EquatableArray<string>.Empty;

    /// <summary>The members, for objects, in declaration order (base types first).</summary>
    public EquatableArray<SchemaProperty> Properties { get; init; } = EquatableArray<SchemaProperty>.Empty;

    /// <summary>A recursive type, cut here: the object's members aren't described again.</summary>
    public bool Truncated { get; init; }
}

/// <summary>A member of an object, under its configuration key.</summary>
internal sealed record SchemaProperty(string Key, SchemaNode Schema, bool Required);

/// <summary>One <c>[DovetailConfiguration]</c> declaration that produced a schema.</summary>
internal sealed record ConfigurationSection(
    string Path,
    int Order,
    bool RegisterOptions,
    bool ValidateOnStart,
    string TypeName,
    SchemaNode Schema
);

/// <summary>What a declaration produced: a section (unless it was invalid) and any diagnostics.</summary>
internal sealed record ConfigurationDeclaration(ConfigurationSection? Section, EquatableArray<DiagnosticInfo> Diagnostics);
