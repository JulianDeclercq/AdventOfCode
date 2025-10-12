namespace AdventOfCode2024;

public class Day17(string inputFilePath)
{
    private class Computer
    {
        public List<int> Program = [];
        public int RegisterA = 0;
        public int RegisterB = 0;
        public int RegisterC = 0;
        public int InstructionPointer = 0;
        
        public override string ToString()
        {
            var programStr = string.Join(",", Program);
            var marker = InstructionPointer >= 0 && InstructionPointer < Program.Count 
                ? $" (at position {InstructionPointer}: {Program[InstructionPointer]})" 
                : " (out of bounds)";
            
            return $"""
                Computer State:
                ---------------
                Register A: {RegisterA}
                Register B: {RegisterB}
                Register C: {RegisterC}
                Instruction Pointer: {InstructionPointer}{marker}
                Program: [{programStr}]
                """;
        }
    }
    
    public void Solve()
    {
        var computer = new Computer();
        
        var lines = File.ReadAllLines(inputFilePath);
        computer.RegisterA = int.Parse(lines[0].Split(':').Last().Trim());
        computer.RegisterB = int.Parse(lines[1].Split(':').Last().Trim());
        computer.RegisterC = int.Parse(lines[2].Split(':').Last().Trim());
        computer.Program = lines[4].Split(':').Last().Trim().Split(',').Select(int.Parse).ToList();
        
        Console.WriteLine(computer);
    }
}