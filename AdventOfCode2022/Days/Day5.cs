using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days;

public class Day5
{
    private readonly Helpers.RegexHelper _regexHelper =
        new(new Regex(@"move (\d+) from (\d+) to (\d+)"), "amount", "source", "destination");

    public void Solve()
    {
        // var lines = File.ReadAllLines(@"..\..\..\input\day5_example.txt");
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
             for (var i = 0; i < amount; ++i)
             {
                 var crate = stacks[source].Pop();
                 stacks[destination].Push(crate);
             }
        }
        Console.WriteLine($"Day 5 part 1: {string.Join("", stacks.Select(s => s.Peek()))}");
    }
}