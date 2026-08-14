namespace WarpCode.Outcomes;

/// <summary>
/// `Then` composition extensions
/// </summary>
public static class ThenExtensions
{
    /// <param name="self">Current outcome.</param>
    extension<T, TNext>(Outcome<T> self)
    {
        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains a valid value, the `next` function is executed to produce the next outcome.
        /// If the outcome contains a problem, the `next` function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="next">Function to execute to obtain next outcome.</param>
        /// <returns>An Outcome{TNext} representing the result of the composition.</returns>
        public Outcome<TNext> Then(Func<T, Outcome<TNext>> next) =>
            self.Match(next, static problem => new(problem));

        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains a valid value, the `next` function is executed to produce the next outcome.
        /// If the outcome contains a problem, the `next` function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="next">Function to execute to obtain next value.</param>
        /// <returns>An Outcome{TNext} representing the result of the composition.</returns>
        public Outcome<TNext> Then(Func<T, TNext> next) =>
            self.Then(value => new Outcome<TNext>(next(value)));
    }

    /// <summary>Then extensions for an outcome containing no value.</summary>
    /// <param name="self">Current value-less outcome.</param>
    extension<TNext>(Outcome<None> self)
    {
        /// <inheritdoc cref=" ThenExtensions.extension{T,TNext}(Outcome{T}).Then(Func{T, Outcome{TNext}})"/>
        public Outcome<TNext> Then(Func<Outcome<TNext>> next)
            => self.Then(_ => next());

        /// <inheritdoc cref=" ThenExtensions.extension{T,TNext}(Outcome{T}).Then(Func{T, TNext})"/>
        public Outcome<TNext> Then(Func<TNext> next)
            => self.Then(_ => new Outcome<TNext>(next()));

        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains no problem, the `next` outcome is returned.
        /// If the outcome contains a problem, the `next` outcome is not returned and the problem is propagated.
        /// </summary>
        /// <param name="next">Outcome to return if the current outcome is successful.</param>
        /// <returns>An Outcome{TNext} representing the result of the composition.</returns>
        public Outcome<TNext> Then(Outcome<TNext> next)
            => self.Then(_ => next);

        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains no problem, the `next` value is returned as an outcome.
        /// If the outcome contains a problem, the `next` outcome is not returned and the problem is propagated.
        /// </summary>
        /// <param name="next">Value to return as an outcome if the current outcome is successful.</param>
        /// <returns>An Outcome{TNext} representing the result of the composition.</returns>
        public Outcome<TNext> Then(TNext next)
            => self.Then(_ => new Outcome<TNext>(next));
    }

    /// <summary>Then extensions for an outcome containing a tuple of two values.</summary>
    /// <param name="self">Current outcome.</param>
    extension<T, T1, TNext>(Outcome<(T, T1)> self)
    {
        /// <inheritdoc cref="ThenExtensions.extension{T,TNext}(Outcome{T}).Then(Func{T, Outcome{TNext}})"/>
        public Outcome<TNext> Then(Func<T, T1, Outcome<TNext>> next)
            => self.Then(value => next(value.Item1, value.Item2));

        /// <inheritdoc cref="ThenExtensions.extension{T,TNext}(Outcome{T}).Then(Func{T, TNext})"/>
        public Outcome<TNext> Then(Func<T, T1, TNext> next)
            => self.Then(value => next(value.Item1, value.Item2));
    }

    /// <summary>Then extensions for an outcome containing a tuple of three values.</summary>
    /// <param name="self">Current outcome.</param>
    extension<T, T1, T2, TNext>(Outcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="ThenExtensions.extension{T,TNext}(Outcome{T}).Then(Func{T, Outcome{TNext}})"/>
        public Outcome<TNext> Then(Func<T, T1, T2, Outcome<TNext>> next)
            => self.Then(value => next(value.Item1, value.Item2, value.Item3));

        /// <inheritdoc cref="ThenExtensions.extension{T,TNext}(Outcome{T}).Then(Func{T, TNext})"/>
        public Outcome<TNext> Then(Func<T, T1, T2, TNext> next)
            => self.Then(value => next(value.Item1, value.Item2, value.Item3));
    }

    /// <summary>Then extensions for an outcome containing a tuple of four values.</summary>
    /// <param name="self">Current outcome.</param>
    extension<T, T1, T2, T3, TNext>(Outcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="ThenExtensions.extension{T,TNext}(Outcome{T}).Then(Func{T, Outcome{TNext}})"/>
        public Outcome<TNext> Then(Func<T, T1, T2, T3, Outcome<TNext>> next)
            => self.Then(value => next(value.Item1, value.Item2, value.Item3, value.Item4));

        /// <inheritdoc cref="ThenExtensions.extension{T,TNext}(Outcome{T}).Then(Func{T, TNext})"/>
        public Outcome<TNext> Then(Func<T, T1, T2, T3, TNext> next)
            => self.Then(value => next(value.Item1, value.Item2, value.Item3, value.Item4));
    }
}