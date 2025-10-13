using System.Numerics;

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

    public BigInteger Part2()
    {
        var lines = File.ReadAllLines(inputFilePath);
        var patterns = lines[0].Split(", ").ToList();

        BigInteger answer = 0;
        foreach (var design in lines.Skip(2))
            answer += CountDesignPossibleWays(patterns, design);
        
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

    private static readonly Dictionary<string, BigInteger> Memo = [];

    private static BigInteger CountDesignPossibleWays(IReadOnlyCollection<string> patterns, string design)
    {
        if (design.Length == 0)
            return 1;

        if (Memo.TryGetValue(design, out var memodCount))
            return memodCount;

        BigInteger count = 0;
        foreach (var candidate in patterns.Where(design.StartsWith))
        {
            var candidateCount = CountDesignPossibleWays(patterns, design[candidate.Length..]);
            count += candidateCount;
        }

        // memo it
        Memo[design] = count;
        return count;
    }
}