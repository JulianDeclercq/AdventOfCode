using System.Text;

namespace AdventOfCode2021.days;

public class Day8
{
    private class Entry
    {
        public Entry(List<string> uniqueSignalPatterns, List<string> outputValues)
        {
            UniqueSignalPatterns = uniqueSignalPatterns;
            OutputValues = outputValues;
        }
        public readonly List<string> UniqueSignalPatterns;
        public readonly List<string> OutputValues;
    }
    
    // {number, amount of segments}
    private readonly Dictionary<int, int> _uniqueLengths = new() { {1, 2}, {4, 4}, {7, 3}, {8, 7} };
    
    public void Part1()
    {
        var entries = ParseInput();
        var answer = entries.SelectMany(e => e.OutputValues).Count(fdov => _uniqueLengths.ContainsValue(fdov.Length));
        Console.WriteLine($"Day 8 part 1: {answer}");
    }
    
    public void Part2()
    {
        var entries = ParseInput();
        Console.WriteLine($"Day 8 part 2: {entries.Select(FourDigitOutputValue).Sum()}");
    }

    private static IEnumerable<Entry> ParseInput()
    {
        return File.ReadLines(@"..\..\..\input\day8.txt").
            Select(a => a.Split('|')
           .Select(b => b.Trim())
           .Select(c => c.Split(' ').ToList()).ToList())
           .Select(d => new Entry(d[0], d[1])).ToList();
    }

    private int FourDigitOutputValue(Entry e)
    {
        // automatically map the values that have unique lengths (1, 4, 7, 8) to their pattern
        var map = _uniqueLengths.ToDictionary(x => x.Key,
            x => e.UniqueSignalPatterns.Single(usp => usp.Length == _uniqueLengths[x.Key]).Ordered());

        var unmapped = e.UniqueSignalPatterns.Where(x => !map.ContainsValue(x.Ordered())).ToList();
        
        // digits with length 6: 0, 6, 9
        // 9 is the only 6 length digit that contains all signals that 4 also contains
        var nine = unmapped.Where(x => x.Length == 6).Single(x => x.Intersect(map[4]).OrderedEquals(map[4]));
        map.Add(9, nine.Ordered());
        unmapped.Remove(nine);

        // 0 is the only 6 length digit left that contains all signals that 7 also contains
        var zero = unmapped.Where(x => x.Length == 6).Single(x => x.Intersect(map[7]).OrderedEquals(map[7]));
        map.Add(0, zero.Ordered());
        unmapped.Remove(zero);
        
        // 6 is the last 6 length digit left
        var six = unmapped.Single(x => x.Length == 6);
        map.Add(6, six.Ordered());
        unmapped.Remove(six);
        
        // only 5 length digits are left now: 2, 3, 5
        // 3 is the only digit left that contains all signals that 1 also contains
        var three = unmapped.Single(x => x.Intersect(map[1]).OrderedEquals(map[1]));
        map.Add(3, three.Ordered());
        unmapped.Remove(three);
        
        // 2 is the only digit left that contains what 6 is missing to be an 8 (complete)
        var missing = map[8].Except(map[6]).Single();
        var two = unmapped.Single(x => x.Contains(missing));
        map.Add(2, two.Ordered());
        map.Add(5, unmapped.Single(x => x != two).Ordered()); // 5 is the only remaining digit

        // create four digit output value
        var lookup = map.ToDictionary(x => x.Value, x => x.Key);
        var sb = new StringBuilder();
        foreach (var outputValue in e.OutputValues)
            sb.Append(lookup[outputValue.Ordered()]);

        return int.Parse(sb.ToString());
    }
}