using Dovetail.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS8601 // Possible null reference assignment.

namespace Dovetail;

/// <summary>
///     Base convention extensions
/// </summary>
[PublicAPI]
public static class DovetailHostBuilderExtensions
{
    /// <summary>
    ///     Set the service provider factory to be used for hosting or other systems.
    /// </summary>
    /// <param name="builder">The context builder to configure.</param>
    /// <param name="serviceProviderFactory">The service provider factory to use.</param>
    /// <returns>The <paramref name="builder" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static DovetailContextBuilder UseServiceProviderFactory<TContainerBuilder>(
        this DovetailContextBuilder builder,
        IServiceProviderFactory<TContainerBuilder> serviceProviderFactory
    ) where TContainerBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Set<ServiceProviderFactoryAdapter>((_, _, _) => ValueTask.FromResult<IServiceProviderFactory<object>>(new ServiceProviderWrapper<TContainerBuilder>(serviceProviderFactory)));
        return builder;
    }

    /// <summary>
    ///     Set the service provider factory to be used for hosting or other systems.
    /// </summary>
    /// <param name="builder">The context builder to configure.</param>
    /// <param name="serviceProviderFactory">A factory method that asynchronously produces the service provider factory to use.</param>
    /// <returns>The <paramref name="builder" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static DovetailContextBuilder UseServiceProviderFactory<TContainerBuilder>(
        this DovetailContextBuilder builder,
        Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>> serviceProviderFactory
    ) where TContainerBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Set<ServiceProviderFactoryAdapter>(async (context, collection, cancellationToken) => new ServiceProviderWrapper<TContainerBuilder>(await serviceProviderFactory(context, collection, cancellationToken).ConfigureAwait(false)));
        return builder;
    }

    /// <summary>
    ///     Set the service provider factory to be used for hosting or other systems.
    /// </summary>
    /// <param name="builder">The context builder to configure.</param>
    /// <param name="serviceProviderFactory">A factory method that asynchronously produces the service provider factory to use.</param>
    /// <returns>The <paramref name="builder" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static DovetailContextBuilder UseServiceProviderFactory<TContainerBuilder>(
        this DovetailContextBuilder builder,
        Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>> serviceProviderFactory
    ) where TContainerBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Set<ServiceProviderFactoryAdapter>(async (context, collection, _) => new ServiceProviderWrapper<TContainerBuilder>(await serviceProviderFactory(context, collection).ConfigureAwait(false)));
        return builder;
    }

    /// <summary>
    ///     Set the service provider factory to be used for hosting or other systems.
    /// </summary>
    /// <param name="builder">The context to configure.</param>
    /// <param name="serviceProviderFactory">The service provider factory to use.</param>
    /// <returns>The <paramref name="builder" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static IDovetailContext UseServiceProviderFactory<TContainerBuilder>(
        this IDovetailContext builder,
        IServiceProviderFactory<TContainerBuilder> serviceProviderFactory
    ) where TContainerBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Set<ServiceProviderFactoryAdapter>((_, _, _) => ValueTask.FromResult<IServiceProviderFactory<object>>(new ServiceProviderWrapper<TContainerBuilder>(serviceProviderFactory)));
        return builder;
    }

    /// <summary>
    ///     Set the service provider factory to be used for hosting or other systems.
    /// </summary>
    /// <param name="builder">The context to configure.</param>
    /// <param name="serviceProviderFactory">A factory method that asynchronously produces the service provider factory to use.</param>
    /// <returns>The <paramref name="builder" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static IDovetailContext UseServiceProviderFactory<TContainerBuilder>(
        this IDovetailContext builder,
        Func<IDovetailContext, IServiceCollection, CancellationToken, ValueTask<IServiceProviderFactory<TContainerBuilder>>> serviceProviderFactory
    ) where TContainerBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Set<ServiceProviderFactoryAdapter>(async (context, collection, cancellationToken) => new ServiceProviderWrapper<TContainerBuilder>(await serviceProviderFactory(context, collection, cancellationToken).ConfigureAwait(false)));
        return builder;
    }

    /// <summary>
    ///     Set the service provider factory to be used for hosting or other systems.
    /// </summary>
    /// <param name="builder">The context to configure.</param>
    /// <param name="serviceProviderFactory">A factory method that asynchronously produces the service provider factory to use.</param>
    /// <returns>The <paramref name="builder" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static IDovetailContext UseServiceProviderFactory<TContainerBuilder>(
        this IDovetailContext builder,
        Func<IDovetailContext, IServiceCollection, ValueTask<IServiceProviderFactory<TContainerBuilder>>> serviceProviderFactory
    ) where TContainerBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Set<ServiceProviderFactoryAdapter>(async (context, collection, _) => new ServiceProviderWrapper<TContainerBuilder>(await serviceProviderFactory(context, collection).ConfigureAwait(false)));
        return builder;
    }

    /// <summary>
    ///     Get a value by type from the context
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="context">The context</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static T? Get<T>(this DovetailContextBuilder context)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(context);

        return (T?)context.Properties[typeof(T)];
    }

    /// <summary>
    ///     Get a value by type from the context or throw
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="context">The context</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="KeyNotFoundException">No value of type <typeparamref name="T" /> was found in the context.</exception>
    public static T Require<T>(this DovetailContextBuilder context)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);

        return context.Properties.TryGetValue(typeof(T), out var value) && value is T t
            ? t
            : throw new KeyNotFoundException($"The value of type {typeof(T).Name} was not found in the context");
    }

    /// <summary>
    ///     Get a value by key from the context
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="context">The context</param>
    /// <param name="key">The key where the value is saved</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static T? Get<T>(this DovetailContextBuilder context, string key)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(context);

        return (T?)context.Properties[key];
    }

    /// <summary>
    ///     Get a value by type from the context or throw
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="context">The context</param>
    /// <param name="key">The key where the value is saved</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="KeyNotFoundException">No value of type <typeparamref name="T" /> was found at <paramref name="key" />.</exception>
    public static T Require<T>(this DovetailContextBuilder context, string key)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);

        return context.Properties.TryGetValue(key, out var value) && value is T t
            ? t
            : throw new KeyNotFoundException($"The value of type {typeof(T).Name} with the {key} was not found in the context");
    }

    /// <summary>
    ///     Get a value by key from the context
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="builder">The builder</param>
    /// <param name="factory">The factory method in the event the type is not found</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> or <paramref name="factory" /> is <see langword="null" />.</exception>
    public static T GetOrAdd<T>(this DovetailContextBuilder builder, Func<T> factory)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(builder);

        ArgumentNullException.ThrowIfNull(factory);

        if (builder.Properties[typeof(T)] is T value) return value;

        value = factory();
        builder.Set(value);

        return value;
    }

    /// <summary>
    ///     Get a value by key from the context
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="builder">The builder</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="factory">The factory method in the event the type is not found</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> or <paramref name="factory" /> is <see langword="null" />.</exception>
    public static T GetOrAdd<T>(this DovetailContextBuilder builder, string key, Func<T> factory)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(builder);

        ArgumentNullException.ThrowIfNull(factory);

        if (builder.Properties[key] is not T value)
        {
            value = factory();
            builder.Set(value);
        }

        return value;
    }

    /// <summary>
    ///     Get a value by type from the context
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="context">The context</param>
    /// <param name="value">The value to save</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static DovetailContextBuilder Set<T>(this DovetailContextBuilder context, T value)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Properties[typeof(T)] = value;
        return context;
    }

    /// <summary>
    ///     Get a value by type from the context
    /// </summary>
    /// <param name="context">The context</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="value">The value to save</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static DovetailContextBuilder Set(this DovetailContextBuilder context, Type key, object value)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Properties[key] = value;
        return context;
    }

    /// <summary>
    ///     Get a value by type from the context
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="context">The context</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="value">The value to save</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static DovetailContextBuilder Set<T>(this DovetailContextBuilder context, string key, T value)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Properties[key] = value;
        return context;
    }

    /// <summary>
    ///     Set key to the value if the type is missing
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="builder">The builder</param>
    /// <param name="value">The value to save</param>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static DovetailContextBuilder AddIfMissing<T>(this DovetailContextBuilder builder, T value) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Properties.AddIfMissing(value);
        return builder;
    }

    /// <summary>
    ///     Set key to the value if the key is missing
    /// </summary>
    /// <param name="builder">The builder</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="value">The value to save</param>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static DovetailContextBuilder AddIfMissing(this DovetailContextBuilder builder, Type key, object value)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Properties.AddIfMissing(key, value);
        return builder;
    }

    /// <summary>
    ///     Set key to the value if the key is missing
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="builder">The builder</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="value">The value to save</param>
    /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
    public static DovetailContextBuilder AddIfMissing<T>(this DovetailContextBuilder builder, string key, T value) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Properties.AddIfMissing(key, value);
        return builder;
    }

    /// <summary>
    ///     Check if this is a test host (to allow conventions to behave differently during unit tests)
    /// </summary>
    /// <param name="context">The context</param>
    /// <returns><see langword="true" /> if <paramref name="context" /> is running under <see cref="DovetailHostType.UnitTest" />; otherwise, <see langword="false" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static bool IsUnitTestHost(this DovetailContextBuilder context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return context.GetHostType() == DovetailHostType.UnitTest;
    }

    /// <summary>
    ///     Get the host type the builder is currently configured for
    /// </summary>
    /// <param name="context">The context</param>
    /// <returns>The <see cref="DovetailHostType" /> for the builder, or <see cref="DovetailHostType.Undefined" /> if none was registered.</returns>
    public static DovetailHostType GetHostType(this DovetailContextBuilder context)
    {
        return context.Properties.TryGetValue(typeof(DovetailHostType), out var hostType)
         && ( hostType is DovetailHostType ht || ( hostType is string str && Enum.TryParse(str, true, out ht) ) )
                ? ht
                : DovetailHostType.Undefined;
    }

    private class ServiceProviderWrapper<TContainerBuilder>
        (IServiceProviderFactory<TContainerBuilder> serviceProviderFactoryImplementation) : IServiceProviderFactory<object>
        where TContainerBuilder : notnull
    {
        public object CreateBuilder(IServiceCollection services) => serviceProviderFactoryImplementation.CreateBuilder(services);

        public IServiceProvider CreateServiceProvider(object containerBuilder) => serviceProviderFactoryImplementation.CreateServiceProvider((TContainerBuilder)containerBuilder);
    }
}
