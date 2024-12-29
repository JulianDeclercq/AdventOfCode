using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day7
{
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
                
        var lines = File.ReadAllLines("input/day7.txt");
        var helper = new RegexHelper(new Regex(@"(\d+): (.+)"), "result", "operands");
        long answer = 0;
        
        foreach (var line in lines)
        {
            if (!helper.Match(line))
                throw new Exception($"No match on line {line}");

            var result = helper.GetLong("result");
            var operands = helper.Get("operands").Split(' ').Select(long.Parse);
            if (IsValid(result, operands.ToArray(), part))
                answer += result;
        }
        Console.WriteLine(answer);
    }

    private static bool IsValid(long result, long[] operands, int part)
    {
        if (Step(operands[0], result, operands[1..], OperandType.Addition, part))
            return true;
        
        if (Step(operands[0], result, operands[1..], OperandType.Multiplication, part))
            return true;

        if (part is 2 && Step(operands[0], result, operands[1..], OperandType.Concatenation, part))
            return true;
        
        return false;
    }
    
    private static bool Step(long current, long result, long[] operands, OperandType type, int part)
    {
        if (operands.Length == 0)
            return false;
        
        var currentOperand = operands[0];
        switch (type)
        {
            case OperandType.Addition:
                current += currentOperand;
                break;
            case OperandType.Multiplication:
                current *= currentOperand;
                break;
            case OperandType.Concatenation:
                current = long.Parse($"{current}{currentOperand}");
                break;
            default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        if (current > result) // early return
            return false;

        if (current == result && operands.Length == 1)
            return true;
        
        if (Step(current, result, operands[1..], OperandType.Addition, part))
            return true;
        
        if (Step(current, result, operands[1..], OperandType.Multiplication, part)) 
            return true;

        if (part is 2 && Step(current, result, operands[1..], OperandType.Concatenation, part))
            return true;

        return false;
    }
    
    private enum OperandType
    {
        None = 0,
        Addition = 1,
        Multiplication = 2,
        Concatenation = 3
    }
}