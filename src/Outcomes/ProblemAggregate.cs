namespace WarpCode.Outcomes;

/// <summary>
/// A Problem used in outcome aggregation that holds an array of multiple problems.
/// </summary>
public sealed record ProblemAggregate(Problem[] Problems)
    : Problem(Message), IEquatable<ProblemAggregate>
{
    private const string Message =
        "More than one problem occurred. " +
        "Please see the Problems property for individual problem details.";

    /// <inheritdoc />
    public bool Equals(ProblemAggregate? other) =>
        other switch
        {
            null => false,
            _ when ReferenceEquals(this, other) => true,
            _ => Problems.SequenceEqual(other.Problems)
        };

    /// <inheritdoc />
    public override int GetHashCode() =>
        Problems.Aggregate(base.GetHashCode(), HashCode.Combine);
}
