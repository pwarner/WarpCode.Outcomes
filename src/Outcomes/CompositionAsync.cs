namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via `ThenAsync`
/// </summary>
public static class CompositionAsync
{
    #region Extensions on Task<Outcome<T>>

    extension<T>(Task<Outcome<T>> self)
    {
        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,TNext})"/>
        public async Task<Outcome<TNext>> ThenAsync<TNext>(Func<T, TNext> map) =>
            (await self.ConfigureAwait(false)).Then(map);

        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
        public async Task<Outcome<TNext>> ThenAsync<TNext>(Func<T, Outcome<TNext>> factory) =>
            (await self.ConfigureAwait(false)).Then(factory);

        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
        public async Task<Outcome<TNext>> ThenAsync<TNext>(Func<T, Task<Outcome<TNext>>> factory) =>
            await (await self.ConfigureAwait(false))
                .ThenAsync(factory)
                .ConfigureAwait(false);

        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
        public async ValueTask<Outcome<TNext>> ThenAsync<TNext>(Func<T, ValueTask<Outcome<TNext>>> factory) =>
            await (await self.ConfigureAwait(false))
                .ThenAsync(factory)
                .ConfigureAwait(false);

        /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
        public async Task<Outcome<T>> ThenAsync(Func<T, Outcome<None>> factory) =>
            (await self.ConfigureAwait(false)).Then(factory);

        /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
        public async Task<Outcome<T>> ThenAsync(Func<T, Task<Outcome<None>>> factory) =>
            await (await self.ConfigureAwait(false))
                .ThenAsync(factory)
                .ConfigureAwait(false);

        /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
        public async ValueTask<Outcome<T>> ThenAsync(Func<T, ValueTask<Outcome<None>>> factory) =>
            await (await self.ConfigureAwait(false))
                .ThenAsync(factory)
                .ConfigureAwait(false);
    }

    extension<T>(Outcome<T> self)
    {
        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
        public Task<Outcome<TNext>> ThenAsync<TNext>(Func<T, Task<Outcome<TNext>>> factory) =>
            self.Match(
                factory,
                problem => Task.FromResult(new Outcome<TNext>(problem))
            );

        /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
        public Task<Outcome<T>> ThenAsync(Func<T, Task<Outcome<None>>> factory) =>
            self.Match(
                value => factory(value).ThenAsync(_ => self),
                _ => Task.FromResult(self)
            );

        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
        public ValueTask<Outcome<TNext>> ThenAsync<TNext>(Func<T, ValueTask<Outcome<TNext>>> factory) =>
            self.Match(
                factory,
                problem => ValueTask.FromResult(new Outcome<TNext>(problem))
            );

        /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
        public ValueTask<Outcome<T>> ThenAsync(Func<T, ValueTask<Outcome<None>>> factory) =>
            self.Match(
                value => factory(value).ThenAsync(_ => self),
                _ => ValueTask.FromResult(self)
            );
    }

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    extension<T>(ValueTask<Outcome<T>> self)
    {
        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,TNext})"/>
        public async ValueTask<Outcome<TNext>> ThenAsync<TNext>(Func<T, TNext> map) =>
            (await self.ConfigureAwait(false)).Then(map);

        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
        public async ValueTask<Outcome<TNext>> ThenAsync<TNext>(Func<T, Outcome<TNext>> factory) =>
            (await self.ConfigureAwait(false)).Then(factory);

        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
        public async Task<Outcome<TNext>> ThenAsync<TNext>(Func<T, Task<Outcome<TNext>>> factory) =>
            await (await self.ConfigureAwait(false))
                .ThenAsync(factory)
                .ConfigureAwait(false);

        /// <inheritdoc cref="Composition.Then{T,TNext}(Outcome{T},Func{T,Outcome{TNext}})"/>
        public async ValueTask<Outcome<TNext>> ThenAsync<TNext>(Func<T, ValueTask<Outcome<TNext>>> factory) =>
            await (await self.ConfigureAwait(false))
                .ThenAsync(factory)
                .ConfigureAwait(false);

        /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
        public async Task<Outcome<T>> ThenAsync(Func<T, Outcome<None>> factory) =>
            (await self.ConfigureAwait(false)).Then(factory);

        /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
        public async Task<Outcome<T>> ThenAsync(Func<T, Task<Outcome<None>>> factory) =>
            await (await self.ConfigureAwait(false))
                .ThenAsync(factory)
                .ConfigureAwait(false);

        /// <inheritdoc cref="Composition.Then{T}(Outcome{T},Func{T,Outcome{None}})"/>
        public async ValueTask<Outcome<T>> ThenAsync(Func<T, ValueTask<Outcome<None>>> factory) =>
            await (await self.ConfigureAwait(false))
                .ThenAsync(factory)
                .ConfigureAwait(false);
    }

    #endregion
}
