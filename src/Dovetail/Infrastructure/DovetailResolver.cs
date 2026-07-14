using System.Collections.Immutable;

namespace Dovetail.Infrastructure;

internal static class DovetailResolver
{

    public static ImmutableList<IDovetailJoint> Resolve(DovetailHostType hostType, ImmutableHashSet<DovetailCategory> categories, IEnumerable<IDovetailJointMetadata> contributions)
        => ResolveMetadata(categories, contributions)
          .Where(cod => cod.HostType == DovetailHostType.Undefined || cod.HostType == hostType)
          .Select(z => z.Joint)
          .ToImmutableList();

    private static ImmutableList<IDovetailJointMetadata> ResolveMetadata(ImmutableHashSet<DovetailCategory> categories, IEnumerable<IDovetailJointMetadata> contributions)
    {
        IEnumerable<IDovetailJointMetadata> c = contributions.Order(Comparer<IDovetailJointMetadata>.Create((x, y) => x.Joint.Priority.CompareTo(y.Joint.Priority))); ;
        if (categories.Any()) c = c.Where(z => !categories.Contains(z.Category));

        var r = c.ToImmutableList();
        if (!r.Any(z => z.Dependencies.Count > 0)) return r;

        var conventions = c
                         .Select(convention =>
                                 {
                                     return (
                                         convention,
                                         // ReSharper disable once NullableWarningSuppressionIsUsed RedundantSuppressNullableWarningExpression
                                         type: convention.Joint!.GetType(),
                                         dependsOn: convention
                                                   .Dependencies.Where(x => x.Direction == DependencyDirection.DependsOn)
                                                   .Select(z => z.Type),
                                         dependentFor: convention
                                                      .Dependencies
                                                      .Where(x => x.Direction == DependencyDirection.DependentOf)
                                                      .Select(z => z.Type)
                                     );
                                 }
                          )
                         .ToArray();

        var lookup = conventions.ToLookup(z => z.type, z => z.convention);
        var dependentFor = conventions
                          .SelectMany(data => data
                                             .dependentFor
                                             .SelectMany(z => lookup[z])
                                             .Select(innerDependentFor => (dependentFor: innerDependentFor, data.convention))
                           )
                          .ToLookup(z => z.dependentFor, z => z.convention);

        var dependsOn = conventions
                       .SelectMany(data => data
                                          .dependsOn
                                          .SelectMany(z => lookup[z])
                                          .Select(innerDependsOn => (data.convention, dependsOn: innerDependsOn))
                        )
                       .Concat(
                            conventions
                               .SelectMany(data =>
                                               dependentFor[data.convention]
                                                  .Select(innerDependsOn => (data.convention, dependsOn: innerDependsOn))
                                )
                        )
                       .ToLookup(x => x.convention.Joint, x => x.dependsOn);

        return TopographicalSort(c, x => dependsOn[x.Joint]).ToImmutableList();
    }

    private static List<T> TopographicalSort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies)
    {
        var sorted = new List<T>();
        var visited = new HashSet<T>();

        foreach (var item in source)
        {
            Visit(item, visited, sorted, dependencies);
        }

        return sorted;
    }

    private static void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies)
    {
        if (visited.Add(item))
        {
            foreach (var dep in dependencies(item))
            {
                Visit(dep, visited, sorted, dependencies);
            }

            sorted.Add(item);
        }
        else
        {
            if (!sorted.Contains(item)) throw new NotSupportedException($"Cyclic dependency found {item}");
        }
    }
}
