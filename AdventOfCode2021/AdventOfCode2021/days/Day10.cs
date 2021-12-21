using System.Collections;

namespace AdventOfCode2021.days;

public class Day10
{
    private static readonly Dictionary<char, char> _pairs = new()
    {
        {')', '('}, {']', '['}, {'}', '{'}, {'>', '<'},
    };

    private static readonly Dictionary<char, int> _score = new()
    {
        {')', 3}, {']', 57}, {'}', 1197}, {'>', 25137},
    };
    
    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day10.txt");

        var score = 0;
        foreach(var line in lines)
            score += CorruptionScore(line);
        
        Console.WriteLine($"Day 10 part 1: {score}");
    }

    private static int CorruptionScore(string line)
    {
        var stack = new Stack<char>();
        foreach (var c in line)
        {
            // if the pairs contains it, it is a closing bracket
            if (_pairs.TryGetValue(c, out var matching))
            {
                var peek = stack.Peek(); 
                if (peek != matching)
                    return _score[c];

                stack.Pop();
                continue;
            }

            // push the opening brackets to the stack
            stack.Push(c);
        }
        return 0;
    }
}