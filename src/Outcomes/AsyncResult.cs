using System.Runtime.CompilerServices;

namespace WarpCode.Outcomes;

/// <summary>
/// Syntactic sugar to wrap an asynchronous operation that produces a <see cref="Result{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public readonly record struct AsyncResult<T>
{
    internal readonly ValueTask<Result<T>> ResultTask;
    internal readonly CancellationToken CancellationToken;
    internal AsyncResult(ValueTask<Result<T>> resultTask, CancellationToken cancellationToken) 
        => (ResultTask, CancellationToken) = (resultTask, cancellationToken);

    /// <summary>
    /// Convenient syntax to await the underlying ValueTask{Result{T}} directly on the AsyncResult{T} instance.
    /// </summary>
    /// <returns>The awaitable of the ValueTask{Result{T}}</returns>
    public ConfiguredValueTaskAwaitable<Result<T>>.ConfiguredValueTaskAwaiter GetAwaiter() 
        => ResultTask.ConfigureAwait(false).GetAwaiter();

    /// <summary>
    /// Explicit public constructor that throws if used.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the constructor is accessed.</exception>
    public AsyncResult() => throw new InvalidOperationException(
        $"Creating an {nameof(AsyncResult<>)} with the default parameterless constructor is forbidden."
    );

    /// <summary>
    /// Convenience method to build a new AsyncResult with the existing cancellation token and a new ValueTask{Result{TNext}}.
    /// </summary>
    /// <param name="task">The new result task.</param>
    /// <typeparam name="TNext">Type of the new result that the task will resolve to.</typeparam>
    /// <returns>A new AsyncResult{TNext}</returns>
    public AsyncResult<TNext> Then<TNext>(ValueTask<Result<TNext>> task)=> new(task, CancellationToken);

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