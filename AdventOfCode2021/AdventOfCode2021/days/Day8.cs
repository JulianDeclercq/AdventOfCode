namespace AdventOfCode2021.days;

public class Day8
{
    public class Entry
    {
        public Entry(List<string> usp, List<string> fdov)
        {
            USP = usp;
            FDOV = fdov;
        }
        public List<string> USP; // unique signal patterns
        public List<string> FDOV; // four digit output value
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
        var answer = entries.SelectMany(e => e.FDOV).Count(fdov => targetLengths.Contains(fdov.Length));
        Console.WriteLine($"Day 8 part 1: {answer}");
    }
}