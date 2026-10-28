using DryIoc;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace Joinery.Dovetail.DryIoc;

/// <summary>
///  A joint that registers DryIoc services into the context.  This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
[DovetailExport, ConnectsJoint<IDryIocJoint>]
public class DryIocJoint : IServiceAsyncJoint
{
    ValueTask IServiceAsyncJoint.Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken) => new Container().ApplyDryIoc(context, cancellationToken);
}
