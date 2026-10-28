using Joinery.Dovetail.Attributes;
using Microsoft.Extensions.Configuration;

namespace Joinery.Dovetail.Configuration.Parity.Tests;

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

/// <summary>A section in underscore case: the same shapes as <see cref="CamelStyled" />, keyed with underscores.</summary>
[DovetailConfiguration("Styled:Underscore", KeyStyle = ConfigurationKeyStyle.Underscore)]
public class UnderscoreStyled
{
    public string? DisplayName { get; set; }
    public UnderscoreInner InnerObject { get; set; } = new();
    public UnderscoreRecord? RecordMember { get; set; }
    public List<UnderscoreItem> ItemList { get; set; } = [];
    public Dictionary<string, int> TenantLimits { get; set; } = [];

    [ConfigurationKeyName("Legacy-Name")]
    public string? LegacyName { get; set; }
}

public class UnderscoreInner
{
    public int RetryCount { get; set; }
    public string? HostName { get; set; }
}

public record UnderscoreRecord(string HostName, int PortNumber = 80);

public class UnderscoreItem
{
    public string? ItemName { get; set; }
    public int ItemWeight { get; set; }
}

/// <summary>Declared names that start in lower case, kept as written: <see cref="ConfigurationKeyStyle.Undefined" /> is the default.</summary>
[DovetailConfiguration("Styled:Undefined")]
public class UndefinedLower
{
    public string? myKey { get; set; }
    public UndefinedLowerInner inner { get; set; } = new();
    public List<UndefinedLowerItem> itemList { get; set; } = [];
}

public class UndefinedLowerInner
{
    public int waitTime { get; set; }
}

public class UndefinedLowerItem
{
    public string? itemName { get; set; }
}

[DovetailConfiguration("Styled:UndefinedRecord")]
public record UndefinedLowerRecord(string hostName, int portNumber = 80);

/// <summary>The same declared names, with their first letter upper-cased by <see cref="ConfigurationKeyStyle.Pascal" />.</summary>
[DovetailConfiguration("Styled:Pascal", KeyStyle = ConfigurationKeyStyle.Pascal)]
public class PascalLower
{
    public string? myKey { get; set; }
    public PascalLowerInner inner { get; set; } = new();
    public List<PascalLowerItem> itemList { get; set; } = [];
}

public class PascalLowerInner
{
    public int waitTime { get; set; }
}

public class PascalLowerItem
{
    public string? itemName { get; set; }
}

[DovetailConfiguration("Styled:PascalRecord", KeyStyle = ConfigurationKeyStyle.Pascal)]
public record PascalLowerRecord(string hostName, int portNumber = 80);

/// <summary>The default section name follows the key style: <c>derived-dash</c>, not <c>DerivedDash</c>.</summary>
[DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Dash)]
public class DerivedDashSettings
{
    public int MaxConnections { get; set; }
}

/// <summary>The default section name follows the key style: <c>derived_underscore</c>, not <c>DerivedUnderscore</c>.</summary>
[DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.Underscore)]
public class DerivedUnderscoreSettings
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
    public async Task Binds_underscore_case_data()
    {
        var configuration = Build(
            ("Styled:Underscore:display_name", "shown"),
            ("Styled:Underscore:inner_object:retry_count", "4"),
            ("Styled:Underscore:inner_object:host_name", "inner-host"),
            ("Styled:Underscore:record_member:host_name", "record-host"),
            ("Styled:Underscore:record_member:port_number", "8080"),
            ("Styled:Underscore:item_list:0:item_name", "first"),
            ("Styled:Underscore:item_list:0:item_weight", "1"),
            ("Styled:Underscore:item_list:1:item_name", "second"),
            ("Styled:Underscore:item_list:1:item_weight", "2"),
            ("Styled:Underscore:tenant_limits:AcmeCorp", "3"),
            ("Styled:Underscore:tenant_limits:beta_corp", "5"),
            ("Styled:Underscore:Legacy-Name", "legacy")
        );

        var bound = Binders.Dovetail<UnderscoreStyled>(configuration);

        bound.DisplayName.ShouldBe("shown");
        bound.InnerObject.RetryCount.ShouldBe(4);
        bound.InnerObject.HostName.ShouldBe("inner-host");
        bound.RecordMember.ShouldBe(new UnderscoreRecord("record-host", 8080));
        bound.ItemList.Select(z => (z.ItemName, z.ItemWeight)).ShouldBe([("first", 1), ("second", 2)]);
        // Dictionary keys are data: neither restyled nor split.
        bound.TenantLimits.ShouldBe(new Dictionary<string, int> { ["AcmeCorp"] = 3, ["beta_corp"] = 5 });
        bound.LegacyName.ShouldBe("legacy");
        await Task.CompletedTask;
    }

    [Test]
    public async Task Does_not_bind_pascal_case_data_in_underscore_case_because_the_keys_differ()
    {
        var configuration = Build(
            ("Styled:Underscore:DisplayName", "shown"),
            ("Styled:Underscore:InnerObject:RetryCount", "4"),
            ("Styled:Underscore:RecordMember:HostName", "record-host"),
            ("Styled:Underscore:RecordMember:PortNumber", "8080"),
            ("Styled:Underscore:ItemList:0:ItemName", "first"),
            ("Styled:Underscore:ItemList:0:ItemWeight", "1"),
            // Dashes are other letters than underscores.
            ("Styled:Underscore:display-name", "dashed"),
            ("Styled:Underscore:legacy_name", "not the explicit key")
        );

        var bound = Binders.Dovetail<UnderscoreStyled>(configuration);

        bound.DisplayName.ShouldBeNull();
        bound.InnerObject.RetryCount.ShouldBe(0);
        bound.RecordMember.ShouldBeNull();
        bound.ItemList.ShouldBeEmpty();
        bound.LegacyName.ShouldBeNull();
        await Task.CompletedTask;
    }

    [Test]
    [Arguments("Undefined")]
    [Arguments("Pascal")]
    public async Task Binds_lower_case_declared_names_in_any_casing_when_undefined_or_pascal(string style)
    {
        // Keys ignore case, so the canonical spelling (myKey as declared, MyKey from Pascal) doesn't matter for the data.
        Func<string, string>[] casings = [z => z, z => char.ToUpperInvariant(z[0]) + z[1..], z => z.ToUpperInvariant(), z => z.ToLowerInvariant()];
        for (var index = 0; index < casings.Length; index++)
        {
            var casing = casings[index];
            var configuration = Build(
                ($"Styled:{style}:{casing("myKey")}", "value"),
                ($"Styled:{style}:{casing("inner")}:{casing("waitTime")}", "7"),
                ($"Styled:{style}:{casing("itemList")}:0:{casing("itemName")}", "first"),
                ($"Styled:{style}Record:{casing("hostName")}", "record-host"),
                ($"Styled:{style}Record:{casing("portNumber")}", "8080")
            );

            if (style == "Undefined")
            {
                var bound = Binders.Dovetail<UndefinedLower>(configuration);
                (bound.myKey, bound.inner.waitTime, bound.itemList.Single().itemName).ShouldBe(("value", 7, "first"), $"casing {index}");
                Binders.Dovetail<UndefinedLowerRecord>(configuration).ShouldBe(new UndefinedLowerRecord("record-host", 8080), $"casing {index}");
            }
            else
            {
                var bound = Binders.Dovetail<PascalLower>(configuration);
                (bound.myKey, bound.inner.waitTime, bound.itemList.Single().itemName).ShouldBe(("value", 7, "first"), $"casing {index}");
                Binders.Dovetail<PascalLowerRecord>(configuration).ShouldBe(new PascalLowerRecord("record-host", 8080), $"casing {index}");
            }
        }

        await Task.CompletedTask;
    }

    [Test]
    public async Task Derives_the_underscore_section_name()
    {
        var configuration = Build(("DerivedUnderscore:max_connections", "7"), ("derived_underscore:max_connections", "5"), ("derived-underscore:max_connections", "9"));

        Binders.Dovetail<DerivedUnderscoreSettings>(configuration).MaxConnections.ShouldBe(5);
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
