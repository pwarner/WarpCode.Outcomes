#pragma warning disable CS1591 
using System.Runtime.InteropServices;

namespace WarpCode.Outcomes;

/// <summary>
/// Value-less Result struct that can hold a problem or not.
/// </summary>
public readonly struct Result
{
    internal readonly Problem? _problem;
    public Result(Problem problem) => _problem = problem ?? throw new ArgumentNullException(nameof(problem));

    /// <summary>
    /// Returns A successful <see cref="Result{T}"/> with the no-value type <see cref="None"/>, which acts in place of <see cref="void"/>.
    /// </summary>
    /// <returns>A successful <see cref="Result{None}"/>.</returns>
    public static Result Ok => default;

    /// <summary>
    /// Creates a new <see cref="Result{T}"/> that represents a value of type T.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="value">The value with which to produce a result.</param>
    /// <returns>An <see cref="Result{T}"/> representing this value.</returns>
    public static Result<T> From<T>(T value) => new(value);
}


/// <summary>
/// Primitve union type that can hold either a value or a <see cref="Problem"/>, but not both.
/// </summary>
/// <typeparam name="T">The type of the outcome value.</typeparam>
[StructLayout(LayoutKind.Auto)]
public readonly struct Result<T>
{
    internal readonly T _value;
    internal readonly Problem? _problem;
    private Result(T value, Problem? problem) => (_value, _problem) = (value, problem);

    /// <summary>
    /// Explicit public constructor that throws if used.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the constructor is accessed.</exception>
    public Result() => throw new InvalidOperationException(
            "Creating a Result with the default parameterless constructor is forbidden."
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
}
