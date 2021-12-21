using System.Collections;
using System.Text;

namespace AdventOfCode2021.days;

public class Day10
{
    private static readonly Dictionary<char, char> Openings = new()
    {
        {')', '('}, {']', '['}, {'}', '{'}, {'>', '<'},
    };
    
    private static readonly Dictionary<char, char> Closings = new()
    {
        {'(', ')'}, {'[', ']'}, {'{', '}'}, {'<', '>'},
    };

    private static readonly Dictionary<char, int> CorruptionScoreLookup = new()
    {
        {')', 3}, {']', 57}, {'}', 1197}, {'>', 25137},
    };
    
    private static readonly Dictionary<char, ulong> AutocompleteScoreLookup = new()
    {
        {')', 1}, {']', 2}, {'}', 3}, {'>', 4},
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
            if (Openings.TryGetValue(c, out var matching))
            {
                var peek = stack.Peek(); 
                if (peek != matching)
                    return CorruptionScoreLookup[c];

                stack.Pop();
                continue;
            }

            // push the opening brackets to the stack
            stack.Push(c);
        }
        return 0;
    }

    public void Part2()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day10.txt");
        var incompleteLines = lines.Where(l => CorruptionScore(l) == 0);

        var list = new List<ulong>();
        foreach (var line in incompleteLines)
            list.Add(AutocompleteScore(CompletionList(line)));

        list.Sort();
        
        Console.WriteLine($"Day 10 part 2: {list[list.Count / 2]}");
    }

    private static string CompletionList(string incompleteLine)
    {
        var stack = new Stack<char>();
        foreach (var c in incompleteLine)
        {
            // if the pairs contains it, it is a closing bracket
            if (Openings.TryGetValue(c, out var matching))
            {
                // don't have to check if it matches, because this method only gets valid input
                stack.Pop();
                continue;
            }

            // push the opening brackets to the stack
            stack.Push(c);
        }

        var completionList = new StringBuilder();
        foreach (var opening in stack)
            completionList.Append(Closings[opening]);

        return completionList.ToString();
    }

    private static ulong AutocompleteScore(string completionList)
    {
        ulong score = 0;
        foreach (var c in completionList)
        {
            score *= 5;
            score += AutocompleteScoreLookup[c];
        }
        return score;
    }
    
}