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
        None = 0,
        TooLow = 1,
        Equal = 2,
        TooHigh = 3
    }

    readonly Dictionary<string, long> _numberByMonkey = new();
    readonly Dictionary<string, MonkeyOperation> _operationByMonkey = new();
    private const string MyMonkey = "humn";

    private void ParseInput()
    {
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

        long last = 0;
        // var input = 5_000_000_000_000;
        var input = 3_665_520_865_950;
        
        var answer1 = 3_665_520_865_942; // EQUAL, but submit says it's too high
        var answer2 = 3_665_520_865_941; // EQUAL, but submit says it's also too high 
        var answer3 = 3_665_520_865_940; // correct answer

        for (;;)
        {
            var status = Cycle(input);

            var halfDif = Math.Max(1, Math.Abs(last - input) / 2);
            last = input;
            
            switch (status)
            {
                case Status.TooLow:
                    input += halfDif;
                    break;
                case Status.Equal:
                    Console.WriteLine($"Day 21 part 2: {input}");
                    return;
                case Status.TooHigh:
                    input -= halfDif;
                    break;
            }
        }
    }

    private Status Cycle(long input)
    {
        _numberByMonkey[MyMonkey] = input;

        var operation = _operationByMonkey["root"];
        var lhs = MonkeyValue(operation.Lhs);
        var rhs = MonkeyValue(operation.Rhs);
        var dif = rhs - lhs;
        // Console.WriteLine($"{lhs} | {rhs} | {dif}");

        return dif switch
        {
            < 0 => Status.TooLow,
            0 => Status.Equal,
            > 0 => Status.TooHigh
        };
    }

    private long MonkeyValue(string monkey)
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
                "/" => lhs / rhs,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            return value;
        }
    }
}