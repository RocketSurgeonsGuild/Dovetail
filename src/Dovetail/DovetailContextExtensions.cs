using Dovetail.Infrastructure;

#pragma warning disable CS8601 // Possible null reference assignment.

namespace Dovetail;

/// <summary>
///     Base convention extensions
/// </summary>
[PublicAPI]
public static class DovetailContextExtensions
{
    /// <summary>
    ///     Set key to the value if the type is missing
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="context">The context</param>
    /// <param name="value">The value to save</param>
    /// <returns>The <paramref name="context" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static IDovetailContext AddIfMissing<T>(this IDovetailContext context, T value) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Properties.AddIfMissing(value);
        return context;
    }

    /// <summary>
    ///     Set key to the value if the key is missing
    /// </summary>
    /// <param name="context">The properties</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="value">The value to save</param>
    /// <returns>The <paramref name="context" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static IDovetailContext AddIfMissing(this IDovetailContext context, Type key, object value)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Properties.AddIfMissing(key, value);
        return context;
    }

    /// <summary>
    ///     Set key to the value if the key is missing
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="context">The properties</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="value">The value to save</param>
    /// <returns>The <paramref name="context" />, for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static IDovetailContext AddIfMissing<T>(this IDovetailContext context, string key, T value) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Properties.AddIfMissing(key, value);
        return context;
    }

    /// <summary>
    ///     Get a value by type from the context
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="context">The context</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static T? Get<T>(this IDovetailContext context) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Properties.Get<T>();
    }

    /// <summary>
    ///     Get a value by key from the context
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="context">The context</param>
    /// <param name="key">The key where the value is saved</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static T? Get<T>(this IDovetailContext context, string key) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Properties.Get<T>(key);
    }

    /// <summary>
    ///     Get the host type the context is currently running under
    /// </summary>
    /// <param name="context">The context</param>
    /// <returns>The <see cref="DovetailHostType" /> for the context, or <see cref="DovetailHostType.Undefined" /> if none was registered.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static DovetailHostType GetHostType(this IDovetailContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Properties.TryGetValue(typeof(DovetailHostType), out var hostType)
         && ( hostType is DovetailHostType ht || ( hostType is string str && Enum.TryParse(str, true, out ht) ) )
                ? ht
                : DovetailHostType.Undefined;
    }

    /// <summary>
    ///     Get a value by key from the context
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="context">The context</param>
    /// <param name="factory">The factory method in the event the type is not found</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> or <paramref name="factory" /> is <see langword="null" />.</exception>
    public static T GetOrAdd<T>(this IDovetailContext context, Func<T> factory)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(factory);
        return context.Properties.GetOrAdd(factory);
    }

    /// <summary>
    ///     Get a value by key from the context
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="context">The context</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="factory">The factory method in the event the type is not found</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> or <paramref name="factory" /> is <see langword="null" />.</exception>
    public static T GetOrAdd<T>(this IDovetailContext context, string key, Func<T> factory)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(factory);
        return context.Properties.GetOrAdd(key, factory);
    }

    /// <summary>
    ///     Check if this is a test host (to allow conventions to behave differently during unit tests)
    /// </summary>
    /// <param name="context">The context</param>
    /// <returns><see langword="true" /> if <paramref name="context" /> is running under <see cref="DovetailHostType.UnitTest" />; otherwise, <see langword="false" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    public static bool IsUnitTestHost(this IDovetailContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.GetHostType() == DovetailHostType.UnitTest;
    }

    /// <summary>
    ///     Register a set of conventions
    /// </summary>
    /// <param name="context">The context to execute the conventions against.</param>
    /// <param name="configure">A callback used to configure which joints the <see cref="DovetailExecutor" /> should execute.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> or <paramref name="configure" /> is <see langword="null" />.</exception>
    public static void RegisterJoints(this IDovetailContext context, Action<DovetailExecutor> configure)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(configure);
        var executor = new DovetailExecutor(context);
        configure(executor);
        executor.Execute();
    }

    /// <summary>
    ///     Register a set of conventions
    /// </summary>
    /// <param name="context">The context to execute the conventions against.</param>
    /// <param name="configure">A callback used to configure which joints the <see cref="DovetailExecutor" /> should execute.</param>
    /// <returns>The <paramref name="context" /> after executing the conventions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> or <paramref name="configure" /> is <see langword="null" />.</exception>
    public static IDovetailContext RegisterJointsWithContext(this IDovetailContext context, Action<DovetailExecutor> configure)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(configure);
        var executor = new DovetailExecutor(context);
        configure(executor);
        return executor.ExecuteWithContext();
    }


    /// <summary>
    ///     Register a set of conventions
    /// </summary>
    /// <param name="context">The context to execute the conventions against.</param>
    /// <param name="configure">A callback used to configure which joints the <see cref="DovetailExecutor" /> should execute.</param>
    /// <param name="cancellationToken">The cancellation token used during execution.</param>
    /// <returns>A <see cref="ValueTask" /> representing the asynchronous execution of the conventions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> or <paramref name="configure" /> is <see langword="null" />.</exception>
    public static ValueTask RegisterJointsAsync(this IDovetailContext context, Action<DovetailExecutor> configure, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(configure);
        var executor = new DovetailExecutor(context);
        configure(executor);
        return executor.ExecuteAsync(cancellationToken);
    }


    /// <summary>
    ///     Register a set of conventions
    /// </summary>
    /// <param name="context">The context to execute the conventions against.</param>
    /// <param name="configure">A callback used to configure which joints the <see cref="DovetailExecutor" /> should execute.</param>
    /// <param name="cancellationToken">The cancellation token used during execution.</param>
    /// <returns>A <see cref="ValueTask{TResult}" /> producing the <paramref name="context" /> after executing the conventions.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> or <paramref name="configure" /> is <see langword="null" />.</exception>
    public static ValueTask<IDovetailContext> RegisterJointsWithContextAsync(this IDovetailContext context, Action<DovetailExecutor> configure, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(configure);
        var executor = new DovetailExecutor(context);
        configure(executor);
        return executor.ExecuteWithContextAsync(cancellationToken);
    }

    /// <summary>
    ///     Get a value by type from the context or throw
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="context">The context</param>
    /// <returns>T.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="KeyNotFoundException">No value of type <typeparamref name="T" /> was found in the context.</exception>
    public static T Require<T>(this IDovetailContext context)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);

        return context.Properties.TryGetValue(typeof(T), out var value) && value is T t
            ? t
            : throw new KeyNotFoundException($"The value of type {typeof(T).Name} was not found in the context");
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
    public static T Require<T>(this IDovetailContext context, string key)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);

        return context.Properties.TryGetValue(key, out var value) && value is T t
            ? t
            : throw new KeyNotFoundException($"The value of type {typeof(T).Name} with the {key} was not found in the context");
    }

    /// <summary>
    ///     Get a value by type from the context
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="context">The context</param>
    /// <param name="value">The value to save</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    public static IDovetailContext Set<T>(this IDovetailContext context, T value) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(value);

        context.Properties.Set(value);
        return context;
    }

    /// <summary>
    ///     Get a value by type from the context
    /// </summary>
    /// <param name="context">The context</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="value">The value to save</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> or <paramref name="key" /> is <see langword="null" />.</exception>
    public static IDovetailContext Set(this IDovetailContext context, Type key, object value)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(key);

        context.Properties.Set(key, value);
        return context;
    }

    /// <summary>
    ///     Get a value by type from the context
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="context">The context</param>
    /// <param name="key">The key where the value is saved</param>
    /// <param name="value">The value to save</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />, or <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException"><paramref name="key" /> is empty or consists only of white-space characters.</exception>
    public static IDovetailContext Set<T>(this IDovetailContext context, string key, T value) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        context.Properties.Set(key, value);
        return context;
    }
}
