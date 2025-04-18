namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via `ThenAsync`
/// </summary>
public static class CompositionAsync
{
    #region Extensions on Task<Outcome<T>>

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, TNext> map) =>
        (await self.ConfigureAwait(false)).Then(map);

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this Outcome<T> self,
        Func<T, Task<Outcome<TNext>>> factory) =>
        self.Match(
            factory,
            problem => Task.FromResult(new Outcome<TNext>(problem))
        );

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, Outcome<TNext>> factory) =>
        (await self.ConfigureAwait(false)).Then(factory);

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static Task<Outcome<T>> ThenAsync<T>(
        this Outcome<T> self,
        Func<T, Task<Outcome<None>>> factory) =>
        self.Match(
            value => factory(value).ThenAsync(_ => self),
            _ => Task.FromResult(self)
        );

    /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async Task<Outcome<T>> ThenAsync<T>(
        this Task<Outcome<T>> self,
        Func<T, Outcome<None>> factory) =>
        (await self.ConfigureAwait(false)).Then(factory);

    /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async Task<Outcome<T>> ThenAsync<T>(
        this Task<Outcome<T>> self,
        Func<T, Task<Outcome<None>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async ValueTask<Outcome<T>> ThenAsync<T>(
        this Task<Outcome<T>> self,
        Func<T, ValueTask<Outcome<None>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, TNext> map) =>
        (await self.ConfigureAwait(false)).Then(map);

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this Outcome<T> self,
        Func<T, ValueTask<Outcome<TNext>>> factory) =>
        self.Match(
            factory,
            problem => ValueTask.FromResult(new Outcome<TNext>(problem))
        );

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, Outcome<TNext>> factory) =>
        (await self.ConfigureAwait(false)).Then(factory);

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async Task<Outcome<TNext>> ThenAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
    public static async ValueTask<Outcome<TNext>> ThenAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static ValueTask<Outcome<T>> ThenAsync<T>(
        this Outcome<T> self,
        Func<T, ValueTask<Outcome<None>>> factory) =>
        self.Match(
            value => factory(value).ThenAsync(_ => self),
            _ => ValueTask.FromResult(self)
        );

    /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async Task<Outcome<T>> ThenAsync<T>(
        this ValueTask<Outcome<T>> self,
        Func<T, Outcome<None>> factory) =>
        (await self.ConfigureAwait(false)).Then(factory);

    /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async Task<Outcome<T>> ThenAsync<T>(
        this ValueTask<Outcome<T>> self,
        Func<T, Task<Outcome<None>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
    public static async ValueTask<Outcome<T>> ThenAsync<T>(
        this ValueTask<Outcome<T>> self,
        Func<T, ValueTask<Outcome<None>>> factory) =>
        await (await self.ConfigureAwait(false))
            .ThenAsync(factory)
            .ConfigureAwait(false);

    #endregion
}
