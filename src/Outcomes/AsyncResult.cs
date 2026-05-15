using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WarpCode.Outcomes;

/// <summary>
/// Syntactic sugar to wrap an asynchronous operation that produces a <see cref="Result{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
[DebuggerDisplay("{ToString(),nq}")]
[StructLayout(LayoutKind.Auto)]
public readonly record struct AsyncResult<T>(ValueTask<Result<T>> ResultTask, CancellationToken CancellationToken)
{
    /// <inheritdoc cref="Result{T}.Match{TFinal}(Func{T, TFinal}, Func{Problem, TFinal})"/>
    public async ValueTask<TFinal> Match<TFinal>(Func<T, TFinal> onValue, Func<Problem, TFinal> onProblem)
        => (await this).Match(onValue, onProblem);

    /// <inheritdoc cref="ValueTask{T}.ToString()"/>
    public override string ToString() =>
        this switch
        {
            { CancellationToken: { IsCancellationRequested: true } } => $"Cancelled AsyncResult<{typeof(T)}>",
            { ResultTask: { IsCompletedSuccessfully: true } task } => $"Completed Result<{typeof(T)}>: {task.Result}",
            { ResultTask.IsFaulted: true } => $"Faulted Result<{typeof(T)}>",
            _ => $"Pending AsyncResult<{typeof(T)}>"
        };

    /// <summary>
    /// Convenient syntax to await the underlying ValueTask{Result{T}} directly on the AsyncResult{T} instance.
    /// </summary>
    /// <returns>The awaitable of the ValueTask{Result{T}}</returns>
    internal ConfiguredValueTaskAwaitable<Result<T>>.ConfiguredValueTaskAwaiter GetAwaiter() 
        => ResultTask.ConfigureAwait(false).GetAwaiter();

    /// <summary>
    /// Convenience method to build a new AsyncResult with the existing cancellation token and a new ValueTask{Result{TNext}}.
    /// Used when `self { ResultTask = [expression]` record semantics are not available where
    /// the generic parameter of AsyncResult{T} changes to AsyncResult{TNext}
    /// </summary>
    internal AsyncResult<TNext> Then<TNext>(ValueTask<Result<TNext>> task)=> new(task, CancellationToken);

    /// <summary>
    /// Implicitly converts an AsyncResult{T} to a ValueTask{Result{T}}.
    /// </summary>
    /// <param name="asyncResult">The AsyncResult{T} to convert.</param>
    /// <returns>The wrapped ValueTask{Result{T}}.</returns>
    public static implicit operator ValueTask<Result<T>>(AsyncResult<T> asyncResult) => asyncResult.ResultTask;

    /// <summary>
    /// Implicitly converts an AsyncResult{T} to a Task{Result{T}}.
    /// </summary>
    /// <param name="asyncResult">The AsyncResult{T} to convert.</param>
    /// <returns>The wrapped ValueTask{Result{T}} as a Task{Result{T}}.</returns>
    public static implicit operator Task<Result<T>>(AsyncResult<T> asyncResult) => asyncResult.ResultTask.AsTask(); 
}