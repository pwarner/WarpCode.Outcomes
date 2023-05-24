using System.Runtime.InteropServices;

namespace Outcomes;

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
        _problem = null;
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
    /// Calls the action delegate if the Outcome does not hold a problem.
    /// </summary>
    /// <param name="action">The action delegate to execute, which will be passed the outcome value as a parameter.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the action delegate is null.</exception>
    public Outcome<T> OnSuccess(Action<T> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        if (_problem is null)
            action(_value);

        return this;
    }

    /// <summary>
    /// Calls the action delegate if the Outcome holds a problem.
    /// </summary>
    /// <param name="action">The action delegate to execute, which will be passed the outcome problem as a parameter.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the action delegate is null.</exception>
    public Outcome<T> OnFail(Action<IProblem> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        if (_problem is not null)
            action(_problem);

        return this;
    }

    /// <summary>
    /// Produces a new Outcome in a composition chain.
    /// </summary>
    /// <remarks>
    /// If the Outcome holds a problem, this Outcome is returned.
    /// If the Outcome holds a value, the selector function in invoked to produce the value of a new Outcome.
    /// </remarks>
    /// <typeparam name="TResult">Type of resulting value.</typeparam>
    /// <param name="selector">A transform function to apply to the outcome value.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector function is null.</exception>
    public Outcome<TResult> Map<TResult>(Func<T, TResult> selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return _problem switch
        {
            not null => new Outcome<TResult>(_problem),
            null => selector(_value)
        };
    }

    /// <summary>
    /// Produces a new Outcome in a composition chain.
    /// </summary>
    /// <remarks>
    /// If the Outcome holds a problem, this Outcome is returned.
    /// If the Outcome holds a value, the selector function in invoked to produce a new Outcome.
    /// </remarks>
    /// <typeparam name="TResult">Type of resulting value.</typeparam>
    /// <param name="selector">A transform function to apply to the outcome value.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector function is null.</exception>
    public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return _problem switch
        {
            not null => new Outcome<TResult>(_problem),
            null => selector(_value)
        };
    }

    /// <summary>
    /// Ensures that an Outcome{T} holds an expected value.
    /// </summary>
    /// <remarks>
    /// If the outcome holds a problem, this outcome is returned.
    /// If the outcome holds a value, then the <param name="predicate"/> is invoked with the outcome value.
    /// If the predicate returns false, the <param name="factory"/> is invoked with the outcome value
    /// to produce a problem outcome.
    /// </remarks>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="predicate">A predicate delegate to test the outcome value.</param>
    /// <param name="factory">A problem factory delegate to produce a problem if the predicate test fails.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either of the <param name="predicate"/> or <param name="factory"/> parameters are null.</exception>
    public Outcome<T> Ensure(Predicate<T> predicate, Func<T, IProblem> factory)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (factory is null) throw new ArgumentNullException(nameof(factory));

        return this switch
        {
            { _problem: null } when !predicate(_value) => factory(_value).ToOutcome<T>(),
            _ => this
        };
    }

    /// <summary>
    /// Helper method that can handle a known problem and return a corrected outcome.
    /// </summary>
    /// <remarks>
    /// If the outcome holds a value, this outcome is returned.
    /// If the outcome holds a problem, then the <param name="predicate"/> is invoked with the outcome problem.
    /// If the predicate returns true, the <param name="factory"/> is invoked with the outcome problem
    /// to produce a new successful outcome.
    /// </remarks>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="predicate">A predicate delegate to test the outcome problem.</param>
    /// <param name="factory">A factory delegate to produce a value if the predicate test succeeds.</param>
    /// <returns>An <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either of the <param name="predicate"/> or <param name="factory"/> parameters are null.</exception>
    public Outcome<T> Rescue(Predicate<IProblem> predicate, Func<IProblem, T> factory)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (factory is null) throw new ArgumentNullException(nameof(factory));

        return this switch
        {
            { _problem: not null } when predicate(_problem) => new Outcome<T>(factory(_problem)),
            _ => this
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
