namespace WarpCode.Outcomes.Samples;

// 📄 docs/outcome-aggregation.md

public static class OutcomeAggregation
{
    // A round of drinks for the table: every drink must be available, or the round fails.
    public static Outcome<Drink[]> OrderRound(IEnumerable<string> drinkNames, bool bailEarly = false) =>
        drinkNames
            .Select(name => CoffeeShop.FindDrink(name).Ensure(CoffeeShop.CheckStock))
            .Aggregate(bailEarly);

    // Value-less outcomes aggregate too: all the opening checks must pass.
    public static Outcome<None> OpeningChecks() =>
        new[]
        {
            Outcome.Ok,                                    // machine is hot
            Outcome.OfProblem("The milk fridge is warm"),
            Outcome.OfProblem("We're low on oat milk")
        }.Aggregate();

    // Orders arriving as an async stream.
    public static async IAsyncEnumerable<Outcome<Drink>> IncomingOrders(IEnumerable<string> drinkNames)
    {
        foreach (string name in drinkNames)
        {
            await Task.Delay(5);
            yield return CoffeeShop.FindDrink(name);
        }
    }

    public static Task<Outcome<Drink[]>> OrderRoundAsync(IEnumerable<string> drinkNames) =>
        IncomingOrders(drinkNames).AggregateAsync();

    public static async Task RunAsync()
    {
        string[] goodRound = ["Latte", "Espresso", "Flat White"];
        string[] badRound = ["Latte", "Mocha", "Babyccino"];

        Console.WriteLine($"   Good round        ➜ {Describe(OrderRound(goodRound))}");
        Console.WriteLine($"   Bad round         ➜ {Describe(OrderRound(badRound))}");
        Console.WriteLine($"   Bad round, bail   ➜ {Describe(OrderRound(badRound, bailEarly: true))}");
        Console.WriteLine($"   Opening checks    ➜ {OpeningChecks().Match(_ => "all good", Describe)}");
        Console.WriteLine($"   Async round       ➜ {Describe(await OrderRoundAsync(goodRound))}");
    }

    private static string Describe(Outcome<Drink[]> round) =>
        round.Match(drinks => string.Join(", ", drinks.Select(drink => drink.Name)), Describe);

    private static string Describe(Problem problem) => problem switch
    {
        ProblemAggregate aggregate => string.Join(" | ", aggregate.Problems.Select(p => p.Detail)),
        _ => problem.Detail
    };
}
