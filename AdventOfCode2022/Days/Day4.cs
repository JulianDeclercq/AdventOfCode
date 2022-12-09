using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days;

public class Day4
{
    private readonly RegexHelper _regexHelper =
        new(new Regex(@"(\d+)-(\d+),(\d+)-(\d+)"), "aStart", "aEnd", "bStart", "bEnd");

    public void Solve()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day4.txt");

        int part1 = 0, part2 = 0;
        foreach (var line in lines)
        {
            _regexHelper.Parse(line);
            var lhs = new ValueTuple<int, int>(_regexHelper.GetInt("aStart"), _regexHelper.GetInt("aEnd"));
            var rhs = new ValueTuple<int, int>(_regexHelper.GetInt("bStart"), _regexHelper.GetInt("bEnd"));
            
            if (AreSubsetOfEachOther(lhs, rhs))
                part1++;
            
            if (Overlap(lhs, rhs))
                part2++;
        }
        Console.WriteLine($"Day 4 Part 1: {part1}");
        Console.WriteLine($"Day 4 Part 2: {part2}");
    }

    private static bool AreSubsetOfEachOther((int start, int end) lhs, (int start, int end) rhs)
    {
        if (lhs.start <= rhs.start && lhs.end >= rhs.end)
            return true;
        
        if (rhs.start <= lhs.start && rhs.end >= lhs.end)
            return true;

        return false;
    }
    
    private static bool Overlap((int start, int end) lhs, (int start, int end) rhs)
    {
        if (lhs.start <= rhs.end && lhs.end >= rhs.end)
            return true;
        
        if (rhs.start <= lhs.end && rhs.end >= lhs.end)
            return true;

        return false;
    }
}