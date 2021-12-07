namespace AdventOfCode2021.days;

public class Day7
{
    // position, fuel
    private static int Median(IReadOnlyList<int> sorted)
    {
        var mid = sorted.Count / 2;
        
        if (sorted.Count % 2 != 0)
            return sorted[mid];
        
        return (sorted[mid] + sorted[mid + 1]) / 2;
    }

    private static int Fuel(IEnumerable<int> crabs, int targetPosition) 
        => crabs.Sum(crab => Math.Abs(targetPosition - crab));
    
    public void Part1()
    {
        //var crabs = File.ReadLines(@"..\..\..\input\day7_example.txt").First().Split(',').Select(int.Parse).ToArray();
        var crabs = File.ReadLines(@"..\..\..\input\day7.txt").First().Split(',').Select(int.Parse).ToArray();
        Array.Sort(crabs);
        var answer = int.MaxValue;

        var startingPoint = Median(crabs);
        for (var i = 0; i < crabs.Length / 2; ++i)
        {
            var pos = startingPoint - i;
            var moveLeft = Fuel(crabs, pos);
            if (moveLeft < answer)
                answer = moveLeft;

            pos = startingPoint + i;
            var moveRight = Fuel(crabs, pos);
            if (moveRight < answer)
                answer = moveRight;
        }
        Console.WriteLine($"Day 7 part 1: {answer}");
    }
}