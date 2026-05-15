namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via the | operator on <see cref="Result{T}"/>
/// </summary>
public static partial class ResultComposition
{
    extension<T, TNext>(Result<T>)
    {
        /// <summary>
        /// Happy-path composition operator.
        /// If the result contains a valid value, the bind function is executed to produce the next result.
        /// If the result contains a problem, the bind function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="self">Current result.</param>
        /// <param name="bind">Function to execute to obtain next result.</param>
        /// <returns>The next result.</returns>
        public static Result<TNext> operator |(Result<T> self, Func<T, Result<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind(self.Value);

        /// <summary>
        /// Happy-path composition operator.
        /// If the result contains a valid value, the map function is executed to produce the value of the next result.
        /// If the result contains a problem, the map function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="self">Current result.</param>
        /// <param name="map">Function to execute to obtain next value.</param>
        /// <returns>The next result.</returns>
        public static Result<TNext> operator |(Result<T> self, Func<T, TNext> map)
            => self | (value => new Result<TNext>(map(value)));
    }

    extension<T>(Result<T>)
    {
        /// <summary>
        /// Happy-path composition operator.
        /// If the result contains a valid value, the ensure function is executed to validate the value.
        /// If the result contains a problem, the ensure function is not executed and the problem is propagated.
        /// </summary>
        /// <remarks>
        /// Represents a function that validates the result value against defined criteria and returns a result indicating whether the validation succeeded or failed.
        /// </remarks>
        /// <param name="self">Current result.</param>
        /// <param name="ensure">Function to execute for validation.</param>
        /// <returns>The next result.</returns>
        public static Result<T> operator |(Result<T> self, Func<T, Result<None>> ensure)
            => (self.Problem ?? ensure(self.Value).Problem) is { } problem
                ? new(problem)
                : self;

        /// <summary>
        /// Sad-path composition operator.
        /// If the result contains a problem, the rescue function is executed to attempt to recover from the problem and produce the next result.
        /// If the result contains a value , the rescue function is not executed and the current result returned.
        /// </summary>
        /// <param name="self">Current result.</param>
        /// <param name="rescue">Function to execute to attempt recovery from the problem.</param>
        /// <returns>The next result.</returns>
        public static Result<T> operator |(Result<T> self, Func<Problem, Result<T>> rescue)
            => self.Problem is not null
                ? rescue(self.Problem)
                : self;

        /// <summary>
        /// Happy-path composition operator.
        /// If the result contains a valid value, the onValue action is executed.
        /// If the result contains a problem, the onValue action is not executed and the problem is propagated.
        /// </summary>
        /// <param name="self">Current result.</param>
        /// <param name="onValue">Action to execute if the result is successful.</param>
        /// <returns>The next result.</returns>
        public static Result<T> operator |(Result<T> self, Action<T> onValue)
            => self | (value =>
            {
                onValue(value);
                return Result.Ok;
            });

        /// <summary>
        /// Sad-path composition operator.
        /// If the result contains a problem, the onProblem action is executed.
        /// If the result contains a value, the onProblem action is not executed and the current result is returned.
        /// </summary>
        /// <param name="self">Current result.</param>
        /// <param name="onProblem">Action to execute if the result is a problem.</param>
        /// <returns>The next result.</returns>
        public static Result<T> operator |(Result<T> self, Action<Problem> onProblem)
            => self | (problem =>
            {
                onProblem(problem);
                return problem;
            });
    }

    extension<TNext>(Result<None>)
    {

        /// <inheritdoc cref=" ResultComposition.extension{T,TNext}(Result{T}).operator |(Result{T}, Func{T, Result{TNext}})"/>
        public static Result<TNext> operator |(Result<None> self, Func<Result<TNext>> bind)
            => self | (_ => bind());

        /// <inheritdoc cref=" ResultComposition.extension{T,TNext}(Result{T}).operator |(Result{T}, Func{T, TNext})"/>
        public static Result<TNext> operator |(Result<None> self, Func<TNext> map)
            => self | (_ => new Result<TNext>(map()));
    }

    extension(Result<None>)
    {
        /// <inheritdoc cref=" ResultComposition.extension{T}(Result{T}).operator |(Result{T}, Action{T})"/>
        public static Result<None> operator |(Result<None> self, Action onValue)
            => self | ((None _) =>
            {
                onValue();
                return Result.Ok;
            });
    }
}
