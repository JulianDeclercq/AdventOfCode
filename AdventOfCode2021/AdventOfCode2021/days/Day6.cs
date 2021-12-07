namespace AdventOfCode2021.days;

public class Day6
{
    private readonly Dictionary<int, ulong> _memo = new();
    
    private ulong MakeChildren(int startingValue, int daysLeft)
    {
        // only memo the newly made ones, first generation is fine to not memo
        var shouldMemo = startingValue == 8;

        // try to fetch from memo
        if (shouldMemo && _memo.TryGetValue(daysLeft, out var m))
            return m;

        // it takes startingValue + 1 days to make the first child
        if (daysLeft < startingValue + 1)
            return 0;

        // make the first child
        ulong children = 1;
        
        // for every 7 days left, a new one is created (that has value 8)
        var newDaysLeft = (daysLeft - (startingValue + 1));
        var childrenMade = newDaysLeft / 7;
        children += (ulong) childrenMade;

        // add the future children of the children
        for (var i = 0; i < childrenMade; ++i)
            children += MakeChildren(8, newDaysLeft - i * 7);

        // memo
        if (shouldMemo)
            _memo.Add(daysLeft, children);
        
        // return the total amount of children this fish and its offspring (and their offspring, ...) will make
        return children;
    }

    private ulong Solve(int numberOfDays)
    {
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