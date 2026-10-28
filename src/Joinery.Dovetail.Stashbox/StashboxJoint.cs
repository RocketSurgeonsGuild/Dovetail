using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Infrastructure;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Stashbox;

// ReSharper disable once CheckNamespace

namespace Joinery.Dovetail.Stashbox;

/// <summary>
///  A joint that registers Stashbox services into the context.  This joint is automatically registered by the Dovetail generator, so it is not necessary to register this joint manually.
/// </summary>
[DovetailExport, ConnectsJoint<IStashboxJoint>]
public class StashboxJoint : IServiceAsyncJoint
{
    ValueTask IServiceAsyncJoint.Register(IDovetailContext context, IServiceCollection services, CancellationToken cancellationToken) => new StashboxContainer().ApplyStashbox(context, cancellationToken);
}
