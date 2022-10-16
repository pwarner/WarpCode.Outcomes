namespace Outcomes;

/// <summary>
/// Default implementation of <see cref="IProblem"/>.
/// </summary>
public class Problem : IProblem, IEquatable<Problem>
{
    /// <summary>
    /// Creates a new problem with the provided detail.
    /// </summary>
    /// <param name="detail"><see cref="Detail"/> parameter.</param>
    public Problem(string detail) => Detail = detail;

    /// <inheritdoc />
    public string Detail { get; }

    /// <inheritdoc />
    public override string ToString() =>
        $"[Problem] Type: {GetType().FullName}, Detail: {Detail}";

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals(Problem? other) =>
        other is not null && string.Equals(Detail, other.Detail);

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is Problem other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Detail.GetHashCode();
}
