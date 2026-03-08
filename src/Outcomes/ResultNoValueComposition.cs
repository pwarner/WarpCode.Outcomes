#pragma warning disable CS1591

using static WarpCode.Outcomes.FunctionAdaptation;

namespace WarpCode.Outcomes;

public static partial class ResultComposition
{
    extension<TNext>(Result<None>)
    {
        public static Result<TNext> operator |(Result<None> self, Func<Result<TNext>> next)
            => self.Problem is not null
                ? new(self.Problem)
                : next();

        public static Result<TNext> operator |(Result<None> self, Func<TNext> next)
            => self | Wrap(next);
    }

    extension(Result<None>)
    {
        public static Result<None> operator |(Result<None> self, Func<Result<None>> next)
            => (self.Problem ?? next().Problem) is { } problem
                ? new(problem)
                : self;

        public static Result<None> operator |(Result<None> self, Action onValue)
            => self | Wrap(onValue);

        public static Result<None> operator |(Result<None> self, Action<Problem> onProblem)
            => self | Wrap<None>(onProblem);
    }
}
