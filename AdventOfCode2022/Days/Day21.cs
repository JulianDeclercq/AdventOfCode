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

        long last = 0, input = 5_000_000_000_000;
        //input = 3_665_520_865_950;
        // input = 3_665_520_865_941;
        // input = 3_665_520_865_940;

        long invalidCtr = 0;
        
        var answer1 = 3_665_520_865_942; // EQUAL, but submit says it's too high
        var answer2 = 3_665_520_865_941; // EQUAL, but submit says it's also too high 
        var answer3 = 3_665_520_865_940; // correct answer

        var prevention = new HashSet<long>();

        Console.WriteLine($"Starting input: {input}");
        
        // to prevent going back up / down again to the last check that failed and getting stuck in an infinite loop
        var lastStatus = Status.Invalid;
        for (;;)
        {
            if (prevention.Contains(input))
            {
                Console.WriteLine($"Caught duplicate {input}, exiting..");
                return;
            }
            prevention.Add(input);

            var status = Cycle(input);
            var halfDif = Math.Max(1, Math.Abs(last - input) / 2);

            if (status is Status.Invalid)
            {
                switch (lastStatus)
                {
                    case Status.Invalid:
                    case Status.TooLow:
                        input++;
                        break;
                    case Status.TooHigh:
                        input--;
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
                ++invalidCtr;
                continue;
            }
            last = input;

            checked
            {
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

            lastStatus = status;
            Console.WriteLine($"Trying {input} next.");
        }
    }

    private Status Cycle(long input)
    {
        _numberByMonkey[MyMonkey] = input;

        var operation = _operationByMonkey["root"];
        var lhs = MonkeyValue(operation.Lhs); // cmmh was negative??
        var rhs = MonkeyValue(operation.Rhs);

        if (lhs == null || rhs == null)
            return Status.Invalid;

        long dif = 0;
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
                // "/" => lhs / rhs,
                "/" => Division(lhs, rhs),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (value < 0)
            {
                int brkpt = 5;
            }
            
            return value;
        }
    }

    // Avoid pursuing paths that have implicit flooring because of integer division 
    private static long? Division(long? lhs, long? rhs)
    {
        var answer = lhs / rhs;
        if (answer * rhs == lhs)
            return answer;
        
        return null;
    }
}