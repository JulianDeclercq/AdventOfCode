using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day7
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/day7.txt");
        var helper = new RegexHelper(new Regex(@"(\d+): (.+)"), "result", "operands");
        long answer = 0;
        
        foreach (var line in lines)
        {
            if (!helper.Match(line))
                throw new Exception($"No match on line {line}");

            var result = helper.GetLong("result");
            var operands = helper.Get("operands").Split(' ').Select(long.Parse);
            if (IsValid(result, operands.ToArray()))
                answer += result;
        }
        Console.WriteLine(answer);
    }

    private static bool IsValid(long result, long[] operands)
    {
        if (Step(operands[0], result, operands[1..], true))
            return true;
        
        if (Step(operands[0], result, operands[1..], false))
            return true;
        
        return false;
    }
    
    private static bool Step(long current, long result, long[] operands, bool add)
    {
        if (operands.Length == 0)
            return false;

        if (add)
            current += operands[0];
        else
            current *= operands[0];
        
        if (current > result) // early return
            return false;

        if (current == result && operands.Length == 1)
            return true;

        if (Step(current, result, operands[1..], true))
            return true;
        
        if (Step(current, result, operands[1..], false))
            return true;

        return false;
    }
}