namespace WarpCode.Outcomes.Samples;

// 📄 docs/what-is-a-problem.md

public static class WhatIsAProblem
{
    // A plain Problem converts implicitly to an outcome, and so does the value.
    public static Outcome<int> RedeemStamps(Customer customer, int stamps)
    {
        if (stamps > customer.LoyaltyStamps)
            return new Problem($"{customer.Name} only has {customer.LoyaltyStamps} stamps.");

        return customer.LoyaltyStamps - stamps;
    }

    // Misusing the API is exceptional: guard it. A missing customer is expected: return a problem.
    public static Outcome<Customer> FindCustomerGuarded(string customerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerId);

        return CoffeeShop.FindCustomer(customerId);
    }

    public static void Run()
    {
        Customer grace = Database.Customers.Find("grace")!;

        Console.WriteLine($"   Redeem 1 stamp  ➜ {RedeemStamps(grace, 1)}");
        Console.WriteLine($"   Redeem 5 stamps ➜ {RedeemStamps(grace, 5)}");
        Console.WriteLine($"   Unknown drink   ➜ {CoffeeShop.FindDrink("Pumpkin Spice")}");

        try
        {
            FindCustomerGuarded(" ");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"🛡️ Guard still throws: {exception.Message}");
        }
    }
}
