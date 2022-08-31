using System.Runtime.InteropServices;

namespace Matterlab.Rails;

/// <summary>
/// Helpers/entry-points for producing outcomes.
/// </summary>
public static class Outcome
{
    /// <summary>
    /// Creates a new outcome that represents a value.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="value">The value with which to produce an outcome.</param>
    /// <returns>An <see cref="Outcome{T}"/> representing this value.</returns>
    public static Outcome<T> Ok<T>(T value) => new(value);

    /// <summary>
    /// Creates a new outcome that represents a <see cref="Rails.Problem"/>.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="problem">The <see cref="Rails.Problem"/> with which to produce a problem.</param>
    /// <returns>An <see cref="Outcome{T}"/> representing this problem.</returns>
    public static Outcome<T> Problem<T>(Problem problem) => new(problem);

    /// <summary>
    /// An outcome that represents the no-value type <see cref="None"/>, which acts in place of <see cref="Void"/>
    /// </summary>
    public static Outcome<None> NoProblem => default;

    /// <summary>
    /// An outcome representing a <see cref="Rails.Problem"/>, whose value type is <see cref="None"/>
    /// </summary>
    /// <param name="problem">The <see cref="Rails.Problem"/> with which to produce a problem.</param>
    /// <returns>An <see cref="Outcome{None}"/> representing this problem.</returns>
    public static Outcome<None> Problem(Problem problem) => new(problem);
}

/// <summary>
/// Primitve union type that can hold either a value or a <see cref="Problem"/>, but not both.
/// </summary>
/// <typeparam name="T">The type of the outcome value.</typeparam>
[StructLayout(LayoutKind.Auto)]
public readonly struct Outcome<T> : IEquatable<Outcome<T>>
{
    private readonly T _value;
    private readonly Problem? _problem;

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
    public Outcome(Problem problem)
    {
        _problem = problem ?? throw new ArgumentNullException(nameof(problem));
        _value = default!;
    }

    /// <summary>
    /// Operation to produce a new value from this outcome.
    /// </summary>
    /// <typeparam name="TResult">Type of resulting value.</typeparam>
    /// <param name="onSuccess">Transformation function to apply when there is no problem.</param>
    /// <param name="onProblem">Transformation function to apply when there is a problem.</param>
    /// <returns>The result of applying one of the transformation functions.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either onSuccess or onProblem is null.</exception>
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Problem, TResult> onProblem) =>
        onSuccess is null
            ? throw new ArgumentNullException(nameof(onSuccess))
            : onProblem is null
                ? throw new ArgumentNullException(nameof(onProblem))
                : _problem switch
                {
                    not null => onProblem(_problem),
                    null => onSuccess(_value)
                };

    internal Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector) =>
        _problem switch
        {
            not null => _problem,
            _ => selector(_value)
        };

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
}
