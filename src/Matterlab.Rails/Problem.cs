namespace Matterlab.Rails;

/// <summary>
/// A simple, immutable data object intended to replace the practice of
/// throwing exceptions in your code when a business rule fails.
/// </summary>
public class Problem : IEquatable<Problem>
{
    /// <summary>
    /// Creates a new problem with the provided detail.
    /// </summary>
    /// <param name="detail"><see cref="Detail"/> parameter.</param>
    public Problem(string detail) => Detail = detail;

    /// <summary>
    /// Human-readable detail of the problem that occured.
    /// </summary>
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
