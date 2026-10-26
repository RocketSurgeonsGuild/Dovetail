using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;

namespace Dovetail.Configuration.Parity.Tests;

/// <summary>
///     One parity case: configuration data, how Microsoft's generated binder binds it, and how Dovetail's does. The P0 binder spike's
///     catalogue (minus <c>BinderOptions</c>, which Dovetail never passes, and the scalar <c>Get</c>/<c>GetValue</c> calls).
/// </summary>
public sealed record ParityCase(
    string Id,
    IReadOnlyDictionary<string, string?>? Data,
    string? Json,
    Func<IConfiguration, object?> Microsoft,
    Func<IConfiguration, object?> Dovetail
)
{
    public IConfigurationRoot Build()
    {
        var builder = new ConfigurationBuilder();
        if (Data is not null)
        {
            builder.Add(new MemoryConfigurationSource { InitialData = Data });
        }

        if (Json is not null)
        {
            builder.Add(new JsonStreamConfigurationSource { Stream = new MemoryStream(Encoding.UTF8.GetBytes(Json)) });
        }

        return builder.Build();
    }

    public override string ToString() => Id;
}

internal static class ParityCases
{
    public static IEnumerable<ParityCase> All() =>
        EmptyStrings().Concat(Nullables()).Concat(Constructors()).Concat(ValueAndChildren()).Concat(Collections()).Concat(Scalars()).Concat(UnknownKeys());

    /// <summary>
    ///     <paramref name="microsoft" /> binds the section; Dovetail binds the type's own section through <c>IOptions</c>.
    ///     <c>Get&lt;T&gt;()</c> returns null for a section without values, which a case avoids: Dovetail always creates the options.
    /// </summary>
    private static ParityCase Case<T>(
        string id,
        Func<IConfiguration, T?> microsoft,
        Func<T, object?> select,
        IReadOnlyDictionary<string, string?>? data = null,
        string? json = null,
        string section = "S"
    ) where T : class =>
        new(
            id,
            data,
            json,
            configuration => microsoft(configuration.GetSection(section)) is { } value ? select(value) : "<Get returned null>",
            configuration => select(Binders.Dovetail<T>(configuration))
        );

    private static Dictionary<string, string?> D(params (string Key, string? Value)[] pairs)
    {
        var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        foreach (var (key, value) in pairs)
        {
            data[key] = value;
        }

        return data;
    }

    private static IEnumerable<ParityCase> EmptyStrings()
    {
        var properties = new (string Key, Func<EmptyTarget, object?> Select)[]
        {
            ("Str", z => z.Str), ("Int", z => z.Int), ("NInt", z => z.NInt), ("Bool", z => z.Bool), ("En", z => z.En), ("Span", z => z.Span),
            ("Link", z => z.Link), ("Nested", z => z.Nested), ("Items", z => z.Items), ("NullNested", z => z.NullNested),
            ("NullItems", z => z.NullItems), ("Arr", z => z.Arr), ("Map", z => z.Map), ("Double", z => z.Double), ("Date", z => z.Date),
            ("DateOffset", z => z.DateOffset), ("Day", z => z.Day), ("Time", z => z.Time), ("Id", z => z.Id), ("Ver", z => z.Ver),
            ("Culture", z => z.Culture), ("Bytes", z => z.Bytes), ("Obj", z => z.Obj), ("Ch", z => z.Ch),
        };
        foreach (var (key, select) in properties)
        {
            yield return Case($"1.empty.{key}", MicrosoftBinder.EmptyTarget, select, D(($"S:{key}", "")));
        }

        foreach (var (key, select) in properties.Where(z => z.Key is "Str" or "Int" or "NInt" or "En" or "Link" or "Nested" or "Items"))
        {
            yield return Case($"1.whitespace.{key}", MicrosoftBinder.EmptyTarget, select, D(($"S:{key}", " ")));
        }

        static object? summary(EmptyTarget z) => $"Str={z.Str} Int={z.Int} Nested={z.Nested?.Value} Items={z.Items?.Count}";

        yield return Case("1.section.valueEmptyNoChildren", MicrosoftBinder.EmptyTarget, summary, D(("S", "")));
        yield return Case("1.section.missing", MicrosoftBinder.EmptyTarget, summary, D(("Other", "x")));
        yield return Case("1.section.valueNonEmptyNoChildren", MicrosoftBinder.EmptyTarget, summary, D(("S", "foo")));
        foreach (var (key, select) in properties.Where(z => z.Key is "Str" or "Int" or "NInt" or "Bool" or "Link" or "Nested" or "Items" or "Date" or "Ch"))
        {
            yield return Case($"1.memNull.{key}", MicrosoftBinder.EmptyTarget, select, D(($"S:{key}", null)));
        }

        foreach (var (key, json, select) in new (string, string, Func<EmptyTarget, object?>)[]
                 {
                     ("Str", "null", z => z.Str), ("Int", "null", z => z.Int), ("NInt", "null", z => z.NInt), ("Link", "null", z => z.Link),
                     ("Nested", "null", z => z.Nested), ("Nested", "{}", z => z.Nested), ("NullNested", "{}", z => z.NullNested),
                     ("Items", "null", z => z.Items), ("Items", "[]", z => z.Items), ("NullItems", "[]", z => z.NullItems), ("Arr", "[]", z => z.Arr),
                     ("Map", "{}", z => z.Map),
                 })
        {
            yield return Case($"1.json.{key}={json}", MicrosoftBinder.EmptyTarget, select, json: $$"""{ "S": { "{{key}}": {{json}} } }""");
        }
    }

    private static IEnumerable<ParityCase> Nullables()
    {
        foreach (var (label, value) in new (string, string?)[] { ("missing", null), ("empty", ""), ("whitespace", " "), ("valid", "12"), ("invalid", "abc") })
        {
            yield return Case($"2.int?(init 7).{label}", MicrosoftBinder.NullableTarget, z => z.NInt, value is null ? D(("S:Other", "x")) : D(("S:NInt", value)));
            yield return Case(
                $"2.int?(init null).{label}",
                MicrosoftBinder.NullableTarget,
                z => z.NIntNull,
                value is null ? D(("S:Other", "x")) : D(("S:NIntNull", value))
            );
        }

        foreach (var (key, bad, select) in new (string, string, Func<NullableTarget, object?>)[]
                 {
                     ("NBool", "maybe", z => z.NBool), ("NEnum", "Purple", z => z.NEnum), ("NSpan", "notatime", z => z.NSpan),
                     ("NDouble", "abc", z => z.NDouble), ("NDate", "notadate", z => z.NDate),
                 })
        {
            yield return Case($"2.{key}.empty", MicrosoftBinder.NullableTarget, select, D(($"S:{key}", "")));
            yield return Case($"2.{key}.invalid", MicrosoftBinder.NullableTarget, select, D(($"S:{key}", bad)));
        }
    }

    private static IEnumerable<ParityCase> Constructors()
    {
        yield return Case("3.posRecord.allKeys", MicrosoftBinder.PosRecord, z => z, D(("S:Name", "n"), ("S:Port", "80")));
        yield return Case("3.posRecord.missingPort", MicrosoftBinder.PosRecord, z => z, D(("S:Name", "n")));
        yield return Case("3.posRecord.missingName", MicrosoftBinder.PosRecord, z => z, D(("S:Port", "80")));
        yield return Case("3.posRecord.emptyPort", MicrosoftBinder.PosRecord, z => z, D(("S:Name", "n"), ("S:Port", "")));
        yield return Case("3.posRecord.nullName", MicrosoftBinder.PosRecord, z => z, D(("S:Name", null), ("S:Port", "80")));
        yield return Case("3.posRecord.keyCasing", MicrosoftBinder.PosRecord, z => z, D(("S:NAME", "n"), ("S:port", "80")));
        yield return Case("3.posRecordDefault.missingPort", MicrosoftBinder.PosRecordDefault, z => z, D(("S:Name", "n")));
        yield return Case("3.posRecordDefault.emptyPort", MicrosoftBinder.PosRecordDefault, z => z, D(("S:Name", "n"), ("S:Port", "")));
        yield return Case("3.posRecordDefault.nullPort", MicrosoftBinder.PosRecordDefault, z => z, D(("S:Name", "n"), ("S:Port", null)));
        yield return Case("3.nullableParamRecord.missingBoth", MicrosoftBinder.NullableParamRecord, z => z, D(("S:Other", "x")));
        yield return Case("3.nullableParamRecord.emptyPort", MicrosoftBinder.NullableParamRecord, z => z, D(("S:Name", "n"), ("S:Port", "")));
        yield return Case("3.singleCtor.allKeys+Extra", MicrosoftBinder.SingleCtorClass, z => z, D(("S:Name", "n"), ("S:Port", "80"), ("S:Extra", "e")));
        yield return Case("3.singleCtor.missingPort", MicrosoftBinder.SingleCtorClass, z => z, D(("S:Name", "n")));
        yield return Case("3.bothCtors", MicrosoftBinder.BothCtorsClass, z => z, D(("S:Name", "n")));
        yield return Case("3.ctorThenSettableProp", MicrosoftBinder.CtorThenSetter, z => z, D(("S:Name", "n")));
        yield return Case("3.recordCtorTransformsInitProp", MicrosoftBinder.TransformRecord, z => z, D(("S:Name", "n")));
        yield return Case("3.recordWithInitList.present", MicrosoftBinder.RecordWithInitList, z => z.Aliases, D(("S:Name", "n"), ("S:Aliases:0", "a")));
        yield return Case("3.recordWithInitList.missing", MicrosoftBinder.RecordWithInitList, z => z.Aliases, D(("S:Name", "n")));
        foreach (var (key, select, isDictionary) in new (string, Func<GetOnlyCollections, object?>, bool)[]
                 {
                     ("Items", z => z.Items, false), ("Arr", z => z.Arr, false), ("IList", z => z.IList, false), ("ICollection", z => z.ICollection, false),
                     ("IEnumerable", z => z.IEnumerable, false), ("IReadOnlyList", z => z.IReadOnlyList, false), ("ISet", z => z.ISet, false),
                     ("Map", z => z.Map, true), ("IDictionary", z => z.IDictionary, true), ("IReadOnlyDictionary", z => z.IReadOnlyDictionary, true),
                 })
        {
            yield return Case($"3.getOnly.{key}", MicrosoftBinder.GetOnlyCollections, select, isDictionary ? D(($"S:{key}:b", "x")) : D(($"S:{key}:0", "x")));
        }

        yield return Case("3.getOnly.NestedPoco", MicrosoftBinder.GetOnlyCollections, z => z.Nested, D(("S:Nested:Value", "x")));
        yield return Case("3.ctorParamCasing(NAME,pORT)", MicrosoftBinder.CasingCtor, z => z, D(("S:name", "n"), ("S:Port", "80")));
        yield return Case("3.keyName.positionalRecord", MicrosoftBinder.KeyNameRecord, z => z, D(("S:custom_key", "viaKeyName"), ("S:Value", "viaParamName")));
        yield return Case("3.keyName.positionalRecord.onlyCustomKey", MicrosoftBinder.KeyNameRecord, z => z, D(("S:custom_key", "viaKeyName")));
        yield return Case("3.keyName.class", MicrosoftBinder.KeyNameClass, z => z, D(("S:custom_key", "viaKeyName"), ("S:Value", "viaPropName")));
        yield return Case("3.recordStruct", MicrosoftBinder.HasStructs, z => z.Rec, D(("S:Rec:Name", "n"), ("S:Rec:Port", "80")));
        yield return Case("3.recordStruct.missingPort", MicrosoftBinder.HasStructs, z => z.Rec, D(("S:Rec:Name", "n")));
        yield return Case("3.plainStruct", MicrosoftBinder.HasStructs, z => z.Plain, D(("S:Plain:Name", "n")));
        yield return Case("3.nullablePlainStruct", MicrosoftBinder.HasStructs, z => z.NullablePlain, D(("S:NullablePlain:Port", "80")));
        yield return Case("3.nestedRecord.allKeys", MicrosoftBinder.HasRecordProp, z => z.Rec, D(("S:Rec:Name", "n"), ("S:Rec:Port", "80")));
        yield return Case("3.nestedRecord.missingPort", MicrosoftBinder.HasRecordProp, z => z.Rec, D(("S:Rec:Name", "n")));
        yield return Case("3.uncreatable.mismatchCtor", MicrosoftBinder.HasUncreatable, z => z.Mismatch, D(("S:Mismatch:Port", "80")));
        yield return Case("3.uncreatable.multipleCtors", MicrosoftBinder.HasUncreatable, z => z.Multi, D(("S:Multi:Name", "n")));
        yield return Case("3.uncreatable.existingInstance", MicrosoftBinder.HasUncreatable, z => z.ExistingMulti, D(("S:ExistingMulti:Port", "80")));
        yield return Case("3.poly.interfaceProp(existing Square)", MicrosoftBinder.Polymorphic, z => z.Shape, D(("S:Shape:Sides", "5"), ("S:Shape:Fill", "red")));
        yield return Case("3.poly.interfaceProp(null)", MicrosoftBinder.Polymorphic, z => z.NullShape, D(("S:NullShape:Sides", "5")));
        yield return Case("3.poly.baseClassProp(existing Derived)", MicrosoftBinder.Polymorphic, z => z.Opts, D(("S:Opts:BaseValue", "b"), ("S:Opts:DerivedValue", "d")));
        yield return Case("3.poly.objectPropWithChildren", MicrosoftBinder.Polymorphic, z => z.Obj, D(("S:Obj:X", "y")));
        yield return Case("3.poly.objectPropWithValue", MicrosoftBinder.Polymorphic, z => z.Obj, D(("S:Obj", "v")));
    }

    private static IEnumerable<ParityCase> ValueAndChildren()
    {
        var valueAndChild = D(("S:Section", "foo"), ("S:Section:Child", "bar"));
        var valueOnly = D(("S:Section", "foo"));
        var emptyAndChild = D(("S:Section", ""), ("S:Section:Child", "bar"));
        foreach (var (label, data) in new[] { ("valueAndChild", valueAndChild), ("valueOnly", valueOnly), ("emptyValueAndChild", emptyAndChild) })
        {
            yield return Case($"4.poco.{label}", MicrosoftBinder.HolderPoco, z => z.Section, data);
            yield return Case($"4.string.{label}", MicrosoftBinder.HolderString, z => z.Section, data);
            yield return Case(
                $"4.int.{label}",
                MicrosoftBinder.HolderInt,
                z => z.Section,
                label == "valueOnly" ? D(("S:Section", "5")) : label == "valueAndChild" ? D(("S:Section", "5"), ("S:Section:Child", "bar")) : data
            );
            yield return Case($"4.object.{label}", MicrosoftBinder.HolderObject, z => z.Section, data);
            yield return Case(
                $"4.list.{label}",
                MicrosoftBinder.HolderList,
                z => z.Section,
                label == "valueOnly" ? valueOnly : D(("S:Section", label == "valueAndChild" ? "foo" : ""), ("S:Section:0", "a"))
            );
            yield return Case($"4.Inner@S:Section.{label}", MicrosoftBinder.Inner, z => z, data, section: "S:Section");
        }

        yield return Case("4.listElement.valueAndChild", MicrosoftBinder.HolderList, z => z.Section, D(("S:Section:0", "foo"), ("S:Section:0:Child", "bar")));
        yield return Case("4.dictOfPoco.valueAndChild", MicrosoftBinder.HolderDictInner, z => z.Section, D(("S:Section:k", "foo"), ("S:Section:k:Child", "bar")));
        yield return Case("4.dictOfPoco.valueOnly", MicrosoftBinder.HolderDictInner, z => z.Section, D(("S:Section:k", "foo")));
    }

    private static IEnumerable<ParityCase> Collections()
    {
        foreach (var (key, select) in new (string, Func<CollTarget, object?>)[]
                 {
                     ("List", z => z.List), ("Arr", z => z.Arr), ("IList", z => z.IList), ("ICollection", z => z.ICollection), ("IEnumerable", z => z.IEnumerable),
                     ("IEnumerableOverArray", z => z.IEnumerableOverArray), ("IReadOnlyList", z => z.IReadOnlyList),
                     ("IReadOnlyCollection", z => z.IReadOnlyCollection), ("ISet", z => z.ISet), ("HashSet", z => z.HashSet), ("IReadOnlySet", z => z.IReadOnlySet),
                 })
        {
            yield return Case($"5.init[a,b]+cfg[b,c].{key}", MicrosoftBinder.CollTarget, select, D(($"S:{key}:0", "b"), ($"S:{key}:1", "c")));
        }

        foreach (var (key, select) in new (string, Func<CollTarget, object?>)[]
                 {
                     ("Dict", z => z.Dict), ("IDictionary", z => z.IDictionary), ("IReadOnlyDictionary", z => z.IReadOnlyDictionary),
                 })
        {
            yield return Case($"5.init{{a:1,b:2}}+cfg{{b:20,c:3}}.{key}", MicrosoftBinder.CollTarget, select, D(($"S:{key}:b", "20"), ($"S:{key}:c", "3")));
        }

        yield return Case("5.enumKeyDict.valid(Green,blue)", MicrosoftBinder.CollTarget, z => z.EnumKeyDict, D(("S:EnumKeyDict:Green", "g"), ("S:EnumKeyDict:blue", "b")));
        yield return Case("5.enumKeyDict.numericKey(3)", MicrosoftBinder.CollTarget, z => z.EnumKeyDict, D(("S:EnumKeyDict:3", "three")));
        yield return Case(
            "5.enumKeyDict.invalidKey(Purple)+valid(Green)",
            MicrosoftBinder.CollTarget,
            z => z.EnumKeyDict,
            D(("S:EnumKeyDict:Purple", "p"), ("S:EnumKeyDict:Green", "g"))
        );
        yield return Case("5.intKeyDict.valid(2)+invalid(x)", MicrosoftBinder.CollTarget, z => z.IntKeyDict, D(("S:IntKeyDict:2", "two"), ("S:IntKeyDict:x", "ex")));
        yield return Case("5.intList.invalidElement", MicrosoftBinder.CollTarget, z => z.IntList, D(("S:IntList:0", "2"), ("S:IntList:1", "abc"), ("S:IntList:2", "3")));
        yield return Case("5.intArr.invalidElement", MicrosoftBinder.CollTarget, z => z.IntArr, D(("S:IntArr:0", "2"), ("S:IntArr:1", "abc"), ("S:IntArr:2", "3")));
        yield return Case("5.intValueDict.invalidValue", MicrosoftBinder.CollTarget, z => z.IntValueDict, D(("S:IntValueDict:b", "abc"), ("S:IntValueDict:c", "3")));
        yield return Case("5.intList.emptyElement", MicrosoftBinder.CollTarget, z => z.IntList, D(("S:IntList:0", "2"), ("S:IntList:1", ""), ("S:IntList:2", "3")));
        yield return Case("5.nullableIntList.emptyElement", MicrosoftBinder.CollTarget, z => z.NIntList, D(("S:NIntList:0", "2"), ("S:NIntList:1", ""), ("S:NIntList:2", "3")));
        yield return Case("5.intValueDict.emptyValue", MicrosoftBinder.CollTarget, z => z.IntValueDict, D(("S:IntValueDict:b", ""), ("S:IntValueDict:c", "3")));
        yield return Case("5.list.emptyElement", MicrosoftBinder.CollTarget, z => z.List, D(("S:List:0", ""), ("S:List:1", "c")));
        yield return Case("5.list.nonNumericIndex", MicrosoftBinder.CollTarget, z => z.List, D(("S:List:foo", "x"), ("S:List:bar", "y")));
        yield return Case("5.list.sparseIndex(0,5)", MicrosoftBinder.CollTarget, z => z.List, D(("S:List:0", "x"), ("S:List:5", "y")));
        yield return Case("5.nestedList.existingIndex0", MicrosoftBinder.CollTarget, z => z.NestedList, D(("S:NestedList:0:Value", "x")));
        yield return Case("5.nestedList.emptyElement", MicrosoftBinder.CollTarget, z => z.NestedList, D(("S:NestedList:0", "")));
        yield return Case("5.nestedDict.existingKeyMerge", MicrosoftBinder.CollTarget, z => z.NestedDict, D(("S:NestedDict:a:Other", "ignored"), ("S:NestedDict:b:Value", "b")));
        yield return Case("5.nestedDict.existingKeySetValue", MicrosoftBinder.CollTarget, z => z.NestedDict, D(("S:NestedDict:a:Value", "A2")));
        yield return Case(
            "5.recordList",
            MicrosoftBinder.CollTarget,
            z => z.RecordList,
            D(("S:RecordList:0:Name", "a"), ("S:RecordList:0:Port", "1"), ("S:RecordList:1:Name", "b"), ("S:RecordList:1:Port", "2"))
        );
        yield return Case("5.recordList.missingPort", MicrosoftBinder.CollTarget, z => z.RecordList, D(("S:RecordList:0:Name", "a")));
        yield return Case(
            "5.recordDict",
            MicrosoftBinder.CollTarget,
            z => z.RecordDict,
            D(("S:RecordDict:x:Name", "a"), ("S:RecordDict:x:Port", "1"), ("S:RecordDict:y:Name", "b"), ("S:RecordDict:y:Port", "2"))
        );
        foreach (var (key, select, isDictionary) in new (string, Func<CollTargetNull, object?>, bool)[]
                 {
                     ("List", z => z.List, false), ("Arr", z => z.Arr, false), ("IList", z => z.IList, false), ("ICollection", z => z.ICollection, false),
                     ("IEnumerable", z => z.IEnumerable, false), ("IReadOnlyList", z => z.IReadOnlyList, false),
                     ("IReadOnlyCollection", z => z.IReadOnlyCollection, false), ("ISet", z => z.ISet, false), ("IReadOnlySet", z => z.IReadOnlySet, false),
                     ("IDictionary", z => z.IDictionary, true), ("IReadOnlyDictionary", z => z.IReadOnlyDictionary, true), ("NestedList", z => z.NestedList, false),
                 })
        {
            yield return Case(
                $"5.nullInit.{key}",
                MicrosoftBinder.CollTargetNull,
                select,
                key == "NestedList" ? D(("S:NestedList:0:Value", "x")) : isDictionary ? D(($"S:{key}:k", "v")) : D(($"S:{key}:0", "x"), ($"S:{key}:1", "x"))
            );
        }
    }

    private static IEnumerable<ParityCase> Scalars()
    {
        foreach (var (key, value, select) in new (string, string, Func<ScalarTarget, object?>)[]
                 {
                     ("Int", "abc", z => z.Int), ("Int", "0x10", z => z.Int), ("Int", "#10", z => z.Int), ("Int", " 42 ", z => z.Int), ("Int", "+42", z => z.Int),
                     ("Int", "1,000", z => z.Int), ("Int", "1.0", z => z.Int), ("Int", "99999999999", z => z.Int), ("Long", "9223372036854775808", z => z.Long),
                     ("Byte", "300", z => z.Byte), ("UShort", "-1", z => z.UShort), ("Bool", "maybe", z => z.Bool), ("Bool", "1", z => z.Bool),
                     ("Bool", " True ", z => z.Bool), ("Bool", "yes", z => z.Bool), ("Span", "notatime", z => z.Span), ("Span", "5", z => z.Span),
                     ("Span", "1.02:03:04", z => z.Span), ("Span", "00:00:90", z => z.Span), ("Span", "P1D", z => z.Span), ("Link", "http://[::1", z => z.Link),
                     ("Link", "relative/path", z => z.Link), ("Link", "http://exa mple.com", z => z.Link), ("Link", "not a uri", z => z.Link),
                     ("En", "Purple", z => z.En), ("En", "red", z => z.En), ("En", "2", z => z.En), ("En", "99", z => z.En), ("En", "Red, Blue", z => z.En),
                     ("En", " Red ", z => z.En), ("En", "-1", z => z.En), ("Flags", "Read, Write", z => z.Flags), ("Flags", "Read|Write", z => z.Flags),
                     ("Flags", "8", z => z.Flags), ("Double", "1.5", z => z.Double), ("Double", "1,5", z => z.Double), ("Double", "abc", z => z.Double),
                     ("Double", "1e3", z => z.Double), ("Double", "NaN", z => z.Double), ("Double", "0x10", z => z.Double), ("Decimal", "1e3", z => z.Decimal),
                     ("Decimal", "1,000.5", z => z.Decimal), ("Float", "3.5e39", z => z.Float), ("Id", "xyz", z => z.Id),
                     ("Id", "{11111111-1111-1111-1111-111111111111}", z => z.Id), ("Date", "notadate", z => z.Date), ("Date", "2024-02-30", z => z.Date),
                     ("Date", "2024-01-02T03:04:05Z", z => z.Date), ("Date", "01/02/2024", z => z.Date), ("DateOffset", "2024-01-02T03:04:05+02:00", z => z.DateOffset),
                     ("DateOffset", "2024-01-02", z => z.DateOffset), ("Ch", "ab", z => z.Ch), ("Ch", "a", z => z.Ch), ("Ch", " ", z => z.Ch),
                     ("Ver", "1.2.3.4.5", z => z.Ver), ("Ver", "abc", z => z.Ver), ("Ver", "1", z => z.Ver), ("Bytes", "AQID", z => z.Bytes),
                     ("Bytes", "not base64!", z => z.Bytes), ("Culture", "xx-INVALID-zz", z => z.Culture), ("Culture", "en-us", z => z.Culture),
                     ("Half", "1.5", z => z.Half), ("Half", "70000", z => z.Half), ("Big", "170141183460469231731687303715884105727", z => z.Big),
                     ("Big", "x", z => z.Big),
                 })
        {
            yield return Case($"6.{key}=\"{value}\"", MicrosoftBinder.ScalarTarget, select, D(($"S:{key}", value)));
        }
    }

    private static IEnumerable<ParityCase> UnknownKeys()
    {
        yield return Case("7.unknown.ignored", MicrosoftBinder.UnknownTarget, z => z, D(("S:Name", "n"), ("S:Bogus", "x"), ("S:ReadOnly", "x"), ("S:PrivSet", "x")));
        yield return Case("7.keyName.clrName", MicrosoftBinder.UnknownTarget, z => z.Renamed, D(("S:Renamed", "x")));
        yield return Case("7.keyName.customName", MicrosoftBinder.UnknownTarget, z => z.Renamed, D(("S:renamed_key", "x")));
        yield return Case("7.keyCasing", MicrosoftBinder.UnknownTarget, z => z.Name, D(("S:NAME", "x")));
    }
}
