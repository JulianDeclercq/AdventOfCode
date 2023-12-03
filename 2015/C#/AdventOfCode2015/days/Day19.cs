using System.Text.RegularExpressions;

namespace AdventOfCode2015.Days;

public class Day19
{
    public void Part1()
    {
        var regex = new Regex(@"(\w+) => (\w+)");
        var input = File.ReadAllLines(@"..\..\..\input\Day9.txt");
        var start = input.Last();
        
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
        
        var parts = new List<string>();
        var answer = new HashSet<string>();
        for (var i = 0; i < start.Length; ++i)
        {
            if (i == start.Length - 1)
            {
                parts.Add($"{start[i]}");
                break;
            }

            if (char.IsLower(start[i + 1]))
            {
                parts.Add(start.Substring(i, 2));
                i++;
            }
            else
            {
                parts.Add($"{start[i]}");
            }
        }

        for (var i = 0; i < parts.Count; ++i)
        {
            if (!replacementsLookup.TryGetValue($"{parts[i]}", out var replacements))
                continue;

            var idx = i;
            answer.UnionWith(replacements.Select(replacement => {
                        var before = parts.Take(idx);
                        var after = parts.Skip(idx + 1);
                        return string.Join("", before.Append(replacement).Concat(after));
                    }));
        }
        
        Console.WriteLine($"Day 19 Part 1: {answer.Count}");
    }

    public void Part2(){}
}