using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2015.Days;

public class Day19
{
    public void Part1()
    {
        const string start = "HOH";
        var regex = new Regex(@"(\w+) => (\w+)");
        var input = File.ReadAllLines(@"..\..\..\input\Day9_example.txt");
        
        var replacementsLookup = new Dictionary<string, List<string>>();
        foreach (var line in input.TakeWhile(s => !string.IsNullOrWhiteSpace(s)))
        {
            var match = regex.Match(line);
            var lhs = match.Groups[1].ToString();
            var rhs = match.Groups[2].ToString();
            
            var possibilities = replacementsLookup.TryGetValue(lhs, out var list) ? list : new List<string>();
            possibilities.Add(rhs);
            replacementsLookup[lhs] = possibilities;
        }
        
        var answer = new HashSet<string>();
        for (var i = 0; i < start.Length; ++i)
        {
            if (!replacementsLookup.TryGetValue($"{start[i]}", out var replacements))
                continue;

            var idx = i;
            var possibilities = replacements.Select(r => $"{start[..idx]}{r}{start[(idx + 1)..]}");
            answer.UnionWith(possibilities);
        }
        Console.WriteLine($"Day 19 Part 1: {answer.Count}");
    }
    
    public void Part2(){}
}