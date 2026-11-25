namespace WarpCode.Outcomes;

/// <summary>
/// A Problem used in outcome aggregation that holds an array of multiple problems.
/// </summary>
public sealed record ProblemAggregate(Problem[] Problems)
    : Problem(Describe(Problems)), IEquatable<ProblemAggregate>
{
    private static string Describe(Problem[] problems) =>
        (problems ?? throw new ArgumentNullException(nameof(Problems))).Length switch
        {
            1 => "1 problem occurred. ",
            var count => $"{count} problems occurred. "
        } +
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
