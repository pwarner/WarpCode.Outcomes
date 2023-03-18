namespace Outcomes;

public static class OutcomeExtensions
{
    public static Outcome<T> OnSuccess<T>(
        this Outcome<T> outcome,
        Action<T> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        if (outcome.Problem is null)
            action(outcome.Value);

        return outcome;
    }

    public static Outcome<T> OnProblem<T>(
        this Outcome<T> outcome,
        Action<IProblem> action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        if (outcome.Problem is not null)
            action(outcome.Problem);

        return outcome;
    }

    public static Outcome<List<T>?> Aggregate<T>(
        this IEnumerable<Outcome<T>> outcomes,
        bool bailEarly = false)
    {
        List<T>? list = null;
        List<IProblem>? problems = null;

        foreach (Outcome<T> outcome in outcomes)
        {
            _ = outcome
                .OnSuccess(x => (list ??= new List<T>()).Add(x))
                .OnProblem(x => (problems ??= new List<IProblem>()).Add(x));

            if (problems is not null && bailEarly)
                return problems[0].ToOutcome();
        }

        return problems switch
        {
            not null => new ProblemAggregate(problems),
            _ => list
        };
    }

    public static Outcome<None> Aggregate(
        this IEnumerable<Outcome<None>> outcomes,
        bool bailEarly = false)
    {
        List<IProblem>? problems = null;

        foreach (Outcome<None> outcome in outcomes)
        {
            _ = outcome
                .OnProblem(x => (problems ??= new List<IProblem>()).Add(x));

            if (problems is not null && bailEarly)
                return problems[0].ToOutcome();
        }

        return problems switch
        {
            not null => new ProblemAggregate(problems),
            _ => Ok()
        };
    }

    public static Outcome<T> Ensure<T>(
        this Outcome<T> outcome,
        Predicate<T> predicate,
        Func<T, IProblem> factory)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (factory is null) throw new ArgumentNullException(nameof(factory));

        Outcome<T> Switch(T subject) =>
            predicate(subject) switch
            {
                true => subject,
                false => factory(subject).ToOutcome()
            };

        return
            from value in outcome
            from next in Switch(value)
            select next;
    }
}
