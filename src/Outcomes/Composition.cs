namespace Outcomes;

public static class Composition
{
    #region Extensions on Outcome<T>

    /// <summary>
    /// Produces an outcome from a map function.
    /// <remarks>
    /// The map function is invoked if the outcome does not contain a problem.
    /// Otherwise the function is never invoked and an outcome returned that carries the problem.
    /// </remarks>
    /// </summary>
    public static Outcome<TNext> Then<T, TNext>(
        this Outcome<T> self,
        Func<T, TNext> map) =>
        self.Match(
            value => new Outcome<TNext>(map(value)),
            problem => new Outcome<TNext>(problem)
        );

    /// <summary>
    /// Produces an outcome from a factory function.
    /// <remarks>
    /// The factory function is invoked if the outcome does not contain a problem.
    /// Otherwise the function is never invoked and an outcome returned that carries the problem.
    /// </remarks>
    /// </summary>
    public static Outcome<TNext> Then<T, TNext>(
        this Outcome<T> self,
        Func<T, Outcome<TNext>> factory) =>
        self.Match(
            factory,
            problem => new Outcome<TNext>(problem)
        );

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this Outcome<T> self,
        Func<T, Task<Outcome<TNext>>> factory) =>
        self.Match(
            factory,
            problem => Task.FromResult(new Outcome<TNext>(problem))
        );

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this Outcome<T> self,
        Func<T, ValueTask<Outcome<TNext>>> factory) =>
        self.Match(
            factory,
            problem => ValueTask.FromResult(new Outcome<TNext>(problem))
        );

    #endregion

    #region Extensions on Task<Outcome<T>>

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, TNext> map) =>
        (await self.ConfigureAwait(false)).Then(map);

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, Outcome<TNext>> factory) =>
        (await self.ConfigureAwait(false)).Then(factory);

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, TNext> map) =>
        (await self.ConfigureAwait(false)).Then(map);

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, Outcome<TNext>> factory) =>
        (await self.ConfigureAwait(false)).Then(factory);

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    #endregion
}
