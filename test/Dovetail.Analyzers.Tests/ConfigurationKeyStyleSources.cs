namespace Dovetail.Analyzers.Tests;

/// <summary>Option types declared in one key style, shared by the schema and the binder tests.</summary>
internal static class ConfigurationKeyStyleSources
{
    /// <summary>
    ///     A section with a nested object, a list of objects, dictionaries, a <c>[ConfigurationKeyName]</c> override, a record and
    ///     an init-only type, all in <paramref name="style" />.
    /// </summary>
    public static string Styled(string style) =>
        $$"""
          using System;
          using System.Collections.Generic;
          using Dovetail.Attributes;
          using Microsoft.Extensions.Configuration;

          namespace Sample;

          public enum RetryMode { ExponentialBackoff, FixedDelay }

          [DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.{{style}})]
          public class HttpClientOptions
          {
              public string BaseAddress { get; set; } = "";
              public bool UseSSL { get; set; }
              public int Http2Limit { get; set; }
              public RetryPolicy RetryPolicy { get; set; } = new();
              public List<EndpointSettings> BackupEndpoints { get; set; } = [];
              public Dictionary<string, int> TenantLimits { get; set; } = [];
              public Dictionary<RetryMode, string> ModeLabels { get; set; } = [];
              public Dictionary<string, EndpointSettings> NamedEndpoints { get; set; } = [];

              [ConfigurationKeyName("Legacy_Name")]
              public string? ExplicitName { get; set; }
          }

          public class RetryPolicy
          {
              public int MaxAttempts { get; set; } = 3;
              public TimeSpan InitialDelay { get; set; }
              public RetryMode BackoffMode { get; set; }
          }

          public class EndpointSettings
          {
              public string HostName { get; set; } = "";
              public int PortNumber { get; set; } = 80;
          }

          [DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.{{style}})]
          public record ServerSettings(string HostName, int PortNumber = 80, bool UseTls = false)
          {
              public List<string> AliasNames { get; init; } = [];
              public ServerLimits ConnectionLimits { get; init; } = new();
          }

          public class ServerLimits
          {
              public int MaxConnections { get; set; } = 100;
          }

          [DovetailConfiguration(KeyStyle = ConfigurationKeyStyle.{{style}})]
          public class InitOnlyConfig
          {
              public required string DisplayName { get; set; }
              public int RetryLimit { get; init; } = 5;
              public InitOnlyDetails MoreDetails { get; init; } = new();
          }

          public class InitOnlyDetails
          {
              public required string ContactEmail { get; init; }
              public string? Description { get; set; }
          }
          """;
}
