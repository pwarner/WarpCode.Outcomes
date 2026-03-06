#pragma warning disable CS1591 

namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via the | operator on <see cref="Result{T}"/>s.
/// </summary>
public static class Result_OfT_Composition
{
    extension<T, TNext>(Result<T>)
    {
        public static Result<TNext> operator |(Result<T> self, Func<T, TNext> next)
            => self._problem is not null ? new(self._problem) : new(next(self._value));

        public static Result<TNext> operator |(Result<T> self, Func<T, Result<TNext>> next)
            => self._problem is not null ? new(self._problem) : next(self._value);
    }

    extension<T>(Result<T>)
    {
        public static Result<T> operator |(Result<T> self, Func<T, Result> next)
            => (self._problem ?? next(self._value)._problem) is { } problem
                ? new(problem)
                : self;

        public static Result<T> operator |(Result<T> self, Func<Problem, Result<T>> rescue)
            => self._problem is not null ? rescue(self._problem) : self;

        public static Result<T> operator |(Result<T> self, Action<T> onValue)
            => self | Wrap(onValue);

        public static Result<T> operator |(Result<T> self, Action<Problem> onProblem)
            => self | Wrap<T>(onProblem);
    }

    private static Func<T, Result<T>> Wrap<T>(Action<T> onValue) => value =>
    {
        onValue(value);
        return new(value);
    };

    private static Func<Problem, Result<T>> Wrap<T>(Action<Problem> onProblem) => problem =>
    {
        onProblem(problem);
        return new(problem);
    };
}
