namespace Outcomes;

/// <summary>
/// Default implementation of <see cref="IProblem"/>.
/// </summary>
public class Problem : IProblem, IEquatable<IProblem>
{
    /// <summary>
    /// Creates a new problem with the provided detail.
    /// </summary>
    /// <param name="detail"><see cref="Detail"/> parameter.</param>
    public Problem(string detail) =>
        Detail = detail ?? throw new ArgumentNullException(nameof(detail));

    /// <inheritdoc />
    public string Detail { get; }

    /// <inheritdoc />
    public override string ToString() =>
        $"Problem: {GetType().FullName}, Detail: {Detail}";

    /// <inheritdoc />
    public virtual bool Equals(IProblem? other) =>
        other switch
        {
            null => false,
            _ when ReferenceEquals(this, other) => true,
            _ => string.Equals(Detail, other.Detail)
        };

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is IProblem other && Equals(other);

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
