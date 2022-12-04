using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days;

public class Day4
{
    private readonly Helpers.RegexHelper _regexHelper =
        new(new Regex(@"(\d+)-(\d+),(\d+)-(\d+)"), "aStart", "aEnd", "bStart", "bEnd");

    public void Solve()
    {
        // var lines = File.ReadAllLines(@"..\..\..\input\day4_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day4.txt");

        var result = 0;
        foreach (var line in lines)
        {
            var comma = line.IndexOf(',');
            _regexHelper.Parse(line);
            if (AreSubsetOfEachother(
                    new(_regexHelper.GetInt("aStart"), _regexHelper.GetInt("aEnd")),
                    new(_regexHelper.GetInt("bStart"), _regexHelper.GetInt("bEnd"))))
            {
                result++;
            }
        }
        Console.WriteLine(result);
    }

    private bool AreSubsetOfEachother((int start, int end) lhs, (int start, int end) rhs)
    {
        if (lhs.start <= rhs.start && lhs.end >= rhs.end)
            return true;
        
        if (rhs.start <= lhs.start && rhs.end >= lhs.end)
            return true;

        return false;
    }
}