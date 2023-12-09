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
    public void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day8.txt");
        var instructions = input.First();
        var nodes = input.Skip(2).Select(LineToNode).ToDictionary(k => k.Name, v => v);

        var startingNodes = nodes.Keys.Where(k => k.EndsWith('A')).ToArray();

        // start, current
        var currentByStarting = startingNodes.ToDictionary(k => k, v => v); 
        ulong answer = 0, iterationCounter = 0;
        for (var i = 0;; ++i)
        {
            if (iterationCounter != 0 && iterationCounter % 10_000 == 0)
                Qonsole.OverWrite($"{iterationCounter / 1_000_000} million iterations{string.Concat(Enumerable.Repeat('.', (int)(iterationCounter / 1_000_000) % 3 + 1))}");

            ++answer;
            ++iterationCounter;
            
            var instruction = instructions[i];
            foreach (var startingNode in startingNodes)
            {
                var current = currentByStarting[startingNode];
                currentByStarting[startingNode] = instruction switch
                {
                    'L' => nodes[current].Left,
                    'R' => nodes[current].Right,
                    _ => throw new Exception($"Invalid instruction {instruction}")
                };
            }

            if (currentByStarting.Values.All(v => v.EndsWith('Z')))
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