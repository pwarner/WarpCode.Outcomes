using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WarpCode.Outcomes;

/// <summary>
/// Async entry point helpers for <see cref="AsyncOutcome{T}"/>.
/// </summary>
/// <remarks>
/// Also available as method on <see cref="Outcome{T}"/> via <see cref="Outcome{T}.AsAsync(CancellationToken)"/>.
/// </remarks>
public static class AsyncOutcome
{
    /// <summary>
    /// Returns a successful <see cref="AsyncOutcome{None}"/> with the no-value type <see cref="None"/>, which acts in place of <see cref="void"/>.
    /// </summary>
    /// <returns>An <see cref="AsyncOutcome{None}"/>.</returns>
    public static AsyncOutcome<None> Ok(CancellationToken cancellationToken = default) 
        => new(ValueTask.FromResult(Outcome.Ok), cancellationToken);

    /// <summary>
    /// Creates a new <see cref="AsyncOutcome{T}"/> from a value of type T.
    /// </summary>
    /// <typeparam name="T">The type of the value held</typeparam>
    /// <param name="value">The value to wrap in an AsyncOutcome{T}.</param>
    /// <param name="cancellationToken">The cancellation token to associate with the asynchronous operation.</param>
    /// <returns>An <see cref="AsyncOutcome{T}"/>.</returns>
    public static AsyncOutcome<T> Of<T>(T value, CancellationToken cancellationToken = default) 
        => new(ValueTask.FromResult(Outcome.Of(value)), cancellationToken);

    /// <summary>
    /// Creates a new <see cref="AsyncOutcome{T}"/> from a <see cref="Problem"/>.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="problem">The problem instance.</param>
    /// <param name="cancellationToken">The cancellation token to associate with the asynchronous operation.</param>
    /// <returns>An <see cref="AsyncOutcome{T}"/> that represents the specified problem.</returns>
    public static AsyncOutcome<T> OfProblem<T>(Problem problem, CancellationToken cancellationToken = default) 
        => new(ValueTask.FromResult(Outcome.OfProblem<T>(problem)), cancellationToken);
    
    /// <summary>
    /// Creates a new <see cref="AsyncOutcome{T}"/> from a problem detail string.
    /// </summary>
    /// <typeparam name="T">The type of the outcome value.</typeparam>
    /// <param name="detail">The problem detail string.</param>
    /// <param name="cancellationToken">The cancellation token to associate with the asynchronous operation.</param>
    /// <returns>An <see cref="AsyncOutcome{T}"/> that represents the specified problem.</returns>
    public static AsyncOutcome<T> OfProblem<T>(string detail, CancellationToken cancellationToken = default) 
        => new(ValueTask.FromResult(Outcome.OfProblem<T>(detail)), cancellationToken);

    /// <summary>
    /// Creates a new <see cref="AsyncOutcome{None}"/> from a <see cref="Problem"/>.
    /// </summary>
    /// <param name="problem">The problem instance.</param>
    /// <param name="cancellationToken">The cancellation token to associate with the asynchronous operation.</param>
    /// <returns>An <see cref="AsyncOutcome{None}"/> that represents the specified problem.</returns>   
    public static AsyncOutcome<None> OfProblem(Problem problem, CancellationToken cancellationToken = default) 
        => new(ValueTask.FromResult(Outcome.OfProblem(problem)), cancellationToken);

    /// <summary>
    /// Creates a new <see cref="AsyncOutcome{None}"/> from a problem detail string.
    /// </summary>
    /// <param name="detail">The problem detail string.</param>
    /// <param name="cancellationToken">The cancellation token to associate with the asynchronous operation.</param>
    /// <returns>An <see cref="AsyncOutcome{None}"/> that represents the specified problem.</returns>
    public static AsyncOutcome<None> OfProblem(string detail, CancellationToken cancellationToken = default) 
        => new(ValueTask.FromResult(Outcome.OfProblem(detail)), cancellationToken);
}

/// <summary>
/// Syntactic sugar to wrap an asynchronous operation that produces a <see cref="Outcome{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the outcome value.</typeparam>
[DebuggerDisplay("{ToString(),nq}")]
[StructLayout(LayoutKind.Auto)]
public readonly struct AsyncOutcome<T>
{
    /// <summary>
    /// Explicit public constructor that throws if used.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the constructor is accessed.</exception>
    public AsyncOutcome() => throw new InvalidOperationException(
        $"Creating a {nameof(AsyncOutcome<>)} with the default parameterless constructor is forbidden."
    );

    internal AsyncOutcome(ValueTask<Outcome<T>> outcomeTask, CancellationToken cancellationToken) => (OutcomeTask, CancellationToken) = (outcomeTask, cancellationToken);
    
    internal readonly ValueTask<Outcome<T>> OutcomeTask;
    internal readonly CancellationToken CancellationToken;
    
    /// <inheritdoc cref="Outcome{T}.Match{TFinal}(Func{T, TFinal}, Func{Problem, TFinal})"/>
    public async ValueTask<TFinal> Match<TFinal>(Func<T, TFinal> onValue, Func<Problem, TFinal> onProblem)
        => (await this).Match(onValue, onProblem);

    /// <inheritdoc />
    public override string ToString() =>
        this switch
        {
            { CancellationToken.IsCancellationRequested: true } => $"Cancelled AsyncOutcome<{typeof(T)}>",
            { OutcomeTask: { IsCompletedSuccessfully: true } task } => $"Completed Outcome<{typeof(T)}>: {task.Result}",
            { OutcomeTask.IsFaulted: true } => $"Faulted Outcome<{typeof(T)}>",
            _ => $"Pending AsyncOutcome<{typeof(T)}>"
        };

    /// <summary>
    /// Convenient syntax to await the underlying ValueTask{Outcome{T}} directly on the AsyncOutcome{T} instance.
    /// </summary>
    /// <returns>The awaitable of the ValueTask{Outcome{T}}</returns>
    public ConfiguredValueTaskAwaitable<Outcome<T>>.ConfiguredValueTaskAwaiter GetAwaiter() 
        => OutcomeTask.ConfigureAwait(false).GetAwaiter();

    /// <summary>
    /// Convenience method to build a new AsyncOutcome with the existing cancellation token and a new ValueTask{Outcome{TNext}}.
    /// Used when `self with { OutcomeTask = [expression] }` record semantics are not available where
    /// the generic parameter of AsyncOutcome{T} changes to AsyncOutcome{TNext}
    /// </summary>
    internal AsyncOutcome<TNext> With<TNext>(ValueTask<Outcome<TNext>> task)=> new(task, CancellationToken);

    /// <summary>
    /// Implicitly converts an AsyncOutcome{T} to a ValueTask{Outcome{T}}.
    /// </summary>
    /// <param name="asyncOutcome">The AsyncOutcome{T} to convert.</param>
    /// <returns>The wrapped ValueTask{Outcome{T}}.</returns>
    public static implicit operator ValueTask<Outcome<T>>(AsyncOutcome<T> asyncOutcome) => asyncOutcome.OutcomeTask;

    /// <summary>
    /// Implicitly converts an AsyncOutcome{T} to a Task{Outcome{T}}.
    /// </summary>
    /// <param name="asyncOutcome">The AsyncOutcome{T} to convert.</param>
    /// <returns>The wrapped ValueTask{Outcome{T}} as a Task{Outcome{T}}.</returns>
    public static implicit operator Task<Outcome<T>>(AsyncOutcome<T> asyncOutcome) => asyncOutcome.OutcomeTask.AsTask(); 
}