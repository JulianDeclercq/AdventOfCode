namespace AdventOfCode2021.days;

public class Day6
{
    private static int _ctr = 0;
    private Dictionary<int, int> _memo = new();
    
    private int ChildrenMadeNew(int startingValue, int daysLeft)
    {
        var children = 0;

        // it takes startingValue + 1 days to make the first one
        if (daysLeft < startingValue + 1)
            return children;
        
        children++;
        var newDaysLeft = daysLeft - (startingValue + 1);
        
        // for every 6 days left, a new one is created (that has value 8)
        var childrenMade = newDaysLeft / 7;
        children += childrenMade;

        for (var i = 0; i < childrenMade; ++i)
            children += ChildrenMadeNew(8, newDaysLeft - i * 7);

        return children;
    }
  
    public void Part1()
    {
        //var input = File.ReadLines(@"..\..\..\input\day6_example.txt").First().Split(',').Select(int.Parse).ToArray();
        var input = File.ReadLines(@"..\..\..\input\day6.txt").First().Split(',').Select(int.Parse).ToArray();
        var days = 80;
        
        foreach (var number in input)
        {
            var child = ChildrenMadeNew(number, days);
            _ctr += child;
        }
        Console.WriteLine($"Day 6 part 1: {_ctr + input.Length}");
    }
}