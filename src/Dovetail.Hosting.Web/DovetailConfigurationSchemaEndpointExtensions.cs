using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace Dovetail.Hosting.Web;

/// <summary>
///     Serves the application's merged configuration JSON Schema (<c>appsettings.schema.json</c>) over HTTP.
/// </summary>
/// <remarks>
///     Applications do not call this directly. Setting the MSBuild property <c>DovetailConfigurationSchemaEndpoint</c> to
///     <c>true</c> generates a joint that maps the endpoint; <c>DovetailConfigurationSchemaEndpointEnvironments</c> restricts it
///     to the listed environments. The implementation uses a plain <see cref="RequestDelegate" />, so it does not rely on
///     reflection and is trimming and AOT friendly.
/// </remarks>
[PublicAPI]
public static class DovetailConfigurationSchemaEndpointExtensions
{
    /// <summary>
    ///     The route the schema is served from.
    /// </summary>
    public const string RoutePattern = "/.well-known/dovetail/configuration-schema";

    /// <summary>
    ///     The name of the schema file, in the application's output directory and in <c>obj/</c>.
    /// </summary>
    public const string SchemaFileName = "appsettings.schema.json";

    /// <summary>
    ///     The media type the schema is served as.
    /// </summary>
    public const string SchemaMediaType = "application/schema+json";

    /// <summary>
    ///     Maps <c>GET /.well-known/dovetail/configuration-schema</c>, which serves <c>appsettings.schema.json</c> from the application's
    ///     output directory (falling back to <paramref name="fallbackDirectory" />). Responds <c>404</c> when no file exists, and supports
    ///     <c>ETag</c> and <c>If-None-Match</c>.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder, typically the <c>WebApplication</c>.</param>
    /// <param name="fallbackDirectory">
    ///     A directory to look in when the output directory has no schema file, typically the project's <c>obj/</c> directory when
    ///     running from source.
    /// </param>
    /// <param name="environments">
    ///     Semicolon-separated host environment names. When set, the endpoint is only mapped when the current environment is one of them.
    /// </param>
    /// <returns>The endpoint convention builder, or <see langword="null" /> when <paramref name="environments" /> excludes the current environment.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="endpoints" /> is <see langword="null" />.</exception>
    public static IEndpointConventionBuilder? MapDovetailConfigurationSchema(
        this IEndpointRouteBuilder endpoints,
        string? fallbackDirectory = null,
        string? environments = null
    ) => Map(endpoints, AppContext.BaseDirectory, fallbackDirectory, environments);

    internal static IEndpointConventionBuilder? Map(IEndpointRouteBuilder endpoints, string baseDirectory, string? fallbackDirectory, string? environments)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        ArgumentNullException.ThrowIfNull(baseDirectory);

        if (!IsEnabledFor(endpoints.ServiceProvider.GetRequiredService<IHostEnvironment>(), environments)) return null;

        var schema = new SchemaFileCache(baseDirectory, fallbackDirectory);
        return endpoints.MapGet(RoutePattern, (RequestDelegate)schema.HandleAsync);
    }

    private static bool IsEnabledFor(IHostEnvironment environment, string? environments)
    {
        if (string.IsNullOrWhiteSpace(environments)) return true;

        foreach (var name in environments.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (environment.IsEnvironment(name)) return true;
        }

        return false;
    }

    private sealed class SchemaFileCache(string baseDirectory, string? fallbackDirectory)
    {
        private LoadedSchema? _loaded;

        public async Task HandleAsync(HttpContext context)
        {
            if (Load() is not { } schema)
            {
                await Results.NotFound().ExecuteAsync(context).ConfigureAwait(false);
                return;
            }

            // Revalidate on every request: the file changes with each build, and the ETag makes that cheap.
            context.Response.Headers.CacheControl = "no-cache";
            await Results
                 .Bytes(schema.Content, SchemaMediaType, entityTag: schema.ETag)
                 .ExecuteAsync(context)
                 .ConfigureAwait(false);
        }

        private LoadedSchema? Load()
        {
            var file = Locate();
            if (file is null) return null;

            var cached = _loaded;
            if (cached is not null && cached.Matches(file)) return cached;

            try
            {
                var content = File.ReadAllBytes(file.FullName);
                var loaded = new LoadedSchema(file.FullName, file.LastWriteTimeUtc, file.Length, content);
                _loaded = loaded;
                return loaded;
            }
            catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
            {
                // Removed between the existence check and the read, e.g. by a rebuild.
                return null;
            }
        }

        private FileInfo? Locate()
        {
            foreach (var directory in (ReadOnlySpan<string?>)[baseDirectory, fallbackDirectory])
            {
                if (string.IsNullOrEmpty(directory)) continue;
                var file = new FileInfo(Path.Combine(directory, SchemaFileName));
                if (file.Exists) return file;
            }

            return null;
        }
    }

    private sealed class LoadedSchema
    {
        private readonly string _path;
        private readonly DateTime _lastWriteTimeUtc;
        private readonly long _length;

        public LoadedSchema(string path, DateTime lastWriteTimeUtc, long length, byte[] content)
        {
            _path = path;
            _lastWriteTimeUtc = lastWriteTimeUtc;
            _length = length;
            Content = content;
            // A strong validator over the bytes served: the same schema yields the same ETag across restarts and instances.
            ETag = new EntityTagHeaderValue($"\"{Convert.ToHexStringLower(SHA256.HashData(content).AsSpan(0, 16))}\"");
        }

        public byte[] Content { get; }

        public EntityTagHeaderValue ETag { get; }

        public bool Matches(FileInfo file) =>
            _length == file.Length && _lastWriteTimeUtc == file.LastWriteTimeUtc && string.Equals(_path, file.FullName, StringComparison.Ordinal);
    }
}
