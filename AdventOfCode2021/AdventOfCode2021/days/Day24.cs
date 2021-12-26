using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day24
{
    private readonly RegexHelper _regexHelper = new(new Regex(@"(\w+) (\w+) ?(-?\w+)?"), "instruction", "lhs", "rhs");
    private int _inputIndex = 0;
    private string _inputBuffer = string.Empty;
    private IEnumerable<Action> MONAD;
    private Dictionary<char, int> _registers = new() { {'x', 0}, {'y', 0}, {'z', 0}, {'w', 0} };
    
    public void Part1()
    {
        MONAD = File.ReadAllLines(@"..\..\..\input\day24.txt").Select(ParseInstruction);

        // TODO: Reverse engineer the program with restrictions so that Z has to be zero
        // per number (index in the final number and inp w in the instructions) you have to find the requirements by looking at the instructions that follows
        // once you figure out those requirements you ahve to form all numbers that meet all those requirements
        // for example th efirst number can only be even / divisable by 3 so your final number will need a number on index 0 that meets those reqs
        
        var ctr = 99999996548000; // 99999914260000
        var watch = new Stopwatch();
        watch.Start();
        do
        {
            ctr--;

            if (ctr % 10000 == 0)
                Console.WriteLine(ctr);

            var s = ctr.ToString();

            if (s.Contains('0'))
                continue;

            RunMONAD(s);
            //Console.WriteLine($"Registers after running");
            //PrintRegisters();

        } while (_registers['z'] != 0);// && ctr >= 99999999900000);
        watch.Stop();
        Console.WriteLine(watch.ElapsedMilliseconds / 1000);

        Console.WriteLine($"Day 24 part 1: {ctr}");
    }

    private void ProcessInstruction(string instruction, string inputBuffer)
    {
        if (!_regexHelper.Match(instruction))
            return;

        var instructionType = _regexHelper.Get("instruction");
        
        var key = _regexHelper.Get("lhs")[0];
        if (instructionType.Equals("inp"))
        {
            _registers[key] = int.Parse(inputBuffer[_inputIndex++].ToString()); // TODO: Check if i can use ++ here
            return;
        }
        
        if (!_regexHelper.TryGetInt("rhs", out var rhs))
            rhs = _registers[_regexHelper.Get("rhs")[0]];
        
        switch (instructionType)
        {
            case "add":
            {
                _registers[key] += rhs;
                break;
            }
            case "mul":
            {
                _registers[key] *= rhs;
                break;
            }
            case "div":
            {
                _registers[key] /= rhs; // TODO: check if this truncates correctly
                break;
            }
            case "mod":
            {
                _registers[key] %= rhs; // TODO: check if this gives correct modulo
                break;
            }
            case "eql":
            {
                _registers[key] = _registers[key] == rhs ? 1 : 0;         
                break;
            }
            default:
                throw new Exception($"Invalid instruction {instructionType}");
        }
    }

    private void INP(char register)
    {
        var value = int.Parse(_inputBuffer[_inputIndex++].ToString());
        _registers[register] = value;
    }

    private Action ParseInstruction(string instruction)
    {
        if (!_regexHelper.Match(instruction))
            return () => throw new Exception($"Instruction {instruction} didn't match pattern.");
        
        var instructionType = _regexHelper.Get("instruction");
        var key = _regexHelper.Get("lhs")[0];
        if (instructionType.Equals("inp"))
            return () => INP(key);
        
        if (!_regexHelper.TryGetInt("rhs", out var rhs))
            rhs = _registers[_regexHelper.Get("rhs")[0]];

        return instructionType switch
        {
            "add" => () => _registers[key] += rhs,
            "mul" => () => _registers[key] *= rhs,
            "div" => () => _registers[key] /= rhs, // TODO: check if this truncates correctly
            "mod" => () => _registers[key] %= rhs, // TODO: check if this gives correct modulo
            "eql" => () => _registers[key] = _registers[key] == rhs ? 1 : 0,
            _ => () => throw new Exception($"Invalid instruction type {instructionType}")
        };
    }

    private void RunMONAD(string input)
    {
        // reset
        _registers['x'] = 0;
        _registers['y'] = 0;
        _registers['z'] = 0;
        _registers['w'] = 0;
        _inputIndex = 0;
        _inputBuffer = input;

        foreach (var action in MONAD)
            action();
    }

    private void PrintRegisters()
    {
        foreach(var (name, value) in _registers) 
            Console.WriteLine($"Register {name} contains: {value}");   
    }
    
    
}