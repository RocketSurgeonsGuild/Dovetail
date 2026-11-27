using System.Collections;
using System.Collections.Immutable;

namespace Joinery.Dovetail;

/// <summary>
///     An immutable array compared by its items, so a pipeline value that holds one compares equal to the value of the previous
///     run when nothing in it changed, and the generator's incremental cache can skip the work after it.
/// </summary>
internal readonly struct EquatableArray<T>(ImmutableArray<T> items) : IEquatable<EquatableArray<T>>, IReadOnlyList<T>
    where T : IEquatable<T>
{
    public ImmutableArray<T> Items { get => field.IsDefault ? [] : field; } = items;

    public int Count => Items.Length;

    public T this[int index] => Items[index];

    public bool Equals(EquatableArray<T> other)
    {
        var (left, right) = (Items, other.Items);
        if (left.Length != right.Length) return false;
        for (var i = 0; i < left.Length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(left[i], right[i])) return false;
        }

        return true;
    }

    public override bool Equals(object? obj) => obj is EquatableArray<T> other && Equals(other);

    public override int GetHashCode()
    {
        var hash = 17;
        foreach (var item in Items)
        {
            hash = unchecked(( hash * 31 ) + ( item?.GetHashCode() ?? 0 ));
        }

        return hash;
    }

    public ImmutableArray<T>.Enumerator GetEnumerator() => Items.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => ( (IEnumerable<T>)Items ).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)Items ).GetEnumerator();

    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) => left.Equals(right);

    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) => !left.Equals(right);
}

internal static class EquatableArray
{
    public static EquatableArray<T> ToEquatableArray<T>(this IEnumerable<T> items) where T : IEquatable<T> => new([.. items]);
}
