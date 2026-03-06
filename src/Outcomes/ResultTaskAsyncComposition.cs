#pragma warning disable CS1591 
using static WarpCode.Outcomes.FunctionAdaptation;

namespace WarpCode.Outcomes;

public static partial class ResultComposition
{
    extension<T, TNext>(ValueTask<Result<T>>)
    {
        public static ValueTask<Result<TNext>> operator |(ValueTask<Result<T>> self, Func<T, ValueTask<Result<TNext>>> next)
            => self.Then(next);

        public static ValueTask<Result<TNext>> operator |(ValueTask<Result<T>> self, Func<T, Task<Result<TNext>>> next)
            => self.Then(Wrap(next));

        public static ValueTask<Result<TNext>> operator |(ValueTask<Result<T>> self, Func<T, ValueTask<TNext>> next)
            => self.Then(Wrap(next));

        public static ValueTask<Result<TNext>> operator |(ValueTask<Result<T>> self, Func<T, Task<TNext>> next)
            => self.Then(Wrap(next));
    }

    extension<T, TNext>(ValueTask<Result<T>> self)
    {
        private async ValueTask<Result<TNext>> Then(Func<T, ValueTask<Result<TNext>>> next)
            => await (await self).Then(next);
    }

    extension<T>(ValueTask<Result<T>>)
    {
        public static ValueTask<Result<T>> operator |(ValueTask<Result<T>> self, Func<T, ValueTask<Result<None>>> next)
            => self.Then(next);

        public static ValueTask<Result<T>> operator |(ValueTask<Result<T>> self, Func<Problem, ValueTask<Result<T>>> rescue)
            => self.Then(rescue);

        public static ValueTask<Result<T>> operator |(ValueTask<Result<T>> self, Func<T, Task<Result<None>>> next)
            => self.Then(Wrap(next));

        public static ValueTask<Result<T>> operator |(ValueTask<Result<T>> self, Func<Problem, Task<Result<T>>> rescue)
            => self.Then(Wrap(rescue));

        public static ValueTask<Result<T>> operator |(ValueTask<Result<T>> self, Func<T, ValueTask> onValue)
            => self.Then(Wrap(onValue));

        public static ValueTask<Result<T>> operator |(ValueTask<Result<T>> self, Func<Problem, ValueTask> onProblem)
            => self.Then(Wrap<T>(onProblem));

        public static ValueTask<Result<T>> operator |(ValueTask<Result<T>> self, Func<T, Task> onValue)
            => self.Then(Wrap(onValue));

        public static ValueTask<Result<T>> operator |(ValueTask<Result<T>> self, Func<Problem, Task> onProblem)
            => self.Then(Wrap<T>(onProblem));
    }

    extension<T>(ValueTask<Result<T>> self)
    {
        private async ValueTask<Result<T>> Then(Func<T, ValueTask<Result<None>>> next)
            => await (await self).Then(next);

        private async ValueTask<Result<T>> Then(Func<Problem, ValueTask<Result<T>>> rescue)
            => await (await self).Then(rescue);
    }

    extension<T, TNext>(Task<Result<T>>)
    {
        public static ValueTask<Result<TNext>> operator |(Task<Result<T>> self, Func<T, ValueTask<Result<TNext>>> next)
            => self.Then(next);

        public static ValueTask<Result<TNext>> operator |(Task<Result<T>> self, Func<T, Task<Result<TNext>>> next)
            => self.Then(Wrap(next));

        public static ValueTask<Result<TNext>> operator |(Task<Result<T>> self, Func<T, ValueTask<TNext>> next)
            => self.Then(Wrap(next));

        public static ValueTask<Result<TNext>> operator |(Task<Result<T>> self, Func<T, Task<TNext>> next)
            => self.Then(Wrap(next));
    }

    extension<T, TNext>(Task<Result<T>> self)
    {
        private async ValueTask<Result<TNext>> Then(Func<T, ValueTask<Result<TNext>>> next)
            => await (await self).Then(next);
    }

    extension<T>(Task<Result<T>>)
    {
        public static ValueTask<Result<T>> operator |(Task<Result<T>> self, Func<T, ValueTask<Result<None>>> next)
            => self.Then(next);

        public static ValueTask<Result<T>> operator |(Task<Result<T>> self, Func<Problem, ValueTask<Result<T>>> rescue)
            => self.Then(rescue);

        public static ValueTask<Result<T>> operator |(Task<Result<T>> self, Func<T, Task<Result<None>>> next)
            => self.Then(Wrap(next));

        public static ValueTask<Result<T>> operator |(Task<Result<T>> self, Func<Problem, Task<Result<T>>> rescue)
            => self.Then(Wrap(rescue));

        public static ValueTask<Result<T>> operator |(Task<Result<T>> self, Func<T, ValueTask> onValue)
            => self.Then(Wrap(onValue));

        public static ValueTask<Result<T>> operator |(Task<Result<T>> self, Func<Problem, ValueTask> onProblem)
            => self.Then(Wrap<T>(onProblem));

        public static ValueTask<Result<T>> operator |(Task<Result<T>> self, Func<T, Task> onValue)
            => self.Then(Wrap(onValue));

        public static ValueTask<Result<T>> operator |(Task<Result<T>> self, Func<Problem, Task> onProblem)
            => self.Then(Wrap<T>(onProblem));
    }

    extension<T>(Task<Result<T>> self)
    {
        private async ValueTask<Result<T>> Then(Func<T, ValueTask<Result<None>>> next)
            => await (await self).Then(next);

        private async ValueTask<Result<T>> Then(Func<Problem, ValueTask<Result<T>>> rescue)
            => await (await self).Then(rescue);
    }
}
