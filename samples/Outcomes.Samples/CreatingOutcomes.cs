namespace WarpCode.Outcomes.Samples;

// 📄 docs/creating-outcomes.md

public static class CreatingOutcomes
{
    public static void Run()
    {
        // ✅ Success outcomes
        var byConstructor = new Outcome<string>("Flat White");
        Outcome<string> byConversion = "Flat White";
        Outcome<string> byFactory = Outcome.Of("Flat White");

        // ✅ Value-less success outcomes
        Outcome<None> ok = Outcome.Ok;
        Outcome<None> alsoOk = default;

        // ❌ Problem outcomes
        var problemByConstructor = new Outcome<Drink>(new OutOfStockProblem("Mocha"));
        Outcome<Drink> problemByConversion = new OutOfStockProblem("Mocha");
        Outcome<Drink> problemByFactory = Outcome.OfProblem<Drink>(new OutOfStockProblem("Mocha"));

        // ❌ Value-less problem outcomes
        Outcome<None> valuelessProblem = Outcome.OfProblem(new OutOfStockProblem("Mocha"));

        // ❌ Quick problems from a detail message, using the base Problem type
        Outcome<Drink> fromDetail = Outcome.OfProblem<Drink>("The grinder is jammed");
        Outcome<None> valuelessFromDetail = Outcome.OfProblem("The grinder is jammed");

        Console.WriteLine($"   {byConstructor} | {byConversion} | {byFactory}");
        Console.WriteLine($"   {ok} | {alsoOk}");
        Console.WriteLine($"   {problemByConstructor}");
        Console.WriteLine($"   {problemByConversion == problemByFactory} (outcomes have value equality)");
        Console.WriteLine($"   {valuelessProblem}");
        Console.WriteLine($"   {fromDetail} | {valuelessFromDetail}");
    }
}
