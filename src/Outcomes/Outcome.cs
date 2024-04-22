using System.Runtime.InteropServices;

namespace Outcomes;

/// <summary>
/// Entry-points for producing outcomes.
/// </summary>
public static class Outcome
{
    /// <summary>
    /// Returns A successful <see cref="Outcome{T}"/> with the no-value type <see cref="None"/>, which acts in place of <see cref="Void"/>.
    /// </summary>
    /// <returns>A successful <see cref="Outcome{None}"/>.</returns>
    public static Outcome<None> Ok => default;

    /// <summary>
    /// Creates a new <see cref="Outcome{T}"/> that represents a value of type T.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="value">The value with which to produce an outcome.</param>
    /// <returns>An <see cref="Outcome{T}"/> representing this value.</returns>
    public static Outcome<T> Of<T>(T value) => new(value);

    /// <summary>
    /// Creates an <see cref="Outcome{None}"/> from a problem.
    /// </summary>
    public static Outcome<None> ToOutcome(this IProblem problem) => new(problem);

    /// <summary>
    /// Creates an <see cref="Outcome{T}"/> from a problem.
    /// </summary>
    public static Outcome<T> ToOutcome<T>(this IProblem problem) => new(problem);
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

    public Outcome()
    {
        throw new InvalidOperationException(
            "Creating an Outcome with the default parameterless constructor is forbidden."
        );
    }

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
        ArgumentNullException.ThrowIfNull(problem);

        _problem = problem;
        _value = default!;
    }

    /// <summary>
    /// Exit function that resolves an Outcome to a final value.
    /// </summary>
    /// <typeparam name="TFinal">Type of the final value.</typeparam>
    /// <param name="onSuccess">Factory function that will be called if the Outcome holds a value.</param>
    /// <param name="onProblem">Factory function that will be called if the Outcome holds a problem.</param>
    /// <returns>A final value resulting from calling one of the factory functions.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either of the parameters are null.</exception>
    public TFinal Match<TFinal>(Func<T, TFinal> onSuccess, Func<IProblem, TFinal> onProblem)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onProblem);
        return _problem switch
        {
            null => onSuccess(_value),
            not null => onProblem(_problem)
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

    public static bool operator ==(Outcome<T> left, Outcome<T> right) => left.Equals(right);
    public static bool operator !=(Outcome<T> left, Outcome<T> right) => !(left == right);

    public static implicit operator Outcome<T>(T value) => new(value);
    public static implicit operator Outcome<T>(Problem problem) => new(problem);
}
