using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WarpCode.Outcomes;

/// <summary>
/// Syntactic sugar to wrap an asynchronous operation that produces a <see cref="Outcome{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the outcome value.</typeparam>
[DebuggerDisplay("{ToString(),nq}")]
[StructLayout(LayoutKind.Auto)]
public readonly record struct AsyncOutcome<T>(ValueTask<Outcome<T>> OutcomeTask, CancellationToken CancellationToken)
{
    /// <inheritdoc cref="Outcome{T}.Match{TFinal}(Func{T, TFinal}, Func{Problem, TFinal})"/>
    public async ValueTask<TFinal> Match<TFinal>(Func<T, TFinal> onValue, Func<Problem, TFinal> onProblem)
        => (await this).Match(onValue, onProblem);

    /// <inheritdoc cref="ValueTask{T}.ToString()"/>
    public override string ToString() =>
        this switch
        {
            { CancellationToken: { IsCancellationRequested: true } } => $"Cancelled AsyncOutcome<{typeof(T)}>",
            { OutcomeTask: { IsCompletedSuccessfully: true } task } => $"Completed Outcome<{typeof(T)}>: {task.Result}",
            { OutcomeTask.IsFaulted: true } => $"Faulted Outcome<{typeof(T)}>",
            _ => $"Pending AsyncOutcome<{typeof(T)}>"
        };

    /// <summary>
    /// Convenient syntax to await the underlying ValueTask{Outcome{T}} directly on the AsyncOutcome{T} instance.
    /// </summary>
    /// <returns>The awaitable of the ValueTask{Outcome{T}}</returns>
    internal ConfiguredValueTaskAwaitable<Outcome<T>>.ConfiguredValueTaskAwaiter GetAwaiter() 
        => OutcomeTask.ConfigureAwait(false).GetAwaiter();

    /// <summary>
    /// Convenience method to build a new AsyncOutcome with the existing cancellation token and a new ValueTask{Outcome{TNext}}.
    /// Used when `self { OutcomeTask = [expression]` record semantics are not available where
    /// the generic parameter of AsyncOutcome{T} changes to AsyncOutcome{TNext}
    /// </summary>
    internal AsyncOutcome<TNext> Then<TNext>(ValueTask<Outcome<TNext>> task)=> new(task, CancellationToken);

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