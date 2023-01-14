using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days;

public class Day21
{
    private readonly RegexHelper _numberPattern = new(new Regex(@"(\w{4}): (\d+)"),
        "monkey", "number");
    
    private readonly RegexHelper _operationPattern = new(new Regex(@"(\w{4}): (\w{4}) (.+) (\w{4})"),
        "monkey", "lhs", "operator", "rhs");

    private record MonkeyOperation(string Lhs, string Operator, string Rhs);
    
    private enum Status
    {
        Invalid = 0,
        TooLow = 1,
        Equal = 2,
        TooHigh = 3
    }

    private readonly Dictionary<string, long> _numberByMonkey = new();
    private readonly Dictionary<string, MonkeyOperation> _operationByMonkey = new();
    private const string MyMonkey = "humn";

    private void ParseInput()
    {
        // var lines = File.ReadAllLines(@"..\..\..\input\day21_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day21.txt");

        foreach (var line in lines)
        {
            if (_numberPattern.Match(line))
            {
                _numberByMonkey[_numberPattern.Get("monkey")] = _numberPattern.GetInt("number");
            }
            else if (_operationPattern.Match(line))
            {
                _operationByMonkey[_operationPattern.Get("monkey")] = new MonkeyOperation(
                    _operationPattern.Get("lhs"), _operationPattern.Get("operator"), _operationPattern.Get("rhs"));
            }
            else throw new Exception($"Invalid input line {line}");
        }
    }

    public void Solve()
    {
        ParseInput();
        Console.WriteLine($"Day 21 part 1: {MonkeyValue("root")}");
    }
    
    public void Solve2()
    {
        ParseInput();
        
        long lastInput = 0, input = 5_000_000_000_000;
        Console.WriteLine($"Starting input: {input}");

        var lastStatus = Status.Invalid;
        for (;;)
        {
            var status = Cycle(input);
            switch (status)
            {
                case Status.Equal:
                    Console.WriteLine($"Day 21 part 2: {input}");
                    return;
                case Status.Invalid:
                    input = NextInputWhenInvalid(input, lastStatus);
                    continue;
                case Status.TooLow:
                case Status.TooHigh:
                    var newInput = NextInput(lastInput, input, status);
                    lastInput = input;
                    input = newInput;
                    lastStatus = status;
                    Console.WriteLine($"Trying {input} next.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    private Status Cycle(long input)
    {
        _numberByMonkey[MyMonkey] = input;

        var operation = _operationByMonkey["root"];
        var lhs = MonkeyValue(operation.Lhs);
        var rhs = MonkeyValue(operation.Rhs);

        if (lhs == null || rhs == null)
            return Status.Invalid;

        long dif;
        checked
        {
            //dif = lhs.Value - rhs.Value; // WORKS FOR EXAMPLE BUT NOT FOR INPUT
            dif = rhs.Value - lhs.Value; // WORKS FOR INPUT BUT NOT FOR EXAMPLE
        }
        
        var status = dif switch
        {
            < 0 => Status.TooLow,
            0 => Status.Equal,
            > 0 => Status.TooHigh
        };
        
        Console.WriteLine($"Input: {input}, status: {status}, lhs:{lhs}, rhs: {rhs}, dif: {dif}");
        return status;
    }
    
    private static long NextInput(long lastInput, long currentInput, Status status)
    {
        var halfDif = Math.Max(1, Math.Abs(lastInput - currentInput) / 2);

        checked
        {
            return status switch
            {
                Status.TooLow => currentInput + halfDif,
                Status.TooHigh => currentInput - halfDif,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    private static long NextInputWhenInvalid(long currentInput, Status lastStatus)
    {
        return lastStatus switch
        {
            Status.Invalid => currentInput + 1, // in case of starting with an invalid input
            Status.TooLow => currentInput + 1,
            Status.TooHigh => currentInput - 1,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private long? MonkeyValue(string monkey)
    {
        if (_numberByMonkey.TryGetValue(monkey, out var number))
            return number;
        
        var operation = _operationByMonkey[monkey];
        var lhs = MonkeyValue(operation.Lhs);
        var rhs = MonkeyValue(operation.Rhs);

        checked
        {
            var value = operation.Operator switch
            {
                "+" => lhs + rhs,
                "-" => lhs - rhs,
                "*" => lhs * rhs,
                "/" => Division(lhs, rhs),
                _ => throw new ArgumentOutOfRangeException()
            };
            return value;
        }
    }

    // Avoid pursuing paths that have implicit flooring because of integer division.
    private static long? Division(long? lhs, long? rhs)
    {
        var answer = lhs / rhs;
        return answer * rhs == lhs ? answer : null;
    }
}