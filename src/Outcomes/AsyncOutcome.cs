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
        this(new ValueTask<Outcome<T>>(outcomeTask))
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

    /// <summary>   
    /// Aynchronous version of
    /// <see cref="Outcome{T}.Match{TResult}(Func{T,TResult}, Func{IProblem,TResult})"/>
    /// </summary>
    /// <typeparam name="TResult">Type of resulting value.</typeparam>
    /// <param name="onSuccess">Transformation function to apply when there is no problem.</param>
    /// <param name="onProblem">Transformation function to apply when there is a problem.</param>
    /// <returns>The result of applying one of the transformation functions.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either onSuccess or onProblem is null.</exception>
    public async ValueTask<TResult> MatchAsync<TResult>(
        Func<T, TResult> onSuccess,
        Func<IProblem, TResult> onProblem) =>
        onSuccess is null
            ? throw new ArgumentNullException(nameof(onSuccess))
            : onProblem is null
                ? throw new ArgumentNullException(nameof(onProblem))
                : (await this).Match(onSuccess, onProblem);

    internal AsyncOutcome<TResult> Then<TResult>(Func<T, AsyncOutcome<TResult>> selector)
    {
        return new AsyncOutcome<TResult>(Undress(this));

        async ValueTask<Outcome<TResult>> Undress(AsyncOutcome<T> self)
        {
            Outcome<T> outcome = await self;

            return await outcome.Match(
                selector,
                p => new AsyncOutcome<TResult>(Outcome.Problem<TResult>(p)));
        }
    }
}
