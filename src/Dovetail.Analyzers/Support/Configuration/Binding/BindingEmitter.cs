// Ported from dotnet/runtime (MIT): the configuration binding generator's CoreBindingHelpers emitter, at version 10 and
// version 11 (switched by BinderVersion), without BinderOptions (Dovetail never passes any) and with entry points for the
// options pipeline instead of intercepted calls. Framework types are fully qualified because the binder is emitted into
// the consuming project's namespace. See BindingModel.cs.

using Microsoft.CodeAnalysis.CSharp;

namespace Dovetail.Support.Configuration.Binding;

/// <summary>Emits a static class that binds the given roots, and everything they reach, from configuration.</summary>
internal sealed class BindingEmitter
{
    private const string IConfiguration = "global::Microsoft.Extensions.Configuration.IConfiguration";
    private const string IConfigurationSection = "global::Microsoft.Extensions.Configuration.IConfigurationSection";
    private const string ConfigurationSection = "global::Microsoft.Extensions.Configuration.ConfigurationSection";
    private const string InvalidOperationException = "global::System.InvalidOperationException";
    private const string CultureInfo = "global::System.Globalization.CultureInfo";
    private const string NumberStyles = "global::System.Globalization.NumberStyles";
    private const string Enumerable = "global::System.Linq.Enumerable";

    private const string FailedBinding = "Failed to convert configuration value '{0}' at '{1}' to type '{2}'.";

    private const string ParameterHasNoMatchingConfig =
        "Cannot create instance of type '{0}' because parameter '{1}' has no matching config. Each parameter in the constructor that does not have a default value must have a corresponding config entry.";

    private readonly BindingTypeIndex _typeIndex;
    private readonly BindingMethods _methods;
    private readonly BinderVersion _version;
    private readonly SourceWriter _writer = new();
    private int _valueSuffixIndex;
    private bool _emitBlankLineBeforeNextStatement;

    private BindingEmitter(BindingTypeIndex typeIndex, BindingMethods methods, BinderVersion version)
    {
        _typeIndex = typeIndex;
        _methods = methods;
        _version = version;
    }

    private enum InitializationKind
    {
        None = 0,
        SimpleAssignment = 1,
        AssignmentWithNullCheck = 2,
        Declaration = 3,
    }

    /// <summary>Whether a missing configuration key calls the setter with the current value (Get) or not (Bind).</summary>
    private enum ValueDefaulting
    {
        None = 0,
        CallSetter = 1,
    }

    private bool V11 => _version >= BinderVersion.V11;

    /// <summary>The name of the entry point that binds an existing instance of a type: <c>Bind…(configuration, instance)</c>.</summary>
    public static string BindMethodName(TypeSpec type) => "Bind" + type.IdentifierCompatibleSubstring;

    /// <summary>The name of the entry point that creates an instance of a type: <c>Create…(configuration)</c>.</summary>
    public static string CreateMethodName(TypeSpec type) => "Create" + type.IdentifierCompatibleSubstring;

    /// <summary>
    ///     The members of the binder class: entry points for <paramref name="bindRoots" /> (bound into the instance the options
    ///     pipeline creates, like <c>services.Configure&lt;T&gt;(section)</c>) and <paramref name="createRoots" /> (created from
    ///     configuration, like <c>section.Get&lt;T&gt;()</c>), then the helpers they use.
    /// </summary>
    public static string Emit(
        BindingTypeIndex typeIndex,
        BindingMethods methods,
        BinderVersion version,
        IEnumerable<ObjectSpec> bindRoots,
        IEnumerable<ObjectSpec> createRoots
    )
    {
        var emitter = new BindingEmitter(typeIndex, methods, version);
        foreach (var root in bindRoots)
        {
            emitter.EmitBindEntryPoint(root);
        }

        foreach (var root in createRoots)
        {
            emitter.EmitCreateEntryPoint(root);
        }

        emitter.EmitBindCoreMethods();
        emitter.EmitInitializeMethods();
        emitter.EmitHelperMethods();
        return emitter._writer.ToString();
    }

    private void EmitBindEntryPoint(ObjectSpec type)
    {
        EmitBlankLineIfRequired();
        EmitStartBlock($"public static void {BindMethodName(type)}({IConfiguration} configuration, {type.TypeRef.FullyQualifiedName} instance)");
        if (_typeIndex.HasBindableMembers(type))
        {
            // BindCoreMain: a section with neither a value nor children leaves the instance as it is.
            _writer.WriteLine(
                """
                if (!HasValueOrChildren(configuration))
                {
                    return;
                }

                """
            );
            _writer.WriteLine("var temp = instance;");
            EmitBindingLogic(type, "temp", "configuration", InitializationKind.None, ValueDefaulting.None);
        }

        EmitEndBlock();
        _emitBlankLineBeforeNextStatement = true;
    }

    private void EmitCreateEntryPoint(ObjectSpec type)
    {
        EmitBlankLineIfRequired();
        EmitStartBlock($"public static {type.TypeRef.FullyQualifiedName} {CreateMethodName(type)}({IConfiguration} configuration)");

        // GetCore, without its early null for an empty section: the options pipeline needs an instance.
        EmitBindingLogic(type, "instance", "configuration", InitializationKind.Declaration, ValueDefaulting.CallSetter);
        _writer.WriteLine("return instance;");
        EmitEndBlock();
        _emitBlankLineBeforeNextStatement = true;
    }

    private void EmitBindCoreMethods()
    {
        foreach (var type in _methods.BindCore.OrderBy(z => z.TypeRef.FullyQualifiedName, StringComparer.Ordinal))
        {
            EmitBlankLineIfRequired();
            EmitBindCoreMethod(type);
        }
    }

    private void EmitBindCoreMethod(ComplexTypeSpec type)
    {
        var effectiveType = (ComplexTypeSpec)_typeIndex.GetEffectiveTypeSpec(type);

        // Objects created through a parameterized constructor tell BindCore so, and BindCore then leaves alone the properties
        // the constructor bound.
        var boundThroughConstructorParameter = ShouldEmitBoundThroughConstructorParameter(effectiveType) ? ", bool boundThroughConstructor = false" : "";
        EmitStartBlock(
            $"public static void BindCore({IConfiguration} configuration, ref {type.TypeRef.FullyQualifiedName} instance, bool defaultValueIfNotFound{boundThroughConstructorParameter})"
        );
        switch (effectiveType)
        {
            case ArraySpec arrayType:
                EmitBindCoreImplForArray(arrayType);
                break;
            case EnumerableSpec enumerableType:
                EmitBindCoreImplForEnumerableWithAdd(enumerableType);
                break;
            case DictionarySpec dictionaryType:
                EmitBindCoreImplForDictionary(dictionaryType);
                break;
            case ObjectSpec objectType:
                EmitBindCoreImplForObject(objectType);
                break;
            default:
                break;
        }

        EmitEndBlock();
    }

    private void EmitInitializeMethods()
    {
        foreach (var type in _methods.Initialize.OrderBy(z => z.TypeRef.FullyQualifiedName, StringComparer.Ordinal))
        {
            EmitBlankLineIfRequired();
            EmitInitializeMethod(type);
        }
    }

    private void EmitInitializeMethod(ObjectSpec type)
    {
        var initOnlyProperties = type.Properties.Where(z => z.SetOnInit && _typeIndex.ShouldBindTo(z)).ToList();
        var constructorArguments = new List<string>();
        EmitStartBlock($"public static {type.TypeRef.FullyQualifiedName} {InitializeMethodName(type)}({IConfiguration} configuration)");
        _emitBlankLineBeforeNextStatement = false;

        // Dovetail's object-initializer types keep a member's own initial value when its key is missing (as the reflection
        // binder does, as it binds after construction). Required members have none, and must be set to create the type.
        var defaultsInstance = type.InitializerOnly && initOnlyProperties.Any(z => !z.IsRequired);
        if (defaultsInstance)
        {
            _writer.WriteLine($"var defaults = new {type.TypeRef.FullyQualifiedName}(){RequiredMembersInitializer(type, [])};");
            _writer.WriteLine();
        }

        foreach (var parameter in type.ConstructorParameters)
        {
            var name = BindingSyntax.EscapeIdentifier(parameter.Name);
            constructorArguments.Add(
                parameter.RefKind switch
                {
                    Microsoft.CodeAnalysis.RefKind.Ref => $"ref {name}",
                    Microsoft.CodeAnalysis.RefKind.Out => "out _",
                    Microsoft.CodeAnalysis.RefKind.In => $"in {name}",
                    _ => name,
                }
            );
            EmitBindImplForLocal(parameter, null);
        }

        foreach (var property in initOnlyProperties.Where(z => z.MatchingCtorParam is null))
        {
            EmitBindImplForLocal(property, defaultsInstance && !property.IsRequired ? "defaults." + BindingSyntax.EscapeIdentifier(property.Name) : null);
        }

        var returnExpression = $"return new {type.TypeRef.FullyQualifiedName}({string.Join(", ", constructorArguments)})";
        var assignments = initOnlyProperties
                         .Select(
                              // Properties bound through a constructor parameter use the parameter's local.
                              z => $"{BindingSyntax.EscapeIdentifier(z.Name)} = {BindingSyntax.EscapeIdentifier(z.MatchingCtorParam?.Name ?? z.Name)},"
                          )
                         .Concat(UnboundRequiredMembers(type, initOnlyProperties.Select(z => z.Name)).Select(z => $"{z} = default!,"))
                         .ToList();
        if (assignments.Count == 0)
        {
            _writer.WriteLine($"{returnExpression};");
        }
        else
        {
            EmitStartBlock(returnExpression);
            foreach (var assignment in assignments)
            {
                _writer.WriteLine(assignment);
            }

            EmitEndBlock(endBraceTrailingSource: ";");
        }

        EmitEndBlock();
        _emitBlankLineBeforeNextStatement = true;

        void EmitBindImplForLocal(MemberSpec member, string? initialValue)
        {
            var memberType = _typeIndex.GetTypeSpec(member.TypeRef);
            var escapedName = BindingSyntax.EscapeIdentifier(member.Name);
            var declaration = $"{memberType.TypeRef.FullyQualifiedName} {escapedName}";
            var keyLiteral = SymbolDisplay.FormatLiteral(member.ConfigurationKeyName, quote: true);
            switch (memberType)
            {
                case ParsableFromStringSpec { StringParsableTypeKind: StringParsableTypeKind.AssignFromSectionValue }
                    when member is ParameterSpec { ErrorOnFailedBinding: true }:
                    // string and object take the section value: v11 as is, absent or null; v10 throws when it's absent.
                    if (V11)
                    {
                        _writer.WriteLine($"{declaration} = configuration[{keyLiteral}];");
                    }
                    else
                    {
                        EmitThrowBlock($"if (configuration[{keyLiteral}] is not {declaration})");
                    }

                    _writer.WriteLine();
                    return;
                case ConfigurationSectionSpec:
                    _writer.WriteLine($"{declaration} = configuration.GetSection({keyLiteral});");
                    return;
            }

            var bang = memberType.IsValueType ? "" : "!";
            _writer.WriteLine($"{declaration} = {initialValue ?? member.DefaultValueExpr}{bang};");
            _emitBlankLineBeforeNextStatement = false;
            var canBindToMember = EmitBindImplForMember(
                member,
                escapedName,
                $"configuration.GetSection({keyLiteral}).Path",
                // A local can always be read and written.
                canSet: true,
                canGet: true,
                InitializationKind.None,
                bindingToLocal: true
            );
            if (!canBindToMember)
            {
                return;
            }

            if (member is ParameterSpec { ErrorOnFailedBinding: true })
            {
                if (V11)
                {
                    // A type that can hold null is left at null rather than treated as an error.
                    if (!member.TypeRef.CanBeNull)
                    {
                        EmitThrowBlock("else");
                    }
                }
                else
                {
                    // An array or IEnumerable<T> has an extra block for an empty value, so the check can't be an else.
                    var effective = _typeIndex.GetEffectiveTypeSpec(member.TypeRef);
                    EmitThrowBlock(effective is ArraySpec || effective.IsExactIEnumerableOfT ? $"if ({escapedName} is null)" : "else");
                }
            }

            _writer.WriteLine();

            void EmitThrowBlock(string condition) =>
                _writer.WriteLine(
                    $$"""
                    {{condition}}
                    {
                        throw new {{InvalidOperationException}}({{SymbolDisplay.FormatLiteral(string.Format(System.Globalization.CultureInfo.InvariantCulture, ParameterHasNoMatchingConfig, type.FullName, member.Name), quote: true)}});
                    }
                    """
                );
        }
    }

    /// <summary>Sets every required member that isn't otherwise set, so the object can be created at all.</summary>
    private static string RequiredMembersInitializer(ObjectSpec type, IEnumerable<string> assigned)
    {
        var members = UnboundRequiredMembers(type, assigned).ToList();
        return members.Count == 0 ? "" : " { " + string.Join(", ", members.Select(z => z + " = default!")) + " }";
    }

    private static IEnumerable<string> UnboundRequiredMembers(ObjectSpec type, IEnumerable<string> assigned)
    {
        var set = new HashSet<string>(assigned, StringComparer.Ordinal);
        return type.RequiredMembers.Where(z => !set.Contains(z)).Select(BindingSyntax.EscapeIdentifier);
    }

    private void EmitHelperMethods()
    {
        EmitBlankLineIfRequired();
        _writer.WriteLine(
            $$"""
            /// <summary>Tries to get the configuration value for the specified key.</summary>
            public static bool TryGetConfigurationValue({{IConfiguration}} configuration, string key, out string? value)
            {
                if (configuration is {{ConfigurationSection}} section)
                {
                    return section.TryGetValue(key, out value);
                }

                value = key != null ? configuration[key] : configuration is {{IConfigurationSection}} sec ? sec.Value : null;
                return value != null;
            }
            """
        );

        EmitBlankLineIfRequired();
        _writer.WriteLine(
            $$"""
            public static bool HasValueOrChildren({{IConfiguration}} configuration)
            {
                if ((configuration as {{IConfigurationSection}})?.Value is not null)
                {
                    return true;
                }
                return AsConfigWithChildren(configuration) is not null;
            }
            """
        );

        EmitBlankLineIfRequired();
        _writer.WriteLine(
            $$"""
            public static {{IConfiguration}}? AsConfigWithChildren({{IConfiguration}} configuration)
            {
                foreach ({{IConfigurationSection}} _ in configuration.GetChildren())
                {
                    return configuration;
                }
                return null;
            }
            """
        );

        if (_methods.ParseEnum)
        {
            EmitBlankLineIfRequired();
            _writer.WriteLine(
                $$"""
                public static T ParseEnum<T>(string value, string? path) where T : struct
                {
                    try
                    {
                        return global::System.Enum.Parse<T>(value, ignoreCase: true);
                    }
                    catch (global::System.Exception exception)
                    {
                        throw new {{InvalidOperationException}}($"{{FailedBindingMessage("typeof(T)")}}", exception);
                    }
                }
                """
            );
        }

        foreach (var type in _methods.ParsePrimitive.OrderBy(z => BindingTypeIndex.GetParseMethodName(z), StringComparer.Ordinal))
        {
            EmitBlankLineIfRequired();
            EmitPrimitiveParseMethod(type);
        }
    }

    private static string FailedBindingMessage(string typeOfExpression) =>
        string.Format(System.Globalization.CultureInfo.InvariantCulture, FailedBinding, "{value ?? \"null\"}", "{path}", "{" + typeOfExpression + "}");

    private void EmitPrimitiveParseMethod(ParsableFromStringSpec type)
    {
        var typeName = type.TypeRef.FullyQualifiedName;
        var invariantCulture = $"{CultureInfo}.InvariantCulture";
        var parsedValue = type.StringParsableTypeKind switch
        {
            StringParsableTypeKind.ByteArray => "global::System.Convert.FromBase64String(value)",
            StringParsableTypeKind.Integer => $"{typeName}.Parse(value, {NumberStyles}.Integer, {invariantCulture})",
            StringParsableTypeKind.Float => $"{typeName}.Parse(value, {NumberStyles}.Float, {invariantCulture})",
            StringParsableTypeKind.Parse => $"{typeName}.Parse(value)",
            StringParsableTypeKind.ParseInvariant => $"{typeName}.Parse(value, {invariantCulture})",
            StringParsableTypeKind.CultureInfo => $"{CultureInfo}.GetCultureInfo(value)",
            StringParsableTypeKind.Uri => "new global::System.Uri(value, global::System.UriKind.RelativeOrAbsolute)",
            _ => null,
        };
        if (parsedValue is null)
        {
            return;
        }

        _writer.WriteLine(
            $$"""
            public static {{typeName}} {{BindingTypeIndex.GetParseMethodName(type)}}(string value, string? path)
            {
                try
                {
                    return {{parsedValue}};
                }
                catch (global::System.Exception exception)
                {
                    throw new {{InvalidOperationException}}($"{{FailedBindingMessage("typeof(" + typeName + ")")}}", exception);
                }
            }
            """
        );
    }

    private void EmitBindCoreImplForArray(ArraySpec type)
    {
        var elementTypeName = type.ElementTypeRef.FullyQualifiedName;
        var tempIdentifier = GetIncrementalIdentifier("temp");

        // Bind into a temporary list, then grow the array and copy the elements over.
        _writer.WriteLine($"var {tempIdentifier} = new global::System.Collections.Generic.List<{elementTypeName}>();");
        _writer.WriteLine();
        EmitBindingLogicForEnumerableWithAdd(type.ElementTypeRef, tempIdentifier);
        _writer.WriteLine();
        _writer.WriteLine(
            $"""
            int originalCount = instance.Length;
            global::System.Array.Resize(ref instance, originalCount + {tempIdentifier}.Count);
            {tempIdentifier}.CopyTo(instance, originalCount);
            """
        );
    }

    private void EmitBindCoreImplForEnumerableWithAdd(EnumerableSpec type)
    {
        EmitCollectionCastIfRequired(type, out var instanceIdentifier);
        EmitBindingLogicForEnumerableWithAdd(type.ElementTypeRef, instanceIdentifier);
    }

    private void EmitBindingLogicForEnumerableWithAdd(TypeRef elementTypeRef, string enumerableIdentifier)
    {
        EmitStartBlock($"foreach ({IConfigurationSection} section in configuration.GetChildren())");
        var addExpression = $"{enumerableIdentifier}.Add";
        switch (_typeIndex.GetEffectiveTypeSpec(elementTypeRef))
        {
            case ParsableFromStringSpec parsable:
                EmitStartBlock("if (TryGetConfigurationValue(section, key: null, out string? value))");
                EmitBindingLogic(parsable, "value", "section.Path", parsed => _writer.WriteLine($"{addExpression}({parsed});"), checkForNullSectionValue: true);
                EmitEndBlock();
                break;
            case ConfigurationSectionSpec:
                _writer.WriteLine($"{addExpression}(section);");
                break;
            case ComplexTypeSpec complex when _typeIndex.CanInstantiate(complex):
                // A section with a null or empty value and no children binds the type's default; one with a value but no
                // children can't be bound, so it's skipped.
                EmitBindCheckForSectionValue();
                EmitBindingLogic(complex, "value", "section", InitializationKind.Declaration, ValueDefaulting.None);
                _writer.WriteLine($"{addExpression}(value);");
                break;
        }

        EmitEndBlock();
    }

    private void EmitBindCheckForSectionValue()
    {
        EmitStartBlock($"if (!string.IsNullOrEmpty(section.Value) && !{Enumerable}.Any(section.GetChildren()))");
        _writer.WriteLine("continue;");
        EmitEndBlock();
    }

    private void EmitBindCoreImplForDictionary(DictionarySpec type)
    {
        EmitCollectionCastIfRequired(type, out var instanceIdentifier);
        EmitStartBlock($"foreach ({IConfigurationSection} section in configuration.GetChildren())");
        var keyType = (ParsableFromStringSpec)_typeIndex.GetEffectiveTypeSpec(type.KeyTypeRef);
        var elementType = _typeIndex.GetEffectiveTypeSpec(type.ElementTypeRef);
        EmitBindingLogic(keyType, "section.Key", "section.Path", EmitBindAndAddLogicForElement, checkForNullSectionValue: false);
        EmitEndBlock();

        void EmitBindAndAddLogicForElement(string parsedKey)
        {
            switch (elementType)
            {
                case ParsableFromStringSpec parsableElement:
                    EmitStartBlock("if (TryGetConfigurationValue(section, key: null, out string? value))");
                    EmitBindingLogic(
                        parsableElement,
                        "value",
                        "section.Path",
                        parsed => _writer.WriteLine($"{instanceIdentifier}[{parsedKey}] = {parsed};"),
                        checkForNullSectionValue: true
                    );
                    EmitEndBlock();
                    break;
                case ConfigurationSectionSpec:
                    _writer.WriteLine($"{instanceIdentifier}[{parsedKey}] = section;");
                    break;
                case ComplexTypeSpec complexElement:
                {
                    if (keyType.StringParsableTypeKind is not StringParsableTypeKind.AssignFromSectionValue)
                    {
                        // Parse the key once, for both the look-up and the add.
                        _writer.WriteLine($"{keyType.TypeRef.FullyQualifiedName} key = {parsedKey};");
                        parsedKey = "key";
                    }

                    var isValueType = complexElement.IsValueType;
                    var elementTypeName = complexElement.TypeRef.FullyQualifiedName + ( isValueType ? "" : "?" );
                    var useExistingElement = $"{instanceIdentifier}.TryGetValue({parsedKey}, out {elementTypeName} element)";

                    // An existing key binds into the existing element, unless it's a null reference.
                    if (!isValueType)
                    {
                        useExistingElement += " && element is not null";
                    }

                    if (_typeIndex.CanInstantiate(complexElement))
                    {
                        // A new element created through a parameterized constructor mustn't have its constructor-bound properties
                        // bound again; whether it's new is only known at run time.
                        string? constructedExpression = null;
                        if (!isValueType && ShouldEmitBoundThroughConstructorParameter(complexElement))
                        {
                            constructedExpression = GetIncrementalIdentifier("boundThroughConstructor");
                            _writer.WriteLine($"bool {constructedExpression} = false;");
                        }

                        EmitStartBlock($"if (!({useExistingElement}))");
                        EmitObjectInit(complexElement, "element", InitializationKind.SimpleAssignment, "section");
                        if (constructedExpression is not null)
                        {
                            _writer.WriteLine($"{constructedExpression} = true;");
                        }

                        EmitEndBlock();
                        EmitElementBindingLogic(constructedExpression);
                    }
                    else
                    {
                        EmitStartBlock($"if ({useExistingElement})");
                        EmitElementBindingLogic(null);
                        EmitEndBlock();
                    }

                    void EmitElementBindingLogic(string? constructedExpression)
                    {
                        EmitBindingLogic(complexElement, "element", "section", InitializationKind.None, ValueDefaulting.None, constructedExpression: constructedExpression);
                        _writer.WriteLine($"{instanceIdentifier}[{parsedKey}] = element;");
                    }

                    break;
                }
            }
        }
    }

    private void EmitBindCoreImplForObject(ObjectSpec type)
    {
        _emitBlankLineBeforeNextStatement = false;
        List<PropertySpec>? initializeBoundProperties = null;
        var deferInitializeBound = ShouldEmitBoundThroughConstructorParameter(type);
        foreach (var property in type.Properties)
        {
            if (!_typeIndex.ShouldBindTo(property))
            {
                continue;
            }

            // A property filled while Initialize creates the instance (a constructor parameter, or a required or init-only
            // property in the object initializer) would be bound twice, appending duplicates to collections. It's only bound
            // when the instance wasn't created there, e.g. when binding an existing instance.
            if (deferInitializeBound && IsBoundInInitialize(type, property) && IsPropertyReboundInBindCore(property))
            {
                ( initializeBoundProperties ??= [] ).Add(property);
                continue;
            }

            EmitBindImplForProperty(property);
        }

        if (initializeBoundProperties is not null)
        {
            EmitBlankLineIfRequired();
            EmitStartBlock("if (!boundThroughConstructor)");
            _emitBlankLineBeforeNextStatement = false;
            foreach (var property in initializeBoundProperties)
            {
                EmitBindImplForProperty(property);
            }

            EmitEndBlock();
        }

        void EmitBindImplForProperty(PropertySpec property)
        {
            var containingType = property.IsStatic ? type.TypeRef.FullyQualifiedName : "instance";
            var keyLiteral = SymbolDisplay.FormatLiteral(property.ConfigurationKeyName, quote: true);
            EmitBindImplForMember(
                property,
                $"{containingType}.{BindingSyntax.EscapeIdentifier(property.Name)}",
                $"configuration.GetSection({keyLiteral}).Path",
                property.CanSet,
                property.CanGet,
                InitializationKind.Declaration
            );
        }
    }

    /// <summary>
    ///     Whether the type's BindCore takes a <c>boundThroughConstructor</c> argument: version 11 for types created through a
    ///     parameterized constructor that has a property bound there which BindCore would otherwise bind again. Version 10 binds
    ///     those again; Dovetail's object-initializer types (<see cref="ObjectSpec.InitializerOnly" />) never do.
    /// </summary>
    private bool ShouldEmitBoundThroughConstructorParameter(ComplexTypeSpec type) =>
        type is ObjectSpec objectType
     && ( V11 || objectType.InitializerOnly )
     && objectType.Properties.Any(z => IsBoundInInitialize(objectType, z) && _typeIndex.ShouldBindTo(z) && IsPropertyReboundInBindCore(z));

    private static bool IsBoundInInitialize(ObjectSpec type, PropertySpec property) =>
        type.InstantiationStrategy is ObjectInstantiationStrategy.ParameterizedConstructor && ( property.MatchingCtorParam is not null || property.SetOnInit );

    /// <summary>Whether BindCore emits code that reads or writes the property (mirrors the cases in EmitBindImplForMember).</summary>
    private bool IsPropertyReboundInBindCore(PropertySpec property) => _typeIndex.GetEffectiveTypeSpec(property.TypeRef) switch
    {
        ParsableFromStringSpec => property is { CanGet: true, CanSet: true },
        ConfigurationSectionSpec => property.CanSet,
        ComplexTypeSpec complex => _typeIndex.HasBindableMembers(complex)
         || complex.IsValueType
         || complex is CollectionSpec
         || complex is not ObjectSpec { InstantiationStrategy: ObjectInstantiationStrategy.ParameterizedConstructor },
        _ => false,
    };

    private bool EmitBindImplForMember(
        MemberSpec member,
        string memberAccessExpression,
        string sectionPathExpression,
        bool canSet,
        bool canGet,
        InitializationKind initializationKind,
        bool bindingToLocal = false
    )
    {
        var keyLiteral = SymbolDisplay.FormatLiteral(member.ConfigurationKeyName, quote: true);
        var sectionExpression = $"configuration.GetSection({keyLiteral})";
        switch (_typeIndex.GetEffectiveTypeSpec(member.TypeRef))
        {
            case ParsableFromStringSpec parsable:
            {
                // The reflection binder doesn't bind set-only properties either.
                if (!( canSet && canGet ))
                {
                    return true;
                }

                EmitBlankLineIfRequired();
                var valueIdentifier = GetIncrementalIdentifier("value");

                // Version 11: a value-type parameter can't take null (without a default that falls through to the throw, with one
                // it keeps the default), and a parameter's default survives a null value.
                var requireValue = V11 && bindingToLocal && !member.TypeRef.CanBeNull && member is ParameterSpec;
                var hasDeclaredDefault = V11 && bindingToLocal && member is ParameterSpec { ErrorOnFailedBinding: false };
                var valueCondition = requireValue ? $" && {valueIdentifier} is not null" : "";
                EmitStartBlock($"if (TryGetConfigurationValue(configuration, key: {keyLiteral}, out string? {valueIdentifier}){valueCondition})");

                // Nullable members can be set to null; string and object take the value as is anyway.
                var emitNullCheck = member.TypeRef.CanBeNull && ( parsable.StringParsableTypeKind != StringParsableTypeKind.AssignFromSectionValue || hasDeclaredDefault );

                // Version 11 turns an empty value into null for Nullable<T>, as the reflection binder's TryConvertValue does.
                var treatEmptyValueAsNull = V11 && member.TypeRef.SpecialType is Microsoft.CodeAnalysis.SpecialType.System_Nullable_T;
                if (emitNullCheck)
                {
                    var nullValue = hasDeclaredDefault ? member.DefaultValueExpr : "null";
                    EmitStartBlock(treatEmptyValueAsNull ? $"if (string.IsNullOrEmpty({valueIdentifier}))" : $"if ({valueIdentifier} is null)");
                    _writer.WriteLine($"{memberAccessExpression} = {nullValue};");
                    EmitEndBlock();
                    EmitStartBlock("else");
                }

                EmitBindingLogic(
                    parsable,
                    valueIdentifier,
                    sectionPathExpression,
                    parsed => _writer.WriteLine($"{memberAccessExpression} = {parsed};"),
                    checkForNullSectionValue: !V11 || ( !( emitNullCheck && treatEmptyValueAsNull ) && !requireValue )
                );
                if (emitNullCheck)
                {
                    EmitEndBlock();
                }

                EmitEndBlock();
                if (initializationKind == InitializationKind.Declaration)
                {
                    EmitStartBlock("else if (defaultValueIfNotFound)");
                    if (!parsable.TypeRef.CanBeNull)
                    {
                        _writer.WriteLine($"{memberAccessExpression} = {memberAccessExpression};");
                    }
                    else
                    {
                        _writer.WriteLine($"var currentValue = {memberAccessExpression};");
                        EmitStartBlock("if (currentValue is not null)");
                        _writer.WriteLine($"{memberAccessExpression} = currentValue;");
                        EmitEndBlock();
                    }

                    EmitEndBlock();
                }

                return true;
            }
            case ConfigurationSectionSpec:
                if (canSet)
                {
                    EmitBlankLineIfRequired();
                    _writer.WriteLine($"{memberAccessExpression} = {sectionExpression};");
                }

                return true;
            case ComplexTypeSpec complex:
            {
                // A parameterized-constructor object with nothing to bind is skipped.
                if (!_typeIndex.HasBindableMembers(complex)
                 && !complex.IsValueType
                 && complex is ObjectSpec { InstantiationStrategy: ObjectInstantiationStrategy.ParameterizedConstructor })
                {
                    return false;
                }

                EmitBlankLineIfRequired();
                var configSection = GetIncrementalIdentifier("value");
                _writer.WriteLine($"var {configSection} = {sectionExpression};");
                var sectionIdentifier = GetIncrementalIdentifier("section");
                EmitStartBlock($"if (AsConfigWithChildren({configSection}) is {IConfigurationSection} {sectionIdentifier})");
                EmitBindingLogicForComplexMember(member, memberAccessExpression, sectionIdentifier, canSet, bindingToLocal);
                EmitEndBlock();

                // Without children, an empty value gives an array (or IEnumerable<T>) member an empty array.
                if (( complex is ArraySpec || complex.IsExactIEnumerableOfT ) && canSet)
                {
                    var valueIdentifier = GetIncrementalIdentifier("value");
                    EmitStartBlock(
                        $"if ({memberAccessExpression} is null && TryGetConfigurationValue({configSection}, key: null, out string? {valueIdentifier}) && {valueIdentifier} == string.Empty)"
                    );
                    _writer.WriteLine($"{memberAccessExpression} = global::System.Array.Empty<{( (CollectionSpec)complex ).ElementTypeRef.FullyQualifiedName}>();");
                    EmitEndBlock();
                }

                return _typeIndex.CanInstantiate(complex);
            }
            default:
                return false;
        }
    }

    private void EmitBindingLogicForComplexMember(MemberSpec member, string memberAccessExpression, string configArgument, bool canSet, bool bindingToLocal)
    {
        var memberType = _typeIndex.GetTypeSpec(member.TypeRef);
        var effectiveMemberType = (ComplexTypeSpec)_typeIndex.GetEffectiveTypeSpec(memberType);
        var tempIdentifier = GetIncrementalIdentifier("temp");
        InitializationKind initializationKind;
        string targetExpression;
        if (effectiveMemberType.IsValueType)
        {
            if (!canSet)
            {
                return;
            }

            var effectiveTypeName = effectiveMemberType.TypeRef.FullyQualifiedName;
            initializationKind = InitializationKind.None;
            if (memberType is NullableSpec)
            {
                var nullableTemp = GetIncrementalIdentifier("temp");
                _writer.WriteLine($"{memberType.TypeRef.FullyQualifiedName} {nullableTemp} = {memberAccessExpression};");
                _writer.WriteLine($"{effectiveTypeName} {tempIdentifier} = {nullableTemp}.HasValue ? {nullableTemp}.Value : new {effectiveTypeName}();");
            }
            else
            {
                _writer.WriteLine($"{effectiveTypeName} {tempIdentifier} = {memberAccessExpression};");
            }

            targetExpression = tempIdentifier;
        }
        else if (member.CanGet)
        {
            // Version 11 binds a get-only member in place, and only when it has a value; version 10 binds a new instance it then
            // drops when it's null.
            if (V11 && !canSet)
            {
                _writer.WriteLine($"{effectiveMemberType.TypeRef.FullyQualifiedName}? {tempIdentifier} = {memberAccessExpression};");
                EmitStartBlock($"if ({tempIdentifier} is not null)");
                EmitBindingLogic(effectiveMemberType, tempIdentifier, configArgument, InitializationKind.None, ValueDefaulting.None);
                EmitEndBlock();
                return;
            }

            targetExpression = memberAccessExpression;
            initializationKind = InitializationKind.AssignmentWithNullCheck;
        }
        else
        {
            targetExpression = memberAccessExpression;
            initializationKind = InitializationKind.SimpleAssignment;
        }

        Action<string, string?>? writeOnSuccess = !canSet
            ? null
            : (boundValue, tempStoringStatement) =>
            {
                if (memberAccessExpression != boundValue)
                {
                    _writer.WriteLine($"{memberAccessExpression} = {boundValue};");
                    if (tempStoringStatement is not null)
                    {
                        _writer.WriteLine(tempStoringStatement);
                    }

                    // Without the section the member is assigned to itself, so a setter with side effects still runs. Pointless
                    // for a local, where it would be a CS1717 self-assignment.
                    if (!bindingToLocal && member.CanGet && _typeIndex.CanInstantiate(effectiveMemberType))
                    {
                        EmitEndBlock();
                        EmitStartBlock("else");
                        _writer.WriteLine($"{memberAccessExpression} = {memberAccessExpression};");
                    }
                }
                else
                {
                    _writer.WriteLine(tempStoringStatement ?? "");
                }
            };

        EmitBindingLogic(effectiveMemberType, targetExpression, configArgument, initializationKind, ValueDefaulting.None, writeOnSuccess);
    }

    private void EmitBindingLogic(
        ComplexTypeSpec type,
        string memberAccessExpression,
        string configArgument,
        InitializationKind initializationKind,
        ValueDefaulting valueDefaulting,
        Action<string, string?>? writeOnSuccess = null,
        string? constructedExpression = null
    )
    {
        if (!_typeIndex.HasBindableMembers(type))
        {
            if (initializationKind is not InitializationKind.None)
            {
                if (_typeIndex.CanInstantiate(type))
                {
                    EmitObjectInit(type, memberAccessExpression, initializationKind, configArgument);
                }
                else if (type is ObjectSpec { InitExceptionMessage: { } message })
                {
                    _writer.WriteLine($"throw new {InvalidOperationException}({SymbolDisplay.FormatLiteral(message, quote: true)});");
                }
            }

            return;
        }

        var tempIdentifier = GetIncrementalIdentifier("temp");
        if (initializationKind is InitializationKind.AssignmentWithNullCheck)
        {
            _writer.WriteLine($"{type.TypeRef.FullyQualifiedName}? {tempIdentifier} = {memberAccessExpression};");
            EmitBindingLogicCore(tempIdentifier, InitializationKind.AssignmentWithNullCheck);
        }
        else if (initializationKind is InitializationKind.None && type.IsValueType)
        {
            EmitBindingLogicCore(tempIdentifier, InitializationKind.Declaration, $"{memberAccessExpression} = {tempIdentifier};");
        }
        else
        {
            EmitBindingLogicCore(memberAccessExpression, initializationKind);
        }

        // Creates the instance when the initialization kind asks for it, then calls BindCore on it. tempStoringStatement copies a
        // bound value-type temporary back up the chain of temporaries; the write into the member itself is writeOnSuccess's.
        void EmitBindingLogicCore(string instanceExpression, InitializationKind kind, string? tempStoringStatement = null)
        {
            var boundThroughConstructorArgument = "";
            if (_typeIndex.CanInstantiate(type))
            {
                if (kind is not InitializationKind.None)
                {
                    // Created here: through a parameterized constructor, BindCore leaves the constructor-bound properties alone.
                    // With a null check the constructor only runs for a null value, so that's decided at run time.
                    if (ShouldEmitBoundThroughConstructorParameter(type))
                    {
                        if (kind is InitializationKind.AssignmentWithNullCheck)
                        {
                            var wasNull = GetIncrementalIdentifier("wasNull");
                            _writer.WriteLine($"bool {wasNull} = {instanceExpression} is null;");
                            boundThroughConstructorArgument = $", boundThroughConstructor: {wasNull}";
                        }
                        else
                        {
                            boundThroughConstructorArgument = ", boundThroughConstructor: true";
                        }
                    }

                    EmitObjectInit(type, instanceExpression, kind, configArgument);
                }
                else if (constructedExpression is not null)
                {
                    // The caller created the instance and knows at run time whether it went through the constructor.
                    boundThroughConstructorArgument = $", boundThroughConstructor: {constructedExpression}";
                }

                EmitBindCoreCall();
            }
            else
            {
                EmitStartBlock($"if ({instanceExpression} is not null)");
                EmitBindCoreCall();
                EmitEndBlock();
                if (type is ObjectSpec { InitExceptionMessage: { } message })
                {
                    EmitStartBlock("else");
                    _writer.WriteLine($"throw new {InvalidOperationException}({SymbolDisplay.FormatLiteral(message, quote: true)});");
                    EmitEndBlock();
                }
            }

            void EmitBindCoreCall()
            {
                var defaultValueIfNotFound = valueDefaulting == ValueDefaulting.CallSetter ? "true" : "false";
                _writer.WriteLine($"BindCore({configArgument}, ref {instanceExpression}, defaultValueIfNotFound: {defaultValueIfNotFound}{boundThroughConstructorArgument});");
                writeOnSuccess?.Invoke(instanceExpression, tempStoringStatement);
            }
        }
    }

    private void EmitBindingLogic(
        ParsableFromStringSpec type,
        string sectionValueExpression,
        string sectionPathExpression,
        Action<string>? writeOnSuccess,
        bool checkForNullSectionValue
    )
    {
        var kind = type.StringParsableTypeKind;
        var parsedValue = kind switch
        {
            StringParsableTypeKind.AssignFromSectionValue => sectionValueExpression,
            StringParsableTypeKind.Enum => $"ParseEnum<{type.TypeRef.FullyQualifiedName}>({sectionValueExpression}, {sectionPathExpression})",
            _ => $"{BindingTypeIndex.GetParseMethodName(type)}({sectionValueExpression}, {sectionPathExpression})",
        };
        if (!checkForNullSectionValue || kind == StringParsableTypeKind.AssignFromSectionValue)
        {
            writeOnSuccess?.Invoke(parsedValue);
            return;
        }

        // An empty value gives a byte array an empty array, and leaves anything else alone.
        var conditionPrefix = "";
        if (kind == StringParsableTypeKind.ByteArray)
        {
            EmitStartBlock($"if ({sectionValueExpression} == string.Empty)");
            writeOnSuccess?.Invoke(parsedValue);
            EmitEndBlock();
            conditionPrefix = "else ";
        }

        EmitStartBlock($"{conditionPrefix}if (!string.IsNullOrEmpty({sectionValueExpression}))");
        writeOnSuccess?.Invoke(parsedValue);
        EmitEndBlock();
    }

    private void EmitObjectInit(ComplexTypeSpec type, string memberAccessExpression, InitializationKind initializationKind, string configArgument)
    {
        string? castExpression = null;
        string initExpression;
        var typeName = type.TypeRef.FullyQualifiedName;
        if (type is CollectionSpec collection)
        {
            if (collection is ArraySpec)
            {
                var index = typeName.IndexOf("[]", StringComparison.Ordinal);
                initExpression = $"new {typeName[..index]}[0]{typeName[( index + 2 )..]}";
            }
            else
            {
                var withConstructor = (CollectionWithCtorInitSpec)collection;
                if (withConstructor.InstantiationConcreteType is not CollectionInstantiationConcreteType.Self)
                {
                    castExpression = $"({withConstructor.TypeRef.FullyQualifiedName})";
                }

                typeName = BindingTypeIndex.GetInstantiationTypeDisplayString(withConstructor);
                initExpression = $"{castExpression}new {typeName}()";
            }
        }
        else
        {
            var objectType = (ObjectSpec)type;
            initExpression = objectType.InstantiationStrategy is ObjectInstantiationStrategy.ParameterlessConstructor
                // A value tuple is declared as (int, int) value = default, as it can't call a parameterless constructor.
                ? type.IsValueTuple ? "default" : $"new {typeName}()"
                : $"{InitializeMethodName(objectType)}({configArgument})";
        }

        switch (initializationKind)
        {
            case InitializationKind.Declaration:
                _writer.WriteLine($"{( type.IsValueTuple ? typeName : "var" )} {memberAccessExpression} = {initExpression};");
                break;
            case InitializationKind.AssignmentWithNullCheck:
                if (type is CollectionWithCtorInitSpec
                    {
                        InstantiationStrategy: CollectionInstantiationStrategy.CopyConstructor or CollectionInstantiationStrategy.LinqToDictionary,
                    } copied)
                {
                    var valueWhenNotNull = copied.InstantiationStrategy is CollectionInstantiationStrategy.CopyConstructor
                        ? $"new {typeName}({memberAccessExpression})"
                        : $"{Enumerable}.ToDictionary({memberAccessExpression}, pair => pair.Key, pair => pair.Value)";
                    _writer.WriteLine($"{memberAccessExpression} = {memberAccessExpression} is null ? {initExpression} : {castExpression}{valueWhenNotNull};");
                }
                else
                {
                    _writer.WriteLine($"{memberAccessExpression} ??= {initExpression};");
                }

                break;
            case InitializationKind.SimpleAssignment:
                _writer.WriteLine($"{memberAccessExpression} = {initExpression};");
                break;
            default:
                break;
        }
    }

    private void EmitCollectionCastIfRequired(CollectionWithCtorInitSpec type, out string instanceIdentifier)
    {
        if (type.PopulationCastType is CollectionPopulationCastType.NotApplicable)
        {
            instanceIdentifier = "instance";
            return;
        }

        instanceIdentifier = "temp";
        var castExpression = $"{BindingTypeIndex.GetPopulationCastTypeDisplayString(type)} {instanceIdentifier}";
        _writer.WriteLine(
            type.ShouldTryCast
                ? $$"""
                  if (instance is not {{castExpression}})
                  {
                      return;
                  }
                  """
                : $"{castExpression} = instance;"
        );
        _writer.WriteLine();
    }

    private static string InitializeMethodName(ObjectSpec type) => "Initialize" + type.IdentifierCompatibleSubstring;

    private void EmitStartBlock(string? source = null)
    {
        if (source is not null)
        {
            _writer.WriteLine(source);
        }

        _writer.WriteLine("{");
        _writer.Indentation++;
    }

    private void EmitEndBlock(string? source = null, string? endBraceTrailingSource = null)
    {
        if (source is not null)
        {
            _writer.WriteLine(source);
        }

        _writer.Indentation--;
        _writer.WriteLine("}" + endBraceTrailingSource);
    }

    private void EmitBlankLineIfRequired()
    {
        if (_emitBlankLineBeforeNextStatement)
        {
            _writer.WriteLine();
        }

        _emitBlankLineBeforeNextStatement = true;
    }

    private string GetIncrementalIdentifier(string prefix) => prefix + ( _valueSuffixIndex++ ).ToString(System.Globalization.CultureInfo.InvariantCulture);
}
