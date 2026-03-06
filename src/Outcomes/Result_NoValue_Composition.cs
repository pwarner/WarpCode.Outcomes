#pragma warning disable CS1591 

namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via the | operator on value-less <see cref="Result"/>s.
/// </summary>
public static class Result_NoValue_Composition
{
    extension<TNext>(Result)
    {
        public static Result<TNext> operator |(Result self, Func<TNext> next)
            => self._problem is not null ? new(self._problem) : new(next());

        public static Result<TNext> operator |(Result self, Func<Result<TNext>> next)
            => self._problem is not null ? new(self._problem) : next();
    }

    extension(Result)
    {
        public static Result operator |(Result self, Func<Result> next)
            => (self._problem ?? next()._problem) is { } problem
                ? new(problem)
                : self;

        public static Result operator |(Result self, Func<Problem, Result> rescue)
            => self._problem is not null ? rescue(self._problem) : self;

        public static Result operator |(Result self, Action onValue)
            => self | Wrap(onValue);
        
        public static Result operator |(Result self, Action<Problem> onProblem)
            => self | Wrap(onProblem);
    }

    private static Func<Result> Wrap(Action onValue) => () =>
    {
        onValue();
        return Result.Ok;
    };

    private static Func<Problem, Result> Wrap(Action<Problem> onProblem) => problem =>
    {
        onProblem(problem);
        return new(problem);
    };
}
