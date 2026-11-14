using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Extensions.DependencyInjection;
using Stashbox;

namespace Joinery.Dovetail.Stashbox.Tests;

public static class StashboxFixtures
{
    public interface IAbc;

    public interface IAbc2;

    public interface IAbc3;

    public interface IAbc4;

    public interface IOtherAbc3;

    public interface IOtherAbc4;

    [DovetailExport]
    public class AbcDovetail : IStashboxJoint, IServiceJoint
    {
        public void Register(IDovetailContext context, IStashboxContainer container) => container.RegisterInstance(IAbc.Mock().Object);

        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(IAbc2.Mock().Object);
    }

    [DovetailExport]
    public class OtherDovetail : IServiceJoint
    {
        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(IOtherAbc3.Mock().Object);
    }
}
