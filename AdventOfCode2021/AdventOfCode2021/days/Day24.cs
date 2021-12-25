using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day24
{
    private readonly RegexHelper _regexHelper = new(new Regex(@"(\w+) (\w+) ?(-?\w+)?"), "instruction", "lhs", "rhs");
    private int _inputIndex = 0;
    private string[] MONAD;
    private Dictionary<char, int> _registers = new() { {'x', 0}, {'y', 0}, {'z', 0}, {'w', 0} };
    
    public void Part1()
    {
        MONAD = File.ReadAllLines(@"..\..\..\input\day24.txt");
        var ctr = 100000000000000; // 99999996548000
        var watch = new Stopwatch();
        watch.Start();
        do
        {
            ctr--;
            
            if(ctr % 1000 == 0)
                Console.WriteLine(ctr);
            
            var s = ctr.ToString();

            if (s.Contains('0'))
                continue;
            
            RunMONAD(s);
            //Console.WriteLine($"Registers after running");
            //PrintRegisters();
            
        } while (_registers['z'] != 0 && ctr >= 99999999900000);
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

    private void RunMONAD(string input)
    {
        // reset
        _registers['x'] = 0;
        _registers['y'] = 0;
        _registers['z'] = 0;
        _registers['w'] = 0;
        _inputIndex = 0;

        foreach (var instruction in MONAD)
            ProcessInstruction(instruction, input);
    }

    private void PrintRegisters()
    {
        foreach(var (name, value) in _registers) 
            Console.WriteLine($"Register {name} contains: {value}");   
    }
    
    
}