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
    /// Outcome{string} okStringOutcome = Outcome.NoProblem; // value: NULL, problem: NULL
    /// Outcome{int} okIntegerOutcome = Outcome.NoProblem; // value: 0, problem: NULL
    /// </code>
    /// </summary>
    /// <returns>A successful <see cref="Outcome{None}"/>.</returns>
    public static Outcome<None> NoProblem => default;

    /// <summary>
    /// A successful async outcome with the no-value type <see cref="None"/>, which acts in place of <see cref="Void"/>.
    /// The internal task will have the state of already being completed.
    /// </summary>
    /// <returns>A successful <see cref="AsyncOutcome{None}"/>.</returns>
    public static AsyncOutcome<None> NoProblemAsync => new(NoProblem);

    /// <summary>
    /// Creates a new outcome that represents a value.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="value">The value with which to produce an outcome.</param>
    /// <returns>An <see cref="Outcome{T}"/> representing this value.</returns>
    public static Outcome<T> Ok<T>(T value) => new(value);

    /// <summary>
    /// Extension method that creates a new outcome from a <see cref="IProblem"/>, whose value type is {T}.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="problem">The <see cref="IProblem"/> with which to make an outcome.</param>
    /// <returns>An <see cref="Outcome{T}"/> representing this problem.</returns>
    public static Outcome<T> ToOutcome<T>(this IProblem problem) => new(problem);

    /// <summary>
    /// Extension method that creates a new outcome from a <see cref="IProblem"/>, whose value type is <see cref="None"/>.
    /// This outcome implicitly casts to any Outcome{T} where the internal value will be the default for type T.
    /// <code>
    /// Outcome{string} okStringOutcome = p.ToOutcome(); // value: NULL, problem: p
    /// Outcome{int} okIntegerOutcome = p.ToOutcome(); // value: 0, problem: p
    /// </code>
    /// </summary>
    /// <param name="problem">The <see cref="IProblem"/> with which to make an outcome.</param>
    /// <returns>An <see cref="Outcome{None}"/> representing this problem.</returns>
    public static Outcome<None> ToOutcome(this IProblem problem) => new(problem);

    /// <summary>
    /// Creates a new async outcome that represents a value.
    /// The internal task will have the state of already being completed.
    /// </summary>
    /// <typeparam name="T">The type of the async outcome value.</typeparam>
    /// <param name="value">The value with which to produce an async outcome.</param>
    /// <returns>An <see cref="AsyncOutcome{T}"/> representing this value.</returns>
    public static AsyncOutcome<T> OkAsync<T>(T value) => new(value);

    /// <summary>
    /// Extension method that creates a new async outcome from a <see cref="IProblem"/>, whose value type is {T}.
    /// /// The internal task will have the state of already being completed.
    /// </summary>
    /// <typeparam name="T">The type of the async outcome value.</typeparam>
    /// <param name="problem">The <see cref="IProblem"/> with which to make an async outcome.</param>
    /// <returns>An <see cref="AsyncOutcome{T}"/> representing this problem.</returns>
    public static AsyncOutcome<T> ToOutcomeAsync<T>(this IProblem problem) => new(problem.ToOutcome<T>());

    /// <summary>
    /// Extension method that creates a new async outcome from a <see cref="IProblem"/>, whose value type is <see cref="None"/>.
    /// The internal task will have the state of already being completed.
    /// </summary>
    /// <param name="problem">The <see cref="IProblem"/> with which to make an outcome.</param>
    /// <returns>An <see cref="AsyncOutcome{None}"/> representing this problem.</returns>
    public static AsyncOutcome<None> ToOutcomeAsync(this IProblem problem) => new(problem.ToOutcome());
}
