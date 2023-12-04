using System.Text.RegularExpressions;
using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public partial class Day4
{
    public void Part1()
    {
        var pointValues = new List<int>();
        var helper = new RegexHelper(InputRegex(), "winning", "candidates");
        foreach (var line in File.ReadAllLines("../../../input/Day4.txt"))
        {
            if (!helper.Match(line))
            {
                Console.WriteLine($"Failed to match line {line}");
                continue;
            }

            var winning = helper.Get("winning")
                .Split(' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse)
                .ToHashSet();

            var candidates = helper.Get("candidates")
                .Split(' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse)
                .ToHashSet();

            winning.IntersectWith(candidates);

            if (winning.Count == 0)
                continue;

            var answer = 0.5;
            for (var i = 0; i < winning.Count; ++i) answer *= 2;
            pointValues.Add((int)answer);
        }
        Console.WriteLine(pointValues.Sum());
    }
    
    public void Part2()
    {
        
    }

    [GeneratedRegex(".+: (.+) \\| (.+)")]
    private static partial Regex InputRegex();
}