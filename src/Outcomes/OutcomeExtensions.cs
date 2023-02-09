namespace Outcomes;

public static partial class Outcome
{
    public static Outcome<T> OnSuccess<T>(
        this Outcome<T> outcome,
        Action<T> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        return outcome.Match(
            value =>
            {
                action(value);
                return value;
            },
            problem => problem.ToOutcome<T>()
        );
    }

    public static Outcome<T> OnProblem<T>(
        this Outcome<T> outcome,
        Action<IProblem> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        return outcome.Match(
            value => value,
            problem =>
            {
                action(problem);
                return problem.ToOutcome<T>();
            }
        );
    }

    public static Outcome<List<T>?> Aggregate<T>(
        this IEnumerable<Outcome<T>> outcomes,
        bool bailEarly)
    {
        List<T>? list = null;
        List<IProblem>? problems = null;

        foreach (Outcome<T> outcome in outcomes)
        {
            _ = outcome
                .OnSuccess((list ??= new List<T>()).Add)
                .OnProblem((problems ??= new List<IProblem>()).Add);

            if (problems is not null && bailEarly)
                break;
        }

        return problems is not null
            ? new ProblemAggregate(problems)
            : list;
    }

    public static Outcome<None> Aggregate(
        this IEnumerable<Outcome<None>> outcomes,
        bool bailEarly)
    {
        List<IProblem>? problems = null;

        foreach (Outcome<None> outcome in outcomes)
        {
            _ = outcome
                .OnProblem((problems ??= new List<IProblem>()).Add);

            if (problems is not null && bailEarly)
                break;
        }

        return problems is not null
            ? new ProblemAggregate(problems)
            : Ok();
    }

    public static Outcome<T> Ensure<T>(
        T subject,
        Predicate<T> predicate,
        Func<T, IProblem> factory)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (factory is null) throw new ArgumentNullException(nameof(factory));

        return predicate(subject) switch
        {
            true => subject,
            false => factory(subject).ToOutcome()
        };
    }

    public static Outcome<T> Ensure<T>(
        this Outcome<T> subject,
        Predicate<T> predicate,
        Func<T, IProblem> factory)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (factory is null) throw new ArgumentNullException(nameof(factory));

        return
            from value in subject
            from next in Ensure(value, predicate, factory)
            select next;
    }
}
