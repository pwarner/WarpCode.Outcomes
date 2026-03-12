#pragma warning disable CS1591 
using static WarpCode.Outcomes.FunctionAdaptation;

namespace WarpCode.Outcomes;
/// <summary>
/// Extensions that provide composition with the | operator for asynchronous versions of the functions
/// found in <see cref="ResultComposition"/>
/// </summary>
public static class ResultCompositionWithAsync
{
    extension<T, TNext>(Result<T>)
    {
        public static ValueTask<Result<TNext>> operator |(Result<T> self, Func<T, ValueTask<Result<TNext>>> bind)
            => self.BindAsync(bind);

        public static ValueTask<Result<TNext>> operator |(Result<T> self, Func<T, Task<Result<TNext>>> bind)
            => self.BindAsync(AsValueTask(bind));

        public static ValueTask<Result<TNext>> operator |(Result<T> self, Func<T, ValueTask<TNext>> map)
            => self.MapAsync(map);

        public static ValueTask<Result<TNext>> operator |(Result<T> self, Func<T, Task<TNext>> map)
            => self.MapAsync(AsValueTask(map));
    }

    // these extension instance (non-static) members are necessary because the | operator can't be used with async/await keywords
    // so we delegate to these methods that can be used with async/await to implement the operator overloads
    extension<T, TNext>(Result<T> self)
    {
        private async ValueTask<Result<TNext>> BindAsync(Func<T, ValueTask<Result<TNext>>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : await bind(self.Value);

        private async ValueTask<Result<TNext>> MapAsync(Func<T, ValueTask<TNext>> map)
            => self.Problem is not null
                ? new(self.Problem)
                : new(await map(self.Value));
    }

    extension<T>(Result<T>)
    {
        public static ValueTask<Result<T>> operator |(Result<T> self, Func<T, ValueTask<Result<None>>> ensure)
            => self.EnsureAsync(ensure);

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<T, Task<Result<None>>> ensure)
            => self.EnsureAsync(AsValueTask(ensure));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<Problem, ValueTask<Result<T>>> rescue)
            => self.RescueAsync(rescue);

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<Problem, Task<Result<T>>> rescue)
            => self.RescueAsync(AsValueTask(rescue));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<T, ValueTask> onValue)
            => self.BindAsync(WrapAsync(onValue));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<T, Task> onValue)
            => self.BindAsync(WrapAsync(onValue));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<Problem, ValueTask> onProblem)
            => self.RescueAsync(WrapAsync<T>(onProblem));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<Problem, Task> onProblem)
            => self.RescueAsync(WrapAsync<T>(onProblem));
    }

    extension<T>(Result<T> self)
    {
        private async ValueTask<Result<T>> EnsureAsync(Func<T, ValueTask<Result<None>>> ensure)
            => (self.Problem ?? (await ensure(self.Value)).Problem) is { } problem
                ? new(problem)
                : self;

        private async ValueTask<Result<T>> RescueAsync(Func<Problem, ValueTask<Result<T>>> rescue)
            => self.Problem is not null
                ? await rescue(self.Problem)
                : self;
    }

    extension<TNext>(Result<None>)
    {
        public static ValueTask<Result<TNext>> operator |(Result<None> self, Func<ValueTask<Result<TNext>>> bind)
            => self.BindAsync(bind);

        public static ValueTask<Result<TNext>> operator |(Result<None> self, Func<Task<Result<TNext>>> bind)
            => self.BindAsync(AsValueTask(bind));

        public static ValueTask<Result<TNext>> operator |(Result<None> self, Func<ValueTask<TNext>> map)
            => self.MapAsync(map);

        public static ValueTask<Result<TNext>> operator |(Result<None> self, Func<Task<TNext>> map)
            => self.MapAsync(AsValueTask(map));
    }

    extension<TNext>(Result<None> self)
    {
        private async ValueTask<Result<TNext>> BindAsync(Func<ValueTask<Result<TNext>>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : await bind();

        private async ValueTask<Result<TNext>> MapAsync(Func<ValueTask<TNext>> map)
            => self.Problem is not null
                ? new(self.Problem)
                : new(await map());
    }

    extension(Result<None>)
    {
        public static ValueTask<Result<None>> operator |(Result<None> self, Func<ValueTask<Result<None>>> ensure)
            => self.EnsureAsync(ensure);

        public static ValueTask<Result<None>> operator |(Result<None> self, Func<Task<Result<None>>> ensure)
            => self.EnsureAsync(AsValueTask(ensure));

        public static ValueTask<Result<None>> operator |(Result<None> self, Func<ValueTask> onValue)
            => self.BindAsync(WrapAsync(onValue));

        public static ValueTask<Result<None>> operator |(Result<None> self, Func<Task> onValue)
            => self.BindAsync(WrapAsync(onValue));
    }

    extension(Result<None> self)
    {
        public async ValueTask<Result<None>> EnsureAsync(Func<ValueTask<Result<None>>> next)
            => (self.Problem ?? (await next()).Problem) is { } problem
                ? new(problem)
                : self;
    }
}
