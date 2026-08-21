namespace WarpCode.Outcomes;

/// <summary>
/// `Ensure` composition extensions
/// </summary>
public static class EnsureExtensions
{
    /// <param name="self">Current outcome.</param>
    extension<T>(Outcome<T> self)
    {
        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains a valid value, the ensure function is executed to validate the value.
        /// If the outcome contains a problem, the ensure function is not executed and the problem is propagated.
        /// </summary>
        /// <remarks>
        /// Represents a function that validates the outcome value against defined criteria and returns an outcome indicating whether the validation succeeded or failed.
        /// </remarks>
        /// <param name="ensure">Function to execute for validation.</param>
        /// <returns>The current outcome if it was valid, otherwise a new outcome representing the problem.</returns>
        public Outcome<T> Ensure(Func<T, Outcome<None>> ensure)
            => self.Then(value => ensure(value).Then(value));
    }

    /// <summary>Ensure extensions for an outcome containing no value.</summary>
    /// <param name="self">Current value-less outcome.</param>
    extension(Outcome<None> self)
    {
        /// <inheritdoc cref="EnsureExtensions.extension{T}(Outcome{T}).Ensure(Func{T, Outcome{None}})"/>
        public Outcome<None> Ensure(Func<Outcome<None>> ensure)
            => self.Ensure(_ => ensure());

        /// <summary>
        /// Happy-path composition operator.
        /// If the current outcome contains no problem, the `ensure` outcome is evaluated: the current outcome is returned when `ensure` is problem-free, otherwise its problem is propagated.
        /// If the current outcome contains a problem, `ensure` is not evaluated and the current problem is propagated.
        /// </summary>
        /// <remarks>
        /// A value-less outcome holds no value to validate, so this overload chains a sequence of <see cref="Outcome{None}"/> operations, short-circuiting on the first problem.
        /// </remarks>
        /// <param name="ensure">Outcome to evaluate for a problem if the current outcome is successful.</param>
        /// <returns>The current outcome if both it and `ensure` are problem-free, otherwise a new outcome representing the problem.</returns>
        public Outcome<None> Ensure(Outcome<None> ensure)
            => self.Ensure(_ => ensure);
    }

    /// <summary>Ensure extensions for an outcome containing a tuple of two values.</summary>
    /// <param name="self">Current outcome.</param>
    extension<T, T1>(Outcome<(T, T1)> self)
    {
        /// <inheritdoc cref="EnsureExtensions.extension{T}(Outcome{T}).Ensure(Func{T, Outcome{None}})"/>
        public Outcome<(T, T1)> Ensure(Func<T, T1, Outcome<None>> ensure)
            => self.Ensure(value => ensure(value.Item1, value.Item2));
    }

    /// <summary>Ensure extensions for an outcome containing a tuple of three values.</summary>
    /// <param name="self">Current outcome.</param>
    extension<T, T1, T2>(Outcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="EnsureExtensions.extension{T}(Outcome{T}).Ensure(Func{T, Outcome{None}})"/>
        public Outcome<(T, T1, T2)> Ensure(Func<T, T1, T2, Outcome<None>> ensure)
            => self.Ensure(value => ensure(value.Item1, value.Item2, value.Item3));
    }


    /// <summary>Ensure extensions for an outcome containing a tuple of four values.</summary>
    /// <param name="self">Current outcome.</param>
    extension<T, T1, T2, T3>(Outcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="EnsureExtensions.extension{T}(Outcome{T}).Ensure(Func{T, Outcome{None}})"/>
        public Outcome<(T, T1, T2, T3)> Ensure(Func<T, T1, T2, T3, Outcome<None>> ensure)
            => self.Ensure(value => ensure(value.Item1, value.Item2, value.Item3, value.Item4));
    }
}