using Dovetail;
using Dovetail.Attributes;
using Dovetail.Joints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Maui;

/// <summary>
///     The greeting shown on the main page, read from the embedded appsettings files.
/// </summary>
public sealed class Greeting(IConfiguration configuration)
{
    public string Text => configuration["Sample:Greeting"] ?? "No greeting configured";
}

/// <summary>
///     A Dovetail convention in the app itself, registering the greeting.
/// </summary>
[DovetailExport]
public sealed class GreetingJoint : IServiceJoint
{
    public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton<Greeting>();
}
