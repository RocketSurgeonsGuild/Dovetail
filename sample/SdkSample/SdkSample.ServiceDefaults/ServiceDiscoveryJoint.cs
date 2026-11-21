using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace SdkSample.ServiceDefaults;

/// <summary>
///     Service discovery for every <see cref="System.Net.Http.HttpClient" />, and health checks. Under the AppHost, a resource that
///     references another gets its endpoints as configuration (<c>services:web:http:0</c>, and so on), so <c>https+http://web</c>
///     resolves to the web application.
/// </summary>
[DovetailExport]
public sealed class ServiceDiscoveryJoint : IServiceJoint
{
    /// <inheritdoc />
    public void Register(IDovetailContext context, IServiceCollection services)
    {
        services.AddServiceDiscovery();
        services.AddHttpClient();
        services.ConfigureHttpClientDefaults(static http => http.AddServiceDiscovery());
        services.AddHealthChecks();
    }
}
