namespace WarpCode.Outcomes.Samples;

// 📄 docs/composing-async-outcomes.md

public static class ComposingAsyncOutcomes
{
    // ----- Going async -----
    public static void GoingAsync(OrderRequest request, CancellationToken cancellationToken)
    {
        // From an outcome you already have...
        Outcome<OrderRequest> validated = Outcome.Of(request).Ensure(CoffeeShop.Validate);
        AsyncOutcome<OrderRequest> fromOutcome = validated.AsAsync(cancellationToken);

        // ...or straight from a value, a problem, or nothing at all.
        AsyncOutcome<OrderRequest> fromValue = AsyncOutcome.Of(request, cancellationToken);
        AsyncOutcome<Drink> fromProblem = AsyncOutcome.OfProblem<Drink>(new OutOfStockProblem("Mocha"), cancellationToken);
        AsyncOutcome<None> fromNothing = AsyncOutcome.Ok(cancellationToken);

        Console.WriteLine($"   {fromOutcome}");
        Console.WriteLine($"   {fromValue}");
        Console.WriteLine($"   {fromProblem}");
        Console.WriteLine($"   {fromNothing}");
    }

    // ----- A fully async pipeline -----
    public static AsyncOutcome<Receipt> PlaceOrderAsync(OrderRequest request, CancellationToken cancellationToken) =>
        Outcome.Of(request)
            .Ensure(CoffeeShop.Validate)                 // plain sync composition...
            .Then(r => r.CustomerId)
            .AsAsync(cancellationToken)                  // ...until we go async: the token is captured once, here
            .ThenTask(CoffeeShop.FindCustomerAsync)      // an existing Task-returning method ➜ ThenTask
            .ThenAsync(async (customer, ct) =>           // an inline async lambda ➜ ThenAsync (ValueTask)
            {
                Outcome<Drink> drink = await CoffeeShop.FindDrinkAsync(request.DrinkName, ct);
                return drink.Then(d => (customer, d));
            })
            .EnsureAsync((_, drink, ct) => CoffeeShop.CheckStockAsync(drink, ct))      // tuples spread, token comes last
            .Then((customer, drink) => CoffeeShop.PriceOrder(customer, drink, request.Size)) // sync delegates still work
            .ThenTask(CoffeeShop.ChargeCardAsync);

    // ----- Async side effects and the sad path -----
    public static AsyncOutcome<Receipt> PlaceOrderOrSubstituteAsync(OrderRequest request, CancellationToken cancellationToken) =>
        PlaceOrderAsync(request, cancellationToken)
            .RescueAsync((problem, ct) => problem is OutOfStockProblem
                ? PlaceOrderAsync(request with { DrinkName = "Filter Coffee" }, ct) // an AsyncOutcome is a ValueTask<Outcome<T>>
                : AsyncOutcome.OfProblem<Receipt>(problem, ct))
            .OnValueTask(AuditLog.WriteAsync)            // Task-returning side effect
            .OnProblemAsync(async (problem, ct) =>       // ValueTask-returning side effect
            {
                await Task.Delay(5, ct);
                AuditLog.Write($"Order failed: {problem.Detail}");
            });

    // ----- Resolving and returning -----
    public static async Task<string> DescribeOrderAsync(OrderRequest request, CancellationToken cancellationToken)
    {
        // await an AsyncOutcome to get the Outcome inside...
        Outcome<Receipt> outcome = await PlaceOrderAsync(request, cancellationToken);
        Console.WriteLine($"   awaited ➜ {outcome}");

        // ...or resolve it directly: Match returns a ValueTask<TFinal>
        return await PlaceOrderAsync(request, cancellationToken).Match(
            receipt => $"✅ {receipt.Description} for {receipt.CustomerName}: ${receipt.Charged:F2}",
            problem => $"❌ {problem.Detail}");
    }

    // An AsyncOutcome<T> converts implicitly to both ValueTask<Outcome<T>> and Task<Outcome<T>>.
    public static Task<Outcome<Receipt>> HandleAsync(OrderRequest request, CancellationToken cancellationToken) =>
        PlaceOrderAsync(request, cancellationToken);

    public static async Task RunAsync()
    {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        CancellationToken token = cancellation.Token;

        Console.WriteLine("   Going async:");
        GoingAsync(new OrderRequest("ada", "Latte", Size.Medium), token);

        OrderRequest[] requests =
        [
            new("ada", "Flat White", Size.Large),
            new("grace", "Latte", Size.Large),
            new("grace", "Mocha", Size.Small),
            new("linus", "Hot Chocolate", Size.Medium),
        ];

        Console.WriteLine("   PlaceOrderAsync:");
        foreach (OrderRequest request in requests)
            Console.WriteLine($"     {request.CustomerId,-6} {request.DrinkName,-14} ➜ {await PlaceOrderAsync(request, token)}");

        Console.WriteLine("   PlaceOrderOrSubstituteAsync:");
        foreach (OrderRequest request in requests)
            Console.WriteLine($"     {request.CustomerId,-6} {request.DrinkName,-14} ➜ {await PlaceOrderOrSubstituteAsync(request, token)}");

        Console.WriteLine("   Resolving:");
        Console.WriteLine($"   {await DescribeOrderAsync(requests[0], token)}");
        Console.WriteLine($"   {await DescribeOrderAsync(requests[1], token)}");
        Console.WriteLine($"   as Task ➜ {await HandleAsync(requests[0], token)}");

        Console.WriteLine("   Cancellation:");
        using var cancelled = new CancellationTokenSource();
        await cancelled.CancelAsync();
        try
        {
            await PlaceOrderAsync(requests[0], cancelled.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("   🛑 Cancelled: cancellation is exceptional, so it throws rather than becoming a problem");
        }
    }
}
