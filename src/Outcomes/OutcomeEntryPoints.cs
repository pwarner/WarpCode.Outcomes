namespace Outcomes;

/// <summary>
/// Helpers/entry-points for producing outcomes.
/// </summary>
public static class Outcome
{
    /// <summary>
    /// A successful outcome with the no-value type <see cref="None"/>, which acts in place of <see cref="Void"/>.
    /// This outcome implicitly casts to any Outcome{T} where the internal value will be the default for type T.
    /// <code>
    /// Outcome{string} okStringOutcome = Outcome.Ok(); // value: NULL, problem: NULL
    /// Outcome{int} okIntegerOutcome = Outcome.Ok(); // value: 0, problem: NULL
    /// </code>
    /// </summary>
    /// <returns>A successful <see cref="Outcome{None}"/>.</returns>
    public static Outcome<None> Ok() => default;

    /// <summary>
    /// Creates a new outcome that represents a value.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="value">The value with which to produce an outcome.</param>
    /// <returns>An <see cref="Outcome{T}"/> representing this value.</returns>
    public static Outcome<T> Ok<T>(T value) => new(value);

    /// <summary>
    /// Extension method that creates a new outcome from a <see cref="IProblem"/>, whose value type is {T}.
    /// If the problem is null, the Outcome holds the default value of {T}.
    /// If the problem is not null, the Outcome holds the problem.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="problem">The <see cref="IProblem"/> with which to make an outcome.</param>
    /// <returns>An <see cref="Outcome{T}"/> representing this problem.</returns>
    public static Outcome<T> ToOutcome<T>(this IProblem? problem) =>
        problem switch
        {
            not null => new Outcome<T>(problem),
            null => default
        };

    /// <summary>
    /// Extension method that creates a new outcome from a <see cref="IProblem"/>, whose value type is <see cref="None"/>.
    /// If the problem is null, the Outcome holds the default value of {T}.
    /// If the problem is not null, the Outcome holds the problem.
    /// This outcome implicitly casts to any Outcome{T} where the internal value will be the default for type T.
    /// <code>
    /// Outcome{string} okStringOutcome = p.ToOutcome(); // value: NULL, problem: p
    /// Outcome{int} okIntegerOutcome = p.ToOutcome(); // value: 0, problem: p
    /// </code>
    /// </summary>
    /// <param name="problem">The <see cref="IProblem"/> with which to make an outcome.</param>
    /// <returns>An <see cref="Outcome{None}"/> representing this problem.</returns>
    public static Outcome<None> ToOutcome(this IProblem? problem) =>
        problem switch
        {
            not null => new Outcome<None>(problem),
            null => default
        };
}
