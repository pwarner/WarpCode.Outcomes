namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via the | operator on <see cref="Outcome{T}"/>
/// </summary>
public static partial class OutcomeComposition
{
    extension<T, TNext>(Outcome<T>)
    {
        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains a valid value, the bind function is executed to produce the next outcome.
        /// If the outcome contains a problem, the bind function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="self">Current outcome.</param>
        /// <param name="bind">Function to execute to obtain next outcome.</param>
        /// <returns>The next outcome.</returns>
        public static Outcome<TNext> operator |(Outcome<T> self, Func<T, Outcome<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind(self.Value);

        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains a valid value, the map function is executed to produce the value of the next outcome.
        /// If the outcome contains a problem, the map function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="self">Current outcome.</param>
        /// <param name="map">Function to execute to obtain next value.</param>
        /// <returns>The next outcome.</returns>
        public static Outcome<TNext> operator |(Outcome<T> self, Func<T, TNext> map)
            => self | (value => new Outcome<TNext>(map(value)));
    }

    extension<T>(Outcome<T>)
    {
        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains a valid value, the ensure function is executed to validate the value.
        /// If the outcome contains a problem, the ensure function is not executed and the problem is propagated.
        /// </summary>
        /// <remarks>
        /// Represents a function that validates the outcome value against defined criteria and returns an outcome indicating whether the validation succeeded or failed.
        /// </remarks>
        /// <param name="self">Current outcome.</param>
        /// <param name="ensure">Function to execute for validation.</param>
        /// <returns>The next outcome.</returns>
        public static Outcome<T> operator |(Outcome<T> self, Func<T, Outcome<None>> ensure)
            => (self.Problem ?? ensure(self.Value).Problem) is { } problem
                ? new(problem)
                : self;

        /// <summary>
        /// Sad-path composition operator.
        /// If the outcome contains a problem, the rescue function is executed to attempt to recover from the problem and produce the next outcome.
        /// If the outcome contains a value , the rescue function is not executed and the current outcome returned.
        /// </summary>
        /// <param name="self">Current outcome.</param>
        /// <param name="rescue">Function to execute to attempt recovery from the problem.</param>
        /// <returns>The next outcome.</returns>
        public static Outcome<T> operator |(Outcome<T> self, Func<Problem, Outcome<T>> rescue)
            => self.Problem is not null
                ? rescue(self.Problem)
                : self;

        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains a valid value, the onValue action is executed.
        /// If the outcome contains a problem, the onValue action is not executed and the problem is propagated.
        /// </summary>
        /// <param name="self">Current outcome.</param>
        /// <param name="onValue">Action to execute if the outcome is successful.</param>
        /// <returns>The next outcome.</returns>
        public static Outcome<T> operator |(Outcome<T> self, Action<T> onValue)
            => self | (value =>
            {
                onValue(value);
                return Outcome.Ok;
            });

        /// <summary>
        /// Sad-path composition operator.
        /// If the outcome contains a problem, the onProblem action is executed.
        /// If the outcome contains a value, the onProblem action is not executed and the current outcome is returned.
        /// </summary>
        /// <param name="self">Current outcome.</param>
        /// <param name="onProblem">Action to execute if the outcome is a problem.</param>
        /// <returns>The next outcome.</returns>
        public static Outcome<T> operator |(Outcome<T> self, Action<Problem> onProblem)
            => self | (problem =>
            {
                onProblem(problem);
                return problem;
            });
    }

    extension<TNext>(Outcome<None>)
    {

        /// <inheritdoc cref=" OutcomeComposition.extension{T,TNext}(Outcome{T}).operator |(Outcome{T}, Func{T, Outcome{TNext}})"/>
        public static Outcome<TNext> operator |(Outcome<None> self, Func<Outcome<TNext>> bind)
            => self | (_ => bind());

        /// <inheritdoc cref=" OutcomeComposition.extension{T,TNext}(Outcome{T}).operator |(Outcome{T}, Func{T, TNext})"/>
        public static Outcome<TNext> operator |(Outcome<None> self, Func<TNext> map)
            => self | (_ => new Outcome<TNext>(map()));
    }

    extension(Outcome<None>)
    {
        /// <inheritdoc cref=" OutcomeComposition.extension{T}(Outcome{T}).operator |(Outcome{T}, Action{T})"/>
        public static Outcome<None> operator |(Outcome<None> self, Action onValue)
            => self | ((None _) =>
            {
                onValue();
                return Outcome.Ok;
            });
    }
}
