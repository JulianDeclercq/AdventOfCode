using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days;

public class Day5
{
    private readonly RegexHelper _regexHelper =
        new(new Regex(@"move (\d+) from (\d+) to (\d+)"), "amount", "source", "destination");

    public void Solve(bool part1 = true)
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day5.txt");
        var stacks = new List<Stack<char>>();

        const int chunkSize = 4;
        var crateRows = lines.TakeWhile(l => !string.IsNullOrEmpty(l)).Reverse().Skip(1).ToArray();
        var loops = (crateRows.First().Length + 1) / chunkSize; // +1 for last element space padding
        for (var i = 0; i < loops; ++i)
        {
            var captured = i;
            var elements = crateRows.SelectMany(x => x.Skip(captured * chunkSize).Take(chunkSize).Where(char.IsLetter));
            stacks.Add(new Stack<char>(elements));
        }

        var instructions = lines.SkipWhile(l => !string.IsNullOrEmpty(l)).Skip(1);
        foreach (var instruction in instructions)
        {
             _regexHelper.Parse(instruction);
             var amount = _regexHelper.GetInt("amount");
             var source = _regexHelper.GetInt("source") - 1; // -1 since index is 0-based in list but not in input
             var destination = _regexHelper.GetInt("destination") - 1; // -1 since index is 0-based in list but not in input
             if (part1)
             {
                 Part1(amount, stacks[source], stacks[destination]);
             }
             else
             {
                 Part2(amount, stacks[source], stacks[destination]);
             }
        }
        Console.WriteLine($"Day 5 part {(part1 ? 1 : 2)}: {string.Join("", stacks.Select(s => s.Peek()))}");
    }

    private static void Part1(int amount, Stack<char> source, Stack<char> destination)
    {
        for (var i = 0; i < amount; ++i)
            destination.Push(source.Pop());
    }

    private static void Part2(int amount, Stack<char> source, Stack<char> destination)
    {
        var batch = new List<char>();
        for (var i = 0; i < amount; ++i)
            batch.Add(source.Pop());

        batch.Reverse();
        foreach(var element in batch)
            destination.Push(element);
    }
}