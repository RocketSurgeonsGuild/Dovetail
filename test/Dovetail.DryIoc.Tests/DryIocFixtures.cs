using Dovetail.Attributes;
using Dovetail.Joints;
using DryIoc;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;

namespace Dovetail.DryIoc.Tests;

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
        public void Register(IDovetailContext context, IContainer container) => container.RegisterInstance(A.Fake<IAbc>());

        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(A.Fake<IAbc2>());
    }

    [DovetailExport]
    public class OtherDovetail : IServiceJoint
    {
        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(A.Fake<IOtherAbc3>());
    }
}
