namespace WarpCode.Outcomes;

public static partial class OutcomeComposition
{
    extension<T, T1, TNext>(Outcome<(T, T1)>)
    {
        /// <inheritdoc cref="OutcomeComposition.extension{T,TNext}(Outcome{T}).operator |(Outcome{T}, Func{T, Outcome{TNext}})"/>
        public static Outcome<TNext> operator |(Outcome<(T, T1)> self, Func<T, T1, Outcome<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind(self.Value.Item1, self.Value.Item2);

        /// <inheritdoc cref="OutcomeComposition.extension{T,TNext}(Outcome{T}).operator |(Outcome{T}, Func{T, TNext})"/>
        public static Outcome<TNext> operator |(Outcome<(T, T1)> self, Func<T, T1, TNext> map)
            => self | (value => new Outcome<TNext>(map(value.Item1, value.Item2)));
    }

    extension<T, T1>(Outcome<(T, T1)>)
    {
        /// <inheritdoc cref="OutcomeComposition.extension{T}(Outcome{T}).operator |(Outcome{T}, Func{T, Outcome{None}})"/>
        public static Outcome<(T, T1)> operator |(Outcome<(T, T1)> self, Func<T, T1, Outcome<None>> ensure)
            => (self.Problem ?? ensure(self.Value.Item1, self.Value.Item2).Problem) is { } problem
                ? new(problem)
                : self;

        /// <inheritdoc cref="OutcomeComposition.extension{T}(Outcome{T}).operator |(Outcome{T}, Action{T})"/>
        public static Outcome<(T, T1)> operator |(Outcome<(T, T1)> self, Action<T, T1> onValue)
            => self | ((T a, T1 b) =>
            {
                onValue(a, b);
                return Outcome.Ok;
            });
    }

    extension<T, T1, T2, TNext>(Outcome<(T, T1, T2)>)
    {
        /// <inheritdoc cref="OutcomeComposition.extension{T,TNext}(Outcome{T}).operator |(Outcome{T}, Func{T, Outcome{TNext}})"/>
        public static Outcome<TNext> operator |(Outcome<(T, T1, T2)> self, Func<T, T1, T2, Outcome<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind(self.Value.Item1, self.Value.Item2, self.Value.Item3);

        /// <inheritdoc cref="OutcomeComposition.extension{T,TNext}(Outcome{T}).operator |(Outcome{T}, Func{T, TNext})"/>
        public static Outcome<TNext> operator |(Outcome<(T, T1, T2)> self, Func<T, T1, T2, TNext> map)
            => self | (value => new Outcome<TNext>(map(value.Item1, value.Item2, value.Item3)));
    }

    extension<T, T1, T2>(Outcome<(T, T1, T2)>)
    {
        /// <inheritdoc cref="OutcomeComposition.extension{T}(Outcome{T}).operator |(Outcome{T}, Func{T, Outcome{None}})"/>
        public static Outcome<(T, T1, T2)> operator |(Outcome<(T, T1, T2)> self, Func<T, T1, T2, Outcome<None>> ensure)
            => (self.Problem ?? ensure(self.Value.Item1, self.Value.Item2, self.Value.Item3).Problem) is { } problem
                ? new(problem)
                : self;

        /// <inheritdoc cref="OutcomeComposition.extension{T}(Outcome{T}).operator |(Outcome{T}, Action{T})"/>
        public static Outcome<(T, T1, T2)> operator |(Outcome<(T, T1, T2)> self, Action<T, T1, T2> onValue)
            => self | ((T a, T1 b, T2 c) =>
            {
                onValue(a, b, c);
                return Outcome.Ok;
            });
    }

    extension<T, T1, T2, T3, TNext>(Outcome<(T, T1, T2, T3)>)
    {
        /// <inheritdoc cref="OutcomeComposition.extension{T,TNext}(Outcome{T}).operator |(Outcome{T}, Func{T, Outcome{TNext}})"/>
        public static Outcome<TNext> operator |(Outcome<(T, T1, T2, T3)> self, Func<T, T1, T2, T3, Outcome<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind(self.Value.Item1, self.Value.Item2, self.Value.Item3, self.Value.Item4);

        /// <inheritdoc cref="OutcomeComposition.extension{T,TNext}(Outcome{T}).operator |(Outcome{T}, Func{T, TNext})"/>
        public static Outcome<TNext> operator |(Outcome<(T, T1, T2, T3)> self, Func<T, T1, T2, T3, TNext> map)
            => self | (value => new Outcome<TNext>(map(value.Item1, value.Item2, value.Item3, value.Item4)));
    }

    extension<T, T1, T2, T3>(Outcome<(T, T1, T2, T3)>)
    {
        /// <inheritdoc cref="OutcomeComposition.extension{T}(Outcome{T}).operator |(Outcome{T}, Func{T, Outcome{None}})"/>
        public static Outcome<(T, T1, T2, T3)> operator |(Outcome<(T, T1, T2, T3)> self, Func<T, T1, T2, T3, Outcome<None>> ensure)
            => (self.Problem ?? ensure(self.Value.Item1, self.Value.Item2, self.Value.Item3, self.Value.Item4).Problem) is { } problem
                ? new(problem)
                : self;

        /// <inheritdoc cref="OutcomeComposition.extension{T}(Outcome{T}).operator |(Outcome{T}, Action{T})"/>
        public static Outcome<(T, T1, T2, T3)> operator |(Outcome<(T, T1, T2, T3)> self, Action<T, T1, T2, T3> onValue)
            => self | ((T a, T1 b, T2 c, T3 d) =>
            {
                onValue(a, b, c, d);
                return Outcome.Ok;
            });
    }
}