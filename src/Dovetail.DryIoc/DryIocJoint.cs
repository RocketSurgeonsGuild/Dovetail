using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Joints;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace Dovetail.DryIoc;

/// <summary>
///  A joint that registers DryIoc services into the context.  This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
[DovetailExport, ConnectsJoint<IDryIocJoint>]
public class DryIocJoint : IServiceAsyncJoint
{
    ValueTask IServiceAsyncJoint.Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken = default) => new Container().ApplyDryIoc(context, cancellationToken);
}
