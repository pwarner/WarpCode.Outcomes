namespace WarpCode.Outcomes.Samples;

// 📄 docs/composing-outcomes.md
// A regular walks in and asks for "the usual". Each section adds one operator to the pipeline.

public static class ComposingOutcomes
{
    // ----- Then -----
    public static Outcome<string> TheUsual(string customerId) =>
        CoffeeShop.FindCustomer(customerId)                              // Outcome<Customer>
            .Then(customer => CoffeeShop.FindDrink(customer.Favourite))   // bind: Customer ➜ Outcome<Drink>
            .Then(drink => $"One {drink.Name}, coming up!");              // map:  Drink ➜ string

    // ----- Ensure -----
    // A validator written for Then has to hand back the value it was given...
    public static Outcome<OrderRequest> ValidateForThen(OrderRequest request) =>
        string.IsNullOrWhiteSpace(request.DrinkName)
            ? new ValidationProblem(nameof(request.DrinkName), "is required")
            : request;

    // ...whereas Ensure only needs a yes (Outcome.Ok) or a no (a problem).
    public static Outcome<Drink> TheUsualInStock(string customerId) =>
        CoffeeShop.FindCustomer(customerId)
            .Then(customer => CoffeeShop.FindDrink(customer.Favourite))
            .Ensure(CoffeeShop.CheckStock);

    // ----- OnValue -----
    public static Outcome<Drink> TheUsualWithBarista(string customerId) =>
        CoffeeShop.FindCustomer(customerId)
            .Then(customer => CoffeeShop.FindDrink(customer.Favourite))
            .Ensure(CoffeeShop.CheckStock)
            .OnValue(drink => AuditLog.Write($"Grinding beans for a {drink.Name}"));

    // ----- Value-less overloads -----
    public static Outcome<string> OpenShop() =>
        HeatMachine()                                       // Outcome<None>
            .Ensure(CheckMilk)                              // Func<Outcome<None>>: no discard needed
            .OnValue(() => AuditLog.Write("Machine hot, milk cold"))
            .Then("Open for business!");                    // a constant value

    private static Outcome<None> HeatMachine() => Outcome.Ok;
    private static Outcome<None> CheckMilk() => Outcome.Ok;

    // ----- Rescue -----
    public static Outcome<Drink> TheUsualOrFilter(string customerId) =>
        CoffeeShop.FindCustomer(customerId)
            .Then(customer => CoffeeShop.FindDrink(customer.Favourite))
            .Ensure(CoffeeShop.CheckStock)
            .Rescue(OfferFilterCoffee)
            .OnValue(drink => AuditLog.Write($"Grinding beans for a {drink.Name}"));

    private static Outcome<Drink> OfferFilterCoffee(Problem problem) => problem switch
    {
        OutOfStockProblem => CoffeeShop.FindDrink("Filter Coffee"), // back on the happy path
        _ => problem                                                // anything else stays a problem
    };

    // ----- OnProblem -----
    public static Outcome<Drink> TheUsualFullService(string customerId) =>
        CoffeeShop.FindCustomer(customerId)
            .Then(customer => CoffeeShop.FindDrink(customer.Favourite))
            .Ensure(CoffeeShop.CheckStock)
            .OnProblem(problem => AuditLog.Write($"Couldn't make the usual: {problem.Detail}"))
            .Rescue(OfferFilterCoffee)
            .OnValue(drink => AuditLog.Write($"Grinding beans for a {drink.Name}"));

    // ----- Parameter spreading with tuples -----
    // FindCustomerAndDrink returns Outcome<(Customer, Drink, Size)>, which Then spreads into PriceOrder's three parameters.
    public static Outcome<Order> TakeOrder(OrderRequest request) =>
        CoffeeShop.FindCustomerAndDrink(request)
            .Then(CoffeeShop.PriceOrder);

    // Without spreading, every step would have to accept (and unpack) the tuple itself.
    public static Order PriceOrderWithoutSpreading((Customer, Drink, Size) order)
    {
        var (customer, drink, size) = order;
        return CoffeeShop.PriceOrder(customer, drink, size);
    }

    public static void Run()
    {
        string[] customers = ["ada", "grace", "linus", "nobody"];

        Console.WriteLine("   Then:");
        foreach (string id in customers)
            Console.WriteLine($"     {id,-7} ➜ {TheUsual(id)}");

        Console.WriteLine("   Ensure:");
        Console.WriteLine($"     {ValidateForThen(new OrderRequest("ada", "", Size.Small))}");
        foreach (string id in customers)
            Console.WriteLine($"     {id,-7} ➜ {TheUsualInStock(id)}");

        Console.WriteLine("   OnValue:");
        Console.WriteLine($"     ada     ➜ {TheUsualWithBarista("ada")}");

        Console.WriteLine("   Value-less overloads:");
        Console.WriteLine($"     {OpenShop()}");

        Console.WriteLine("   Rescue:");
        foreach (string id in customers)
            Console.WriteLine($"     {id,-7} ➜ {TheUsualOrFilter(id)}");

        Console.WriteLine("   OnProblem:");
        foreach (string id in customers)
            Console.WriteLine($"     {id,-7} ➜ {TheUsualFullService(id)}");

        Console.WriteLine("   Tuple spreading:");
        Console.WriteLine($"     {TakeOrder(new OrderRequest("grace", "Espresso", Size.Small))}");
        Console.WriteLine($"     {PriceOrderWithoutSpreading((Database.Customers.Find("grace")!, new Drink("Espresso", 2.20m), Size.Small))}");
    }
}
