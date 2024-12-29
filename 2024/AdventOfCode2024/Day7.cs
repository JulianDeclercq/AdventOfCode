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
        // string[] lines =
        // [
        //     // "156: 15 6",
        //     // "7290: 6 8 6 15",
        //     // "192: 17 8 14"
        //     // "100: 50 50 2"
        // ];
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
        // I guess the last operation here is a bit fake, but it works for the logic
        if (Step(operands[0], result, operands[1..], OperandType.Addition,
                new Operation(operands[0], OperandType.Addition)))
            return true;
        
        if (Step(operands[0], result, operands[1..], OperandType.Multiplication,
                new Operation(operands[0], OperandType.Multiplication)))
            return true;
        
        if (part is 1)
            return false;
        
        // not sure about the first current, see other comment here too
        if (Step(operands[0], result, operands[1..], OperandType.Concatenation,
                new Operation(operands[0], OperandType.Addition))) // first operation needs to be addition to not make it count
            return true;
        
        return false;
    }
    
    private static bool Step(long current, long result, long[] operands, OperandType type, Operation lastOperation)
    {
        if (operands.Length == 0)
            return false;
        
        var currentOperand = operands[0];
        var (lastOperand, lastOperationType) = lastOperation;
        switch (type)
        {
            case OperandType.Addition:
                current += currentOperand;
                break;
            case OperandType.Multiplication:
                current *= currentOperand;
                break;
            case OperandType.Concatenation:
                // undo the last operation
                switch (lastOperationType)
                {
                    case OperandType.Addition:
                        // current -= lastOperand;
                        break;
                    case OperandType.Multiplication:
                        // current /= lastOperand;
                        break;
                    case OperandType.Concatenation: 
                        break; // do nothing
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var concatenated = long.Parse($"{current}{currentOperand}");
                // Console.WriteLine($"Concatenated {current} and {currentOperand} into {concatenated}");
                current = concatenated; // TODO: NOT SURE
                // if (concatenated is 486)
                // {
                //     int kpbt = 5;
                // }
                // currentOperand = concatenated;
                break;
            default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        if (current > result) // early return
            return false;

        if (current == result && operands.Length == 1)
            return true;
        
        var thisOperation = new Operation(currentOperand, type);
        if (Step(current, result, operands[1..], OperandType.Addition, thisOperation))
            return true;
        
        if (Step(current, result, operands[1..], OperandType.Multiplication, thisOperation))
            return true;
        
        if (Step(current, result, operands[1..], OperandType.Concatenation, thisOperation))
            return true;

        return false;
    }

    private record Operation(long Operand, OperandType Type);
    
    private enum OperandType
    {
        None = 0,
        Addition = 1,
        Multiplication = 2,
        Concatenation = 3
    }
}