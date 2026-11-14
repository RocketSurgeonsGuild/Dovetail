using DryIoc;
using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;

namespace Joinery.Dovetail.DryIoc.Tests;

public static class DryIocFixtures
{
    public interface IAbc;

    public interface IAbc2;

    public interface IAbc3;

    public interface IAbc4;

    public interface IOtherAbc3;

    public interface IOtherAbc4;

    [DovetailExport]
    public class AbcDovetail : IDryIocJoint, IServiceJoint
    {
        public void Register(IDovetailContext context, IContainer container) => container.RegisterInstance(IAbc.Mock().Object);

        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(IAbc2.Mock().Object);
    }

    [DovetailExport]
    public class OtherDovetail : IServiceJoint
    {
        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(IOtherAbc3.Mock().Object);
    }
}
