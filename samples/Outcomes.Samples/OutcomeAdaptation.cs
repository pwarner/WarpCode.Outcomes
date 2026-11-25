namespace WarpCode.Outcomes.Samples;

// 📄 docs/outcome-adaptation.md

public sealed class CardDeclinedException(decimal amount) : Exception($"Card declined for ${amount:F2}.")
{
    public decimal Amount { get; } = amount;
}

/// <summary>A third-party card-terminal SDK we can't change. It throws instead of returning outcomes.</summary>
public static class LegacyCardTerminal
{
    public static string Charge(string cardNumber, decimal amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);

        if (amount > 5.00m)
            throw new CardDeclinedException(amount);

        return $"AUTH-{cardNumber[^4..]}-{amount:F2}";
    }

    public static async Task<string> ChargeAsync(string cardNumber, decimal amount, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        return Charge(cardNumber, amount);
    }
}

public static class OutcomeAdaptation
{
    // Map exceptions as a parameter: return a Problem to adapt, or null to let it throw.
    public static Outcome<string> Charge(string cardNumber, decimal amount)
    {
        Func<string> charge = () => LegacyCardTerminal.Charge(cardNumber, amount);

        return charge.ToOutcome(exception => exception switch
        {
            CardDeclinedException declined => new PaymentDeclinedProblem(declined.Amount),
            _ => null
        });
    }

    // The shortcut for the common case of mapping a single exception type.
    public static Task<Outcome<string>> ChargeAsync(string cardNumber, decimal amount, CancellationToken cancellationToken) =>
        LegacyCardTerminal.ChargeAsync(cardNumber, amount, cancellationToken)
            .ToOutcome((CardDeclinedException declined) => new PaymentDeclinedProblem(declined.Amount));

    // Map exceptions once, for the whole application, at startup.
    public static void ConfigureExceptionMapping() =>
        Adapt.MapExceptions = exception => exception switch
        {
            CardDeclinedException declined => new PaymentDeclinedProblem(declined.Amount),
            // other exception types that represent expected situations go here
            _ => null // everything else keeps on throwing
        };

    public static async Task RunAsync()
    {
        Console.WriteLine($"   Charge $3.00 ➜ {Charge("4111-1111", 3.00m)}");
        Console.WriteLine($"   Charge $9.00 ➜ {Charge("4111-1111", 9.00m)}");
        Console.WriteLine($"   Async $9.00  ➜ {await ChargeAsync("4111-1111", 9.00m, CancellationToken.None)}");

        try
        {
            Charge(" ", 3.00m);
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"🛡️ Guard still throws: {exception.Message}");
        }

        ConfigureExceptionMapping();
        try
        {
            Func<string> charge = () => LegacyCardTerminal.Charge("4111-1111", 9.00m);
            Console.WriteLine($"   Global map   ➜ {charge.ToOutcome()}");
        }
        finally
        {
            Adapt.MapExceptions = null; // it's static: don't leak it into the other samples
        }
    }
}
