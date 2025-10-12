using System.Numerics;

namespace AdventOfCode2024;

public class Day17(string inputFilePath)
{
    private class Computer
    {
        private List<int> _program = [];
        public int RegisterA = 0;
        private int _registerB = 0;
        private int _registerC = 0;
        public readonly List<int> Output = [];
        private int _instructionPointer = 0;

        private int _originalA = 0;
        private int _originalB = 0;
        private int _originalC = 0;

        public void Reset()
        {
            RegisterA = _originalA;
            _registerB = _originalB;
            _registerC = _originalC;
            _instructionPointer = 0;
            Output.Clear();
        }

        public string OutputAsString()
        {
            return string.Join(",", Output);
        }
        
        private BigInteger StringToBigInteger(string str)
        {
            BigInteger result = 0;
            foreach (var c in str)
            {
                if (c != '0' && c != '1')
                    throw new FormatException("Non-binary character found.");

                result <<= 1;
                result += c - '0'; // '0'->0, '1'->1
            }

            return result;
        }
    
        public void BuildPossibilities()
        {
            List<string> threeBitNumbers = // 0 to 7 in 3 bit representation
            [
                "000", // 0 
                "001", // 1
                "010", // 2
                "011", // 3
                "100", // 4
                "101", // 5
                "110", // 6
                "111", // 7
            ];
            
            // cartesian product with repetition
            IEnumerable<string> Sequences(IReadOnlyList<string> alphabet, int length)
            {
                if (length == 0) { yield return ""; yield break; }

                var idx = new int[length]; // base-N odometer
                while (true)
                {
                    yield return string.Join(" ", Enumerable.Range(0, length).Select(p => alphabet[idx[p]]));

                    var pos = length - 1;
                    while (pos >= 0 && ++idx[pos] == alphabet.Count) { idx[pos] = 0; pos--; }
                    if (pos < 0) yield break;
                }
            }

            foreach (var possibility in Sequences(threeBitNumbers, _program.Count))
            {
                Console.WriteLine(possibility);
                var bigInteger = StringToBigInteger(possibility); // TODO: try this for reg A
            }
        }

        public void InitializeFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            RegisterA = int.Parse(lines[0].Split(':').Last().Trim());
            _registerB = int.Parse(lines[1].Split(':').Last().Trim());
            _registerC = int.Parse(lines[2].Split(':').Last().Trim());
            _program = lines[4].Split(':').Last().Trim().Split(',').Select(int.Parse).ToList();
        }

        public void Run(bool part2)
        {
            _originalA = RegisterA;
            _originalB = _registerB;
            _originalC = _registerC;
            
            var hasNext = false;
            do
            {
                hasNext = ExecuteNextInstruction(part2);
            } while (hasNext);
        }

        private bool ExecuteNextInstruction(bool part2)
        {
            if (_instructionPointer >= _program.Count)
                return false;
            
            var opcode = _program[_instructionPointer];
            var operand = _program[_instructionPointer + 1];
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
                    
                    if (part2)
                    {
                        // TODO: More performant check?
                        if (Output.Last() != _program[Output.Count - 1])
                            return false;
                    }
            
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
            _registerB ^= literalOperand;
        }

        private void Bst(int comboOperand)
        {
            // TODO: i ignored thereby keeping only its lowest 3 bits, assuming it's just extra info. might have to change
            /* *The bst instruction (opcode 2) calculates the value of its combo operand modulo 8
             * (thereby keeping only its lowest 3 bits), then writes that value to the B register. */
            var result = GetComboOperandValue(comboOperand) % 8;
            _registerB = result;
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
            _registerB ^= _registerC;
        }

        private int Out(int comboOperand, bool performant = false)
        {
            /* * The out instruction (opcode 5) calculates the value of its combo operand modulo 8,
             * then outputs that value.
             * (If a program outputs multiple values, they are separated by commas.) */
            var value = GetComboOperandValue(comboOperand) % 8;
            if (!performant)
                Output.Add(value);
            
            return value;
        }

        private void Bdv(int comboOperand)
        {
            _registerB = SharedDv(comboOperand);
        }
        
        private void Cdv(int comboOperand)
        {
            _registerC = SharedDv(comboOperand);
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
                5 => _registerB,
                6 => _registerC,
                _ => throw new ArgumentOutOfRangeException(nameof(comboOperand), comboOperand, null)
            };
        }

        public bool OutputsSelf()
        {
            return _program.SequenceEqual(Output); // TODO: Verify if this works
        }
        
        public override string ToString()
        {
            var programStr = string.Join(",", _program);
            var marker = _instructionPointer >= 0 && _instructionPointer < _program.Count 
                ? $" (at position {_instructionPointer}: {_program[_instructionPointer]})" 
                : " (out of bounds)";
            
            return $"""
                Computer State:
                ---------------
                Register A: {RegisterA}
                Register B: {_registerB}
                Register C: {_registerC}
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
        var computer = new Computer();
        computer.InitializeFromFile(inputFilePath);

        const int test = 7;
        for (var i = 0;; i++)
        {
            if (i % 10_000_000 == 0)
                Console.WriteLine($"loop: {i}");

            computer.Reset();
            computer.RegisterA = i;
            computer.RegisterA = 7;
            computer.Run(part2: true);
            Console.WriteLine($"{test}: {computer.OutputAsString()}");
            if (computer.OutputsSelf())
            {
                Console.WriteLine($"Program outputs self with register A: {i}");
                return i;
            }

            break;
        }

        throw new Exception("oops");
    }

    public string TestRegisterA(int registerA)
    {
        var computer = new Computer();
        computer.InitializeFromFile(inputFilePath);
        computer.Reset();
        computer.RegisterA = registerA;
        computer.BuildPossibilities();
        // computer.Run(part2: true);
        return computer.OutputAsString();
    }

    public void Test()
    {
        var computer = new Computer();
        computer.InitializeFromFile(inputFilePath);
        computer.Reset();
        computer.BuildPossibilities();
        // computer.Run(part2: true);
    }
}