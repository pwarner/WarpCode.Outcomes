namespace Outcomes;

public static class CompositionWithOutcomeOfNone
{
    //TODO: TESTS

    #region Extensions on Outcome

    /// <summary>
    /// Produces an outcome from a factory function that returns Outcome{None}.
    /// <remarks>
    /// The factory function is invoked if the outcome does not contain a problem.
    /// Otherwise the function is never invoked and an outcome returned that carries the problem.
    /// If the outcome returned by the function holds no problem, the original outcome is returned.
    /// If it holds a problem, an outcome is returned that carries the problem.
    /// </remarks>
    /// </summary>
    public static Outcome<T> Then<T>(
        this Outcome<T> self,
        Func<T, Outcome<None>> factory) =>
        self.Match(
            value => factory(value).Then(_ => self),
            _ => self
        );

    /// <inheritdoc cref="Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static Task<Outcome<T>> ThenAsync<T>(
        this Outcome<T> self,
        Func<T, Task<Outcome<None>>> factory) =>
        self.Match(
            value => factory(value).ThenAsync(_ => self),
            _ => Task.FromResult(self)
        );

    /// <inheritdoc cref="Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static ValueTask<Outcome<T>> ThenAsync<T>(
        this Outcome<T> self,
        Func<T, ValueTask<Outcome<None>>> factory) =>
        self.Match(
            value => factory(value).ThenAsync(_ => self),
            _ => ValueTask.FromResult(self)
        );

    #endregion

    #region Extensions on Task<Outcome<T>>

    /// <inheritdoc cref="Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async Task<Outcome<T>> ThenAsync<T>(
        this Task<Outcome<T>> self,
        Func<T, Outcome<None>> factory) =>
        (await self.ConfigureAwait(false)).Then(factory);

    /// <inheritdoc cref="Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async Task<Outcome<T>> ThenAsync<T>(
        this Task<Outcome<T>> self,
        Func<T, Task<Outcome<None>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async ValueTask<Outcome<T>> ThenAsync<T>(
        this Task<Outcome<T>> self,
        Func<T, ValueTask<Outcome<None>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    /// <inheritdoc cref="Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async Task<Outcome<T>> ThenAsync<T>(
        this ValueTask<Outcome<T>> self,
        Func<T, Outcome<None>> factory) =>
        (await self.ConfigureAwait(false)).Then(factory);

    /// <inheritdoc cref="Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async Task<Outcome<T>> ThenAsync<T>(
        this ValueTask<Outcome<T>> self,
        Func<T, Task<Outcome<None>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async ValueTask<Outcome<T>> ThenAsync<T>(
        this ValueTask<Outcome<T>> self,
        Func<T, ValueTask<Outcome<None>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    #endregion
}
