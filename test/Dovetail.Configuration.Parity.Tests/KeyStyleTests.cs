using Dovetail.Attributes;
using Microsoft.Extensions.Configuration;

namespace Dovetail.Configuration.Parity.Tests;

/// <summary>A section in camel case: a nested object, a record and a list of objects, all keyed in camel case.</summary>
[DovetailConfiguration("Styled:Camel", KeyStyle = ConfigurationKeyStyle.Camel)]
public class CamelStyled
{
    public string? DisplayName { get; set; }
    public CamelInner InnerObject { get; set; } = new();
    public CamelRecord? RecordMember { get; set; }
    public List<CamelItem> ItemList { get; set; } = [];
    public Dictionary<string, int> TenantLimits { get; set; } = [];

    [ConfigurationKeyName("Legacy_Name")]
    public string? LegacyName { get; set; }
}

public class CamelInner
{
    public int RetryCount { get; set; }
    public string? HostName { get; set; }
}

public record CamelRecord(string HostName, int PortNumber = 80);

public class CamelItem
{
    public string? ItemName { get; set; }
    public int ItemWeight { get; set; }
}

/// <summary>A section in dash case: the same shapes as <see cref="CamelStyled" />, keyed with dashes.</summary>
[DovetailConfiguration("Styled:Dash", KeyStyle = ConfigurationKeyStyle.Dash)]
public class DashStyled
{
    public string? DisplayName { get; set; }
    public DashInner InnerObject { get; set; } = new();
    public DashRecord? RecordMember { get; set; }
    public List<DashItem> ItemList { get; set; } = [];
    public Dictionary<string, int> TenantLimits { get; set; } = [];

    [ConfigurationKeyName("Legacy_Name")]
    public string? LegacyName { get; set; }
}

public class DashInner
{
    public int RetryCount { get; set; }
    public string? HostName { get; set; }
}

public record DashRecord(string HostName, int PortNumber = 80);

public class DashItem
{
    public string? ItemName { get; set; }
    public int ItemWeight { get; set; }
}

/// <summary>The default section name follows the key style: <c>derived-dash</c>, not <c>DerivedDash</c>.</summary>
[DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Dash)]
public class DerivedDashSettings
{
    public int MaxConnections { get; set; }
}

/// <summary>
///     <see cref="ConfigurationKeyStyle" />: data written in the style binds through Dovetail's joint. Keys ignore case, so
///     camel case also reads Pascal-case data, while dash case reads different keys.
/// </summary>
public class KeyStyleTests
{
    [Test]
    public async Task Binds_camel_case_data()
    {
        var configuration = Build(
            ("Styled:Camel:displayName", "shown"),
            ("Styled:Camel:innerObject:retryCount", "4"),
            ("Styled:Camel:innerObject:hostName", "inner-host"),
            ("Styled:Camel:recordMember:hostName", "record-host"),
            ("Styled:Camel:recordMember:portNumber", "8080"),
            ("Styled:Camel:itemList:0:itemName", "first"),
            ("Styled:Camel:itemList:0:itemWeight", "1"),
            ("Styled:Camel:itemList:1:itemName", "second"),
            ("Styled:Camel:itemList:1:itemWeight", "2"),
            ("Styled:Camel:tenantLimits:AcmeCorp", "3"),
            ("Styled:Camel:Legacy_Name", "legacy")
        );

        var bound = Binders.Dovetail<CamelStyled>(configuration);

        bound.DisplayName.ShouldBe("shown");
        bound.InnerObject.RetryCount.ShouldBe(4);
        bound.InnerObject.HostName.ShouldBe("inner-host");
        bound.RecordMember.ShouldBe(new CamelRecord("record-host", 8080));
        bound.ItemList.Select(z => (z.ItemName, z.ItemWeight)).ShouldBe([("first", 1), ("second", 2)]);
        // Dictionary keys are data: they keep the case they were written in.
        bound.TenantLimits.ShouldBe(new Dictionary<string, int> { ["AcmeCorp"] = 3 });
        bound.LegacyName.ShouldBe("legacy");
        await Task.CompletedTask;
    }

    [Test]
    public async Task Binds_dash_case_data()
    {
        var configuration = Build(
            ("Styled:Dash:display-name", "shown"),
            ("Styled:Dash:inner-object:retry-count", "4"),
            ("Styled:Dash:inner-object:host-name", "inner-host"),
            ("Styled:Dash:record-member:host-name", "record-host"),
            ("Styled:Dash:record-member:port-number", "8080"),
            ("Styled:Dash:item-list:0:item-name", "first"),
            ("Styled:Dash:item-list:0:item-weight", "1"),
            ("Styled:Dash:item-list:1:item-name", "second"),
            ("Styled:Dash:item-list:1:item-weight", "2"),
            ("Styled:Dash:tenant-limits:AcmeCorp", "3"),
            ("Styled:Dash:tenant-limits:beta-corp", "5"),
            ("Styled:Dash:Legacy_Name", "legacy")
        );

        var bound = Binders.Dovetail<DashStyled>(configuration);

        bound.DisplayName.ShouldBe("shown");
        bound.InnerObject.RetryCount.ShouldBe(4);
        bound.InnerObject.HostName.ShouldBe("inner-host");
        bound.RecordMember.ShouldBe(new DashRecord("record-host", 8080));
        bound.ItemList.Select(z => (z.ItemName, z.ItemWeight)).ShouldBe([("first", 1), ("second", 2)]);
        // Dictionary keys are data: neither restyled nor split.
        bound.TenantLimits.ShouldBe(new Dictionary<string, int> { ["AcmeCorp"] = 3, ["beta-corp"] = 5 });
        bound.LegacyName.ShouldBe("legacy");
        await Task.CompletedTask;
    }

    [Test]
    public async Task Binds_pascal_case_data_in_camel_case_because_keys_ignore_case()
    {
        var configuration = Build(
            ("Styled:Camel:DisplayName", "shown"),
            ("Styled:Camel:InnerObject:RetryCount", "4"),
            ("Styled:Camel:RecordMember:HostName", "record-host"),
            ("Styled:Camel:RecordMember:PortNumber", "8080"),
            ("Styled:Camel:ItemList:0:ItemName", "first"),
            ("Styled:Camel:ItemList:0:ItemWeight", "1")
        );

        var bound = Binders.Dovetail<CamelStyled>(configuration);

        bound.DisplayName.ShouldBe("shown");
        bound.InnerObject.RetryCount.ShouldBe(4);
        bound.RecordMember.ShouldBe(new CamelRecord("record-host", 8080));
        bound.ItemList.Select(z => (z.ItemName, z.ItemWeight)).ShouldBe([("first", 1)]);
        await Task.CompletedTask;
    }

    [Test]
    public async Task Does_not_bind_pascal_case_data_in_dash_case_because_the_keys_differ()
    {
        var configuration = Build(
            ("Styled:Dash:DisplayName", "shown"),
            ("Styled:Dash:InnerObject:RetryCount", "4"),
            ("Styled:Dash:RecordMember:HostName", "record-host"),
            ("Styled:Dash:RecordMember:PortNumber", "8080"),
            ("Styled:Dash:ItemList:0:ItemName", "first"),
            ("Styled:Dash:ItemList:0:ItemWeight", "1"),
            ("Styled:Dash:legacy-name", "not the explicit key")
        );

        var bound = Binders.Dovetail<DashStyled>(configuration);

        bound.DisplayName.ShouldBeNull();
        bound.InnerObject.RetryCount.ShouldBe(0);
        bound.RecordMember.ShouldBeNull();
        bound.ItemList.ShouldBeEmpty();
        // An explicit [ConfigurationKeyName] is used verbatim, not restyled.
        bound.LegacyName.ShouldBeNull();
        await Task.CompletedTask;
    }

    [Test]
    public async Task Derives_the_section_name_in_the_key_style()
    {
        // The suffix is removed first: DerivedDashSettings -> DerivedDash -> derived-dash, so the Pascal-case section is another one.
        var configuration = Build(("DerivedDash:max-connections", "7"), ("derived-dash:max-connections", "5"));

        Binders.Dovetail<DerivedDashSettings>(configuration).MaxConnections.ShouldBe(5);
        await Task.CompletedTask;
    }

    private static IConfigurationRoot Build(params (string Key, string? Value)[] pairs) =>
        new ConfigurationBuilder().AddInMemoryCollection(pairs.Select(z => KeyValuePair.Create(z.Key, z.Value))).Build();
}
