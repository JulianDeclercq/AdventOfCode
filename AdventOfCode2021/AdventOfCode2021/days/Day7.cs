namespace AdventOfCode2021.days;

public class Day7
{
    private static int TotalFuel(IEnumerable<int> crabs, int targetPosition) => 
        crabs.Sum(crab => Math.Abs(targetPosition - crab));

    private static int TotalFuel2(IEnumerable<int> crabs, int targetPosition) =>
        crabs.Select(crab => Math.Abs(targetPosition - crab)).Select(n => (n * (n - 1) / 2) + n).Sum();

    private static int Solve(Func<IEnumerable<int>, int, int> evaluationFunction)
    {
        var crabs = File.ReadLines(@"..\..\..\input\day7.txt").First().Split(',').Select(int.Parse).ToArray();
        return crabs.Select((t, i) => evaluationFunction(crabs, i)).Min();
    }
    
    public static void Part1() => Console.WriteLine($"Day 7 part 1: {Solve(TotalFuel)}");
    public static void Part2() => Console.WriteLine($"Day 7 part 1: {Solve(TotalFuel2)}");
}