namespace Outcomes;

/// <summary>
/// A Problem used in outcome aggregation that holds an array of multiple problems.
/// </summary>
public sealed class ProblemAggregate : Problem, IEquatable<ProblemAggregate>
{
    public ProblemAggregate(IReadOnlyList<IProblem> problems) :
        base("More than one problem occurred. " +
             "Please see the Problems property for individual problem details.") =>
        Problems = problems;

    /// <summary>
    /// The set of problems that this aggregate holds.
    /// </summary>
    public IReadOnlyList<IProblem> Problems { get; }

    /// <inheritdoc />
    public bool Equals(ProblemAggregate? other) =>
        other switch
        {
            null => false,
            _ when ReferenceEquals(this, other) => true,
            _ => Problems.SequenceEqual(other.Problems)
        };

    /// <inheritdoc />
    public override bool Equals(IProblem? other) =>
        other is ProblemAggregate p && Equals(p);

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is ProblemAggregate other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(base.GetHashCode(), Problems);

    /// <summary>
    /// Override of the equality operator.
    /// </summary>
    /// <param name="left">Left equality operand.</param>
    /// <param name="right">Right equality operand.</param>
    /// <returns>True if the operands are considered equal, else false.</returns>
    public static bool operator ==(ProblemAggregate? left, ProblemAggregate? right) =>
        Equals(left, right);

    /// <summary>
    /// Override of the inequality operator.
    /// </summary>
    /// <param name="left">Left inequality operand.</param>
    /// <param name="right">Right inequality operand.</param>
    /// <returns>True if the operands are considered not equal, else false.</returns>
    public static bool operator !=(ProblemAggregate? left, ProblemAggregate? right) =>
        !Equals(left, right);
}
