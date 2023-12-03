using System.Text.RegularExpressions;

namespace AdventOfCode2015.Days;

public class Day7
{
    private enum InstructionType
    {
        Invalid = 0,
        Assignment = 1,
        And = 2,
        Or = 3,
        Not = 4,
        LShift = 5,
        RShift = 6
    }
    
    private class Instruction
    {
        public InstructionType Type = InstructionType.Invalid;
        public string Lhs = string.Empty;
        public string Rhs = string.Empty;
        public string Target = string.Empty;
    }
    
    private List<Instruction> _instructions = new();
    private List<Instruction> Instructions
    {
        get
        {
            if (_instructions.Count == 0)
                _instructions = ParseInput(@"..\..\..\input\Day7.txt");

            return _instructions;
        }
    }
    
    // key: identifier, value: signal
    private Dictionary<string, ushort> _circuit = new();

    private static List<Instruction> ParseInput(string path)
    {
        List<Instruction> instructions = new();
        var input = File.ReadAllLines(path);
        foreach (var line in input)
        {
            var instruction = new Instruction();

            var andRegex = new Regex(@"(\w+) (AND|OR|LSHIFT|RSHIFT) (\w+) -> (\w+)");
            var match = andRegex.Match(line);
            if (match.Success)
            {
                switch (match.Groups[2].ToString())
                {
                    case "AND":
                        instruction.Type = InstructionType.And;
                        break;
                    
                    case "OR":
                        instruction.Type = InstructionType.Or;
                        break;
                    
                    case "LSHIFT":
                        instruction.Type = InstructionType.LShift;
                        break;
                    
                    case "RSHIFT":
                        instruction.Type = InstructionType.RShift;
                        break;
                    
                    default:
                        Console.WriteLine($"Invalid instruction {match.Groups[2]}");
                        break;
                }
                
                instruction.Lhs = match.Groups[1].ToString();
                instruction.Rhs = match.Groups[3].ToString();
                instruction.Target = match.Groups[4].ToString();
                instructions.Add(instruction);
                continue;
            }
            
            var notRegex = new Regex(@"NOT (\w+) -> (\w+)");
            match = notRegex.Match(line);
            if (match.Success)
            {
                instructions.Add(new Instruction()
                {
                    Type = InstructionType.Not,
                    Lhs = match.Groups[1].ToString(),
                    Target = match.Groups[2].ToString()
                });
                continue;
            }
            
            var assignmentRegex = new Regex(@"(\w+) -> (\w+)");
            match = assignmentRegex.Match(line);
            if (match.Success)
            {
                instructions.Add(new Instruction()
                {
                    Type = InstructionType.Assignment,
                    Lhs = match.Groups[1].ToString(),
                    Target = match.Groups[2].ToString()
                });
                continue;
            }
            Console.WriteLine($"Invalid input {line}"); 
        }
        return instructions;
    }

    private void ExecuteInstruction(Instruction instruction)
    {
        var lhs = GetWireValue(instruction.Lhs);
        var rhs = GetWireValue(instruction.Rhs);
        switch (instruction.Type)
        {
            case InstructionType.Assignment:
                _circuit[instruction.Target] = lhs;
                break;
            case InstructionType.And:
                _circuit[instruction.Target] = (ushort)(lhs & rhs);
                break;
            case InstructionType.Or:
                _circuit[instruction.Target] = (ushort)(lhs | rhs);
                break;
            case InstructionType.Not:
                _circuit[instruction.Target] = (ushort)(~lhs);
                break;
            case InstructionType.LShift:
                _circuit[instruction.Target] = (ushort)(lhs << rhs);
                break;
            case InstructionType.RShift:
                _circuit[instruction.Target] = (ushort)(lhs >> rhs);
                break;
            default:
                Console.WriteLine($"Invalid instruction type: {instruction.Type}");
                break;
        }
    }

    private ushort GetWireValue(string identifier)
    {
        // ignore empty identifiers (e.g. "rhs" for assignment instructions)
        if (string.IsNullOrEmpty(identifier))
            return ushort.MinValue;
        
        // return the value if it exists
        if (_circuit.ContainsKey(identifier))
            return _circuit[identifier];

        // check if the value input is a number rather than a wire identifier 
        if (ushort.TryParse(identifier, out var signal))
            return signal;
        
        // if the wire value doesn't exist yet, calculate it by executing the corresponding instruction
        var instruction = Instructions.Single(i => i.Target.Equals(identifier));
        
        // recursive: ExecuteInstruction calls GetWireValue()
        ExecuteInstruction(instruction);
        
        // return the wire value as it is now guaranteed to exist
        return _circuit[identifier];
    }
    
    public void Part1()
    {
        Console.WriteLine($"Day 7 part 1: {GetWireValue("a")}");;
    }

    public void Part2()
    {
        var signalA = GetWireValue("a");
        
        _circuit.Clear();
        _circuit["b"] = signalA;

        Console.WriteLine($"Day 7 part 2: {GetWireValue("a")}");;
    }
}