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

        // 568358894 is too low
        var lines = File.ReadAllLines("input/day24e2.txt").ToList();
        var wireDefinitions = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToList();
        var gateDefinitions = lines.Skip(wireDefinitions.Count + 1).ToList();

        Dictionary<string, int> wires = [];
        foreach (var wireDefinition in wireDefinitions)
        {
            var split = wireDefinition.Split(": ");
            wires.Add(split[0], (int)char.GetNumericValue(split[1][0]));
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
        
        SimulateSystem(wires, gates);
        
        // making the number
        var zWires = wires.Where(wire => wire.Key.StartsWith('z')).ToList();
        var answer = 0b_0000_0000_0000_0000_0000_0000_0000_0000;
        foreach (var wire in zWires)
        {
            var toShift = int.Parse(wire.Key[1..]); // parse the numbers after z
            // var kek = answer << shift;
            var shifted = wire.Value << toShift;
            answer |= shifted;
            // answer <<= shift;
            // Console.WriteLine(Convert.ToString(answer, 2));
        }

        Console.WriteLine(answer);
    }
    
    private static void SimulateSystem(Dictionary<string, int> wires, List<Gate> gates)
    {
        for (;;)
        {
            // TODO: verify this
            if (gates.All(gate => wires.ContainsKey(gate.OutputWire)))
                break;
            
            foreach (var gate in gates)
            {
                // already processed
                if (wires.ContainsKey(gate.OutputWire))
                    continue;
                
                // gates wait until both inputs are received before producing output
                if (!wires.ContainsKey(gate.InputWire) || !wires.ContainsKey(gate.InputWire2))
                    continue;

                // process
                var result = gate.Type switch
                {
                    GateType.And => wires[gate.InputWire] & wires[gate.InputWire2],
                    GateType.Or => wires[gate.InputWire] | wires[gate.InputWire2],
                    GateType.Xor => wires[gate.InputWire] ^ wires[gate.InputWire2],
                    _ => throw new ArgumentOutOfRangeException()
                };

                wires[gate.OutputWire] = result;
            }
        }
    }
}