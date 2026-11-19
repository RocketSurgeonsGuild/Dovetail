using Joinery.Dovetail.Attributes;
using Joinery.Dovetail.Joints;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Joinery.Dovetail.Hosting.AzureFunctions.Tests;

public static class FunctionsFixtures
{
    public interface IFromHostApplicationJoint;

    public interface IFromFunctionsApplicationBuilderJoint;

    public interface IFromFunctionsWorkerJoint;

    public sealed class FromHostApplicationJoint : IFromHostApplicationJoint;

    public sealed class FromFunctionsApplicationBuilderJoint : IFromFunctionsApplicationBuilderJoint;

    public sealed class FromFunctionsWorkerJoint : IFromFunctionsWorkerJoint;

    /// <summary>
    ///     What the exported joints saw on their most recent run, for assertions. Tests that read it don't run in parallel.
    /// </summary>
    public static class Observed
    {
        public static FunctionsApplicationBuilder? Builder { get; set; }
        public static IFunctionsWorkerApplicationBuilder? WorkerBuilder { get; set; }
        public static IFunctionsWorkerApplicationBuilder? ContextWorkerBuilder { get; set; }
    }

    /// <summary>
    ///     A joint for any <see cref="IHostApplicationBuilder" />, which runs for Azure Functions too since <see cref="FunctionsApplicationBuilder" />
    ///     is one.
    /// </summary>
    [DovetailExport]
    public class HostApplicationJoint : IHostApplicationJoint<IHostApplicationBuilder>
    {
        public void Register(IDovetailContext context, IHostApplicationBuilder builder) =>
            builder.Services.AddSingleton<IFromHostApplicationJoint, FromHostApplicationJoint>();
    }

    [DovetailExport]
    public class FunctionsApplicationBuilderJoint : IHostApplicationJoint<FunctionsApplicationBuilder>
    {
        public void Register(IDovetailContext context, FunctionsApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IFromFunctionsApplicationBuilderJoint, FromFunctionsApplicationBuilderJoint>();
            Observed.Builder = builder;
        }
    }

    /// <summary>
    ///     A joint for the worker builder alone, as a library that adds Functions middleware would write it.
    /// </summary>
    [DovetailExport]
    public class FunctionsWorkerJoint : IHostApplicationJoint<IFunctionsWorkerApplicationBuilder>
    {
        public void Register(IDovetailContext context, IFunctionsWorkerApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IFromFunctionsWorkerJoint, FromFunctionsWorkerJoint>();
            builder.UseMiddleware<MarkerMiddleware>();
            Observed.WorkerBuilder = builder;
            Observed.ContextWorkerBuilder = context.Get<IFunctionsWorkerApplicationBuilder>();
        }
    }

    public sealed class MarkerMiddleware : IFunctionsWorkerMiddleware
    {
        public Task Invoke(FunctionContext context, FunctionExecutionDelegate next) => next(context);
    }
}
