namespace Outcomes;

public static class LinqComposition
{
    #region Extensions on Outcome<T>

    /// <summary>
    /// Produces an outcome from a map function.
    /// <remarks>
    /// The map function is invoked if the outcome does not contain a problem.
    /// Otherwise the function is never invoked and a new outcome is produced to carry the problem forward.
    /// </remarks>
    /// </summary>
    public static Outcome<TNext> Select<T, TNext>(
        this Outcome<T> self,
        Func<T, TNext> map) => self.Then(map);

    /// <summary>
    /// Produces an outcome from a factory function.
    /// <remarks>
    /// The factory function is invoked if the outcome does not contain a problem.
    /// Otherwise the function is never invoked and a new outcome is produced to carry the problem forward.
    /// </remarks>
    /// </summary>
    public static Outcome<TResult> SelectMany<T, TNext, TResult>(
        this Outcome<T> self,
        Func<T, Outcome<TNext>> factory,
        Func<T, TNext, TResult> projector) =>
        self.Then(value => factory(value)
            .Then(next => projector(value, next))
        );

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Outcome<T> self,
        Func<T, Task<Outcome<TNext>>> factory,
        Func<T, TNext, TResult> projector) =>
        self.ThenAsync(value => factory(value)
            .ThenAsync(next => projector(value, next))
        );

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Outcome<T> self,
        Func<T, ValueTask<Outcome<TNext>>> factory,
        Func<T, TNext, TResult> projector) =>
        self.ThenAsync(value => factory(value)
            .ThenAsync(next => projector(value, next))
        );

    #endregion

    #region Extensions on Task<Outcome<T>>

    /// <inheritdoc cref="Select{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async Task<Outcome<TNext>> Select<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, TNext> map) =>
        (await self.ConfigureAwait(false)).Select(map);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Task<Outcome<T>> self,
        Func<T, Outcome<TNext>> factory,
        Func<T, TNext, TResult> projector) =>
        (await self.ConfigureAwait(false)).SelectMany(factory, projector);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Task<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> factory,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(factory, projector)
            .ConfigureAwait(false);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Task<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> factory,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(factory, projector)
            .ConfigureAwait(false);

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    /// <inheritdoc cref="Select{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async Task<Outcome<TNext>> Select<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, TNext> selector) =>
        (await self.ConfigureAwait(false)).Select(selector);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this ValueTask<Outcome<T>> self,
        Func<T, Outcome<TNext>> selector,
        Func<T, TNext, TResult> projector) =>
        (await self.ConfigureAwait(false)).SelectMany(selector, projector);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this ValueTask<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(selector, projector)
            .ConfigureAwait(false);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this ValueTask<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(selector, projector)
            .ConfigureAwait(false);

    #endregion
}
