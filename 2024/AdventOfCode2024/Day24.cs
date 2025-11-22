using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day24
{
    private class Gate
    {
        public string InputWire { get; set; }
        public string InputWire2 { get; set; }
        public GateType Type { get; set; }
        public string OutputWire { get; set; }
        
        public Gate Clone()
        {
            return new Gate
            {
                InputWire = InputWire,
                InputWire2 = InputWire2,
                Type = Type,
                OutputWire = OutputWire
            };
        }
        
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

        var lines = File.ReadAllLines("input/real/day24.txt").ToList();
        var wireDefinitions = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToList();
        var gateDefinitions = lines.Skip(wireDefinitions.Count + 1).ToList();

        Dictionary<string, int> initialWires = [];
        foreach (var wireDefinition in wireDefinitions)
        {
            var split = wireDefinition.Split(": ");
            initialWires.Add(split[0], (int)char.GetNumericValue(split[1][0]));
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
        
        if (part == 1)
        {
            var wires = new Dictionary<string, int>(initialWires);
        SimulateSystem(wires, gates);
        
        // making the number
        var zWires = wires.Where(wire => wire.Key.StartsWith('z')).ToList();
        ulong answer = 0;
        foreach (var wire in zWires)
        {
            var toShift = int.Parse(wire.Key[1..]); // parse the numbers after z
            var shifted = (ulong)wire.Value << toShift; // move the value into the right position
            answer |= shifted; // use an OR operation to put the value into the answer
        }

        Console.WriteLine(answer);
        }
        else
        {
            // Part 2: Bit-by-bit validation approach (much faster than brute force)
            var xWires = initialWires.Keys.Where(k => k.StartsWith('x')).OrderBy(k => int.Parse(k[1..])).ToList();
            var yWires = initialWires.Keys.Where(k => k.StartsWith('y')).OrderBy(k => int.Parse(k[1..])).ToList();
            var numBits = xWires.Count;
            
            // Create a mutable copy of gates for swapping
            var mutableGates = gates.Select(g => g.Clone()).ToList();
            
            // Create a map for quick lookup: output wire -> gate
            var gateByOutput = mutableGates.ToDictionary(g => g.OutputWire);
            
            var allSwappedWires = new List<string>();
            
            // Process each bit position independently
            for (int n = 0; n < numBits; n++)
            {
                if (ValidateBit(n, mutableGates, initialWires, numBits))
                    continue; // This bit is correct, skip
                
                // Fix this bit
                var swappedWires = FixBitN(n, mutableGates, gateByOutput, numBits);
                allSwappedWires.AddRange(swappedWires);
            }
            
            // Sort and output the wire names (matching Python: ",".join(sorted(part2)))
            var sortedWires = allSwappedWires.OrderBy(w => w).ToList();
            Console.WriteLine(string.Join(",", sortedWires));
        }
    }
    
    // Memoized gate dependency map (Phase 5 optimization)
    private static Dictionary<string, Dictionary<string, List<Gate>>> GateMapCache = new();
    
    private static Dictionary<string, List<Gate>> GetGateMap(List<Gate> gates)
    {
        // Use gates list as key (simple approach - could use hash if needed)
        var key = gates.Count.ToString(); // Simple key for now
        
        if (!GateMapCache.TryGetValue(key, out var cached))
        {
            cached = new Dictionary<string, List<Gate>>();
            foreach (var gate in gates)
            {
                if (!cached.ContainsKey(gate.InputWire))
                    cached[gate.InputWire] = new List<Gate>();
                if (!cached.ContainsKey(gate.InputWire2))
                    cached[gate.InputWire2] = new List<Gate>();
                
                cached[gate.InputWire].Add(gate);
                cached[gate.InputWire2].Add(gate);
            }
            GateMapCache[key] = cached;
        }
        
        return cached;
    }
    
    private static void SimulateSystem(Dictionary<string, int> wires, List<Gate> gates)
    {
        // Optimized: Use a queue-based approach with memoized gate map
        var readyGates = new Queue<Gate>();
        var gateMap = GetGateMap(gates); // Use cached gate map
        
        // Find initially ready gates
        foreach (var gate in gates)
        {
            if (!wires.ContainsKey(gate.OutputWire) && 
                wires.ContainsKey(gate.InputWire) && 
                wires.ContainsKey(gate.InputWire2))
            {
                readyGates.Enqueue(gate);
            }
        }
        
        // Process gates in order
        while (readyGates.Count > 0)
        {
            var gate = readyGates.Dequeue();
            
            // Skip if already processed
                if (wires.ContainsKey(gate.OutputWire))
                    continue;
                
            // Process gate
                var result = gate.Type switch
                {
                    GateType.And => wires[gate.InputWire] & wires[gate.InputWire2],
                    GateType.Or => wires[gate.InputWire] | wires[gate.InputWire2],
                    GateType.Xor => wires[gate.InputWire] ^ wires[gate.InputWire2],
                    _ => throw new ArgumentOutOfRangeException()
                };

                wires[gate.OutputWire] = result;
            
            // Check if any gates waiting on this output are now ready
            if (gateMap.TryGetValue(gate.OutputWire, out var dependentGates))
            {
                foreach (var dependentGate in dependentGates)
                {
                    if (!wires.ContainsKey(dependentGate.OutputWire) &&
                        wires.ContainsKey(dependentGate.InputWire) &&
                        wires.ContainsKey(dependentGate.InputWire2))
                    {
                        readyGates.Enqueue(dependentGate);
                    }
                }
            }
        }
    }
    
    private static bool ValidateBit(int n, List<Gate> gates, Dictionary<string, int> initialWires, int numBits)
    {
        // Test all combinations of x[n], y[n], and carry-in
        // Based on Python: init_x = [0] * (44 - n) + [x] + ([c] + [0] * (n - 1) if n > 0 else [])
        for (int x = 0; x <= 1; x++)
        {
            for (int y = 0; y <= 1; y++)
            {
                for (int carry = 0; carry <= 1; carry++)
                {
                    if (n == 0 && carry > 0)
                        continue; // Bit 0 has no carry-in
                    
                    // Set up input values
                    var wires = new Dictionary<string, int>(initialWires);
                    
                    // Set all bits after n to 0
                    for (int i = n + 1; i < numBits; i++)
                    {
                        wires[$"x{i:D2}"] = 0;
                        wires[$"y{i:D2}"] = 0;
                    }
                    
                    // Set bit n to x/y
                    wires[$"x{n:D2}"] = x;
                    wires[$"y{n:D2}"] = y;
                    
                    // Set bits before n for carry
                    if (n > 0)
                    {
                        // Set bit n-1 to carry value, all others to 0
                        wires[$"x{n - 1:D2}"] = carry;
                        wires[$"y{n - 1:D2}"] = carry;
                        
                        for (int i = 0; i < n - 1; i++)
                        {
                            wires[$"x{i:D2}"] = 0;
                            wires[$"y{i:D2}"] = 0;
                        }
                    }
                    
                    // Simulate
                    SimulateSystem(wires, gates);
                    
                    // Check if z[n] is correct
                    var zWire = $"z{n:D2}";
                    if (!wires.TryGetValue(zWire, out var actualZ))
                        return false;
                    
                    var expectedZ = (x + y + carry) % 2;
                    if (actualZ != expectedZ)
                        return false;
                }
            }
        }
        
        return true; // All combinations passed
    }
    
    private static List<string> FixBitN(int n, List<Gate> gates, Dictionary<string, Gate> gateByOutput, int numBits)
    {
        // Find gates using adder structure knowledge
        // zn = nxor XOR m1
        // nxor = xn XOR yn
        // m1 = m2 OR prevand
        // prevand = xn-1 AND yn-1
        // m2 = prevxor AND (something from previous bit)
        // prevxor = xn-1 XOR yn-1
        
        var expectedZWire = $"z{n:D2}";
        var swappedWires = new List<string>();
        
        // Find nxor gate: xn XOR yn
        var nxorGate = FindGate(gates, GateType.Xor, $"x{n:D2}", $"y{n:D2}");
        
        // Find prevand gate: xn-1 AND yn-1 (if n > 0)
        Gate? prevandGate = null;
        Gate? prevxorGate = null;
        Gate? m2Gate = null;
        Gate? m1Gate = null;
        
        if (n > 0)
        {
            prevandGate = FindGate(gates, GateType.And, $"x{n - 1:D2}", $"y{n - 1:D2}");
            prevxorGate = FindGate(gates, GateType.Xor, $"x{n - 1:D2}", $"y{n - 1:D2}");
            
            // Find m2: prevxor AND (something) - gate with AND op and prevxor.out as one input
            if (prevxorGate != null)
            {
                m2Gate = FindGate(gates, GateType.And, prevxorGate.OutputWire);
            }
            
            // Find m1: m2 OR prevand
            if (m2Gate != null && prevandGate != null)
            {
                m1Gate = FindGate(gates, GateType.Or, m2Gate.OutputWire, prevandGate.OutputWire);
            }
        }
        
        // Find zn gate: nxor XOR m1
        Gate? znGate = null;
        if (nxorGate != null && m1Gate != null)
        {
            znGate = FindGate(gates, GateType.Xor, nxorGate.OutputWire, m1Gate.OutputWire);
        }
        
        // Prepare expected inputs
        var expectedInputs = new HashSet<string>();
        if (nxorGate != null) expectedInputs.Add(nxorGate.OutputWire);
        if (m1Gate != null) expectedInputs.Add(m1Gate.OutputWire);
        
        List<string> toSwap = new List<string>();
        
        // If znGate is null, use the current gate outputting to z[n]
        if (znGate == null)
        {
            znGate = gateByOutput[expectedZWire];
            
            // Check if inputs are wrong - find what should be connected (symmetric difference)
            var actualInputs = new HashSet<string> { znGate.InputWire, znGate.InputWire2 };
            
            // Find the symmetric difference: wires that are in one set but not the other
            if (expectedInputs.Count > 0)
            {
                toSwap = expectedInputs.Except(actualInputs).Union(actualInputs.Except(expectedInputs)).ToList();
            }
        }
        
        // If znGate outputs to wrong wire, swap it with the gate currently outputting to z[n]
        // This is a separate check (not nested) - can overwrite toSwap from above
        if (znGate != null && znGate.OutputWire != expectedZWire)
        {
            toSwap = new List<string> { expectedZWire, znGate.OutputWire };
        }
        
        // Perform the swap if we have exactly 2 wires to swap
        if (toSwap.Count == 2)
        {
            var gate1 = gateByOutput[toSwap[0]];
            var gate2 = gateByOutput[toSwap[1]];
            SwapGateOutputs(gates, gateByOutput, gate1, gate2);
            return toSwap;
        }
        
        return swappedWires; // Return empty if no swap needed (shouldn't happen if bit is failing)
    }
    
    private static Gate? FindGate(List<Gate> gates, GateType op, string? in1 = null, string? in2 = null)
    {
        foreach (var gate in gates)
        {
            if (gate.Type != op)
                continue;
            
            if (in1 != null && in2 != null)
            {
                // Both inputs specified - must match both (order doesn't matter)
                if ((gate.InputWire == in1 && gate.InputWire2 == in2) ||
                    (gate.InputWire == in2 && gate.InputWire2 == in1))
                {
                    return gate;
                }
            }
            else if (in1 != null)
            {
                // Only in1 specified - must have in1 as one of the inputs
                if (gate.InputWire == in1 || gate.InputWire2 == in1)
                {
                    return gate;
                }
            }
            else
            {
                // No inputs specified - just match the operation
                return gate;
            }
        }
        
        return null;
    }
    
    private static void SwapGateOutputs(List<Gate> gates, Dictionary<string, Gate> gateByOutput, Gate gate1, Gate gate2)
    {
        // Swap the output wires of two gates
        var temp = gate1.OutputWire;
        
        // Update gate1's output
        gate1.OutputWire = gate2.OutputWire;
        
        // Update gate2's output
        gate2.OutputWire = temp;
        
        // Update the lookup dictionary
        gateByOutput[gate1.OutputWire] = gate1;
        gateByOutput[gate2.OutputWire] = gate2;
    }
    
    private static List<(Dictionary<string, int> xValues, Dictionary<string, int> yValues)> GenerateTestCases(int numBits)
    {
        var testCases = new List<(Dictionary<string, int>, Dictionary<string, int>)>();
        
        // Phase 5: Minimal test suite - only most critical cases
        // Edge cases (most important for catching errors)
        testCases.Add((GenerateXValues(numBits, 0), GenerateYValues(numBits, 0))); // 0 + 0
        var maxVal = (1UL << numBits) - 1;
        testCases.Add((GenerateXValues(numBits, maxVal), GenerateYValues(numBits, maxVal))); // max + max
        
        // Add one pattern case for diversity
        var mask = (1UL << Math.Min(16, numBits)) - 1;
        testCases.Add((GenerateXValues(numBits, 0b1010101010101010 & mask), GenerateYValues(numBits, 0b0101010101010101 & mask)));
        
        return testCases;
    }
    
    private static Dictionary<string, int> GenerateXValues(int numBits, ulong value)
    {
        var result = new Dictionary<string, int>();
        for (int i = 0; i < numBits; i++)
        {
            result[$"x{i:D2}"] = (int)((value >> i) & 1);
        }
        return result;
    }
    
    private static Dictionary<string, int> GenerateYValues(int numBits, ulong value)
    {
        var result = new Dictionary<string, int>();
        for (int i = 0; i < numBits; i++)
        {
            result[$"y{i:D2}"] = (int)((value >> i) & 1);
        }
        return result;
    }
    
    private static List<(string, string)> FindSwaps(
        List<Gate> originalGates,
        Dictionary<string, int> initialWires,
        List<string> xWires,
        List<string> yWires,
        List<(Dictionary<string, int> xValues, Dictionary<string, int> yValues)> testCases)
    {
        // Phase 1: Identify problem areas - find which z wires are wrong
        // Phase 5: Use minimal test cases (2-3) for faster identification
        var wrongZWires = IdentifyWrongZWires(originalGates, initialWires, xWires, yWires, testCases.Take(3).ToList());
        
        if (wrongZWires.Count == 0)
        {
            throw new Exception("No wrong z wires found - system might already be correct?");
        }
        
        // Phase 2: Build dependency graph and find candidate gates
        Console.WriteLine("Building dependency graph...");
        Console.Out.Flush();
        var candidateWires = FindCandidateWires(originalGates, wrongZWires);
        
        Console.WriteLine($"Found {wrongZWires.Count} wrong z wires, {candidateWires.Count} candidate gates");
        Console.Write("Grouping candidates...");
        Console.Out.Flush();
        
        // Phase 2.5: Group candidates by which wrong z wires they affect (for smarter prioritization)
        var candidateGroups = GroupCandidatesByAffectedZWires(candidateWires, originalGates, wrongZWires);
        
        Console.WriteLine(" Done.");
        Console.WriteLine("Searching for swaps...");
        Console.Write("Pair 1/4: Starting search..."); // Initial progress message
        Console.Out.Flush();
        
        // Phase 3: Constraint-based search on candidate gates only
        var progressCounter = new ProgressCounter();
        var cache = new SimulationCache();
        var result = FindSwapsRecursive(originalGates, initialWires, xWires, yWires, testCases, candidateWires, candidateGroups, new List<(string, string)>(), 0, progressCounter, cache);
        
        Console.WriteLine(); // New line after progress updates
        
        return result ?? throw new Exception("Could not find the 4 pairs of swaps");
    }
    
    private static HashSet<string> IdentifyWrongZWires(
        List<Gate> gates,
        Dictionary<string, int> initialWires,
        List<string> xWires,
        List<string> yWires,
        List<(Dictionary<string, int> xValues, Dictionary<string, int> yValues)> testCases)
    {
        var wrongZWires = new HashSet<string>();
        
        foreach (var testCase in testCases)
        {
            var wires = new Dictionary<string, int>(initialWires);
            
            // Set x and y values
            foreach (var kvp in testCase.xValues)
                wires[kvp.Key] = kvp.Value;
            foreach (var kvp in testCase.yValues)
                wires[kvp.Key] = kvp.Value;
            
            // Simulate with broken system
            SimulateSystem(wires, gates);
            
            // Calculate expected result
            ulong xValue = 0;
            ulong yValue = 0;
            for (int i = 0; i < xWires.Count; i++)
            {
                if (wires.TryGetValue(xWires[i], out var xBit))
                    xValue |= ((ulong)xBit) << i;
                if (wires.TryGetValue(yWires[i], out var yBit))
                    yValue |= ((ulong)yBit) << i;
            }
            
            ulong expectedSum = xValue + yValue;
            
            // Get actual z values
            var zWires = wires.Keys.Where(k => k.StartsWith('z')).OrderBy(k => int.Parse(k[1..])).ToList();
            ulong actualSum = 0;
            foreach (var zWire in zWires)
            {
                if (wires.TryGetValue(zWire, out var bit))
                {
                    var bitPos = int.Parse(zWire[1..]);
                    actualSum |= ((ulong)bit) << bitPos;
                }
            }
            
            // If wrong, find which z bits differ
            if (actualSum != expectedSum)
            {
                ulong diff = actualSum ^ expectedSum;
                for (int i = 0; i < zWires.Count; i++)
                {
                    if (((diff >> i) & 1) == 1)
                    {
                        wrongZWires.Add(zWires[i]);
                    }
                }
            }
        }
        
        return wrongZWires;
    }
    
    private static List<string> FindCandidateWires(List<Gate> gates, HashSet<string> wrongZWires)
    {
        // Build reverse dependency: trace back from wrong z wires to find all affecting gates
        // But limit depth to keep candidates manageable
        var affectingWires = new HashSet<string>();
        var visited = new HashSet<string>();
        
        // Optimization: Reduce depth from 5 to 3 for tighter candidate set
        foreach (var zWire in wrongZWires)
        {
            TraceBackDependenciesLimited(zWire, gates, affectingWires, visited, maxDepth: 3);
        }
        
        // Get all gate outputs that affect wrong z wires (exclude x/y inputs and z outputs)
        var candidateWires = affectingWires
            .Where(w => !w.StartsWith('x') && !w.StartsWith('y') && !w.StartsWith('z'))
            .Distinct()
            .ToList();
        
        // Prioritize by impact and limit to top candidates
        var impactCount = new Dictionary<string, int>();
        foreach (var candidate in candidateWires)
        {
            impactCount[candidate] = CountAffectedZWires(candidate, gates, wrongZWires);
        }
        
        // Take top 30 by impact (reduced from 40 for Phase 5)
        candidateWires = candidateWires
            .OrderByDescending(w => impactCount.GetValueOrDefault(w, 0))
            .Take(30)
            .ToList();
        
        return candidateWires;
    }
    
    private static void TraceBackDependenciesLimited(string wire, List<Gate> gates, HashSet<string> affectingWires, HashSet<string> visited, int maxDepth, int currentDepth = 0)
    {
        if (visited.Contains(wire) || currentDepth > maxDepth)
            return;
        
        visited.Add(wire);
        affectingWires.Add(wire);
        
        if (currentDepth >= maxDepth)
            return;
        
        // Find gates that output to this wire
        foreach (var gate in gates)
        {
            if (gate.OutputWire == wire)
            {
                // Trace back through inputs
                TraceBackDependenciesLimited(gate.InputWire, gates, affectingWires, visited, maxDepth, currentDepth + 1);
                TraceBackDependenciesLimited(gate.InputWire2, gates, affectingWires, visited, maxDepth, currentDepth + 1);
            }
        }
    }
    
    private static int CountAffectedZWires(string candidateWire, List<Gate> gates, HashSet<string> wrongZWires)
    {
        // Find all z wires that depend on this candidate wire
        var affectedZWires = new HashSet<string>();
        var visited = new HashSet<string>();
        
        ForwardTrace(candidateWire, gates, affectedZWires, visited);
        
        return affectedZWires.Count(w => wrongZWires.Contains(w));
    }
    
    private static void ForwardTrace(string wire, List<Gate> gates, HashSet<string> affectedZWires, HashSet<string> visited)
    {
        if (visited.Contains(wire))
            return;
        
        visited.Add(wire);
        
        if (wire.StartsWith('z'))
        {
            affectedZWires.Add(wire);
        }
        
        // Find gates that use this wire as input
        foreach (var gate in gates)
        {
            if (gate.InputWire == wire || gate.InputWire2 == wire)
            {
                ForwardTrace(gate.OutputWire, gates, affectedZWires, visited);
            }
        }
    }
    
    private class ProgressCounter
    {
        private int _count = 0;
        private int _lastDepth = 0;
        private readonly object _lock = new object();
        
        public void Increment(int depth)
        {
            lock (_lock)
            {
                _count++;
                
                // Update every 10k operations to avoid CPU overhead from console writes
                if (_count % 10000 == 0)
                {
                    _lastDepth = depth;
                    Console.Write($"\rPair {depth}/4: Tried {_count:N0} combinations...");
                    Console.Out.Flush(); // Force output to appear immediately
                }
            }
        }
        
        public void Reset()
        {
            lock (_lock)
            {
                _count = 0;
                _lastDepth = 0;
            }
        }
    }
    
    private static Dictionary<string, HashSet<string>> GroupCandidatesByAffectedZWires(
        List<string> candidateWires,
        List<Gate> gates,
        HashSet<string> wrongZWires)
    {
        var groups = new Dictionary<string, HashSet<string>>();
        
        // Optimize: Build a reverse lookup map once instead of tracing for each candidate
        var wireToAffectedZ = new Dictionary<string, HashSet<string>>();
        
        // Pre-compute which z wires each wire affects (limited depth to avoid explosion)
        foreach (var candidate in candidateWires)
        {
            if (!wireToAffectedZ.ContainsKey(candidate))
            {
                var affectedZWires = new HashSet<string>();
                var visited = new HashSet<string>();
                ForwardTraceLimited(candidate, gates, affectedZWires, visited, maxDepth: 10);
                wireToAffectedZ[candidate] = affectedZWires;
            }
        }
        
        // Group candidates
        foreach (var candidate in candidateWires)
        {
            var affectedZWires = wireToAffectedZ[candidate];
            var wrongAffected = affectedZWires.Where(z => wrongZWires.Contains(z)).ToList();
            
            if (wrongAffected.Count > 0)
            {
                // Create a key based on affected z wires (for grouping)
                var key = string.Join(",", wrongAffected.OrderBy(z => z));
                if (!groups.ContainsKey(key))
                    groups[key] = new HashSet<string>();
                groups[key].Add(candidate);
            }
        }
        
        return groups;
    }
    
    private static void ForwardTraceLimited(string wire, List<Gate> gates, HashSet<string> affectedZWires, HashSet<string> visited, int maxDepth, int currentDepth = 0)
    {
        if (visited.Contains(wire) || currentDepth > maxDepth)
            return;
        
        visited.Add(wire);
        
        if (wire.StartsWith('z'))
        {
            affectedZWires.Add(wire);
        }
        
        if (currentDepth >= maxDepth)
            return;
        
        // Find gates that use this wire as input
        foreach (var gate in gates)
        {
            if (gate.InputWire == wire || gate.InputWire2 == wire)
            {
                ForwardTraceLimited(gate.OutputWire, gates, affectedZWires, visited, maxDepth, currentDepth + 1);
            }
        }
    }
    
    private class SimulationCache
    {
        private readonly Dictionary<string, Dictionary<string, int>> _cache = new();
        private readonly object _lock = new object();
        
        public Dictionary<string, int>? Get(string key)
        {
            lock (_lock)
            {
                return _cache.TryGetValue(key, out var value) ? new Dictionary<string, int>(value) : null;
            }
        }
        
        public void Set(string key, Dictionary<string, int> wires)
        {
            lock (_lock)
            {
                if (!_cache.ContainsKey(key))
                {
                    _cache[key] = new Dictionary<string, int>(wires);
                }
            }
        }
        
        private string CreateKey(List<(string, string)> swaps, Dictionary<string, int> xValues, Dictionary<string, int> yValues)
        {
            // Optimize: Use StringBuilder and avoid multiple string allocations
            var sb = new System.Text.StringBuilder();
            
            // Swap key
            if (swaps.Count > 0)
            {
                var sortedSwaps = swaps.OrderBy(s => s.Item1).ThenBy(s => s.Item2).ToList();
                for (int i = 0; i < sortedSwaps.Count; i++)
                {
                    if (i > 0) sb.Append('|');
                    sb.Append(sortedSwaps[i].Item1).Append(',').Append(sortedSwaps[i].Item2);
                }
            }
            sb.Append('|');
            
            // X values key
            var sortedX = xValues.OrderBy(kv => kv.Key).ToList();
            for (int i = 0; i < sortedX.Count; i++)
            {
                if (i > 0) sb.Append(',');
                sb.Append(sortedX[i].Key).Append(':').Append(sortedX[i].Value);
            }
            sb.Append('|');
            
            // Y values key
            var sortedY = yValues.OrderBy(kv => kv.Key).ToList();
            for (int i = 0; i < sortedY.Count; i++)
            {
                if (i > 0) sb.Append(',');
                sb.Append(sortedY[i].Key).Append(':').Append(sortedY[i].Value);
            }
            
            return sb.ToString();
        }
        
        public Dictionary<string, int>? GetCached(List<(string, string)> swaps, Dictionary<string, int> xValues, Dictionary<string, int> yValues)
        {
            return Get(CreateKey(swaps, xValues, yValues));
        }
        
        public void SetCached(List<(string, string)> swaps, Dictionary<string, int> xValues, Dictionary<string, int> yValues, Dictionary<string, int> wires)
        {
            Set(CreateKey(swaps, xValues, yValues), wires);
        }
    }
    
    private static List<(string, string)> FindSwapsRecursive(
        List<Gate> originalGates,
        Dictionary<string, int> initialWires,
        List<string> xWires,
        List<string> yWires,
        List<(Dictionary<string, int> xValues, Dictionary<string, int> yValues)> testCases,
        List<string> outputWires,
        Dictionary<string, HashSet<string>> candidateGroups,
        List<(string, string)> currentSwaps,
        int startIndex,
        ProgressCounter progressCounter,
        SimulationCache cache)
    {
        int currentDepth = currentSwaps.Count + 1;
        
        // Incremental validation: test after each pair is added (not just at the end)
        // Phase 5: Only test when we have 3+ pairs to reduce overhead further
        if (currentSwaps.Count >= 3)
        {
            // Use only 1 test case for faster validation
            if (!TestConfigurationQuick(originalGates, initialWires, xWires, yWires, testCases.Take(1).ToList(), currentSwaps, cache))
            {
                return null; // Prune early if this partial solution doesn't help
            }
        }
        
        if (currentSwaps.Count == 4)
        {
            // Test if this configuration works with all test cases
            if (TestConfiguration(originalGates, initialWires, xWires, yWires, testCases, currentSwaps))
            {
                return currentSwaps;
            }
            return null;
        }
        
        // Try all pairs of output wires with smarter prioritization
        // First try pairs from the same group (affect same z wires), then others
        var prioritizedPairs = GetPrioritizedPairs(outputWires, candidateGroups, currentSwaps, startIndex);
        
        foreach (var (wire1, wire2) in prioritizedPairs)
        {
            progressCounter.Increment(currentDepth);
            
            var newSwaps = new List<(string, string)>(currentSwaps) { (wire1, wire2) };
            
            // Skip DoesPairHelp check entirely - it's adding overhead and the quick test will catch bad pairs
            
            var result = FindSwapsRecursive(originalGates, initialWires, xWires, yWires, testCases, outputWires, candidateGroups, newSwaps, outputWires.IndexOf(wire1) + 1, progressCounter, cache);
            if (result != null)
                return result;
        }
        
        return null;
    }
    
    private static List<(string, string)> GetPrioritizedPairs(
        List<string> outputWires,
        Dictionary<string, HashSet<string>> candidateGroups,
        List<(string, string)> currentSwaps,
        int startIndex)
    {
        var pairs = new List<(string, string)>();
        var usedWires = new HashSet<string>(currentSwaps.SelectMany(s => new[] { s.Item1, s.Item2 }));
        
        // Phase 5: Heuristic scoring - prioritize pairs that affect different wrong z wires
        var pairScores = new Dictionary<(string, string), int>();
        
        // First: pairs from same group (high priority)
        foreach (var group in candidateGroups.Values)
        {
            var groupWires = group.Where(w => outputWires.Contains(w) && !usedWires.Contains(w)).ToList();
            for (int i = 0; i < groupWires.Count; i++)
            {
                for (int j = i + 1; j < groupWires.Count; j++)
                {
                    var idx1 = outputWires.IndexOf(groupWires[i]);
                    var idx2 = outputWires.IndexOf(groupWires[j]);
                    if (idx1 >= startIndex && idx2 > idx1)
                    {
                        var pair = (groupWires[i], groupWires[j]);
                        pairs.Add(pair);
                        pairScores[pair] = 100; // High score for same-group pairs
                    }
                }
            }
        }
        
        // Then: all other pairs with scoring
        for (int i = startIndex; i < outputWires.Count; i++)
        {
            for (int j = i + 1; j < outputWires.Count; j++)
            {
                var wire1 = outputWires[i];
                var wire2 = outputWires[j];
                
                if (!usedWires.Contains(wire1) && !usedWires.Contains(wire2))
                {
                    // Check if already added from same group
                    bool alreadyAdded = pairs.Any(p => 
                        (p.Item1 == wire1 && p.Item2 == wire2) || 
                        (p.Item1 == wire2 && p.Item2 == wire1));
                    
                    if (!alreadyAdded)
                    {
                        var pair = (wire1, wire2);
                        pairs.Add(pair);
                        // Lower score for cross-group pairs
                        pairScores[pair] = 50;
                    }
                }
            }
        }
        
        // Sort by score (higher first), then by wire names for consistency
        return pairs.OrderByDescending(p => pairScores.GetValueOrDefault(p, 0))
                    .ThenBy(p => p.Item1)
                    .ThenBy(p => p.Item2)
                    .ToList();
    }
    
    private static (bool correct, ulong actual, ulong expected) TestSingleCase(
        List<Gate> originalGates,
        Dictionary<string, int> initialWires,
        List<string> xWires,
        List<string> yWires,
        (Dictionary<string, int> xValues, Dictionary<string, int> yValues) testCase,
        List<(string, string)> swaps,
        SimulationCache cache)
    {
        // Check cache first
        var cached = cache.GetCached(swaps, testCase.xValues, testCase.yValues);
        Dictionary<string, int> wires;
        
        if (cached != null)
        {
            wires = cached;
        }
        else
        {
            wires = new Dictionary<string, int>(initialWires);
            foreach (var kvp in testCase.xValues)
                wires[kvp.Key] = kvp.Value;
            foreach (var kvp in testCase.yValues)
                wires[kvp.Key] = kvp.Value;
            
            var swappedGates = ApplySwaps(originalGates, swaps);
            SimulateSystem(wires, swappedGates);
            cache.SetCached(swaps, testCase.xValues, testCase.yValues, wires);
        }
        
        ulong xValue = 0;
        ulong yValue = 0;
        for (int i = 0; i < xWires.Count; i++)
        {
            if (wires.TryGetValue(xWires[i], out var xBit))
                xValue |= ((ulong)xBit) << i;
            if (wires.TryGetValue(yWires[i], out var yBit))
                yValue |= ((ulong)yBit) << i;
        }
        
        ulong expectedSum = xValue + yValue;
        
        var zWires = wires.Keys.Where(k => k.StartsWith('z')).OrderBy(k => int.Parse(k[1..])).ToList();
        ulong actualSum = 0;
        foreach (var zWire in zWires)
        {
            if (wires.TryGetValue(zWire, out var bit))
            {
                var bitPos = int.Parse(zWire[1..]);
                actualSum |= ((ulong)bit) << bitPos;
            }
        }
        
        return (actualSum == expectedSum, actualSum, expectedSum);
    }
    
    private static bool TestConfigurationQuick(
        List<Gate> originalGates,
        Dictionary<string, int> initialWires,
        List<string> xWires,
        List<string> yWires,
        List<(Dictionary<string, int> xValues, Dictionary<string, int> yValues)> testCases,
        List<(string, string)> swaps,
        SimulationCache cache)
    {
        // Quick test with reduced test cases (2 instead of 3) for faster pruning
        foreach (var testCase in testCases)
        {
            var result = TestSingleCase(originalGates, initialWires, xWires, yWires, testCase, swaps, cache);
            if (!result.correct)
                return false;
        }
        
        return true;
    }
    
    private static bool TestConfiguration(
        List<Gate> originalGates,
        Dictionary<string, int> initialWires,
        List<string> xWires,
        List<string> yWires,
        List<(Dictionary<string, int> xValues, Dictionary<string, int> yValues)> testCases,
        List<(string, string)> swaps)
    {
        // Create swapped gates
        var swappedGates = ApplySwaps(originalGates, swaps);
        
        // Test each test case - use a subset for faster filtering
        var testSubset = testCases.Take(Math.Min(20, testCases.Count)).ToList();
        
        foreach (var testCase in testSubset)
        {
            var wires = new Dictionary<string, int>(initialWires);
            
            // Set x and y values for this test case
            foreach (var kvp in testCase.xValues)
                wires[kvp.Key] = kvp.Value;
            foreach (var kvp in testCase.yValues)
                wires[kvp.Key] = kvp.Value;
            
            // Simulate
            SimulateSystem(wires, swappedGates);
            
            // Calculate expected result
            ulong xValue = 0;
            ulong yValue = 0;
            for (int i = 0; i < xWires.Count; i++)
            {
                if (wires.TryGetValue(xWires[i], out var xBit))
                    xValue |= ((ulong)xBit) << i;
                if (wires.TryGetValue(yWires[i], out var yBit))
                    yValue |= ((ulong)yBit) << i;
            }
            
            ulong expectedSum = xValue + yValue;
            
            // Get actual z values
            var zWires = wires.Keys.Where(k => k.StartsWith('z')).OrderBy(k => int.Parse(k[1..])).ToList();
            ulong actualSum = 0;
            foreach (var zWire in zWires)
            {
                if (wires.TryGetValue(zWire, out var bit))
                {
                    var bitPos = int.Parse(zWire[1..]);
                    actualSum |= ((ulong)bit) << bitPos;
                }
            }
            
            if (actualSum != expectedSum)
                return false;
        }
        
        // If passed subset, test all cases
        foreach (var testCase in testCases.Skip(testSubset.Count))
        {
            var wires = new Dictionary<string, int>(initialWires);
            
            foreach (var kvp in testCase.xValues)
                wires[kvp.Key] = kvp.Value;
            foreach (var kvp in testCase.yValues)
                wires[kvp.Key] = kvp.Value;
            
            SimulateSystem(wires, swappedGates);
            
            ulong xValue = 0;
            ulong yValue = 0;
            for (int i = 0; i < xWires.Count; i++)
            {
                if (wires.TryGetValue(xWires[i], out var xBit))
                    xValue |= ((ulong)xBit) << i;
                if (wires.TryGetValue(yWires[i], out var yBit))
                    yValue |= ((ulong)yBit) << i;
            }
            
            ulong expectedSum = xValue + yValue;
            
            var zWires = wires.Keys.Where(k => k.StartsWith('z')).OrderBy(k => int.Parse(k[1..])).ToList();
            ulong actualSum = 0;
            foreach (var zWire in zWires)
            {
                if (wires.TryGetValue(zWire, out var bit))
                {
                    var bitPos = int.Parse(zWire[1..]);
                    actualSum |= ((ulong)bit) << bitPos;
                }
            }
            
            if (actualSum != expectedSum)
                return false;
        }
        
        return true;
    }
    
    // Cache for swapped gates to avoid recreating them
    private static readonly Dictionary<string, List<Gate>> SwappedGatesCache = new();
    
    private static List<Gate> ApplySwaps(List<Gate> originalGates, List<(string, string)> swaps)
    {
        // Create cache key
        var cacheKey = string.Join("|", swaps.OrderBy(s => s.Item1).ThenBy(s => s.Item2).Select(s => $"{s.Item1},{s.Item2}"));
        
        if (SwappedGatesCache.TryGetValue(cacheKey, out var cached))
        {
            return cached;
        }
        
        // Create a mapping of old wire -> new wire
        var wireMap = new Dictionary<string, string>();
        foreach (var swap in swaps)
        {
            wireMap[swap.Item1] = swap.Item2;
            wireMap[swap.Item2] = swap.Item1;
        }
        
        // Create new gates with swapped outputs
        var swappedGates = new List<Gate>(originalGates.Count);
        foreach (var gate in originalGates)
        {
            var newOutputWire = wireMap.TryGetValue(gate.OutputWire, out var swapped) ? swapped : gate.OutputWire;
            swappedGates.Add(new Gate
            {
                InputWire = gate.InputWire,
                InputWire2 = gate.InputWire2,
                Type = gate.Type,
                OutputWire = newOutputWire
            });
        }
        
        SwappedGatesCache[cacheKey] = swappedGates;
        return swappedGates;
    }
}