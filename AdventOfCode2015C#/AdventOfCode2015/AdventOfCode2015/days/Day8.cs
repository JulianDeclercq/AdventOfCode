using System.Text.RegularExpressions;

namespace AdventOfCode2015.Days;

public class Day8
{
    public void Part1()
    {
        var input = File.ReadAllLines(@"..\..\..\input\Day8.txt");
        var answer = input.Sum(x => x.Length) - input.Sum(x => InMemoryString(x).Length);
        Console.WriteLine($"Day 8 part 1: {answer}");
    }

    private static readonly Regex ReHex = new(@"\\x[0-9a-f]{2}");
    private static string InMemoryString(string input)
    {
        // [1..^1] to remove first and last " character
        return ReHex.Replace(input[1..^1], "X").Replace("\\\"", "\"").Replace(@"\\", @"\");
    }
    
    public void Part2(){}

}