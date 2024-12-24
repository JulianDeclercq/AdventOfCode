using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day24
{
    private class Gate
    {
        public string InputWire { get; init; }
        public string InputWire2 { get; init; }
        public GateType Type { get; init; }
        public string OutputWire { get; init; }
        
        public override string ToString()
        {
            return $"{InputWire} {Type} {InputWire2} -> {OutputWire}";
        }
    }

    private enum GateType
    {
        None = 0,
        And = 1,
        Or = 2,
        Xor = 3
    }
    
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");

        var lines = File.ReadAllLines("input/day24e.txt").ToList();
        var wireDefinitions = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToList();
        var gateDefinitions = lines.Skip(wireDefinitions.Count + 1).ToList();

        Dictionary<string, bool?> wires = [];
        foreach (var wireDefinition in wireDefinitions)
        {
            var split = wireDefinition.Split(": ");
            wires.Add(split[0], split[1] == "1");
        }
        
        List<Gate> gates = [];
        var regex = new RegexHelper(
            new Regex(@"(\w+) (AND|OR|XOR) (\w+) -> (\w+)"),
            "input1", "type", "input2", "output");
        
        foreach (var gateDefinition in gateDefinitions)
        {
            regex.Match(gateDefinition);
            var gate = new Gate
            {
                InputWire = regex.Get("input1"),
                InputWire2 = regex.Get("input2"),
                Type = Enum.Parse<GateType>(regex.Get("type"), ignoreCase: true),
                OutputWire = regex.Get("output")
            };
            gates.Add(gate);
        }
    }
}