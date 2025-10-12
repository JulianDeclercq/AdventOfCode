namespace AdventOfCode2024;

public class Day17(string inputFilePath)
{
    private class Computer
    {
        public List<int> Program = [];
        public int RegisterA = 0;
        public int RegisterB = 0;
        public int RegisterC = 0;
        private int _instructionPointer = 0;

        public void ReadNextInstruction()
        {
            if (_instructionPointer >= Program.Count)
            {
                Console.WriteLine("Program halted, out of instructions!");
                return;
            }
            
            var opcode = Program[_instructionPointer];
            var operand = Program[_instructionPointer + 1];
            switch (opcode)
            {
                case 0:
                    Adv(operand);
                    break;
                default:
                    throw new Exception("Unknown opcode: " + opcode);
            }

            // TODO: don't do this on a jump instruction?
            // move the pointer
            _instructionPointer += 2;
        }

        private void Adv(int comboOperand)
        {
            var numerator = RegisterA;
            var denominator = Math.Pow(2, GetComboOperandValue(comboOperand));
            var result = numerator / (int)denominator;
            RegisterA = result;
        }

        private void Bxl(int literalOperand)
        {
        }

        private int GetComboOperandValue(int comboOperand)
        {
            // Combo operands 0 through 3 represent literal values 0 through 3.
            // Combo operand 4 represents the value of register A.
            // Combo operand 5 represents the value of register B.
            // Combo operand 6 represents the value of register C.
            // Combo operand 7 is reserved and will not appear in valid programs.
            return comboOperand switch
            {
                0 or 1 or 2 or 3 => comboOperand,
                4 => RegisterA,
                5 => RegisterB,
                6 => RegisterC,
                _ => throw new ArgumentOutOfRangeException(nameof(comboOperand), comboOperand, null)
            };
        }
        public override string ToString()
        {
            var programStr = string.Join(",", Program);
            var marker = _instructionPointer >= 0 && _instructionPointer < Program.Count 
                ? $" (at position {_instructionPointer}: {Program[_instructionPointer]})" 
                : " (out of bounds)";
            
            return $"""
                Computer State:
                ---------------
                Register A: {RegisterA}
                Register B: {RegisterB}
                Register C: {RegisterC}
                Instruction Pointer: {_instructionPointer}{marker}
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