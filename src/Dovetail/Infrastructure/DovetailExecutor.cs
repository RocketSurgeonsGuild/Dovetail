namespace Dovetail.Infrastructure;

/// <summary>
///     A class to help with executing conventions
/// </summary>
/// <remarks>
///     This class uses <see cref="DovetailExceptionPolicyDelegate" /> to handle exceptions
/// </remarks>
/// <param name="context"></param>
public class DovetailExecutor(IDovetailContext context)
{
    /// <summary>
    ///     Add a synchronous convention
    /// </summary>
    /// <param name="action"></param>
    /// <typeparam name="TDovetail"></typeparam>
    /// <returns></returns>
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
    /// <param name="action"></param>
    /// <typeparam name="TDovetail"></typeparam>
    /// <returns></returns>
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
    /// <param name="action"></param>
    /// <typeparam name="TDovetail"></typeparam>
    /// <returns></returns>
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
    /// <param name="cancellationToken"></param>
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
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
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
