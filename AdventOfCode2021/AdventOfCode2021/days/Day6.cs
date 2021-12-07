namespace AdventOfCode2021.days;

public class Day6
{
    private readonly Dictionary<int, ulong> _memo = new();
    
    private ulong MakeChildren(int startingValue, int daysLeft)
    {
        // try to fetch from memo
        if (startingValue == 8 && _memo.TryGetValue(daysLeft, out var m))
            return m;
        
        ulong children = 0;

        // it takes startingValue + 1 days to make the first one
        if (daysLeft < startingValue + 1)
            return children;
        
        children++;
        var newDaysLeft = (daysLeft - (startingValue + 1));
        
        // for every 6 days left, a new one is created (that has value 8)
        var childrenMade = newDaysLeft / 7;
        children += (ulong) childrenMade;

        for (var i = 0; i < childrenMade; ++i)
            children += MakeChildren(8, newDaysLeft - i * 7);

        // memo if starting value was 8
        if (startingValue == 8)
            _memo.Add(daysLeft, children);
        
        return children;
    }

    private ulong Solve(int numberOfDays)
    {
        //var input = File.ReadLines(@"..\..\..\input\day6_example.txt").First().Split(',').Select(int.Parse).ToArray();
        var input = File.ReadLines(@"..\..\..\input\day6.txt").First().Split(',').Select(int.Parse).ToArray();
        
        ulong ctr = 0;
        foreach (var number in input)
        {
            var child = MakeChildren(number, numberOfDays);
            ctr += child;
        }
        return ctr + (ulong) input.Length;
    }
  
    public void Part1() => Console.WriteLine($"Day 6 part 1: {Solve(80)}");
    public void Part2() => Console.WriteLine($"Day 6 part 2: {Solve(256)}");
}