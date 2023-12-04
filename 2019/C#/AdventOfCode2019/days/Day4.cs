using System.Text.RegularExpressions;

namespace AdventOfCode2019.days;

public partial class Day4
{
    public static void Solve()
    {
        var range = Enumerable.Range(165432, 707912 - 165431).ToArray();
        Console.WriteLine(range.Count(nr => Validate(nr, part: 1)));
        Console.WriteLine(range.Count(nr => Validate(nr, part: 2)));
    }

    private static bool Validate(int nr, int part)
    {
        var str = nr.ToString();

        var adjacentParts = RepeatedAdjacent().Matches(str);
        var validAdjacent = part == 1 ? adjacentParts.Any() : adjacentParts.Any(ep => ep.Length == 2);

        if (!validAdjacent)
            return false;

        var numbers = str.Select(x => int.Parse($"{x}")).ToArray();
        var ordered = numbers.Order().ToArray();
        return numbers.SequenceEqual(ordered);
    }
    
    [GeneratedRegex(@"(.)\1+")]
    private static partial Regex RepeatedAdjacent();
}