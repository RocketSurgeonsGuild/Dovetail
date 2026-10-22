using Dovetail.Attributes;
using Dovetail.Joints;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Stashbox;

namespace Dovetail.Stashbox.Tests;

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
        public void Register(IDovetailContext context, IStashboxContainer container) => container.RegisterInstance(A.Fake<IAbc>());

        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(A.Fake<IAbc2>());
    }

    [DovetailExport]
    public class OtherDovetail : IServiceJoint
    {
        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(A.Fake<IOtherAbc3>());
    }
}
