#pragma warning disable CS1591 

namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow aync composition via the | operator on <see cref="Result{T}"/>s
/// with functions returning <see cref="Task"/> and <see cref="Task{T}"/>.
/// </summary>
public static class Result_OfT_AsyncComposition_Task
{
    extension<T, TNext>(Result<T>)
    {
        public static Task<Result<TNext>> operator |(Result<T> self, Func<T, Task<TNext>> next)
            => self.Then(next);

        public static Task<Result<TNext>> operator |(Result<T> self, Func<T, Task<Result<TNext>>> next)
            => self.Then(next);
    }

    extension<T, TNext>(Result<T> self)
    {
        private async Task<Result<TNext>> Then(Func<T, Task<TNext>> next)
            => self._problem is not null ? new(self._problem) : new(await next(self._value));

        private async Task<Result<TNext>> Then(Func<T, Task<Result<TNext>>> next)
            => self._problem is not null ? new(self._problem) : await next(self._value);
    }

    extension<T>(Result<T>)
    {
        public static Task<Result<T>> operator |(Result<T> self, Func<T, Task<Result>> next)
            => self.Then(next);

        public static Task<Result<T>> operator |(Result<T> self, Func<Problem, Task<Result<T>>> rescue)
            => self.Then(rescue);

        public static Task<Result<T>> operator |(Result<T> self, Func<T, Task> onValue)
            => self.Then(Wrap(onValue));

        public static Task<Result<T>> operator |(Result<T> self, Func<Problem, Task> onProblem)
            => self.Then(Wrap<T>(onProblem));
    }

    extension<T>(Result<T> self)
    {
        private async Task<Result<T>> Then(Func<T, Task<Result>> next)
            => (self._problem ?? (await next(self._value))._problem) is { } problem
                ? new(problem)
                : self;
        private async Task<Result<T>> Then(Func<Problem, Task<Result<T>>> rescue)
            => self._problem is not null ? await rescue(self._problem) : self;
    }

    private static Func<T, Task<Result<T>>> Wrap<T>(Func<T, Task> onValue) => async value =>
    {
        await onValue(value);
        return new(value);
    };

    private static Func<Problem, Task<Result<T>>> Wrap<T>(Func<Problem, Task> onProblem) => async problem =>
    {
        await onProblem(problem);
        return new(problem);
    };
}
