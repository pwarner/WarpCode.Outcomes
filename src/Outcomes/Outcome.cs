using System.Runtime.InteropServices;

namespace Outcomes;

/// <summary>
/// Helpers/entry-points for producing outcomes.
/// </summary>
public static class Outcome
{
    /// <summary>
    /// Returns A successful <see cref="Outcome{T}"/> with the no-value type <see cref="None"/>, which acts in place of <see cref="Void"/>.
    /// </summary>
    /// <remarks>
    /// This outcome implicitly casts to any <see cref="Outcome{T}"/> where the internal value will be the default for type T.
    /// <code>
    /// Outcome{string} okStringOutcome = Outcome.Ok(); // value: NULL, problem: NULL
    /// Outcome{int} okIntegerOutcome = Outcome.Ok(); // value: 0, problem: NULL
    /// </code>
    /// </remarks>
    /// <returns>A successful <see cref="Outcome{None}"/>.</returns>
    public static Outcome<None> Ok() => default;

    /// <summary>
    /// Creates a new <see cref="Outcome{T}"/> that represents a value of type T.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="value">The value with which to produce an outcome.</param>
    /// <returns>An <see cref="Outcome{T}"/> representing this value.</returns>
    public static Outcome<T> Ok<T>(T value) => new(value);

    /// <summary>
    /// Creates a new <see cref="Outcome{None}"/> from a <see cref="IProblem"/>
    /// </summary>
    /// <remarks>
    /// This outcome implicitly casts to any <see cref="Outcome{T}"/> where the internal value will be the default for type T.
    /// <code>
    /// Outcome{string} stringProblem = p.ToOutcome(); // value: NULL, problem: p
    /// Outcome{int} integerProblem = p.ToOutcome(); // value: 0, problem: p
    /// </code>
    /// </remarks>
    /// <param name="problem">The <see cref="IProblem"/> with which to make an outcome.</param>
    /// <returns>An <see cref="Outcome{None}"/> representing this problem.</returns>
    public static Outcome<None> Problem(IProblem problem)
    {
        ArgumentNullException.ThrowIfNull(problem);
        return new Outcome<None>(problem);
    }
}

/// <summary>
/// Primitve union type that can hold either a value or a <see cref="Outcomes.Problem"/>, but not both.
/// </summary>
/// <typeparam name="T">The type of the outcome value.</typeparam>
[StructLayout(LayoutKind.Auto)]
public readonly struct Outcome<T> : IEquatable<Outcome<T>>
{
    private readonly T _value;
    private readonly IProblem? _problem;

    /// <summary>
    /// Creates a new outcome that represents a value.
    /// </summary>
    /// <param name="value">Value that this outcome represents.</param>
    public Outcome(T value)
    {
        _value = value;
        _problem = default;
    }

    /// <summary>
    /// Creates a new outcome that represents a problem.
    /// </summary>
    /// <param name="problem">The problem that this outcome represents.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Outcome(IProblem problem)
    {
        _problem = problem ?? throw new ArgumentNullException(nameof(problem));
        _value = default!;
    }

    /// <summary>
    /// Exit function that resolves an Outcome to a final value.
    /// </summary>
    /// <typeparam name="TFinal">Type of the final value.</typeparam>
    /// <param name="onSuccess">Factory function that will be called if the Outcome holds a value.</param>
    /// <param name="onFail">Factory function that will be called if the Outcome holds a problem.</param>
    /// <returns>A final value resulting from calling one of the factory functions.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either of the parameters are null.</exception>
    public TFinal Match<TFinal>(Func<T, TFinal> onSuccess, Func<IProblem, TFinal> onFail)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        return _problem switch
        {
            null => onSuccess(_value),
            not null => onFail(_problem)
        };
    }

    /// <summary>
    /// Produces a new Outcome in a composition chain, from a function that returns a value.
    /// </summary>
    /// <remarks>
    /// If the Outcome holds a problem, this Outcome is returned.
    /// If the Outcome holds a value, the selector function in invoked to produce the value of a new Outcome.
    /// </remarks>
    /// <typeparam name="TResult">Type of resulting value.</typeparam>
    /// <param name="selector">A transform function to apply to the outcome value.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector function is null.</exception>
    public Outcome<TResult> Then<TResult>(Func<T, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return _problem switch
        {
            not null => new Outcome<TResult>(_problem),
            null => selector(_value)
        };
    }

    /// <summary>
    /// Produces a new Outcome in a composition chain, from a function that returns an Outcome.
    /// </summary>
    /// <remarks>
    /// If the Outcome holds a problem, this Outcome is returned.
    /// If the Outcome holds a value, the selector function in invoked to produce a new Outcome.
    /// </remarks>
    /// <typeparam name="TResult">Type of resulting value.</typeparam>
    /// <param name="selector">A transform function to apply to the outcome value.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector function is null.</exception>
    public Outcome<TResult> Then<TResult>(Func<T, Outcome<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        return _problem switch
        {
            not null => new Outcome<TResult>(_problem),
            null => selector(_value)
        };
    }

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals(Outcome<T> other) =>
        EqualityComparer<T>.Default.Equals(_value, other._value)
        && Equals(_problem, other._problem);

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is Outcome<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(_value, _problem);

    public static bool operator ==(Outcome<T> left, Outcome<T> right) =>
        left.Equals(right);

    public static bool operator !=(Outcome<T> left, Outcome<T> right) =>
        !(left == right);

    public static implicit operator Outcome<T>(T value) => new(value);
    public static implicit operator Outcome<T>(Problem problem) => new(problem);

    public static implicit operator Outcome<T>(Outcome<None> o) =>
        o._problem switch
        {
            null => default,
            not null => new Outcome<T>(o._problem)
        };
}
