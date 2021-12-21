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

            // push the opening bracket to the stack
            stack.Push(c);
        }
        return 0;
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

            // push the opening bracket to the stack
            stack.Push(c);
        }

        return new string(stack.Select(x => Closings[x]).ToArray());
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

    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day10.txt");
        Console.WriteLine($"Day 10 part 1: {lines.Sum(CorruptionScore)}");
    }

    public void Part2()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day10.txt");
        var incompleteLines = lines.Where(l => CorruptionScore(l) == 0);

        var list = incompleteLines.Select(line => AutocompleteScore(CompletionList(line))).ToList();
        list.Sort();
        
        Console.WriteLine($"Day 10 part 2: {list[list.Count / 2]}");
    }
}