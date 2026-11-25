namespace WarpCode.Outcomes.Samples;

// ☕ The coffee-shop domain shared by every sample.

public enum Size { Small, Medium, Large }

public sealed record Customer(string Id, string Name, string Favourite, int LoyaltyStamps, decimal Balance);

public sealed record Drink(string Name, decimal Price);

public sealed record OrderRequest(string CustomerId, string DrinkName, Size Size);

public sealed record Order(Customer Customer, Drink Drink, Size Size, decimal Total);

public sealed record Receipt(string CustomerName, string Description, decimal Charged);

// 🧯 Problems: the expected, non-exceptional ways an order can go wrong.

public abstract record NotFoundProblem(string Detail) : Problem(Detail);

public sealed record NotFoundProblem<T>(string Key)
    : NotFoundProblem($"No {typeof(T).Name.ToLowerInvariant()} found for '{Key}'.");

public sealed record OutOfStockProblem(string DrinkName)
    : Problem($"Sorry, we're all out of {DrinkName}.");

public sealed record ValidationProblem(string Field, string Reason)
    : Problem($"{Field} {Reason}.");

public sealed record PaymentDeclinedProblem(decimal Amount)
    : Problem($"Payment of ${Amount:F2} was declined.");

/// <summary>A tiny in-memory stand-in for a real data store.</summary>
public sealed class Table<T>(Func<T, string> keyOf, params T[] rows) where T : class
{
    private readonly Dictionary<string, T> _rows = rows.ToDictionary(keyOf, StringComparer.OrdinalIgnoreCase);

    public T? Find(string key) => _rows.GetValueOrDefault(key);

    public async Task<T?> FindAsync(string key, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken); // pretend this is a network round-trip
        return Find(key);
    }
}

public static class Database
{
    public static Table<Customer> Customers { get; } = new(
        customer => customer.Id,
        new Customer("ada", "Ada", Favourite: "Flat White", LoyaltyStamps: 9, Balance: 20.00m),
        new Customer("grace", "Grace", Favourite: "Mocha", LoyaltyStamps: 2, Balance: 3.00m),
        new Customer("linus", "Linus", Favourite: "Turmeric Latte", LoyaltyStamps: 0, Balance: 8.00m));

    public static Table<Drink> Menu { get; } = new(
        drink => drink.Name,
        new Drink("Espresso", 2.20m),
        new Drink("Flat White", 3.40m),
        new Drink("Latte", 3.60m),
        new Drink("Mocha", 3.90m),
        new Drink("Filter Coffee", 2.50m));

    public static IReadOnlySet<string> SoldOut { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Mocha" };
}

/// <summary>The coffee shop's operations, all of which return outcomes rather than throwing.</summary>
public static class CoffeeShop
{
    public static Outcome<Customer> FindCustomer(string customerId)
    {
        Customer? customer = Database.Customers.Find(customerId);

        if (customer is null)
            return new NotFoundProblem<Customer>(customerId);

        return customer;
    }

    public static async Task<Outcome<Customer>> FindCustomerAsync(string customerId, CancellationToken cancellationToken)
    {
        Customer? customer = await Database.Customers.FindAsync(customerId, cancellationToken);

        if (customer is null)
            return new NotFoundProblem<Customer>(customerId);

        return customer;
    }

    public static Outcome<Drink> FindDrink(string drinkName) =>
        Database.Menu.Find(drinkName) is { } drink
            ? drink
            : new NotFoundProblem<Drink>(drinkName);

    // The menu is cached, so this usually completes synchronously: ValueTask avoids allocating a Task.
    public static ValueTask<Outcome<Drink>> FindDrinkAsync(string drinkName, CancellationToken cancellationToken) =>
        ValueTask.FromResult(FindDrink(drinkName));

    public static Outcome<None> CheckStock(Drink drink) =>
        Database.SoldOut.Contains(drink.Name)
            ? new OutOfStockProblem(drink.Name)
            : Outcome.Ok;

    public static async ValueTask<Outcome<None>> CheckStockAsync(Drink drink, CancellationToken cancellationToken)
    {
        await Task.Delay(5, cancellationToken); // a quick peek in the stock room
        return CheckStock(drink);
    }

    public static Outcome<None> Validate(OrderRequest request) =>
        string.IsNullOrWhiteSpace(request.CustomerId) ? new ValidationProblem(nameof(request.CustomerId), "is required")
        : string.IsNullOrWhiteSpace(request.DrinkName) ? new ValidationProblem(nameof(request.DrinkName), "is required")
        : Outcome.Ok;

    public static Outcome<(Customer, Drink, Size)> FindCustomerAndDrink(OrderRequest request) =>
        FindCustomer(request.CustomerId)
            .Then(customer => FindDrink(request.DrinkName)
                .Ensure(CheckStock)
                .Then(drink => (customer, drink, request.Size)));

    public static Order PriceOrder(Customer customer, Drink drink, Size size)
    {
        decimal total = customer.LoyaltyStamps >= 9
            ? 0.00m // every tenth coffee is on the house 🎉
            : decimal.Round(drink.Price * SizeMultiplier(size), 2);

        return new Order(customer, drink, size, total);
    }

    public static decimal SizeMultiplier(Size size) => size switch
    {
        Size.Small => 0.8m,
        Size.Medium => 1.0m,
        Size.Large => 1.25m,
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, "Unknown size") // a guard, not a problem
    };

    public static Outcome<Receipt> ChargeCard(Order order) =>
        order.Total > order.Customer.Balance
            ? new PaymentDeclinedProblem(order.Total)
            : new Receipt(order.Customer.Name, $"{order.Size} {order.Drink.Name}", order.Total);

    public static async Task<Outcome<Receipt>> ChargeCardAsync(Order order, CancellationToken cancellationToken)
    {
        await Task.Delay(20, cancellationToken); // talking to the card terminal
        return ChargeCard(order);
    }
}

/// <summary>Somewhere to send side effects.</summary>
public static class AuditLog
{
    public static void Write(string message) => Console.WriteLine($"   📝 {message}");

    public static async Task WriteAsync(Receipt receipt, CancellationToken cancellationToken)
    {
        await Task.Delay(5, cancellationToken);
        Write($"Audited: {receipt.Description} for {receipt.CustomerName}, ${receipt.Charged:F2}");
    }
}
