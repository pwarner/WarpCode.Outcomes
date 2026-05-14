using System.Runtime.InteropServices;

namespace WarpCode.Outcomes;

/// <summary>
/// Entry point helpers for <see cref="Result{T}"/>.
/// </summary>
public static class Result
{
    /// <summary>
    /// Returns A successful <see cref="Result{T}"/> with the no-value type <see cref="None"/>, which acts in place of <see cref="void"/>.
    /// </summary>
    /// <returns>A <see cref="Result{None}"/>.</returns>
    public static Result<None> Ok => default;

    /// <summary>
    /// Creates a new <see cref="Result{T}"/> from a value of type T.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="value">The value with which to produce a result.</param>
    /// <returns>An <see cref="Result{T}"/> representing this value.</returns>
    public static Result<T> From<T>(T value) => new(value);

    /// <summary>
    /// Creates a new <see cref="Result{T}"/> from a <see cref="Problem"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="problem">The problem instance.</param>
    /// <returns>A Result{T} that represents the specified problem.</returns>
    public static Result<T> FromProblem<T>(Problem problem) => new(problem);

    /// <summary>
    /// Creates a new <see cref="Result{T}"/> from a problem detail message.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="detail">The detail message with which to initialise the problem.</param>
    /// <returns>A Result{T} that represents the specified problem.</returns>
    public static Result<T> FromProblem<T>(string detail) => new(new Problem(detail));

    /// <summary>
    /// Creates a new <see cref="Result{None}"/> from a <see cref="Problem"/>.
    /// </summary>
    /// <param name="problem">The problem instance.</param>
    /// <returns>A Result{T} that represents the specified problem.</returns>
    public static Result<None> FromProblem(Problem problem) => new(problem);

    /// <summary>
    /// Creates a new <see cref="Result{None}"/> from a problem detail message.
    /// </summary>
    /// <param name="detail">The detail message with which to initialise the problem.</param>
    /// <returns>A Result{T} that represents the specified problem.</returns>
    public static Result<None> FromProblem(string detail) => new(new Problem(detail));
}

/// <summary>
/// Primitive union type that can hold either a value or a <see cref="Outcomes.Problem"/>, but not both.
/// </summary>
/// <typeparam name="T">The type of the outcome value.</typeparam>
[StructLayout(LayoutKind.Auto)]
public readonly struct Result<T>
{
    internal readonly T Value;
    internal readonly Problem? Problem;
    private Result(T value, Problem? problem) => (Value, Problem) = (value, problem);

    /// <summary>
    /// Explicit public constructor that throws if used.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the constructor is accessed.</exception>
    public Result() => throw new InvalidOperationException(
            $"Creating a {nameof(Result<>)} with the default parameterless constructor is forbidden."
        );

    /// <summary>
    /// Creates a new result that represents a value.
    /// </summary>
    /// <param name="value">Value that this result represents.</param>
    public Result(T value) : this(value, null)
    {
    }

    /// <summary>
    /// Creates a new outcome that represents a problem.
    /// </summary>
    /// <param name="problem">The problem that this outcome represents.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Result(Problem problem) : this(default!, problem ?? throw new ArgumentNullException(nameof(problem)))
    {
    }

    /// <summary>
    /// Evaluates the current instance and returns an output based on whether it contains a valid value or a problem.
    /// </summary>
    /// <remarks>Both onValue and onProblem must not be null; otherwise, an ArgumentNullException is
    /// thrown.</remarks>
    /// <typeparam name="TFinal">The type of the output returned by the provided functions.</typeparam>
    /// <param name="onValue">A function that is invoked to produce a result when the instance contains a valid value.</param>
    /// <param name="onProblem">A function that is invoked to produce a result when the instance contains a problem.</param>
    /// <returns>The result of either the onValue or onProblem function, depending on whether the instance contains a value or a
    /// problem.</returns>
    public TFinal Match<TFinal>(Func<T, TFinal> onValue, Func<Problem, TFinal> onProblem)
    {
        ArgumentNullException.ThrowIfNull(onValue);
        ArgumentNullException.ThrowIfNull(onProblem);
        return Problem is not null ? onProblem(Problem) : onValue(Value);
    }

    /// <summary>
    /// Implicitly converts a value of type T to a new instance of Result{T}.
    /// </summary>
    /// <param name="value">The value of type T to be wrapped in a Result{T} instance.</param>
    public static implicit operator Result<T>(T value) => new(value);

    /// <summary>
    /// Implicitly converts a Problem instance to a Result{T} instance, encapsulating the error information for use in
    /// result-based workflows.
    /// </summary>
    /// <param name="problem">The Problem instance containing error details to be represented as a Result{T}.</param>
    public static implicit operator Result<T>(Problem problem) => new(problem);

    /// <inheritdoc />
    public override string ToString() => Problem is not null ? $"Problem: {Problem}" : $"Value: {Value}";
}