using Autofac;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace Dovetail.Autofac;

/// <summary>
///  A joint that registers Autofac services into the context.  This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
[DovetailExport, ConnectsJoint<IAutofacJoint>]
public class AutofacJoint : IServiceAsyncJoint
{
    ValueTask IServiceAsyncJoint.Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken = default) => new ContainerBuilder().ApplyAutofac(context, cancellationToken);
}
