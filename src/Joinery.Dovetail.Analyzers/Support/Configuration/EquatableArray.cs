using System.Collections;
using System.Collections.Immutable;

namespace Joinery.Dovetail.Support.Configuration;

/// <summary>
///     An immutable array that compares by content, so incremental pipeline values built from it can be cached.
/// </summary>
internal readonly struct EquatableArray<T>(ImmutableArray<T> array) : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    public static EquatableArray<T> Empty { get; } = new([]);

    public ImmutableArray<T> Items { get => field.IsDefault ? [] : field; } = array;
    public int Length => Items.Length;
    public bool IsEmpty => Items.IsEmpty;
    public T this[int index] => Items[index];

    public bool Equals(EquatableArray<T> other) => Items.SequenceEqual(other.Items);
    public override bool Equals(object? obj) => obj is EquatableArray<T> other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            foreach (var item in Items)
            {
                hash = ( hash * 31 ) + ( item?.GetHashCode() ?? 0 );
            }

            return hash;
        }
    }

    public ImmutableArray<T>.Enumerator GetEnumerator() => Items.GetEnumerator();
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => ( (IEnumerable<T>)Items ).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)Items ).GetEnumerator();

    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) => left.Equals(right);
    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) => !left.Equals(right);
}

internal static class EquatableArrayExtensions
{
    public static EquatableArray<T> ToEquatableArray<T>(this IEnumerable<T> items) where T : IEquatable<T> => new([.. items]);
}
