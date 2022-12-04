using System.Runtime.InteropServices;

namespace Outcomes;

/// <summary>
/// Primitve union type that can hold either a value or a <see cref="Problem"/>, but not both.
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
    /// Operation to produce a final value from this outcome.
    /// </summary>
    /// <typeparam name="TResult">Type of resulting value.</typeparam>
    /// <param name="onSuccess">Transformation function to apply when there is no problem.</param>
    /// <param name="onProblem">Transformation function to apply when there is a problem.</param>
    /// <returns>The result of applying one of the transformation functions.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either onSuccess or onProblem is null.</exception>
    public TResult Resolve<TResult>(
        Func<T, TResult> onSuccess,
        Func<IProblem, TResult> onProblem)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onProblem == null) throw new ArgumentNullException(nameof(onProblem));

        return _problem switch
        {
            not null => onProblem(_problem),
            null => onSuccess(_value)
        };
    }

    /// <summary>
    /// Operation to produce a new Outcome in a composition chain.
    /// </summary>
    /// <typeparam name="TResult">Type of resulting value.</typeparam>
    /// <param name="selector">A transform function to apply to the outcome value.</param>
    /// <returns>An <see cref="Outcome{TResult}"/> whose value is either the result of invoking the transform function on the value of source,
    /// or a <see cref="IProblem"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector function is null.</exception>
    public Outcome<TResult> Then<TResult>(Func<T, Outcome<TResult>> selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return _problem switch
        {
            not null => _problem.ToOutcome<TResult>(),
            _ => selector(_value)
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
    public static implicit operator Outcome<T>(Outcome<None> o) => o.Then<T>(_ => default);
}
