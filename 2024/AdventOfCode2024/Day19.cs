namespace AdventOfCode2024;

public class Day19(string inputFilePath)
{
    public int Part1()
    {
        var lines = File.ReadAllLines(inputFilePath);
        var patterns = lines[0].Split(", ").ToList();

        var answer = lines.Skip(2).Count(l => IsDesignPossible(patterns, l));
        Console.WriteLine(answer);
        return answer;
    }

    public int Part2()
    {
        var lines = File.ReadAllLines(inputFilePath);
        var patterns = lines[0].Split(", ").ToList();

        var answer = lines.Skip(2).Sum(l => CountDesignPossibleWays(patterns, l));
        Console.WriteLine(answer);
        return answer;
    }

    private static bool IsDesignPossible(IReadOnlyCollection<string> patterns, string design)
    {
        if (design.Length == 0)
            return true;

        var candidates = patterns.Where(design.StartsWith);
        return candidates.Any(candidate => IsDesignPossible(patterns, design[candidate.Length..]));
    }

    private static int CountDesignPossibleWays(IReadOnlyCollection<string> patterns, string design)
    {
        if (design.Length == 0)
            return 1;

        var candidates = patterns.Where(design.StartsWith);
        return candidates.Sum(candidate => CountDesignPossibleWays(patterns, design[candidate.Length..]));
    }
}