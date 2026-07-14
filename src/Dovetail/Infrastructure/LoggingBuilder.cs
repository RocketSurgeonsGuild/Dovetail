using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dovetail.Infrastructure;

internal class LoggingBuilder(IServiceCollection services) : ILoggingBuilder
{
    public IServiceCollection Services { get; } = services;
}
