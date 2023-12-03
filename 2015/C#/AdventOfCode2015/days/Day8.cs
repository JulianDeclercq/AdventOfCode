using System.Text.RegularExpressions;

namespace AdventOfCode2015.Days;

public class Day8
{
    private static readonly Regex ReHex = new(@"\\x[0-9a-f]{2}");

    public static void Part1()
    {
        var input = File.ReadAllLines(@"..\..\..\input\Day8.txt");
        var answer = input.Sum(x => x.Length) - input.Sum(x => InMemoryString(x).Length);
        Console.WriteLine($"Day 8 part 1: {answer}");
    }

    private static string InMemoryString(string input)
    {
        // [1..^1] to remove first and last " character
        return ReHex.Replace(input[1..^1], "X").Replace("\\\"", "\"").Replace(@"\\", @"\");
    }

    public static void Part2()
    {
        var input = File.ReadAllLines(@"..\..\..\input\Day8.txt");
        var answer = input.Sum(x => Encode(x).Length) - input.Sum(x => x.Length);
        Console.WriteLine($"Day 8 part 2: {answer}");
    }
    
    private static string Encode(string input)
    {
        return $"\"{input.Replace("\"", "XX").Replace(@"\", "XX")}\"";
    }

}