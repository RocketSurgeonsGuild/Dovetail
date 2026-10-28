using Joinery.Dovetail;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.Configuration;

namespace Sample.Uno;

/// <summary>
///     The greeting shown on the main page, read from the appsettings files.
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
