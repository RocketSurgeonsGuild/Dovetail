using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using Dovetail.Attributes;
using Dovetail.Infrastructure;
using Dovetail.Joints;
using PropertiesDictionary = System.Collections.Generic.Dictionary<object, object>;
using PropertiesType = System.Collections.Generic.IDictionary<object, object>;

namespace Dovetail;

/// <summary>
///     Builder that can be used to create a context.
/// </summary>
[PublicAPI]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class DovetailContextBuilder
{
    /// <summary>
    ///     Create a default context builder
    /// </summary>
    /// <param name="conventions">The joints to seed the builder with.</param>
    /// <param name="properties">The initial set of properties to populate the context with.</param>
    /// <param name="categories">The categories the resulting context should be scoped to.</param>
    /// <returns>A new <see cref="DovetailContextBuilder" />.</returns>
    public static DovetailContextBuilder Create(IEnumerable<IDovetailJointMetadata> conventions, PropertiesType properties, IEnumerable<DovetailCategory> categories) =>
        new(conventions, properties, categories);

    private static readonly string[] categoryEnvironmentVariables = ["DOVETAIL__CATEGORY", "DOVETAIL__CATEGORIES"];

    private readonly UniqueQueue _conventions;
    private static readonly string[] hostTypeEnvironmentVariables = ["DOVETAIL__HOSTTYPE"];

    /// <summary>
    ///     Create a context builder with a set of properties
    /// </summary>
    /// <param name="conventions">The joints to seed the builder with.</param>
    /// <param name="properties">The initial set of properties to populate the context with.</param>
    /// <param name="categories">The categories the resulting context should be scoped to.</param>
    private DovetailContextBuilder(IEnumerable<IDovetailJointMetadata> conventions, PropertiesType? properties, IEnumerable<DovetailCategory> categories)
    {
        Properties = new DovetailDictionary(properties ?? new PropertiesDictionary());
        _conventions = new();
        _conventions.Append(conventions);

        foreach (var variable in hostTypeEnvironmentVariables)
        {
            if (Environment.GetEnvironmentVariable(variable) is { Length: > 0 } hostType && Enum.TryParse<DovetailHostType>(hostType, true, out var type)) Properties[typeof(DovetailHostType)] = type;
        }

        List<DovetailCategory> categoriesBuilder = [.. categories];
        foreach (var variable in categoryEnvironmentVariables)
        {
            if (Environment.GetEnvironmentVariable(variable) is not { Length: > 0 } category) continue;
            categoriesBuilder.AddRange(category.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(item => new DovetailCategory(item)));
        }

        Categories = ImmutableHashSet.CreateRange(DovetailCategory.ValueComparer, categoriesBuilder);
    }

    /// <summary>
    ///     The categories of the convention context
    /// </summary>
    public ImmutableHashSet<DovetailCategory> Categories { get; }

    internal IDovetailDictionary Properties { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => ToString()!;

    /// <summary>
    ///     Adds a set of conventions to the scanner
    /// </summary>
    /// <param name="conventions">The conventions.</param>
    /// <returns>IDovetailScanner.</returns>
    public DovetailContextBuilder AppendJoint(params IEnumerable<IDovetailJointMetadata> conventions)
    {
        _conventions.Append(conventions);
        return this;
    }

    /// <summary>
    ///     Adds a set of conventions to the scanner
    /// </summary>
    /// <param name="conventions">The conventions.</param>
    /// <returns><see cref="DovetailContextBuilder" />.</returns>
    public DovetailContextBuilder PrependJoint(params IEnumerable<IDovetailJointMetadata> conventions)
    {
        _conventions.Prepend(conventions);
        return this;
    }

    /// <summary>
    ///     Adds a set of conventions to the scanner
    /// </summary>
    /// <param name="conventions">The conventions.</param>
    /// <returns>IDovetailScanner.</returns>
    public DovetailContextBuilder AppendJoint(params IEnumerable<IDovetailJoint> conventions)
    {
        _conventions.Append(conventions.Select(FromDovetail));
        return this;
    }

    /// <summary>
    ///     Adds a set of conventions to the scanner
    /// </summary>
    /// <param name="conventions">The conventions.</param>
    /// <returns><see cref="DovetailContextBuilder" />.</returns>
    public DovetailContextBuilder PrependJoint(params IEnumerable<IDovetailJoint> conventions)
    {
        _conventions.Prepend(conventions.Select(FromDovetail));
        return this;
    }

    /// <summary>
    ///     Adds a set of conventions to the scanner
    /// </summary>
    /// <returns><see cref="DovetailContextBuilder" />.</returns>
    public DovetailContextBuilder AppendJoint<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : IDovetailJoint
    {
        _conventions.Append(FromDovetail<T>(Properties is IReadOnlyDovetailDictionary ro ? ro : new ReadOnlyDovetailDictionary(Properties)));
        return this;
    }

    /// <summary>
    ///     Adds a set of conventions to the scanner
    /// </summary>
    /// <returns><see cref="DovetailContextBuilder" />.</returns>
    public DovetailContextBuilder PrependJoint<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : IDovetailJoint
    {
        _conventions.Prepend(FromDovetail<T>(Properties is IReadOnlyDovetailDictionary ro ? ro : new ReadOnlyDovetailDictionary(Properties)));
        return this;
    }

    /// <summary>
    ///     Adds an exception to the scanner to exclude a specific convention
    /// </summary>
    /// <param name="assemblies">The additional types to exclude.</param>
    /// <returns><see cref="DovetailContextBuilder" />.</returns>
    public DovetailContextBuilder ExceptJoint(params IEnumerable<Assembly> assemblies)
    {
        var set = assemblies.ToHashSet();
        _conventions.Remove(a => set.Contains(a.Joint.GetType().Assembly));
        return this;
    }

    /// <summary>
    ///     Adds an exception to the scanner to exclude a specific convention
    /// </summary>
    /// <param name="types">The additional types to exclude.</param>
    /// <returns><see cref="DovetailContextBuilder" />.</returns>
    public DovetailContextBuilder ExceptJoint(params IEnumerable<Type> types)
    {
        var set = types.ToHashSet();
        _conventions.Remove(a => set.Contains(a.Joint.GetType()));
        return this;
    }

    private static IDovetailJointMetadata FromDovetail(IDovetailJoint part)
    {
        var type = part.GetType();
        var dependencies = type.GetCustomAttributes().OfType<IDovetailDependency>().ToArray();
        var hostType = type.GetCustomAttributes().OfType<IHostBasedJoint>().FirstOrDefault()?.HostType
         ?? type.GetCustomAttribute<DovetailHostTypeAttribute>()?.HostType
         ?? DovetailHostType.Undefined;
        var category = type.GetCustomAttribute<DovetailCategoryAttribute>()?.Category ?? DovetailCategory.Application;


        return new DovetailJointMetadata(part, hostType, category) { Dependencies = dependencies };
    }

    private static IDovetailJointMetadata FromDovetail<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IReadOnlyDovetailDictionary properties)
        where T : IDovetailJoint
    {
        var type = typeof(T);
        var dependencies = type.GetCustomAttributes().OfType<IDovetailDependency>().ToArray();
        var hostType = type.GetCustomAttributes().OfType<IHostBasedJoint>().FirstOrDefault()?.HostType
         ?? type.GetCustomAttribute<DovetailHostTypeAttribute>()?.HostType
         ?? DovetailHostType.Undefined;
        var category = type.GetCustomAttribute<DovetailCategoryAttribute>()?.Category ?? DovetailCategory.Application;


        return new DovetailDeferredJointMetadata<T>(properties, hostType, category) { Dependencies = dependencies };
    }

    /// <summary>
    ///     Create a new <see cref="IDovetailContext"/>
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used during setup.</param>
    /// <returns>The newly created and set up <see cref="IDovetailContext" />.</returns>
    public async ValueTask<IDovetailContext> CreateAsync(CancellationToken cancellationToken = default)
    {
        var context = new DovetailContext(
            Properties.Get<DovetailHostType>(),
            _conventions,
            new DovetailDictionary(Properties),
            Categories
        );
        await context.ApplySetup(cancellationToken);
        return context;
    }

    private class UniqueQueue : IEnumerable<IDovetailJointMetadata>
    {
        private readonly List<IDovetailJointMetadata> _list = [];

        public void Append(params IEnumerable<IDovetailJointMetadata> conventions) => _list.AddRange(conventions);

        public void Prepend(params IEnumerable<IDovetailJointMetadata> conventions) => _list.InsertRange(0, conventions);

        public void Remove(Func<IDovetailJointMetadata, bool> predicate) => _list.RemoveAll(a => predicate(a));

        IEnumerator<IDovetailJointMetadata> IEnumerable<IDovetailJointMetadata>.GetEnumerator() => _list.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}
