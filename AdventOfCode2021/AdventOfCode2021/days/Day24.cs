using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day24
{
    private readonly RegexHelper _regexHelper = new(new Regex(@"(\w+) (\w+) ?(-?\w+)?"), "instruction", "lhs", "rhs");
    private readonly Dictionary<char, int> _registers = new() { {'x', 0}, {'y', 0}, {'z', 0}, {'w', 0} };
    private Dictionary<(int stepIdx, int initialW, int initialZ), int> _memo = new();
    private List<List<Action<int>>> _parts = new();
    
    public void Part1()
    {
        var monad = File.ReadAllLines(@"..\..\..\input\day24.txt").Select(ParseInstruction).ToArray();
        const int size = 18;
        
        for (var i = 0; i < 14; ++i)
            _parts.Add(monad.Skip(size * i).Take(size).ToList());

        for (var input = 99999999999999; input >= 11111111111111; --input)
        {
            var inputS = input.ToString();
            ResetRegisters();

            var initialZ = 0;
            for (var i = 0; i < _parts.Count; ++i)
                initialZ = RunPart(i, int.Parse(inputS[i].ToString()), initialZ);
            
            if (_registers['z'] == 0)
            {
                Console.WriteLine($"Day 24 part 1: {inputS}");
                break;
            }
            
            if (input % 1000000 == 0)
                Console.WriteLine(inputS);
        }
        
        Console.WriteLine(_memo.Count);
        return;

        foreach (var m in _memo)
        {
            if(m.Key.stepIdx == 13)
                Console.WriteLine($"{m.Key}: {m.Value}");
        }
        
//        Console.WriteLine($"Day 24 part 1: {ctr}");
    }

    private int RunPart(int partIdx, int initialW, int initialZ)
    {
        var key = (partIdx, initialW, initialZ);
        if (_memo.ContainsKey(key))
        {
            // simulate the registers as if everything was run again
            // x and y don't matter since they get reset anyways
            _registers['w'] = initialW;
            _registers['z'] = _memo[key];
            return _memo[key];
        }

        foreach (var action in _parts[partIdx])
            action.Invoke(initialW);
        
        _memo.Add(key, _registers['z']);

        return _registers['z'];
    }

    private List<(int initialW, int initialZ, int targetZ)> FindPossibilities(IEnumerable<Action<int>> step, int targetZ)
    {
        const int zSteps = 100000; //not sure how high this should be , depends on which step i suppose since some steps divide it by 26 while others dont
        var possibilities = new List<(int, int, int)>();
        var stepArr = step.ToArray();
        
        for (var w = 1; w <= 9; ++w)
        {
            for (var z = 0; z <= zSteps; ++z)
            {
                ResetRegisters();
                _registers['z'] = z;
                //PrintRegisters();

//                foreach (var action in _parts.Last())
                foreach (var action in stepArr)
                {
                    action.Invoke(w);
                    //PrintRegisters();
                }
                
                if (_registers['z'] == 0)
                    possibilities.Add(new ValueTuple<int, int, int>(w, z, targetZ));
            }
        }

        return possibilities;
    }

    private Action<int> ParseInstruction(string instruction)
    {
        if (!_regexHelper.Parse(instruction))
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