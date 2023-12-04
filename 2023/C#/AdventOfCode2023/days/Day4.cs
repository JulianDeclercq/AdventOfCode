using System.Text.RegularExpressions;
using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public partial class Day4
{
    [GeneratedRegex(".+: (.+) \\| (.+)")]
    private static partial Regex InputRegex();
    
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
        var helper = new RegexHelper(InputRegex(), "winning", "candidates");
        var lines = File.ReadAllLines("../../../input/Day4.txt");
        
        // Start with one card of each number
        var occurrences = Enumerable.Range(1, lines.Length).ToDictionary(k => k, _ => 1);
        
        for (var i = 0; i < lines.Length; ++i)
        {
            var line = lines[i];
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
            
            var cardNumber = i + 1;
            var copiesWon = Enumerable.Range(cardNumber + 1, winning.Count).ToList();

            // Add the won cards for every card of the current number
            // (e.g. if card 2 wins 3 and 4 and there's 50 copies of card 2, add 50 card 3's and card 4's)
            foreach (var wonCopy in copiesWon)
                occurrences[wonCopy] += occurrences[cardNumber];
        }
        Console.WriteLine(occurrences.Values.Sum());
    }
}