#pragma warning disable CS1591

using static WarpCode.Outcomes.FunctionAdaptation;

namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via the | operator on <see cref="Result{T}"/>
/// </summary>
public static class ResultComposition
{
    extension<T, TNext>(Result<T>)
    {
        public static Result<TNext> operator |(Result<T> self, Func<T, Result<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind(self.Value);

        public static Result<TNext> operator |(Result<T> self, Func<T, TNext> map)
            => self.Problem is not null
                ? new(self.Problem)
                : new(map(self.Value));
    }

    extension<T>(Result<T>)
    {
        public static Result<T> operator |(Result<T> self, Func<T, Result<None>> ensure)
            => (self.Problem ?? ensure(self.Value).Problem) is { } problem
                ? new(problem)
                : self;

        public static Result<T> operator |(Result<T> self, Func<Problem, Result<T>> rescue)
            => self.Problem is not null
                ? rescue(self.Problem)
                : self;

        public static Result<T> operator |(Result<T> self, Action<T> onValue)
            => self | Wrap(onValue);

        public static Result<T> operator |(Result<T> self, Action<Problem> onProblem)
            => self | Wrap<T>(onProblem);
    }

    extension<TNext>(Result<None>)
    {
        public static Result<TNext> operator |(Result<None> self, Func<Result<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind();

        public static Result<TNext> operator |(Result<None> self, Func<TNext> map)
            => self.Problem is not null
                ? new(self.Problem)
                : new(map());
    }

    extension(Result<None>)
    {
        public static Result<None> operator |(Result<None> self, Func<Result<None>> ensure)
            => (self.Problem ?? ensure().Problem) is { } problem
                ? new(problem)
                : self;

        public static Result<None> operator |(Result<None> self, Action onValue)
            => self | Wrap(onValue);
    }
}
