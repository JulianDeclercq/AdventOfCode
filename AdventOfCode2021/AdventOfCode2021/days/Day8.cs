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
        public readonly List<string> UniqueSignalPatterns; // unique signal patterns
        public readonly List<string> OutputValues; // four digit output value
    }
    
    // map the numbers with the segments that represent them (use string here as an array of characters)
    private readonly Dictionary<int, string> _map = new()
    {
        {0, "abcefg"}, {1, "cf"}, {2, "acdeg"}, {3, "acdfg"}, {4, "bcdf"},
        {5, "abdfg"}, {6, "abdefg"}, {7, "acf"}, {8, "abcdefg"}, {9, "abcdfg"},
    };
    
    public void Part1()
    {
        var entries = File.ReadLines(@"..\..\..\input\day8.txt").
            Select(a => a.Split('|')
           .Select(b => b.Trim())
           .Select(c => c.Split(' ').ToList()).ToList())
           .Select(d => new Entry(d[0], d[1])).ToList();
        
        var targetLengths = new List<int>{_map[1].Length, _map[4].Length, _map[7].Length, _map[8].Length};
        var answer = entries.SelectMany(e => e.OutputValues).Count(fdov => targetLengths.Contains(fdov.Length));
        Console.WriteLine($"Day 8 part 1: {answer}");
    }
    
    public void Part2()
    {
        var entries = File.ReadLines(@"..\..\..\input\day8.txt").
            Select(a => a.Split('|')
           .Select(b => b.Trim())
           .Select(c => c.Split(' ').ToList()).ToList())
           .Select(d => new Entry(d[0], d[1])).ToList();

        Console.WriteLine($"Day 8 part 2: {entries.Select(FourDigitOutputValue).Sum()}");
    }

    private int FourDigitOutputValue(Entry e)
    {
        // add (ordered) 1 4 7 and 8 since those have unique lengths
        var map = new Dictionary<int, string>
        {
            {1, e.UniqueSignalPatterns.Single(x => x.Length == _map[1].Length).Ordered()},
            {4, e.UniqueSignalPatterns.Single(x => x.Length == _map[4].Length).Ordered()},
            {7, e.UniqueSignalPatterns.Single(x => x.Length == _map[7].Length).Ordered()},
            {8, e.UniqueSignalPatterns.Single(x => x.Length == _map[8].Length).Ordered()}
        };

        var unmapped = e.UniqueSignalPatterns.Where(x => !map.ContainsValue(x.Ordered())).ToList();
        
        // determine mapping
        // digits with length 6: 0, 6, 9
        // 9 is the only 6 length digit left that contains all signals that 4 also contains
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