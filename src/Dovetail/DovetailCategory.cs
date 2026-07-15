using System.Diagnostics;

namespace Dovetail;

/// <summary>
///     The category of a given convention
/// </summary>
/// <remarks>
///     This is used to load limited sets of conventions based on categories provided.
/// </remarks>
[DebuggerDisplay("{_value}")]
public sealed class DovetailCategory(string name)
{
    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is { } && ( ReferenceEquals(this, obj) || ( obj.GetType() == GetType() && Equals((DovetailCategory)obj) ) );

    /// <inheritdoc />
    public override int GetHashCode() => _value.GetHashCode();

    /// <inheritdoc />
    public override string ToString() => _value;

    /// <summary>
    ///     Implicitly convert to a string
    /// </summary>
    /// <param name="category">The category to convert.</param>
    /// <returns>The underlying string value of the category.</returns>
    public static implicit operator string(DovetailCategory category) => category._value;

    /// <summary>
    ///     Implicitly convert from a string
    /// </summary>
    /// <param name="category">The string value to wrap.</param>
    /// <returns>A new <see cref="DovetailCategory" /> wrapping the given value.</returns>
    public static implicit operator DovetailCategory(string category) => new(category);

    /// <summary>
    ///    The value comparer for this category
    /// </summary>
    public static IEqualityComparer<DovetailCategory> ValueComparer { get; } = new ValueEqualityComparer();

    /// <summary>
    ///     This convention is loaded for any application that might be starting
    /// </summary>
    /// <remarks>Application is the default category for a convention</remarks>
    public const string Application = nameof(Application);

    /// <summary>
    ///     This convention is to load for any infrastructure bits (serializer, logging, etc)
    /// </summary>
    public const string Core = nameof(Core);

    private sealed class ValueEqualityComparer : IEqualityComparer<DovetailCategory>
    {
        public bool Equals(DovetailCategory? x, DovetailCategory? y) => ReferenceEquals(x, y) || ( x is { } && y is { } && x.GetType() == y.GetType() && x._value == y._value );

        public int GetHashCode(DovetailCategory obj) => obj._value.GetHashCode();
    }

    private bool Equals(DovetailCategory other) => _value == other._value;

    private readonly string _value = name;
}
