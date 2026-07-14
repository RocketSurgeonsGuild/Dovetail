using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dovetail.DryIoc.Tests;

internal static class AppExtensions
{
    public static IContainer GetLifetimeScope(this IHost host) => host.Services.GetRequiredService<IContainer>();
}
