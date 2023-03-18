using System.Runtime.InteropServices;

namespace Outcomes;

/// <summary>
/// Primitve union type that can hold either a value or a <see cref="Outcomes.Problem"/>, but not both.
/// </summary>
/// <typeparam name="T">The type of the outcome value.</typeparam>
[StructLayout(LayoutKind.Auto)]
public readonly struct Outcome<T> : IEquatable<Outcome<T>>
{
    public readonly T Value;
    public readonly IProblem? Problem;

    /// <summary>
    /// Creates a new outcome that represents a value.
    /// </summary>
    /// <param name="value">Value that this outcome represents.</param>
    public Outcome(T value)
    {
        Value = value;
        Problem = null;
    }

    /// <summary>
    /// Creates a new outcome that represents a problem.
    /// </summary>
    /// <param name="problem">The problem that this outcome represents.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Outcome(IProblem problem)
    {
        Problem = problem ?? throw new ArgumentNullException(nameof(problem));
        Value = default!;
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

        return Problem switch
        {
            not null => Problem.ToOutcome<TResult>(),
            null => selector(Value)
        };
    }

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals(Outcome<T> other) =>
        EqualityComparer<T>.Default.Equals(Value, other.Value)
        && Equals(Problem, other.Problem);

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is Outcome<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(Value, Problem);

    public static bool operator ==(Outcome<T> left, Outcome<T> right) =>
        left.Equals(right);

    public static bool operator !=(Outcome<T> left, Outcome<T> right) =>
        !(left == right);

    public static implicit operator Outcome<T>(T value) => new(value);
    public static implicit operator Outcome<T>(Problem problem) => new(problem);
    public static implicit operator Outcome<T>(Outcome<None> o) => o.Then<T>(_ => default);
}
