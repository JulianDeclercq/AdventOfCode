using System.Text.RegularExpressions;
using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public partial class Day4
{
    [GeneratedRegex(".+: (.+) \\| (.+)")]
    private static partial Regex InputRegex();
    
    public static void Solve()
    {
        var helper = new RegexHelper(InputRegex(), "winning", "candidates");
        var lines = File.ReadAllLines("../../../input/Day4.txt");
        var part1 = 0;
        
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
            
            // Part 1
            var pointValue = Enumerable.Range(0, winning.Count).Aggregate(0.5, (x, _) => x * 2);
            part1 += (int)pointValue;
            
            // Part 2
            var cardNumber = i + 1;
            var copiesWon = Enumerable.Range(cardNumber + 1, winning.Count).ToList();

            // Add the won cards for every card of the current number
            // (e.g. if card 2 wins 3 and 4 and there's 50 copies of card 2, add 50 card 3's and card 4's)
            foreach (var wonCopy in copiesWon)
                occurrences[wonCopy] += occurrences[cardNumber];
        }
        Console.WriteLine($"Day 4 part 1: {part1}");
        Console.WriteLine($"Day 4 part 2: {occurrences.Values.Sum()}");
    }
}