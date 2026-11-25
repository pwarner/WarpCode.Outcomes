namespace WarpCode.Outcomes.Samples;

// 📄 docs/outcomes-as-monads.md

public static class OutcomesAsMonads
{
    // Map: the function transforms the inner value.
    public static Outcome<decimal> LargePrice(Outcome<Drink> drink) =>
        drink.Then(d => decimal.Round(d.Price * 1.25m, 2));

    // Bind: the function returns another outcome, which Then flattens for you.
    public static Outcome<Drink> Favourite(Outcome<Customer> customer) =>
        customer.Then(c => CoffeeShop.FindDrink(c.Favourite));

    // IEnumerable<T> is a monad too: Select is its map, SelectMany its bind.
    public static (IEnumerable<decimal> Prices, IEnumerable<string> Words) MenuMonads(IEnumerable<Drink> menu) =>
        (menu.Select(drink => drink.Price), menu.SelectMany(drink => drink.Name.Split(' ')));

    public static void Run()
    {
        Console.WriteLine($"   Map  ➜ {LargePrice(CoffeeShop.FindDrink("Latte"))}");
        Console.WriteLine($"   Map  ➜ {LargePrice(CoffeeShop.FindDrink("Babyccino"))}");
        Console.WriteLine($"   Bind ➜ {Favourite(CoffeeShop.FindCustomer("ada"))}");
        Console.WriteLine($"   Bind ➜ {Favourite(CoffeeShop.FindCustomer("linus"))}");

        Drink[] menu = [new("Flat White", 3.40m), new("Filter Coffee", 2.50m)];
        var (prices, words) = MenuMonads(menu);
        Console.WriteLine($"   Select     ➜ {string.Join(", ", prices)}");
        Console.WriteLine($"   SelectMany ➜ {string.Join(", ", words)}");
    }
}
