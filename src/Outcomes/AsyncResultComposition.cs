#pragma warning disable CS1591

namespace WarpCode.Outcomes;
/// <summary>
/// Extensions that provide composition with the | operator for asynchronous versions of Bind, Map, Ensure, Rescue, OnValue and OnProblem operators
/// found in <see cref="ResultComposition"/>
/// Delegates now return Task or ValueTask of Result, and take an additional CancellationToken parameter to allow for asynchronous operations that can be cancelled.
/// </summary>
public static class AsyncResultComposition
{
    // Map and Bind
    extension<T, TNext>(AsyncResult<T>)
    {
        public static AsyncResult<TNext> operator |(AsyncResult<T> self, 
            Func<T, CancellationToken, ValueTask<Result<TNext>>> bind)
            => self.Then(self.BindAsync(bind));

        public static AsyncResult<TNext> operator |(AsyncResult<T> self,
            Func<T, CancellationToken, Task<Result<TNext>>> bind)
            => self.Then(self.BindAsync<T, TNext>((x, c) =>
                new(bind(x, c))
            ));

        public static AsyncResult<TNext> operator |(AsyncResult<T> self,
            Func<T, CancellationToken, ValueTask<TNext>> map)
            => self.Then(self.BindAsync<T, TNext>(async (x, c) =>
                await map(x, c).ConfigureAwait(false)
            ));

        public static AsyncResult<TNext> operator |(AsyncResult<T> self,
            Func<T, CancellationToken, Task<TNext>> map)
            => self.Then(self.BindAsync<T, TNext>(async (x, c) =>
                await map(x, c).ConfigureAwait(false)
            ));
    }

    // these instance (non-static) extension members are necessary because the | operator can't be used with async/await keywords
    // so we delegate to these methods that can be used with async/await to implement the operator overloads.
    // The `self with { ResultTask = ... }` form creates a new AsyncResult with the same cancellation token but a new task,
    // so we can use the same pattern for all the operator overloads.
    extension<T, TNext>(AsyncResult<T> self)
    {
        private async ValueTask<Result<TNext>> BindAsync(Func<T, CancellationToken, ValueTask<Result<TNext>>> bind)
            => (await self) switch
            {
                { Problem: not null } r => new(r.Problem),
                { Value: var v } => await bind(v, self.CancellationToken)
            };
    }

    // Ensure, Rescue, OnValue and OnProblem
    extension<T>(AsyncResult<T>)
    {
        public static AsyncResult<T> operator |(AsyncResult<T> self, 
            Func<T, CancellationToken, ValueTask<Result<None>>> ensure)
            => self with { ResultTask = self.EnsureAsync(ensure) };

        public static AsyncResult<T> operator |(AsyncResult<T> self, 
            Func<T, CancellationToken, Task<Result<None>>> ensure)
            => self with { ResultTask = self.EnsureAsync((x,c) => new(ensure(x,c))) };

        public static AsyncResult<T> operator |(AsyncResult<T> self, 
            Func<Problem, CancellationToken, ValueTask<Result<T>>> rescue)
            => self with { ResultTask = self.RescueAsync(rescue) };

        public static AsyncResult<T> operator |(AsyncResult<T> self, 
            Func<Problem, CancellationToken, Task<Result<T>>> rescue)
            => self with { ResultTask = self.RescueAsync((x,c) => new(rescue(x,c))) };

        public static AsyncResult<T> operator |(AsyncResult<T> self, 
            Func<T, CancellationToken, ValueTask> onValue)
            => self with { ResultTask = self.EnsureAsync(async (x, c) =>
            {
                await onValue(x, c).ConfigureAwait(false);
                return Result.Ok;
            }) };

        public static AsyncResult<T> operator |(AsyncResult<T> self,
            Func<T, CancellationToken, Task> onValue)
            => self with { ResultTask = self.EnsureAsync(async (x, c) =>
            {
                await onValue(x, c).ConfigureAwait(false);
                return Result.Ok;
            }) };

        public static ValueTask<Result<T>> operator |(AsyncResult<T> self, 

            Func<Problem, CancellationToken, ValueTask> onProblem)
            => (self with { ResultTask = self.RescueAsync(async (p, c) =>
            {
                await onProblem(p, c).ConfigureAwait(false);
                return p;
            }) }).ResultTask;

        public static ValueTask<Result<T>> operator |(AsyncResult<T> self, 
            Func<Problem, CancellationToken, Task> onProblem)
            => (self with { ResultTask = self.RescueAsync(async (p, c) =>
            {
                await onProblem(p, c).ConfigureAwait(false);
                return p;
            }) }).ResultTask;
    }

    // instance helpers. Note that OnValue and OnProblem can be implemented in terms of Ensure and Rescue respectively,
    // since they just need to return the original value or problem after executing the provided action.
    extension<T>(AsyncResult<T> self)
    {
        private async ValueTask<Result<T>> EnsureAsync(Func<T, CancellationToken, ValueTask<Result<None>>> ensure) 
            => await self switch
            {
                { Problem: not null } r => new(r.Problem),
                { Value: var v } => (await ensure(v, self.CancellationToken).ConfigureAwait(false)).Problem is { } problem
                    ? new(problem)
                    : new(v)
            };

        private async ValueTask<Result<T>> RescueAsync(Func<Problem, CancellationToken, ValueTask<Result<T>>> rescue)
        => await self switch
            {
                { Problem: not null } r => await rescue(r.Problem, self.CancellationToken).ConfigureAwait(false),
                { Value: var v } => new(v)
            };
    }

    // Map and Bind but without the value parameter in the provided functions, since there is no value to pass along.
    extension<TNext>(AsyncResult<None>)
    {
        public static ValueTask<Result<TNext>> operator |(AsyncResult<None> self,
            Func<CancellationToken, ValueTask<Result<TNext>>> bind)
            => self.Then(self.BindAsync((_, c) =>
                bind(c)
            ));

        public static ValueTask<Result<TNext>> operator |(AsyncResult<None> self,
            Func<CancellationToken, Task<Result<TNext>>> bind)
            => self.Then(self.BindAsync<None, TNext>((_, c) =>
                new(bind(c))
            ));

        public static ValueTask<Result<TNext>> operator |(AsyncResult<None> self, 
            Func<CancellationToken, ValueTask<TNext>> map)
            => self.Then(self.BindAsync<None,TNext>(async (_, c) =>
                await map(c).ConfigureAwait(false)
            ));

        public static ValueTask<Result<TNext>> operator |(AsyncResult<None> self, 
            Func<CancellationToken, Task<TNext>> map)
            => self.Then(self.BindAsync<None, TNext>(async (_, c) =>
                await map(c).ConfigureAwait(false)
            ));
    }

    // OnValue but without the value parameter in the provided functions, since there is no value to pass along.
    // As with the non-async version, Ensure doesn't make sense when you don't have a value to validate.
    // Rescue and OnProblem signatures are unchanged when there is no value since they take a problem as a parameter.
    extension(AsyncResult<None>)
    {
        public static ValueTask<Result<None>> operator |(AsyncResult<None> self, 
            Func<CancellationToken, ValueTask> onValue)
            => (self with { ResultTask = self.EnsureAsync(async (_, c) =>
            {
                await onValue(c);
                return Result.Ok;
            }) }).ResultTask;

        public static ValueTask<Result<None>> operator |(AsyncResult<None> self, 
            Func<CancellationToken, Task> onValue)
            => (self with { ResultTask = self.EnsureAsync(async (_, c) =>
            {
                await onValue(c);
                return Result.Ok;
            }) }).ResultTask;
    }
}
