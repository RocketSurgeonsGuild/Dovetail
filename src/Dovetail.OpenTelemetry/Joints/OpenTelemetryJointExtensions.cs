using System.Runtime.CompilerServices;
using Dovetail.Joints;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Dovetail.OpenTelemetry.Joints;

/// <summary>
///     Extension methods for applying LoggerProvider parts.
/// </summary>
public static partial class OpenTelemetryJointExtensions
{
    extension(DovetailContextBuilder container)
    {
        /// <summary>
        ///     Configure the LoggerProvider delegate to the convention scanner.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="category">The category.</param>
        /// <param name="hostType">The host type.</param>
        /// <returns>The <see cref="DovetailContextBuilder" />, for chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
        public DovetailContextBuilder ConfigureLoggerProvider(LoggerProviderJointDelegate @delegate, int priority = 0, DovetailCategory? category = null, DovetailHostType hostType = DovetailHostType.Undefined, [CallerArgumentExpression(nameof(@delegate))] string? expression = null)
        {
            ArgumentNullException.ThrowIfNull(container);
            return container.ConfigureOpenTelemetry((context, builder) => builder.WithLogging(lp => @delegate(context, lp)), priority, category, hostType, expression);
        }

        /// <summary>
        ///     Configure the ResourceBuilder delegate to the convention scanner.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="category">The category.</param>
        /// <param name="hostType">The host type.</param>
        /// <returns>The <see cref="DovetailContextBuilder" />, for chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
        public DovetailContextBuilder ConfigureResourceBuilder(ResourceBuilderJointDelegate @delegate, int priority = 0, DovetailCategory? category = null, DovetailHostType hostType = DovetailHostType.Undefined, [CallerArgumentExpression(nameof(@delegate))] string? expression = null)
        {
            ArgumentNullException.ThrowIfNull(container);
            return container.ConfigureOpenTelemetry((context, builder) => builder.ConfigureResource(r => @delegate(context, r)), priority, category, hostType, expression);
        }
        /// <summary>
        ///     Configure the TracerProvider delegate to the convention scanner.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="category">The category.</param>
        /// <param name="hostType">The host type.</param>
        /// <returns>The <see cref="DovetailContextBuilder" />, for chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
        public DovetailContextBuilder ConfigureTracerProvider(TracerProviderJointDelegate @delegate, int priority = 0, DovetailCategory? category = null, DovetailHostType hostType = DovetailHostType.Undefined, [CallerArgumentExpression(nameof(@delegate))] string? expression = null)
        {
            ArgumentNullException.ThrowIfNull(container);
            return container.ConfigureOpenTelemetry((context, builder) => builder.WithTracing(t => @delegate(context, t)), priority, category, hostType, expression);
        }
        /// <summary>
        ///     Configure the MeterProvider delegate to the convention scanner.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="category">The category.</param>
        /// <param name="hostType">The host type.</param>
        /// <returns>The <see cref="DovetailContextBuilder" />, for chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
        public DovetailContextBuilder ConfigureMeterProvider(MeterProviderJointDelegate @delegate, int priority = 0, DovetailCategory? category = null, DovetailHostType hostType = DovetailHostType.Undefined, [CallerArgumentExpression(nameof(@delegate))] string? expression = null)
        {
            ArgumentNullException.ThrowIfNull(container);
            return container.ConfigureOpenTelemetry((context, builder) => builder.WithMetrics(m => @delegate(context, m)), priority, category, hostType, expression);
        }
    }
}
