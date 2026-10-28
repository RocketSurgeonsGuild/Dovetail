using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
namespace Sample.Core.Databases;

#region codeblock

[DovetailExport]
public class DatabaseServiceJoint : IServiceAsyncJoint
{
    public async ValueTask Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken = default)
    {
        var configurator = new DatabaseConfigurator();
        await configurator.ApplyDatabaseConfigurator(context, cancellationToken: cancellationToken);
    }
}

#endregion
