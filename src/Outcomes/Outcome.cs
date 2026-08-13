using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WarpCode.Outcomes;

/// <summary>
/// Entry point helpers for <see cref="Outcome{T}"/>.
/// </summary>
public static class Outcome
{
    /// <summary>
    /// Returns A successful <see cref="Outcome{T}"/> with the no-value type <see cref="None"/>, which acts in place of <see cref="void"/>.
    /// </summary>
    /// <returns>A <see cref="Outcome{None}"/>.</returns>
    public static Outcome<None> Ok => default;

    /// <summary>
    /// Creates a new <see cref="Outcome{T}"/> from a value of type T.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="value">The value with which to produce an outcome.</param>
    /// <returns>An <see cref="Outcome{T}"/> representing this value.</returns>
    public static Outcome<T> Of<T>(T value) => new(value);

    /// <summary>
    /// Creates a new <see cref="Outcome{T}"/> from a <see cref="Problem"/>.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="problem">The problem instance.</param>
    /// <returns>An Outcome{T} that represents the specified problem.</returns>
    public static Outcome<T> OfProblem<T>(Problem problem) => new(problem);

    /// <summary>
    /// Creates a new <see cref="Outcome{T}"/> from a problem detail message.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="detail">The detail message with which to initialise the problem.</param>
    /// <returns>An Outcome{T} that represents the specified problem.</returns>
    public static Outcome<T> OfProblem<T>(string detail) => new(new Problem(detail));

    /// <summary>
    /// Creates a new <see cref="Outcome{None}"/> from a <see cref="Problem"/>.
    /// </summary>
    /// <param name="problem">The problem instance.</param>
    /// <returns>An Outcome{T} that represents the specified problem.</returns>
    public static Outcome<None> OfProblem(Problem problem) => new(problem);

    /// <summary>
    /// Creates a new <see cref="Outcome{None}"/> from a problem detail message.
    /// </summary>
    /// <param name="detail">The detail message with which to initialise the problem.</param>
    /// <returns>An Outcome{T} that represents the specified problem.</returns>
    public static Outcome<None> OfProblem(string detail) => new(new Problem(detail));
}

/// <summary>
/// Primitive union type that can hold either a value or a <see cref="Outcomes.Problem"/>, but not both.
/// </summary>
/// <typeparam name="T">The type of the outcome value.</typeparam>
[DebuggerDisplay("{ToString(),nq}")]
[StructLayout(LayoutKind.Auto)]
public readonly struct Outcome<T>
{
    internal readonly T Value;
    internal readonly Problem? Problem;
    private Outcome(T value, Problem? problem) => (Value, Problem) = (value, problem);

    /// <summary>
    /// Explicit public constructor that throws if used.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the constructor is accessed.</exception>
    public Outcome() => throw new InvalidOperationException(
            $"Creating a {nameof(Outcome<>)} with the default parameterless constructor is forbidden."
        );

    /// <summary>
    /// Creates a new outcome that represents a value.
    /// </summary>
    /// <param name="value">Value that this outcome represents.</param>
    public Outcome(T value) : this(value, null)
    {
    }

    /// <summary>
    /// Creates a new outcome that represents a problem.
    /// </summary>
    /// <param name="problem">The problem that this outcome represents.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Outcome(Problem problem) : this(default!, problem ?? throw new ArgumentNullException(nameof(problem)))
    {
    }

    /// <summary>
    /// Evaluates the current instance and returns an output based on whether it contains a valid value or a problem.
    /// </summary>
    /// <remarks>Both onValue and onProblem must not be null; otherwise, an ArgumentNullException is
    /// thrown.</remarks>
    /// <typeparam name="TFinal">The type of the output returned by the provided functions.</typeparam>
    /// <param name="onValue">A function that is invoked to produce an outcome when the instance contains a valid value.</param>
    /// <param name="onProblem">A function that is invoked to produce an outcome when the instance contains a problem.</param>
    /// <returns>The outcome of either the onValue or onProblem function, depending on whether the instance contains a value or a
    /// problem.</returns>
    public TFinal Match<TFinal>(Func<T, TFinal> onValue, Func<Problem, TFinal> onProblem)
    {
        ArgumentNullException.ThrowIfNull(onValue);
        ArgumentNullException.ThrowIfNull(onProblem);
        return Problem is not null ? onProblem(Problem) : onValue(Value);
    }

    /// <summary>
    /// Implicitly converts a value of type T to a new instance of Outcome{T}.
    /// </summary>
    /// <param name="value">The value of type T to be wrapped in an Outcome{T} instance.</param>
    public static implicit operator Outcome<T>(T value) => new(value);

    /// <summary>
    /// Implicitly converts a Problem instance to an Outcome{T} instance, encapsulating the error information for use in
    /// outcome-based workflows.
    /// </summary>
    /// <param name="problem">The Problem instance containing error details to be represented as an Outcome{T}.</param>
    public static implicit operator Outcome<T>(Problem problem) => new(problem);

    /// <inheritdoc />
    public override string ToString() => Problem is not null ? $"Problem: {Problem}" : $"Value: {Value}";
}