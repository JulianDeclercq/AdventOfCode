using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days;

public class Day21
{
    private readonly RegexHelper _numberPattern = new(new Regex(@"(\w{4}): (\d+)"),
        "monkey", "number");
    
    private readonly RegexHelper _operationPattern = new(new Regex(@"(\w{4}): (\w{4}) (.+) (\w{4})"),
        "monkey", "lhs", "operator", "rhs");

    private record MonkeyOperation(string Lhs, string Operator, string Rhs);

    readonly Dictionary<string, long> _numberByMonkey = new();
    readonly Dictionary<string, MonkeyOperation> _operationByMonkey = new();
    public void Solve()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day21.txt");

        foreach (var line in lines)
        {
            if (_numberPattern.Parse(line))
            {
                _numberByMonkey[_numberPattern.Get("monkey")] = _numberPattern.GetInt("number");
            }
            else if (_operationPattern.Parse(line))
            {
                _operationByMonkey[_operationPattern.Get("monkey")] = new MonkeyOperation(
                    _operationPattern.Get("lhs"), _operationPattern.Get("operator"), _operationPattern.Get("rhs"));
            }
            else throw new Exception($"Invalid input line {line}");
        }

        Console.WriteLine($"Day 21 part 1: {MonkeyValue("root")}");
    }

    private long MonkeyValue(string monkey)
    {
        if (_numberByMonkey.TryGetValue(monkey, out var number))
            return number;
        
        var operation = _operationByMonkey[monkey];
        var lhs = MonkeyValue(operation.Lhs);
        var rhs = MonkeyValue(operation.Rhs);

        var value = operation.Operator switch
        {
            "+" => lhs + rhs,
            "-" => lhs - rhs,
            "*" => lhs * rhs,
            "/" => lhs / rhs,
            _ => throw new ArgumentOutOfRangeException()
        };

        _numberByMonkey.Add(monkey, value);
        return value;
    }
}