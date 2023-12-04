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
        var pointValues = new List<int>();
        var helper = new RegexHelper(InputRegex(), "winning", "candidates");

        var lines = File.ReadAllLines("../../../input/Day4_example.txt");
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
            
            // TODO: Be careful for indices vs card nrs, and in general with off by 1 here lol.
            var cardsLeft = lines.Length - 1 - i;
            var winners = Math.Min(winning.Count, cardsLeft); // TODO: Check if this check is needed / makes sense
            var idx = i;
            RegisterCopies(Enumerable.Range(i + 2, winners).Select(x => x + idx));
        }
        Console.WriteLine(pointValues.Sum());
    }

    private readonly Dictionary<int, int> _cardOccurrences = new(); // key: cardNr, value: occurrences
    private void RegisterCopies(IEnumerable<int> cardNrs)
    {
        foreach (var cardNr in cardNrs)
        {
            var occurrences = _cardOccurrences.TryGetValue(cardNr, out var o) ? o : 0;
            _cardOccurrences[cardNr] = occurrences + 1;
        }
    }
    
    [GeneratedRegex(".+: (.+) \\| (.+)")]
    private static partial Regex InputRegex();
}