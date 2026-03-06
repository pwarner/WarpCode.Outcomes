#pragma warning disable CS1591 
using static WarpCode.Outcomes.FunctionAdaptation;

namespace WarpCode.Outcomes;

public static partial class ResultComposition
{
    extension<T, TNext>(Result<T>)
    {
        public static ValueTask<Result<TNext>> operator |(Result<T> self, Func<T, ValueTask<Result<TNext>>> next)
            => self.Then(next);

        public static ValueTask<Result<TNext>> operator |(Result<T> self, Func<T, Task<Result<TNext>>> next)
            => self.Then(Wrap(next));

        public static ValueTask<Result<TNext>> operator |(Result<T> self, Func<T, ValueTask<TNext>> next)
            => self.Then(Wrap(next));

        public static ValueTask<Result<TNext>> operator |(Result<T> self, Func<T, Task<TNext>> next)
            => self.Then(Wrap(next));
    }

    extension<T, TNext>(Result<T> self)
    {
        private async ValueTask<Result<TNext>> Then(Func<T, ValueTask<Result<TNext>>> next)
            => self.Problem is not null
                ? new(self.Problem)
                : await next(self.Value);
    }

    extension<T>(Result<T>)
    {
        public static ValueTask<Result<T>> operator |(Result<T> self, Func<T, ValueTask<Result<None>>> next)
            => self.Then(next);

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<Problem, ValueTask<Result<T>>> rescue)
            => self.Then(rescue);

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<T, Task<Result<None>>> next)
            => self.Then(Wrap(next));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<Problem, Task<Result<T>>> rescue)
            => self.Then(Wrap(rescue));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<T, ValueTask> onValue)
            => self.Then(Wrap(onValue));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<Problem, ValueTask> onProblem)
            => self.Then(Wrap<T>(onProblem));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<T, Task> onValue)
            => self.Then(Wrap(onValue));

        public static ValueTask<Result<T>> operator |(Result<T> self, Func<Problem, Task> onProblem)
            => self.Then(Wrap<T>(onProblem));
    }

    extension<T>(Result<T> self)
    {
        private async ValueTask<Result<T>> Then(Func<T, ValueTask<Result<None>>> next)
            => (self.Problem ?? (await next(self.Value)).Problem) is { } problem
                ? new(problem)
                : self;

        private async ValueTask<Result<T>> Then(Func<Problem, ValueTask<Result<T>>> rescue)
            => self.Problem is not null
                ? await rescue(self.Problem)
                : self;
    }
}
