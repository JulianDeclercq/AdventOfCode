using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day3
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/day3.txt");
        var input = string.Join("", lines);
        var regex = new Regex(@"mul\((\d+),(\d+)\)");

        var answer = regex.Matches(input).Aggregate(0, (total, next) =>
            total + int.Parse(next.Groups[1].Value) * int.Parse(next.Groups[2].Value));
        
        Console.WriteLine(answer);
    }
}