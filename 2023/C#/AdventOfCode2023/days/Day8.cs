using System.Text.RegularExpressions;
using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public partial class Day8
{
    private class Node
    {
        public Node(string name, string left, string right)
        {
            Name = name;
            Left = left;
            Right = right;
        }

        public string Name;
        public string Left;
        public string Right;
    }

    private static readonly RegexHelper Helper = new(InputPattern(), "name", "left", "right");
    public void Part1()
    {
        var input = File.ReadAllLines("../../../input/Day8.txt");
        var instructions = input.First();
        var nodes = input.Skip(2).Select(LineToNode).ToDictionary(k => k.Name, v => v);

        var current = nodes["AAA"];
        ulong answer = 0;
        for (var i = 0;; ++i)
        {
            ++answer;
                
            var instruction = instructions[i];
            current = instruction switch
            {
                'L' => nodes[current.Left],
                'R' => nodes[current.Right],
                _ => throw new Exception($"Invalid instruction {instruction}")
            };

            if (current.Name.Equals("ZZZ"))
            {
                Console.WriteLine(answer);
                return;
            }
            
            // Start over
            if (i == instructions.Length - 1)
                i = -1;
        }
    }

    private static Node LineToNode(string line)
    {
        if (!Helper.Match(line))
            throw new Exception($"Failed to parse line {line}");

        return new Node(Helper["name"], Helper["left"], Helper["right"]);
    }

    [GeneratedRegex(@"(\w{3}) = \((\w{3}), (\w{3})")]
    private static partial Regex InputPattern();
}