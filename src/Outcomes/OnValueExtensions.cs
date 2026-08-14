namespace WarpCode.Outcomes;

/// <summary>
/// `OnValue` composition extensions
/// </summary>
public static class OnValueExtensions
{
    /// <param name="self">Current outcome.</param>
    extension<T>(Outcome<T> self)
    {
        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains a valid value, the onValue action is executed.
        /// If the outcome contains a problem, the onValue action is not executed and the problem is propagated.
        /// </summary>
        /// <param name="onValue">Action to execute if the outcome is successful.</param>
        /// <returns>The current outcome.</returns>
        public Outcome<T> OnValue(Action<T> onValue)
            => self.Then(value =>
            {
                onValue(value);
                return self;
            });
    }

    /// <param name="self">Current value-less outcome.</param>
    extension(Outcome<None> self)
    {
        /// <summary>
        /// Happy-path composition operator.
        /// If the outcome contains no problem, the onValue action is executed.
        /// If the outcome contains a problem, the onValue action is not executed and the problem is propagated.
        /// </summary>
        /// <param name="onValue">Action to execute if the outcome is successful.</param>
        /// <returns>The current outcome.</returns>
        public Outcome<None> OnValue(Action onValue)
            => self.Then(() =>
            {
                onValue();
                return self;
            });
    }

    /// <summary>OnValue extensions for an outcome containing a tuple of two values.</summary>
    /// <param name="self">Current outcome.</param>
    extension<T, T1>(Outcome<(T, T1)> self)
    {
        /// <inheritdoc cref="OnValueExtensions.extension{T}(Outcome{T}).OnValue(Action{T})"/>
        public Outcome<(T, T1)> OnValue(Action<T, T1> onValue)
            => self.OnValue(value => onValue(value.Item1, value.Item2));
    }

    /// <summary>OnValue extensions for an outcome containing a tuple of three values.</summary>
    /// <param name="self">Current outcome.</param>
    extension<T, T1, T2>(Outcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="OnValueExtensions.extension{T}(Outcome{T}).OnValue(Action{T})"/>
        public Outcome<(T, T1, T2)> OnValue(Action<T, T1, T2> onValue)
            => self.OnValue(value => onValue(value.Item1, value.Item2, value.Item3));
    }

    /// <summary>OnValue extensions for an outcome containing a tuple of four values.</summary>
    /// <param name="self">Current outcome.</param>
    extension<T, T1, T2, T3>(Outcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="OnValueExtensions.extension{T}(Outcome{T}).OnValue(Action{T})"/>
        public Outcome<(T, T1, T2, T3)> OnValue(Action<T, T1, T2, T3> onValue)
            => self.OnValue(value => onValue(value.Item1, value.Item2, value.Item3, value.Item4));
    }
}