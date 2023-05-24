namespace Outcomes;

/// <summary>
/// Helper extensions to extend the capabilities of an Outcome.
/// </summary>
public static class OutcomeExtensions
{
    /// <summary>
    /// Aggregates a set of Outcome{T} to produce a single Outcome. The outcome will contain either:
    /// A <see cref="List{T}"/> value if none of the input outcomes held a problem.
    /// Or a <see cref="ProblemAggregate"/> problem containing each of the problems in the input sequence
    /// if the <param name="bailEarly"/> parameter was false.
    /// Or the first problem found in the sequence if <param name="bailEarly"/> was true.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="outcomes">A sequence of Outcome{T} values.</param>
    /// <param name="bailEarly">Whether to return the first problem discovered, or collect all problems.</param>
    /// <returns>An Outcome of type <see cref="List{T}"/>.</returns>
    public static Outcome<List<T>?> Aggregate<T>(this IEnumerable<Outcome<T>> outcomes, bool bailEarly = false)
    {
        List<T>? list = null;
        List<IProblem>? problems = null;

        foreach (Outcome<T> outcome in outcomes)
        {
            _ = outcome
                .OnSuccess(x => (list ??= new List<T>()).Add(x))
                .OnFail(x => (problems ??= new List<IProblem>()).Add(x));

            if (problems is not null && bailEarly)
                return problems[0].ToOutcome();
        }

        return problems switch
        {
            not null => new ProblemAggregate(problems),
            _ => list
        };
    }

    /// <summary>
    /// Aggregates a set of Outcome{None} to produce a single Outcome. The outcome will contain either:
    /// An empty/value-less but successful Outcome if none of the input outcomes held a problem.
    /// Or a <see cref="ProblemAggregate"/> problem containing each of the problems in the input sequence
    /// if the <param name="bailEarly"/> parameter was false.
    /// Or the first problem found in the sequence if <param name="bailEarly"/> was true.
    /// </summary>
    /// <param name="outcomes">A sequence of Outcome{None} values.</param>
    /// <param name="bailEarly">Whether to return the first problem discovered, or collect all problems.</param>
    /// <returns>An Outcome of type <see cref="List{T}"/>.</returns>
    public static Outcome<None> Aggregate(this IEnumerable<Outcome<None>> outcomes, bool bailEarly = false)
    {
        List<IProblem>? problems = null;

        foreach (Outcome<None> outcome in outcomes)
        {
            _ = outcome
                .OnFail(x => (problems ??= new List<IProblem>()).Add(x));

            if (problems is not null && bailEarly)
                return problems[0].ToOutcome();
        }

        return problems switch
        {
            not null => new ProblemAggregate(problems),
            _ => Outcome.Ok()
        };
    }
}
