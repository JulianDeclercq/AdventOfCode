using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day24
{
    private readonly RegexHelper _regexHelper = new(new Regex(@"(\w+) (\w+) ?(-?\w+)?"), "instruction", "lhs", "rhs");
    private readonly List<IEnumerable<Action<int>>> _parts = new();
    private readonly Dictionary<char, int> _registers = new() { {'x', 0}, {'y', 0}, {'z', 0}, {'w', 0} };
    
    public void Part1()
    {
        var monad = File.ReadAllLines(@"..\..\..\input\day24.txt").Select(ParseInstruction).ToArray();
        const int size = 18;
        for (var i = 0; i < 14; ++i)
            _parts.Add(monad.Skip(size * i).Take(size));

        const int zSteps = 100; //not sure how high this should be , depends on which step i suppose since some steps divide it by 26 while others dont
        for (var w = 1; w <= 9; ++w)
        {
            for (var z = 0; z <= zSteps; ++z)
            {
                ResetRegisters();
                _registers['z'] = z;
                //PrintRegisters();

                foreach (var action in _parts.Last())
                {
                    action.Invoke(w);
                    //PrintRegisters();
                }
                
                if (_registers['z'] == 0)
                    Console.WriteLine($"Valid input: w = {w}, z start = {z}");
            }
        }

//        Console.WriteLine($"Day 24 part 1: {ctr}");
    }

    private Action<int> ParseInstruction(string instruction)
    {
        if (!_regexHelper.Match(instruction))
            return (_) => throw new Exception($"Instruction {instruction} didn't match pattern.");
        
        var instructionType = _regexHelper.Get("instruction");
        var key = _regexHelper.Get("lhs")[0];
        if (instructionType.Equals("inp"))
            return i => _registers[key] = i;

        var rhsKey = '@'; // custom invalid value >:)
        var literal = true;
        if (!_regexHelper.TryGetInt("rhs", out var rhs))
        {
            literal = false;
            rhsKey = _regexHelper.Get("rhs")[0];
        }

        return instructionType switch
        {
            "add" => (_) => _registers[key] += literal ? rhs : _registers[rhsKey],
            "mul" => (_) => _registers[key] *= literal ? rhs : _registers[rhsKey],
            "div" => (_) => _registers[key] /= literal ? rhs : _registers[rhsKey],
            "mod" => (_) => _registers[key] %= literal ? rhs : _registers[rhsKey],
            "eql" => (_) => _registers[key] = _registers[key] == (literal ? rhs : _registers[rhsKey]) ? 1 : 0,
            _ => (_) => throw new Exception($"Invalid instruction type {instructionType}")
        };
    }

    private void ResetRegisters()
    {
        _registers['x'] = 0;
        _registers['y'] = 0;
        _registers['z'] = 0;
        _registers['w'] = 0;
    }

    private void PrintRegisters()
    {
        foreach(var (name, value) in _registers) 
            Console.WriteLine($"Register {name} contains: {value}");
        Console.WriteLine('\n');
    }
    
    
}