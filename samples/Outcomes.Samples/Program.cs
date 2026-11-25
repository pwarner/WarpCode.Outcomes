using WarpCode.Outcomes.Samples;

// ☕ Runnable samples for the WarpCode.Outcomes docs.
//    dotnet run --project samples/Outcomes.Samples                      runs every sample
//    dotnet run --project samples/Outcomes.Samples -- composing-outcomes runs one (named after its doc)

(string Name, Func<Task> Run)[] samples =
[
    ("why-outcomes", Sync(WhyOutcomes.Run)),
    ("what-is-a-problem", Sync(WhatIsAProblem.Run)),
    ("creating-outcomes", Sync(CreatingOutcomes.Run)),
    ("composing-outcomes", Sync(ComposingOutcomes.Run)),
    ("composing-async-outcomes", ComposingAsyncOutcomes.RunAsync),
    ("resolving-outcomes", ResolvingOutcomes.RunAsync),
    ("outcome-aggregation", OutcomeAggregation.RunAsync),
    ("outcome-adaptation", OutcomeAdaptation.RunAsync),
    ("outcomes-as-monads", Sync(OutcomesAsMonads.Run)),
];

var selected = args.Length == 0
    ? samples
    : samples.Where(sample => args.Contains(sample.Name, StringComparer.OrdinalIgnoreCase)).ToArray();

if (selected.Length == 0)
{
    Console.WriteLine($"Unknown sample. Choose from: {string.Join(", ", samples.Select(sample => sample.Name))}");
    return 1;
}

foreach (var (name, run) in selected)
{
    Console.WriteLine($"☕ {name}");
    await run();
    Console.WriteLine();
}

return 0;

static Func<Task> Sync(Action run) => () =>
{
    run();
    return Task.CompletedTask;
};
