namespace Outcomes;

/// <summary>
/// Helper extensions to extend the capabilities of an Outcome.
/// </summary>
public static class OutcomeExtensions
{
    /// <summary>
    /// Calls an action delegate if the outcome does not hold a problem.
    /// </summary>
    /// <param name="self">Subject outcome.</param>
    /// <param name="action">The action delegate to execute, which will be passed the outcome value as a parameter.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the action delegate is null.</exception>
    public static Outcome<T> OnSuccess<T>(this Outcome<T> self, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return self.Match(
            value =>
            {
                action(value);
                return self;
            },
            problem => self
        );
    }

    /// <summary>
    /// Calls an action delegate if the Outcome holds a problem.
    /// </summary>
    /// <param name="self">Subject outcome.</param>
    /// <param name="action">The action delegate to execute, which will be passed the outcome problem as a parameter.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the action delegate is null.</exception>
    public static Outcome<T> OnProblem<T>(this Outcome<T> self, Action<IProblem> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return self.Match(
            _ => self,
            problem =>
            {
                action(problem);
                return self;
            }
        );
    }

    /// <summary>
    /// Ensures that an outcome holds an expected value.
    /// </summary>
    /// <remarks>
    /// If the outcome holds a problem, this outcome is returned.
    /// If the outcome holds a value, then the <param name="predicate"/> is invoked with the outcome value.
    /// If the predicate returns false, the <param name="factory"/> is invoked with the outcome value
    /// to produce a problem outcome.
    /// </remarks>
    /// <param name="self">Subject outcome.</param>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="predicate">A predicate delegate to test the outcome value.</param>
    /// <param name="factory">A problem factory delegate to produce a problem if the predicate test fails.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either of the <param name="predicate"/> or <param name="factory"/> parameters are null.</exception>
    public static Outcome<T> Ensure<T>(this Outcome<T> self, Predicate<T> predicate, Func<T, IProblem> factory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(factory);

        return self.Match(
            value => predicate(value) ? self : new Outcome<T>(factory(value)),
            _ => self
        );
    }

    /// <summary>
    /// Handles a problem and returns a corrected outcome.
    /// </summary>
    /// <remarks>
    /// If the outcome holds a value, this outcome is returned.
    /// If the outcome holds a problem, then the <param name="predicate"/> is invoked with the outcome problem.
    /// If the predicate returns true, the <param name="factory"/> is invoked with the outcome problem
    /// to produce a new successful outcome.
    /// </remarks>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="self">Subject outcome.</param>
    /// <param name="predicate">A predicate delegate to test the outcome problem.</param>
    /// <param name="factory">A factory delegate to produce a value if the predicate test succeeds.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either of the <param name="predicate"/> or <param name="factory"/> parameters are null.</exception>
    public static Outcome<T> Rescue<T>(this Outcome<T> self, Predicate<IProblem> predicate, Func<IProblem, T> factory)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(factory);

        return self.Match(
            _ => self,
            problem => predicate(problem) ? factory(problem) : new Outcome<T>(problem)
        );
    }

    /// <summary>
    /// Aggregates a set of Outcome{T} to produce a single Outcome.
    /// </summary>
    /// <remarks>
    /// The outcome will contain either:
    /// A <see cref="List{T}"/> value if none of the input outcomes held a problem.
    /// An outcome of the first problem in the sequence if <param name="bailEarly"/> was true.
    /// An outcome of a <see cref="ProblemAggregate"/> containing each of the problems found.
    /// </remarks>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="outcomes">A sequence of Outcome{T} values.</param>
    /// <param name="bailEarly">Whether to return the first problem discovered, or collect all problems.</param>
    /// <returns>An <see cref="Outcome{T}"/> of type <see cref="List{T}"/>.</returns>
    public static Outcome<List<T>> Aggregate<T>(this IEnumerable<Outcome<T>> outcomes,
        bool bailEarly = false)
    {
        List<T>? list = null;
        List<IProblem>? problems = null;
        int? capacity = outcomes.TryGetNonEnumeratedCount(out int count) ? count : null;

        foreach (Outcome<T> outcome in outcomes)
        {
            outcome
                .OnSuccess(item => Collect(ref list, capacity, item))
                .OnProblem(problem => Collect(ref problems, capacity, problem));

            if (problems is not null && bailEarly)
                return problems[0].ToOutcome<List<T>>();
        }

        return problems switch
        {
            not null => new ProblemAggregate(problems),
            null => list!
        };
    }

    /// <summary>
    /// Aggregates a set of Outcome{None} to produce a single Outcome.
    /// </summary>
    /// <remarks>
    /// The outcome will contain either:
    /// An empty/value-less but successful Outcome if none of the input outcomes held a problem.
    /// An outcome of the first problem in the sequence if <param name="bailEarly"/> was true.
    /// An outcome of a <see cref="ProblemAggregate"/> containing each of the problems found.
    /// </remarks>
    /// <param name="outcomes">A sequence of Outcome{None} values.</param>
    /// <param name="bailEarly">Whether to return the first problem discovered, or collect all problems.</param>
    /// <returns>An <see cref="Outcome{T}"/> of type <see cref="List{T}"/>.</returns>
    public static Outcome<None> Aggregate(this IEnumerable<Outcome<None>> outcomes,
        bool bailEarly = false)
    {
        List<IProblem>? problems = null;
        int? capacity = outcomes.TryGetNonEnumeratedCount(out int count) ? count : null;

        foreach (Outcome<None> outcome in outcomes)
        {
            outcome.OnProblem(problem => Collect(ref problems, capacity, problem));

            if (problems is not null && bailEarly)
                return problems[0].ToOutcome();
        }

        return problems switch
        {
            not null => new ProblemAggregate(problems),
            null => Outcome.Ok
        };
    }

    private static void Collect<T>(ref List<T>? list, int? capacity, T item)
    {
        list ??= capacity.HasValue ? new List<T>(capacity.Value) : [];
        list.Add(item);
    }
}
