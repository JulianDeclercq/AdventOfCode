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
                _instructions = ParseInput(@"..\..\..\input\day7.txt");

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
            
            // TODO: merge this with NOT and check for not?
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

    private ushort? RetrieveSignal(string input)
    {
        if (ushort.TryParse(input, out var signal))
            return signal;

        if (_circuit.TryGetValue(input, out signal))
            return signal;

        return null;
    }

    private void ExecuteInstruction(Instruction instruction)
    {
        var lhs = RetrieveSignal(instruction.Lhs);
        var rhs = RetrieveSignal(instruction.Rhs);
        switch (instruction.Type)
        {
            case InstructionType.Assignment:
            {   
                if (lhs != null)
                    _circuit[instruction.Target] = lhs.Value;

                break;
            }
            case InstructionType.And:
            {
                if (lhs != null && rhs != null)
                    _circuit[instruction.Target] = (ushort)(lhs.Value & rhs.Value);
                
                break;
            }
            case InstructionType.Or:
            {
                if (lhs != null && rhs != null)
                    _circuit[instruction.Target] = (ushort)(lhs.Value | rhs.Value);
                
                break;
            }
            case InstructionType.Not:
            {
                if (lhs != null)
                    _circuit[instruction.Target] = (ushort)(~lhs.Value);

                break;
            }
            case InstructionType.LShift:
            {
                if (lhs != null && rhs != null)
                    _circuit[instruction.Target] = (ushort)(lhs.Value << rhs.Value);
                
                break;
            }
            case InstructionType.RShift:
            {
                if (lhs != null && rhs != null)
                    _circuit[instruction.Target] = (ushort)(lhs.Value >> rhs.Value);
                
                break;
            }
            default:
                Console.WriteLine($"Invalid instruction type: {instruction.Type}");
                break;
        }
    }

    private ushort FindWireValue(string identifier)
    {
        // keep executing until the wire with given identifier has a value
        for (;;)
        {
            foreach (var instruction in Instructions)
            {
                // check if the answer has been found
                if (_circuit.ContainsKey(identifier))
                    return _circuit[identifier];
                
                // if the value for this wire has already been found, skip the instruction
                if (_circuit.ContainsKey(instruction.Target))
                    continue;
                
                // execute the instruction
                ExecuteInstruction(instruction);
            }
        }
    }
    
    public void Part1()
    {
        Console.WriteLine($"Day 7 part 1: {FindWireValue("a")}");;
    }

    public void Part2()
    {
        var signalA = FindWireValue("a");
        
        _circuit.Clear();
        _circuit["b"] = signalA;

        Console.WriteLine($"Day 7 part 2: {FindWireValue("a")}");;
    }
}