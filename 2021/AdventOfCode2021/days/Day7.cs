namespace AdventOfCode2021.days;

public class Day7
{
    private static int TotalFuel(IEnumerable<int> crabs, int targetPosition)
    {
        return crabs.Sum(crab => Math.Abs(targetPosition - crab));
    }

    private static int TotalFuel2(IEnumerable<int> crabs, int targetPosition)
    {
        return crabs.Select(crab => Math.Abs(targetPosition - crab)).Select(n => (n * (n - 1) / 2) + n).Sum();
    }

    private static int Solve(Func<IEnumerable<int>, int, int> totalFuelCalculator)
    {
        var crabs = File.ReadLines(@"..\..\..\input\day7.txt").First().Split(',').Select(int.Parse).ToArray();
        return Enumerable.Range(crabs.Min(), crabs.Max()).Select(x => totalFuelCalculator(crabs, x)).Min();
    }
    
    public static void Part1() => Console.WriteLine($"Day 7 part 1: {Solve(TotalFuel)}");
    public static void Part2() => Console.WriteLine($"Day 7 part 2: {Solve(TotalFuel2)}");
}