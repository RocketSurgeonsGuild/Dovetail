using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Joinery.Dovetail.Configuration.Runtime.Tests;

/// <summary>
///     Stand-in for a mutable configuration type with a public parameterless constructor: <see cref="Create" /> stands in for the
///     generated <c>Create…</c> method, which constructs it and then binds, setting scalars and appending list items the way the
///     configuration binder does.
/// </summary>
public sealed class GreetingsOptions
{
    public string Message { get; set; } = "";

    public List<string> Recipients { get; } = [];

    public static GreetingsOptions Create(IConfiguration configuration)
    {
        var options = new GreetingsOptions();
        if (configuration["Message"] is { } message) options.Message = message;

        foreach (var recipient in configuration.GetSection("Recipients").GetChildren())
        {
            if (recipient.Value is { } value) options.Recipients.Add(value);
        }

        return options;
    }
}

/// <summary>
///     Stand-in for an immutable configuration type the options pipeline can't create (no parameterless constructor):
///     <see cref="Create" /> stands in for the generated factory.
/// </summary>
public sealed record ServerRecord(string Host, int Port)
{
    public List<string> Tags { get; } = [];

    public string? Note { get; set; }

    public static ServerRecord Create(IConfiguration configuration)
    {
        var server = new ServerRecord(
            configuration["Host"] ?? "localhost",
            int.TryParse(configuration["Port"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var port) ? port : 80
        );
        foreach (var tag in configuration.GetSection("Tags").GetChildren())
        {
            if (tag.Value is { } value) server.Tags.Add(value);
        }

        return server;
    }
}

/// <summary>
///     Stand-in for a configuration type with init-only members: it has a parameterless constructor, but only a generated factory
///     can set <see cref="Url" />.
/// </summary>
public sealed class EndpointOptions
{
    public string Url { get; init; } = "http://localhost";

    public static EndpointOptions Create(IConfiguration configuration) => new() { Url = configuration["Url"] ?? "http://localhost" };
}

/// <summary>
///     Stand-in for a generated <c>[OptionsValidator]</c>.
/// </summary>
public sealed class ServerRecordValidator : IValidateOptions<ServerRecord>
{
    public ValidateOptionsResult Validate(string? name, ServerRecord options) =>
        options.Port is > 0 and <= 65535
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail($"Port must be between 1 and 65535, but was {options.Port}.");
}
