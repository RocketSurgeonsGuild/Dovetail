namespace Dovetail.Infrastructure;

/// <summary>
///     A class to help with executing conventions
/// </summary>
/// <remarks>
///     This class uses <see cref="DovetailExceptionPolicyDelegate" /> to handle exceptions
/// </remarks>
/// <param name="context">The context whose joints will be executed.</param>
public class DovetailExecutor(IDovetailContext context)
{
    /// <summary>
    ///     Add a synchronous convention
    /// </summary>
    /// <param name="action">The action to run against each matching <typeparamref name="TDovetail" /> joint.</param>
    /// <typeparam name="TDovetail">The <see cref="IDovetailJoint" /> type to handle.</typeparam>
    /// <returns>This <see cref="DovetailExecutor" />, for chaining.</returns>
    public DovetailExecutor AddHandler<TDovetail>(Action<TDovetail> action) where TDovetail : IDovetailJoint
    {
        _conventionHandlers.Add(
            o =>
            {
                if (o is not TDovetail convention) return;
                try
                {
                    action(convention);
                }
                catch (Exception ex) when (!context.ExceptionPolicy(ex))
                {
                    throw;
                }
            }
        );
        return this;
    }

    /// <summary>
    ///     Add an asynchronous convention
    /// </summary>
    /// <param name="action">The asynchronous action to run against each matching <typeparamref name="TDovetail" /> joint.</param>
    /// <typeparam name="TDovetail">The <see cref="IDovetailJoint" /> type to handle.</typeparam>
    /// <returns>This <see cref="DovetailExecutor" />, for chaining.</returns>
    public DovetailExecutor AddHandler<TDovetail>(Func<TDovetail, ValueTask> action) where TDovetail : IDovetailJoint
    {
        _asyncDovetailHandlers.Add(
            async (o, _) =>
            {
                if (o is not TDovetail convention) return;
                try
                {
                    await action(convention).ConfigureAwait(false);
                }
                catch (Exception ex) when (!context.ExceptionPolicy(ex))
                {
                    throw;
                }
            }
        );
        return this;
    }

    /// <summary>
    ///     Add an asynchronous convention
    /// </summary>
    /// <param name="action">The asynchronous, cancellable action to run against each matching <typeparamref name="TDovetail" /> joint.</param>
    /// <typeparam name="TDovetail">The <see cref="IDovetailJoint" /> type to handle.</typeparam>
    /// <returns>This <see cref="DovetailExecutor" />, for chaining.</returns>
    public DovetailExecutor AddHandler<TDovetail>(Func<TDovetail, CancellationToken, ValueTask> action) where TDovetail : IDovetailJoint
    {
        _asyncDovetailHandlers.Add(
            async (o, ct) =>
            {
                if (o is not TDovetail convention) return;
                try
                {
                    await action(convention, ct).ConfigureAwait(false);
                }
                catch (Exception ex) when (!context.ExceptionPolicy(ex))
                {
                    throw;
                }
            }
        );
        return this;
    }

    /// <summary>
    ///     Run all the conventions
    /// </summary>
    public void Execute()
    {
        foreach (var convention in context.Joints)
        {
            foreach (var handler in _conventionHandlers)
            {
                handler(convention);
            }
        }
    }


    /// <summary>
    ///     Run all the conventions
    /// </summary>
    /// <returns>The context the conventions were executed against.</returns>
    public IDovetailContext ExecuteWithContext()
    {
        foreach (var convention in context.Joints)
        {
            foreach (var handler in _conventionHandlers)
            {
                handler(convention);
            }
        }
        return context;
    }


    /// <summary>
    ///     Run all the conventions
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used while executing the conventions.</param>
    /// <returns>A <see cref="ValueTask" /> representing the asynchronous execution of the conventions.</returns>
    public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        foreach (var convention in context.Joints)
        {
            foreach (var handler in _asyncDovetailHandlers)
            {
                await handler(convention, cancellationToken).ConfigureAwait(false);
            }

            foreach (var handler in _conventionHandlers)
            {
                handler(convention);
            }
        }
    }


    /// <summary>
    ///     Run all the conventions
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used while executing the conventions.</param>
    /// <returns>A <see cref="ValueTask{TResult}" /> producing the context the conventions were executed against.</returns>
    public async ValueTask<IDovetailContext> ExecuteWithContextAsync(CancellationToken cancellationToken)
    {
        foreach (var convention in context.Joints)
        {
            foreach (var handler in _asyncDovetailHandlers)
            {
                await handler(convention, cancellationToken).ConfigureAwait(false);
            }

            foreach (var handler in _conventionHandlers)
            {
                handler(convention);
            }
        }

        return context;
    }

    private readonly List<Func<IDovetailJoint, CancellationToken, ValueTask>> _asyncDovetailHandlers = [];
    private readonly List<Action<IDovetailJoint>> _conventionHandlers = [];
}
