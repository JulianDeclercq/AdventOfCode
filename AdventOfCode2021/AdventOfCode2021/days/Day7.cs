namespace AdventOfCode2021.days;

public class Day7
{
    private static int TotalFuel(IEnumerable<int> crabs, int targetPosition) 
        => crabs.Sum(crab => Math.Abs(targetPosition - crab));

    private static int TotalFuel2(IEnumerable<int> crabs, int targetPosition)
    {
        var answer = 0;
        foreach (var crab in crabs)
        {
            // calculate the distance to the target position
            var n = Math.Abs(targetPosition - crab);
            
            // calculate the fuel and add it to the answer
            answer += (n * (n - 1) / 2) + n;
        }
        return answer;
    }

    private static int Solve(Func<IEnumerable<int>, int, int> evaluationFunction)
    {
        //var crabs = File.ReadLines(@"..\..\..\input\day7_example.txt").First().Split(',').Select(int.Parse).ToArray();
        var crabs = File.ReadLines(@"..\..\..\input\day7.txt").First().Split(',').Select(int.Parse).ToArray();
        var answer = int.MaxValue;

        for (var i = 0; i < crabs.Length; ++i)
        {
            var fuel = evaluationFunction(crabs, i);
            if (fuel < answer)
                answer = fuel;
        }
        return answer;
    }
    
    public static void Part1() => Console.WriteLine($"Day 7 part 1: {Solve(TotalFuel)}");
    public static void Part2() => Console.WriteLine($"Day 7 part 1: {Solve(TotalFuel2)}");
}