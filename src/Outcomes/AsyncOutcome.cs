using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Outcomes;

/// <summary>
/// Syntactic class that wraps an async outcome task, to better support monadic combinatorial operations.
/// </summary>
/// <typeparam name="T">The type of the underlying <see cref="Outcome{T}"/>.</typeparam>
[StructLayout(LayoutKind.Auto)]
public readonly struct AsyncOutcome<T>
{
    private readonly ValueTask<Outcome<T>> _outcomeTask;

    /// <summary>
    /// Creates an async outcome from a ValueTask.
    /// </summary>
    /// <param name="outcomeTask">The <see cref="ValueTask{T}"/> that will complete and yield an <see cref="Outcome{T}"/>.</param>
    public AsyncOutcome(ValueTask<Outcome<T>> outcomeTask) =>
        _outcomeTask = outcomeTask;

    /// <summary>
    /// Creates an async outcome from a Task.
    /// </summary>
    /// <param name="outcomeTask">The <see cref="Task{T}"/> that will complete and yield an <see cref="Outcome{T}"/>.</param>
    public AsyncOutcome(Task<Outcome<T>> outcomeTask) :
        this(new ValueTask<Outcome<T>>(outcomeTask ?? throw new ArgumentNullException(nameof(outcomeTask))))
    {
    }

    /// <summary>
    /// Creates an async outcome from a non-async <see cref="Outcome{T}"/>.
    /// </summary>
    /// <param name="outcome">The outcome to wrap.</param>
    public AsyncOutcome(Outcome<T> outcome) : this(new ValueTask<Outcome<T>>(outcome))
    {
    }

    /// <summary>
    /// Creates an awaiter for this async outcome.
    /// </summary>
    /// <returns>An awaiter.</returns>
    public ValueTaskAwaiter<Outcome<T>> GetAwaiter() =>
        _outcomeTask.GetAwaiter();

    internal AsyncOutcome<TResult> Then<TResult>(Func<T, AsyncOutcome<TResult>> selector) =>
        new(Unwrap(selector));

    private async ValueTask<Outcome<TResult>> Unwrap<TResult>(Func<T, AsyncOutcome<TResult>> selector)
    {
        Outcome<T> outcome = await _outcomeTask.ConfigureAwait(false);

        return await outcome.Match<ValueTask<Outcome<TResult>>>(
            value => selector(value),
            problem => new AsyncOutcome<TResult>(problem.ToOutcome())
        );
    }

    public static implicit operator ValueTask<Outcome<T>>(AsyncOutcome<T> outcome) =>
        outcome._outcomeTask;

    public static implicit operator Task<Outcome<T>>(AsyncOutcome<T> outcome) =>
        outcome._outcomeTask.AsTask();
}
