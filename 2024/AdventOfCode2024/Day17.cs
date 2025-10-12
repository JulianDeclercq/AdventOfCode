namespace AdventOfCode2024;

public class Day17(string inputFilePath)
{
    private class Computer
    {
        public List<int> Program = [];
        public int RegisterA = 0;
        public int RegisterB = 0;
        public int RegisterC = 0;
        public List<int> Output = [];
        private int _instructionPointer = 0;

        public void InitializeFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            RegisterA = int.Parse(lines[0].Split(':').Last().Trim());
            RegisterB = int.Parse(lines[1].Split(':').Last().Trim());
            RegisterC = int.Parse(lines[2].Split(':').Last().Trim());
            
            var temp = lines[4].Split(':').Last().Trim().Split(',').Select(int.Parse).ToList();
            Program = temp;
        }

        public void Run(bool part2)
        {
            var hasNext = false;
            do
            {
                hasNext = ExecuteNextInstruction(part2);
            } while (hasNext);
        }

        private bool ExecuteNextInstruction(bool part2)
        {
            if (_instructionPointer >= Program.Count)
            {
                // Console.WriteLine("Program halted, out of instructions!");
                return false;
            }
            
            var opcode = Program[_instructionPointer];
            var operand = Program[_instructionPointer + 1];
            var hasJumped = false;
            switch (opcode)
            {
                case 0:
                    Adv(operand);
                    break;
                case 1:
                    Bxl(operand);
                    break;
                case 2:
                    Bst(operand);
                    break;
                case 3:
                    hasJumped = Jnz(operand);
                    break;
                case 4:
                    Bxc(operand);
                    break;
                case 5:
                    Out(operand);
                    break;
                case 6: 
                    Bdv(operand);
                    break;
                case 7: 
                    Cdv(operand);
                    break;
                default:
                    throw new Exception("Unknown opcode: " + opcode);
            }

            // move the pointer
            if (!hasJumped)
                _instructionPointer += 2;
            
            return true;
        }

        private void Adv(int comboOperand)
        {
            RegisterA = SharedDv(comboOperand);
        }

        private void Bxl(int literalOperand)
        {
            // TODO: This might be completely wrong since im not ACTUALLY using 3 bit stuff but a whole integer instead
            RegisterB ^= literalOperand;
        }

        private void Bst(int comboOperand)
        {
            // TODO: i ignored thereby keeping only its lowest 3 bits, assuming it's just extra info. might have to change
            /* *The bst instruction (opcode 2) calculates the value of its combo operand modulo 8
             * (thereby keeping only its lowest 3 bits), then writes that value to the B register. */
            var result = GetComboOperandValue(comboOperand) % 8;
            RegisterB = result;
        }

        private bool Jnz(int literalOperand)
        {
            if (RegisterA == 0)
                return false;

            _instructionPointer = literalOperand;
            return true;
        }

        private void Bxc(int literalOperand)
        {
            /* * The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C,
             * then stores the result in register B.
             * (For legacy reasons, this instruction reads an operand but ignores it.) */
            RegisterB ^= RegisterC;
        }

        private void Out(int comboOperand)
        {
            /* * The out instruction (opcode 5) calculates the value of its combo operand modulo 8,
             * then outputs that value.
             * (If a program outputs multiple values, they are separated by commas.) */
            var value = GetComboOperandValue(comboOperand) % 8;
            Output.Add(value);
        }

        private void Bdv(int comboOperand)
        {
            RegisterB = SharedDv(comboOperand);
        }
        
        private void Cdv(int comboOperand)
        {
            RegisterC = SharedDv(comboOperand);
        }

        private int SharedDv(int comboOperand)
        {
            var numerator = RegisterA;
            var denominator = Math.Pow(2, GetComboOperandValue(comboOperand));
            return numerator / (int)denominator;
        }

        private int GetComboOperandValue(int comboOperand)
        {
            return comboOperand switch
            {
                0 or 1 or 2 or 3 => comboOperand,
                4 => RegisterA,
                5 => RegisterB,
                6 => RegisterC,
                _ => throw new ArgumentOutOfRangeException(nameof(comboOperand), comboOperand, null)
            };
        }

        public bool OutputsSelf()
        {
            return Program.SequenceEqual(Output); // TODO: Verify if this works
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
    
    public string Part1()
    {
        var computer = new Computer();
        computer.InitializeFromFile(inputFilePath);
        computer.Run(part2: false);
        Console.WriteLine("Program ran with output:");
        Console.WriteLine(computer.Output);
        
        return string.Join(",", computer.Output);
    }
    
    public int Part2()
    {
        for (var i = 0;; i++)
        {
            if (i % 100_000 == 0)
                Console.WriteLine($"loop: {i}");

            var computer = new Computer();
            computer.InitializeFromFile(inputFilePath);
            computer.RegisterA = i;
            // computer.RegisterA = 117440;

            computer.Run(part2: true);
            if (computer.OutputsSelf())
            {
                Console.WriteLine($"Program outputs self with register A: {i}");
                return i;
            }
        }
    }
}