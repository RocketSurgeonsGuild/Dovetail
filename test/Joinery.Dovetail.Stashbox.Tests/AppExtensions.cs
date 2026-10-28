using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stashbox;

namespace Joinery.Dovetail.Stashbox.Tests;

internal static class AppExtensions
{
    public static IStashboxContainer GetLifetimeScope(this IHost host) => host.Services.GetRequiredService<IStashboxContainer>();
}
