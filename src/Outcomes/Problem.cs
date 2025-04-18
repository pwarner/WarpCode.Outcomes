namespace WarpCode.Outcomes;

/// <summary>
/// Default implementation of <see cref="IProblem"/>.
/// </summary>
/// <remarks>
/// Creates a new problem with the provided detail.
/// </remarks>
/// <param name="detail"><see cref="Detail"/> parameter.</param>
public class Problem(string detail) : IProblem, IEquatable<Problem>
{
    /// <inheritdoc />
    public string Detail { get; } = detail ?? throw new ArgumentNullException(nameof(detail));

    /// <inheritdoc />
    public override string ToString() =>
        $"Problem: {GetType().FullName}, Detail: {Detail}";

    /// <inheritdoc />
    public virtual bool Equals(Problem? other) =>
        other switch
        {
            null => false,
            _ when ReferenceEquals(this, other) => true,
            _ => string.Equals(Detail, other.Detail, StringComparison.InvariantCulture)
        };

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is Problem other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Detail.GetHashCode();

    /// <summary>
    /// Override of the equality operator.
    /// </summary>
    /// <param name="left">Left equality operand.</param>
    /// <param name="right">Right equality operand.</param>
    /// <returns>True if the operands are considered equal, else false.</returns>
    public static bool operator ==(Problem? left, Problem? right) =>
        Equals(left, right);

    /// <summary>
    /// Override of the inequality operator.
    /// </summary>
    /// <param name="left">Left inequality operand.</param>
    /// <param name="right">Right inequality operand.</param>
    /// <returns>True if the operands are considered not equal, else false.</returns>
    public static bool operator !=(Problem? left, Problem? right) =>
        !Equals(left, right);
}
