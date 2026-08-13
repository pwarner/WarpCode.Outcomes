#pragma warning disable CS1591

namespace WarpCode.Outcomes;
/// <summary>
/// Extensions that provide composition with the | operator for asynchronous versions of Bind, Map, Ensure, Rescue, OnValue and OnProblem operators
/// found in <see cref="OutcomeComposition"/>
/// Delegates now return Task or ValueTask of Outcome, and take an additional CancellationToken parameter to allow for asynchronous operations that can be cancelled.
/// </summary>
public static class AsyncOutcomeComposition
{
    // Map and Bind
    extension<T, TNext>(AsyncOutcome<T>)
    {
        public static AsyncOutcome<TNext> operator |(AsyncOutcome<T> self,
            Func<T, CancellationToken, ValueTask<Outcome<TNext>>> bind)
            => self.Then(self.BindAsync(bind));

        public static AsyncOutcome<TNext> operator |(AsyncOutcome<T> self,
            Func<T, CancellationToken, Task<Outcome<TNext>>> bind)
            => self.Then(self.BindAsync<T, TNext>((x, c) =>
                new(bind(x, c))
            ));

        public static AsyncOutcome<TNext> operator |(AsyncOutcome<T> self,
            Func<T, CancellationToken, ValueTask<TNext>> map)
            => self.Then(self.BindAsync<T, TNext>(async (x, c) =>
                await map(x, c).ConfigureAwait(false)
            ));

        public static AsyncOutcome<TNext> operator |(AsyncOutcome<T> self,
            Func<T, CancellationToken, Task<TNext>> map)
            => self.Then(self.BindAsync<T, TNext>(async (x, c) =>
                await map(x, c).ConfigureAwait(false)
            ));
    }

    // these instance (non-static) extension members are necessary because the | operator can't be used with async/await keywords
    // so we delegate to these methods that can be used with async/await to implement the operator overloads.
    extension<T, TNext>(AsyncOutcome<T> self)
    {
        private async ValueTask<Outcome<TNext>> BindAsync(Func<T, CancellationToken, ValueTask<Outcome<TNext>>> bind)
            => await self switch
            {
                { Problem: not null } r => new(r.Problem),
                { Value: var v } => await bind(v, self.CancellationToken)
            };
    }

    // Ensure, Rescue, OnValue and OnProblem
    // The `self with { OutcomeTask = ... }` form creates a new AsyncOutcome with the same cancellation token but a new task.
    // We can use this pattern for operator overloads where the outcome type is the same as the input type.
    extension<T>(AsyncOutcome<T>)
    {
        public static AsyncOutcome<T> operator |(AsyncOutcome<T> self,
            Func<T, CancellationToken, ValueTask<Outcome<None>>> ensure)
            => self with { OutcomeTask = self.EnsureAsync(ensure) };

        public static AsyncOutcome<T> operator |(AsyncOutcome<T> self,
            Func<T, CancellationToken, Task<Outcome<None>>> ensure)
            => self with { OutcomeTask = self.EnsureAsync((x, c) => new(ensure(x, c))) };

        public static AsyncOutcome<T> operator |(AsyncOutcome<T> self,
            Func<Problem, CancellationToken, ValueTask<Outcome<T>>> rescue)
            => self with { OutcomeTask = self.RescueAsync(rescue) };

        public static AsyncOutcome<T> operator |(AsyncOutcome<T> self,
            Func<Problem, CancellationToken, Task<Outcome<T>>> rescue)
            => self with { OutcomeTask = self.RescueAsync((x, c) => new(rescue(x, c))) };

        public static AsyncOutcome<T> operator |(AsyncOutcome<T> self,
            Func<T, CancellationToken, ValueTask> onValue)
            => self with
            {
                OutcomeTask = self.EnsureAsync(async (x, c) =>
                {
                    await onValue(x, c).ConfigureAwait(false);
                    return Outcome.Ok;
                })
            };

        public static AsyncOutcome<T> operator |(AsyncOutcome<T> self,
            Func<T, CancellationToken, Task> onValue)
            => self with
            {
                OutcomeTask = self.EnsureAsync(async (x, c) =>
                {
                    await onValue(x, c).ConfigureAwait(false);
                    return Outcome.Ok;
                })
            };

        public static AsyncOutcome<T> operator |(AsyncOutcome<T> self,
            Func<Problem, CancellationToken, ValueTask> onProblem)
            => self with
            {
                OutcomeTask = self.RescueAsync(async (p, c) =>
                {
                    await onProblem(p, c).ConfigureAwait(false);
                    return p;
                })
            };

        public static AsyncOutcome<T> operator |(AsyncOutcome<T> self,
            Func<Problem, CancellationToken, Task> onProblem)
            => self with
            {
                OutcomeTask = self.RescueAsync(async (p, c) =>
                {
                    await onProblem(p, c).ConfigureAwait(false);
                    return p;
                })
            };
    }

    // instance helpers. Note that OnValue and OnProblem can be implemented in terms of Ensure and Rescue respectively,
    // since they just need to return the original value or problem after executing the provided action.
    extension<T>(AsyncOutcome<T> self)
    {
        private async ValueTask<Outcome<T>> EnsureAsync(Func<T, CancellationToken, ValueTask<Outcome<None>>> ensure)
            => await self switch
            {
                { Problem: not null } r => new(r.Problem),
                { Value: var v } => (await ensure(v, self.CancellationToken).ConfigureAwait(false)).Problem is { } problem
                    ? new(problem)
                    : new(v)
            };

        private async ValueTask<Outcome<T>> RescueAsync(Func<Problem, CancellationToken, ValueTask<Outcome<T>>> rescue)
        => await self switch
        {
            { Problem: not null } r => await rescue(r.Problem, self.CancellationToken).ConfigureAwait(false),
            { Value: var v } => new(v)
        };
    }

    // Map and Bind but without the value parameter in the provided functions, since there is no value to pass along.
    extension<TNext>(AsyncOutcome<None>)
    {
        public static AsyncOutcome<TNext> operator |(AsyncOutcome<None> self,
            Func<CancellationToken, ValueTask<Outcome<TNext>>> bind)
            => self.Then(self.BindAsync((_, c) =>
                bind(c)
            ));

        public static AsyncOutcome<TNext> operator |(AsyncOutcome<None> self,
            Func<CancellationToken, Task<Outcome<TNext>>> bind)
            => self.Then(self.BindAsync<None, TNext>((_, c) =>
                new(bind(c))
            ));

        public static AsyncOutcome<TNext> operator |(AsyncOutcome<None> self,
            Func<CancellationToken, ValueTask<TNext>> map)
            => self.Then(self.BindAsync<None, TNext>(async (_, c) =>
                await map(c).ConfigureAwait(false)
            ));

        public static AsyncOutcome<TNext> operator |(AsyncOutcome<None> self,
            Func<CancellationToken, Task<TNext>> map)
            => self.Then(self.BindAsync<None, TNext>(async (_, c) =>
                await map(c).ConfigureAwait(false)
            ));
    }

    // OnValue but without the value parameter in the provided functions, since there is no value to pass along.
    // As with the non-async version, Ensure doesn't make sense when you don't have a value to validate.
    // Rescue and OnProblem signatures are unchanged when there is no value since they take a problem as a parameter.
    extension(AsyncOutcome<None>)
    {
        public static AsyncOutcome<None> operator |(AsyncOutcome<None> self,
            Func<CancellationToken, ValueTask> onValue)
            => self with
            {
                OutcomeTask = self.EnsureAsync(async (_, c) =>
                {
                    await onValue(c);
                    return Outcome.Ok;
                })
            };

        public static AsyncOutcome<None> operator |(AsyncOutcome<None> self,
            Func<CancellationToken, Task> onValue)
            => self with
            {
                OutcomeTask = self.EnsureAsync(async (_, c) =>
                {
                    await onValue(c);
                    return Outcome.Ok;
                })
            };
    }
}
