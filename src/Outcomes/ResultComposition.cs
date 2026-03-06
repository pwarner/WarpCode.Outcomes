#pragma warning disable CS1591

using static WarpCode.Outcomes.FunctionAdaptation;

namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via the | operator on <see cref="Result{T}"/>, Task{Result{T}}, and ValueTask{Result{T}}
/// </summary>
public static partial class ResultComposition
{
    extension<T, TNext>(Result<T>)
    {
        public static Result<TNext> operator |(Result<T> self, Func<T, Result<TNext>> next)
            => self.Problem is not null
                ? new(self.Problem)
                : next(self.Value);

        public static Result<TNext> operator |(Result<T> self, Func<T, TNext> next)
            => self | Wrap(next);
    }

    extension<T>(Result<T>)
    {
        public static Result<T> operator |(Result<T> self, Func<T, Result<None>> next)
            => (self.Problem ?? next(self.Value).Problem) is { } problem
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
}
