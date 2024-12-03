using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day3
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/day3.txt");
        var input = string.Join("", lines);
        var regex = new Regex(@"mul\((\d+),(\d+)\)");
        var matches = regex.Matches(input);

        var answer = 0;
        foreach (Match match in matches)
            answer += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value) ;
        
        Console.WriteLine(answer);
    }
}