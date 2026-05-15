namespace WarpCode.Outcomes;

public static partial class ResultComposition
{
    extension<T, T1, TNext>(Result<(T, T1)>)
    {
        /// <inheritdoc cref="ResultComposition.extension{T,TNext}(Result{T}).operator |(Result{T}, Func{T, Result{TNext}})"/>
        public static Result<TNext> operator |(Result<(T, T1)> self, Func<T, T1, Result<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind(self.Value.Item1, self.Value.Item2);

        /// <inheritdoc cref="ResultComposition.extension{T,TNext}(Result{T}).operator |(Result{T}, Func{T, TNext})"/>
        public static Result<TNext> operator |(Result<(T, T1)> self, Func<T, T1, TNext> map)
            => self | (value => new Result<TNext>(map(value.Item1, value.Item2)));
    }

    extension<T, T1>(Result<(T, T1)>)
    {
        /// <inheritdoc cref="ResultComposition.extension{T}(Result{T}).operator |(Result{T}, Func{T, Result{None}})"/>
        public static Result<(T, T1)> operator |(Result<(T, T1)> self, Func<T, T1, Result<None>> ensure)
            => (self.Problem ?? ensure(self.Value.Item1, self.Value.Item2).Problem) is { } problem
                ? new(problem)
                : self;

        /// <inheritdoc cref="ResultComposition.extension{T}(Result{T}).operator |(Result{T}, Action{T})"/>
        public static Result<(T, T1)> operator |(Result<(T, T1)> self, Action<T, T1> onValue)
            => self | ((T a, T1 b) =>
            {
                onValue(a, b);
                return Result.Ok;
            });
    }

    extension<T, T1, T2, TNext>(Result<(T, T1, T2)>)
    {
        /// <inheritdoc cref="ResultComposition.extension{T,TNext}(Result{T}).operator |(Result{T}, Func{T, Result{TNext}})"/>
        public static Result<TNext> operator |(Result<(T, T1, T2)> self, Func<T, T1, T2, Result<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind(self.Value.Item1, self.Value.Item2, self.Value.Item3);

        /// <inheritdoc cref="ResultComposition.extension{T,TNext}(Result{T}).operator |(Result{T}, Func{T, TNext})"/>
        public static Result<TNext> operator |(Result<(T, T1, T2)> self, Func<T, T1, T2, TNext> map)
            => self | (value => new Result<TNext>(map(value.Item1, value.Item2, value.Item3)));
    }

    extension<T, T1, T2>(Result<(T, T1, T2)>)
    {
        /// <inheritdoc cref="ResultComposition.extension{T}(Result{T}).operator |(Result{T}, Func{T, Result{None}})"/>
        public static Result<(T, T1, T2)> operator |(Result<(T, T1, T2)> self, Func<T, T1, T2, Result<None>> ensure)
            => (self.Problem ?? ensure(self.Value.Item1, self.Value.Item2, self.Value.Item3).Problem) is { } problem
                ? new(problem)
                : self;

        /// <inheritdoc cref="ResultComposition.extension{T}(Result{T}).operator |(Result{T}, Action{T})"/>
        public static Result<(T, T1, T2)> operator |(Result<(T, T1, T2)> self, Action<T, T1, T2> onValue)
            => self | ((T a, T1 b, T2 c) =>
            {
                onValue(a, b, c);
                return Result.Ok;
            });
    }

    extension<T, T1, T2, T3, TNext>(Result<(T, T1, T2, T3)>)
    {
        /// <inheritdoc cref="ResultComposition.extension{T,TNext}(Result{T}).operator |(Result{T}, Func{T, Result{TNext}})"/>
        public static Result<TNext> operator |(Result<(T, T1, T2, T3)> self, Func<T, T1, T2, T3, Result<TNext>> bind)
            => self.Problem is not null
                ? new(self.Problem)
                : bind(self.Value.Item1, self.Value.Item2, self.Value.Item3, self.Value.Item4);

        /// <inheritdoc cref="ResultComposition.extension{T,TNext}(Result{T}).operator |(Result{T}, Func{T, TNext})"/>
        public static Result<TNext> operator |(Result<(T, T1, T2, T3)> self, Func<T, T1, T2, T3, TNext> map)
            => self | (value => new Result<TNext>(map(value.Item1, value.Item2, value.Item3, value.Item4)));
    }

    extension<T, T1, T2, T3>(Result<(T, T1, T2, T3)>)
    {
        /// <inheritdoc cref="ResultComposition.extension{T}(Result{T}).operator |(Result{T}, Func{T, Result{None}})"/>
        public static Result<(T, T1, T2, T3)> operator |(Result<(T, T1, T2, T3)> self, Func<T, T1, T2, T3, Result<None>> ensure)
            => (self.Problem ?? ensure(self.Value.Item1, self.Value.Item2, self.Value.Item3, self.Value.Item4).Problem) is { } problem
                ? new(problem)
                : self;

        /// <inheritdoc cref="ResultComposition.extension{T}(Result{T}).operator |(Result{T}, Action{T})"/>
        public static Result<(T, T1, T2, T3)> operator |(Result<(T, T1, T2, T3)> self, Action<T, T1, T2, T3> onValue)
            => self | ((T a, T1 b, T2 c, T3 d) =>
            {
                onValue(a, b, c, d);
                return Result.Ok;
            });
    }
}