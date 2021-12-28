namespace AdventOfCode2018.days;

public class Day2
{
    public void Part1()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day2.txt");
        List<string> exactlyTwo = new(), exactlyThree = new();
        foreach (var line in input)
        {
            var map = new Dictionary<char, int>();
            foreach (var c in line)
            {
                map.TryGetValue(c, out var count);
                map[c] = count + 1;
            }
            
            if (map.Values.Any(x => x == 2))
                exactlyTwo.Add(line);
            
            if (map.Values.Any(x => x == 3))
                exactlyThree.Add(line);
        }
        
        Console.WriteLine($"Day 2 part 1: {exactlyTwo.Count * exactlyThree.Count}");
    }
}