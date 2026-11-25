namespace WarpCode.Outcomes.Samples;

// 📄 docs/resolving-outcomes.md

/// <summary>A dependency-free stand-in for an ASP.NET Core IResult.</summary>
public sealed record HttpResult(int StatusCode, string Body)
{
    public static HttpResult Ok<T>(T value) => new(200, $"{value}");
    public static HttpResult BadRequest(string detail) => new(400, detail);
    public static HttpResult PaymentRequired(string detail) => new(402, detail);
    public static HttpResult NotFound(string detail) => new(404, detail);
    public static HttpResult Conflict(string detail) => new(409, detail);
    public static HttpResult Error(string detail) => new(500, detail);
}

public static class ResolvingOutcomes
{
    // Leave the rails with Match: one function for the value, one for the problem.
    public static HttpResult GetTheUsual(string customerId) =>
        ComposingOutcomes.TheUsualInStock(customerId).Match(
            drink => HttpResult.Ok(drink.Name),
            problem => problem switch
            {
                NotFoundProblem notFound => HttpResult.NotFound(notFound.Detail),
                OutOfStockProblem outOfStock => HttpResult.Conflict(outOfStock.Detail),
                _ => HttpResult.Error(problem.Detail)
            });

    // AsyncOutcome<T>.Match returns a ValueTask<TFinal>, so there's no need to await first.
    public static ValueTask<HttpResult> PostOrderAsync(OrderRequest request, CancellationToken cancellationToken) =>
        ComposingAsyncOutcomes.PlaceOrderAsync(request, cancellationToken)
            .Match(HttpResult.Ok, OutcomeHttpExtensions.ToHttpResult);

    // With a shared resolver, every endpoint resolves the same way.
    public static HttpResult GetTheUsualResolved(string customerId) =>
        ComposingOutcomes.TheUsualInStock(customerId).ToHttpResult();

    public static async Task RunAsync()
    {
        foreach (string id in new[] { "ada", "grace", "nobody" })
            Console.WriteLine($"   GET  /usual/{id,-7} ➜ {GetTheUsual(id)}");

        foreach (string id in new[] { "ada", "linus" })
            Console.WriteLine($"   GET  /usual/{id,-7} ➜ {GetTheUsualResolved(id)} (shared resolver)");

        foreach (OrderRequest request in new OrderRequest[] { new("ada", "Latte", Size.Small), new("grace", "Latte", Size.Large), new("", "Latte", Size.Large) })
            Console.WriteLine($"   POST /orders {request.CustomerId,-6} ➜ {await PostOrderAsync(request, CancellationToken.None)}");
    }
}

public static class OutcomeHttpExtensions
{
    public static HttpResult ToHttpResult<T>(this Outcome<T> outcome) =>
        outcome.Match(HttpResult.Ok, ToHttpResult);

    public static ValueTask<HttpResult> ToHttpResultAsync<T>(this AsyncOutcome<T> outcome) =>
        outcome.Match(HttpResult.Ok, ToHttpResult);

    public static HttpResult ToHttpResult(this Problem problem) => problem switch
    {
        ValidationProblem invalid => HttpResult.BadRequest(invalid.Detail),
        PaymentDeclinedProblem declined => HttpResult.PaymentRequired(declined.Detail),
        NotFoundProblem notFound => HttpResult.NotFound(notFound.Detail),
        OutOfStockProblem outOfStock => HttpResult.Conflict(outOfStock.Detail),
        _ => HttpResult.Error(problem.Detail)
    };
}
