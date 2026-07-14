using Autofac;
using Dovetail.Attributes;
using Dovetail.Joints;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;

namespace Dovetail.Autofac.Tests;

public static class AutofacFixtures
{
    public interface IAbc;

    public interface IAbc2;

    public interface IAbc3;

    public interface IAbc4;

    public interface IOtherAbc3;

    public interface IOtherAbc4;

    [DovetailExport]
    public class AbcDovetail : IAutofacJoint, IServiceJoint
    {
        public void Register(IDovetailContext context, ContainerBuilder builder) => builder.RegisterInstance(A.Fake<IAbc>());

        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(A.Fake<IAbc2>());
    }

    [DovetailExport]
    public class OtherDovetail : IServiceJoint
    {
        public void Register(IDovetailContext context, IServiceCollection services) => services.AddSingleton(A.Fake<IOtherAbc3>());
    }
}
