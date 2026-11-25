namespace WarpCode.Outcomes.Samples;

// 📄 docs/why-outcomes.md

public sealed class CustomerNotFoundException(string customerId)
    : Exception($"Customer '{customerId}' was not found.")
{
    public string CustomerId { get; } = customerId;
}

public static class WhyOutcomes
{
    // 👎 Using an exception to signal a perfectly ordinary situation.
    public static Customer FindCustomerOrThrow(string customerId)
    {
        Customer? customer = Database.Customers.Find(customerId);

        if (customer is null)
            throw new CustomerNotFoundException(customerId);

        return customer;
    }

    // 👍 A workflow composed from small steps, each of which may return a problem.
    public static Outcome<Receipt> PlaceOrder(OrderRequest request) =>
        Outcome.Of(request)
            .Ensure(CoffeeShop.Validate)
            .Then(CoffeeShop.FindCustomerAndDrink)
            .Then(CoffeeShop.PriceOrder)
            .Then(CoffeeShop.ChargeCard);

    public static void Run()
    {
        try
        {
            FindCustomerOrThrow("nobody");
        }
        catch (CustomerNotFoundException exception)
        {
            Console.WriteLine($"💥 Thrown and caught: {exception.Message}");
        }

        Console.WriteLine($"🧯 Returned instead: {CoffeeShop.FindCustomer("nobody")}");

        foreach (OrderRequest request in new OrderRequest[]
                 {
                     new("ada", "Flat White", Size.Large),
                     new("grace", "Latte", Size.Large),
                     new("grace", "Mocha", Size.Small),
                     new("", "Latte", Size.Small)
                 })
        {
            Console.WriteLine($"   {request} ➜ {PlaceOrder(request)}");
        }
    }
}
