// The option types the parity cases bind (from the P0 binder spike). Initialisers are deliberate: they show whether binding
// overwrites, resets, appends or replaces. Every root is a [DovetailConfiguration] section, so Dovetail's generator emits a binder
// for it; Microsoft's generator binds the same types through the statically typed calls in MicrosoftBinder.
using System.Globalization;
using Dovetail.Attributes;
using Microsoft.Extensions.Configuration;

// The fixtures keep the exact shapes the binders are compared on (an explicit constructor, `new List<string> { … }` behind an
// interface), so the suggestions to use primary constructors and collection expressions don't apply.
#pragma warning disable IDE0290, IDE0028

namespace Dovetail.Configuration.Parity.Tests;

public enum Color { Red = 1, Green = 2, Blue = 3 }

[Flags]
public enum Perms { None = 0, Read = 1, Write = 2, Exec = 4 }

public class Nested
{
    public string? Value { get; set; }
}

[DovetailConfiguration("S")]
public class EmptyTarget
{
    public string? Str { get; set; } = "init";
    public int Int { get; set; } = 42;
    public int? NInt { get; set; } = 7;
    public bool Bool { get; set; } = true;
    public Color En { get; set; } = Color.Green;
    public TimeSpan Span { get; set; } = TimeSpan.FromSeconds(5);
    public Uri? Link { get; set; } = new("http://init/");
    public Nested? Nested { get; set; } = new() { Value = "init" };
    public List<string>? Items { get; set; } = ["init"];
    public Nested? NullNested { get; set; }
    public List<string>? NullItems { get; set; }
    public string[]? Arr { get; set; } = ["init"];
    public Dictionary<string, string>? Map { get; set; } = new() { ["k"] = "init" };
    public double Double { get; set; } = 1.5;
    public DateTime Date { get; set; } = new(2020, 1, 1);
    public DateTimeOffset DateOffset { get; set; } = new(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
    public DateOnly Day { get; set; } = new(2020, 1, 1);
    public TimeOnly Time { get; set; } = new(1, 2, 3);
    public Guid Id { get; set; } = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public Version? Ver { get; set; } = new(1, 2);
    public CultureInfo? Culture { get; set; } = CultureInfo.GetCultureInfo("en-US");
    public byte[]? Bytes { get; set; } = [1, 2];
    public object? Obj { get; set; } = "init";
    public char Ch { get; set; } = 'x';
}

[DovetailConfiguration("S")]
public class NullableTarget
{
    public int? NInt { get; set; } = 7;
    public int? NIntNull { get; set; }
    public bool? NBool { get; set; } = true;
    public Color? NEnum { get; set; } = Color.Green;
    public TimeSpan? NSpan { get; set; } = TimeSpan.FromSeconds(5);
    public double? NDouble { get; set; } = 1.5;
    public DateTime? NDate { get; set; }
}

[DovetailConfiguration("S")]
public record PosRecord(string Name, int Port);

[DovetailConfiguration("S")]
public record PosRecordDefault(string Name, int Port = 8080);

[DovetailConfiguration("S")]
public record NullableParamRecord(string? Name, int? Port);

[DovetailConfiguration("S")]
public class SingleCtorClass
{
    public SingleCtorClass(string name, int port)
    {
        Name = name;
        Port = port;
    }

    public string Name { get; }
    public int Port { get; }
    public string? Extra { get; set; }
}

[DovetailConfiguration("S")]
public class BothCtorsClass
{
    public BothCtorsClass() => Source = "parameterless";

    public BothCtorsClass(string name)
    {
        Name = name;
        Source = "parameterized";
    }

    public string? Name { get; set; }
    public string Source { get; }
}

[DovetailConfiguration("S")]
public class CtorThenSetter
{
    public CtorThenSetter(string name) => Name = name + "+ctor";

    public string Name { get; set; }
}

[DovetailConfiguration("S")]
public record TransformRecord(string Name)
{
    public string Name { get; init; } = Name + "+ctor";
}

[DovetailConfiguration("S")]
public record RecordWithInitList(string Name)
{
    public List<string> Aliases { get; init; } = ["init"];
}

/// <summary>Dovetail creates this with an object initializer; Microsoft's generator never sets its members.</summary>
[DovetailConfiguration("S")]
public class InitOnlyClass
{
    public string? Name { get; init; } = "init";
    public int Port { get; init; } = 1;
    public List<string> Items { get; init; } = ["init"];
}

/// <summary>Dovetail creates this with an object initializer; Microsoft's generator can't create it (CS9035).</summary>
[DovetailConfiguration("S")]
public class RequiredClass
{
    public required string Name { get; set; }
    public int Port { get; set; }
}

[DovetailConfiguration("S")]
public class GetOnlyCollections
{
    public List<string> Items { get; } = ["init"];
    public Dictionary<string, string> Map { get; } = new() { ["a"] = "init" };
    public string[] Arr { get; } = ["init"];
    public IList<string> IList { get; } = new List<string> { "init" };
    public ICollection<string> ICollection { get; } = new List<string> { "init" };
    public IEnumerable<string> IEnumerable { get; } = new List<string> { "init" };
    public IReadOnlyList<string> IReadOnlyList { get; } = new List<string> { "init" };
    public ISet<string> ISet { get; } = new HashSet<string> { "init" };
    public IDictionary<string, string> IDictionary { get; } = new Dictionary<string, string> { ["a"] = "init" };
    public IReadOnlyDictionary<string, string> IReadOnlyDictionary { get; } = new Dictionary<string, string> { ["a"] = "init" };
    public Nested Nested { get; } = new() { Value = "init" };
}

[DovetailConfiguration("S")]
public class CasingCtor
{
    public CasingCtor(string NAME, int pORT)
    {
        Name = NAME;
        Port = pORT;
    }

    public string Name { get; }
    public int Port { get; }
}

[DovetailConfiguration("S")]
public record KeyNameRecord([property: ConfigurationKeyName("custom_key")] string Value);

[DovetailConfiguration("S")]
public class KeyNameClass
{
    [ConfigurationKeyName("custom_key")]
    public string? Value { get; set; }
}

public record struct PosRecordStruct(string Name, int Port);

public struct PlainStruct
{
    public string? Name { get; set; }
    public int Port { get; set; }
}

/// <summary>Structs can't be options, so they're bound as members.</summary>
[DovetailConfiguration("S")]
public class HasStructs
{
    public PosRecordStruct Rec { get; set; } = new("init", 1);
    public PlainStruct Plain { get; set; } = new() { Name = "init", Port = 1 };
    public PlainStruct? NullablePlain { get; set; }
}

[DovetailConfiguration("S")]
public class HasRecordProp
{
    public PosRecord? Rec { get; set; }
}

/// <summary>Types Microsoft's generator can't create, bound as members: they throw when they have to be created.</summary>
public class MismatchCtor
{
    public MismatchCtor(int portNumber) => Port = portNumber;

    public int Port { get; }
}

public class MultiCtor
{
    public MultiCtor(string name) => Name = name;

    public MultiCtor(int port) => Port = port;

    public string? Name { get; set; }
    public int Port { get; set; }
}

[DovetailConfiguration("S")]
public class HasUncreatable
{
    public MismatchCtor? Mismatch { get; set; }
    public MultiCtor? Multi { get; set; }
    public MultiCtor ExistingMulti { get; set; } = new("init");
}

public interface IShape
{
    int Sides { get; set; }
}

public class Square : IShape
{
    public int Sides { get; set; } = 4;
    public string? Fill { get; set; }
}

public class BaseOpts
{
    public string? BaseValue { get; set; }
}

public class DerivedOpts : BaseOpts
{
    public string? DerivedValue { get; set; }
}

[DovetailConfiguration("S")]
public class Polymorphic
{
    public IShape Shape { get; set; } = new Square();
    public IShape? NullShape { get; set; }
    public BaseOpts Opts { get; set; } = new DerivedOpts();
    public object? Obj { get; set; }
}

[DovetailConfiguration("S:Section")]
public class Inner
{
    public string? Child { get; set; }
}

[DovetailConfiguration("S")]
public class HolderPoco
{
    public Inner? Section { get; set; }
}

[DovetailConfiguration("S")]
public class HolderString
{
    public string? Section { get; set; }
}

[DovetailConfiguration("S")]
public class HolderInt
{
    public int Section { get; set; }
}

[DovetailConfiguration("S")]
public class HolderList
{
    public List<string>? Section { get; set; }
}

[DovetailConfiguration("S")]
public class HolderDictInner
{
    public Dictionary<string, Inner>? Section { get; set; }
}

[DovetailConfiguration("S")]
public class HolderObject
{
    public object? Section { get; set; }
}

[DovetailConfiguration("S")]
public class CollTarget
{
    public List<string> List { get; set; } = ["a", "b"];
    public string[] Arr { get; set; } = ["a", "b"];
    public IList<string> IList { get; set; } = new List<string> { "a", "b" };
    public ICollection<string> ICollection { get; set; } = new List<string> { "a", "b" };
    public IEnumerable<string> IEnumerable { get; set; } = new List<string> { "a", "b" };
    public IEnumerable<string> IEnumerableOverArray { get; set; } = new[] { "a", "b" };
    public IReadOnlyList<string> IReadOnlyList { get; set; } = new List<string> { "a", "b" };
    public IReadOnlyCollection<string> IReadOnlyCollection { get; set; } = new List<string> { "a", "b" };
    public ISet<string> ISet { get; set; } = new HashSet<string> { "a", "b" };
    public HashSet<string> HashSet { get; set; } = ["a", "b"];
    public IReadOnlySet<string> IReadOnlySet { get; set; } = new HashSet<string> { "a", "b" };
    public Dictionary<string, string> Dict { get; set; } = new() { ["a"] = "1", ["b"] = "2" };
    public IDictionary<string, string> IDictionary { get; set; } = new Dictionary<string, string> { ["a"] = "1", ["b"] = "2" };
    public IReadOnlyDictionary<string, string> IReadOnlyDictionary { get; set; } = new Dictionary<string, string> { ["a"] = "1", ["b"] = "2" };
    public Dictionary<Color, string> EnumKeyDict { get; set; } = new() { [Color.Red] = "r" };
    public Dictionary<int, string> IntKeyDict { get; set; } = new() { [1] = "one" };
    public List<int> IntList { get; set; } = [1];
    public List<int?> NIntList { get; set; } = [1];
    public int[] IntArr { get; set; } = [1];
    public Dictionary<string, int> IntValueDict { get; set; } = new() { ["a"] = 1 };
    public List<Nested> NestedList { get; set; } = [new() { Value = "a" }];
    public Dictionary<string, Nested> NestedDict { get; set; } = new() { ["a"] = new() { Value = "a" } };
    public List<PosRecord> RecordList { get; set; } = [];
    public Dictionary<string, PosRecord> RecordDict { get; set; } = [];
    public List<InitOnlyClass> InitOnlyList { get; set; } = [];
}

[DovetailConfiguration("S")]
public class CollTargetNull
{
    public List<string>? List { get; set; }
    public string[]? Arr { get; set; }
    public IList<string>? IList { get; set; }
    public ICollection<string>? ICollection { get; set; }
    public IEnumerable<string>? IEnumerable { get; set; }
    public IReadOnlyList<string>? IReadOnlyList { get; set; }
    public IReadOnlyCollection<string>? IReadOnlyCollection { get; set; }
    public ISet<string>? ISet { get; set; }
    public IReadOnlySet<string>? IReadOnlySet { get; set; }
    public IDictionary<string, string>? IDictionary { get; set; }
    public IReadOnlyDictionary<string, string>? IReadOnlyDictionary { get; set; }
    public List<Nested>? NestedList { get; set; }
}

[DovetailConfiguration("S")]
public class ScalarTarget
{
    public int Int { get; set; }
    public bool Bool { get; set; }
    public TimeSpan Span { get; set; }
    public Uri? Link { get; set; }
    public Color En { get; set; }
    public Perms Flags { get; set; }
    public double Double { get; set; }
    public decimal Decimal { get; set; }
    public float Float { get; set; }
    public long Long { get; set; }
    public byte Byte { get; set; }
    public ushort UShort { get; set; }
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public DateTimeOffset DateOffset { get; set; }
    public char Ch { get; set; }
    public Version? Ver { get; set; }
    public byte[]? Bytes { get; set; }
    public CultureInfo? Culture { get; set; }
    public Half Half { get; set; }
    public Int128 Big { get; set; }
}

[DovetailConfiguration("S")]
public class UnknownTarget
{
    public string? Name { get; set; }
    public Nested? Nested { get; set; }
    public string ReadOnly => "ro";
    public string? PrivSet { get; private set; }

    [ConfigurationKeyName("renamed_key")]
    public string? Renamed { get; set; }

    public List<string>? List { get; set; }
    public Dictionary<string, string>? Dict { get; set; }
}

[DovetailConfiguration("Reload")]
public class ReloadOptions
{
    public string? Name { get; set; }
    public int Count { get; set; }
}
