using System.Numerics;

namespace AdventOfCode2024;

public class Day17(string inputFilePath)
{
    public string Part1()
    {
        var computer = new Computer();
        computer.InitializeFromFile(inputFilePath);
        computer.Run(false);
        Console.WriteLine("Program ran with output:");
        Console.WriteLine(computer.Output);

        return string.Join(",", computer.Output);
    }

    public BigInteger Part2()
    {
        var computer = new Computer();
        computer.InitializeFromFile(inputFilePath);

        List<string> threeBitNumbers = // 0 to 7 in 3 bit representation
        [
            "000", // 0 
            "001", // 1
            "010", // 2
            "011", // 3
            "100", // 4
            "101", // 5
            "110", // 6
            "111" // 7
        ];

        var count = 0;
        foreach (var possibility in Sequences(threeBitNumbers, computer.ProgramLength()))
        {
            // Console.WriteLine(possibility);

            if (count % 1_000_000 == 0)
                Console.WriteLine($"{count} -> {possibility}");

            var bigInteger = BinaryStringToBigInteger(possibility);

            computer.Reset();
            computer.RegisterA = bigInteger;
            computer.Run(true);
            if (computer.OutputsSelf())
            {
                Console.WriteLine($"Program outputs self with register A: {bigInteger}");
                return bigInteger;
            }

            ++count;
        }

        throw new Exception("oops");
    }

    // Adapted from https://stackoverflow.com/a/8774175/4584421
    private static BigInteger BinaryStringToBigInteger(string str)
    {
        BigInteger result = 0;
        foreach (var c in str)
        {
            if (c != '0' && c != '1')
                throw new FormatException("Non-binary character found.");

            result <<= 1;
            result += c - '0'; // '0' -> 0, '1' -> 1
        }

        return result;
    }

    // cartesian product with repetition
    private IEnumerable<string> Sequences(IReadOnlyList<string> alphabet, int length)
    {
        if (length == 0)
        {
            yield return "";
            yield break;
        }

        var idx = new int[length]; // base-N odometer
        while (true)
        {
            yield return string.Join("", Enumerable.Range(0, length).Select(p => alphabet[idx[p]]));

            var pos = length - 1;
            while (pos >= 0 && ++idx[pos] == alphabet.Count)
            {
                idx[pos] = 0;
                pos--;
            }

            if (pos < 0) yield break;
        }
    }

    private class Computer
    {
        public readonly List<BigInteger> Output = [];
        private int _instructionPointer;

        private BigInteger _originalA;
        private BigInteger _originalB;
        private BigInteger _originalC;
        private List<int> _program = [];
        private BigInteger _registerB;
        private BigInteger _registerC;
        public BigInteger RegisterA;

        public int ProgramLength()
        {
            return _program.Count;
        }

        public List<int> GetProgram()
        {
            return _program;
        }

        public List<BigInteger> GetOutput()
        {
            return Output;
        }

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
                        // TODO: More performant check?
                        if (Output.Last() != _program[Output.Count - 1])
                            return false;

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

        private BigInteger Out(int comboOperand, bool performant = false)
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

        private BigInteger SharedDv(int comboOperand)
        {
            var numerator = RegisterA;
            var denominator = Math.Pow(2, (int)GetComboOperandValue(comboOperand));
            return numerator / (BigInteger)denominator;
        }

        private BigInteger GetComboOperandValue(int comboOperand)
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
            if (_program.Count != Output.Count)
                return false;

            for (var i = 0; i < _program.Count; ++i)
            {
                if (Output[i] != _program[i])
                    return false;
            }

            return true;
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
}