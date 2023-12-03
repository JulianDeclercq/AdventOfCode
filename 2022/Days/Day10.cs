using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days;

public class Day10
{
    private readonly RegexHelper _pattern = new(new Regex(@"(addx) (-?(\d+))"),
        "instruction", "amount");

    private int _x = 1;
    public void Solve()
    {
        // var operations = new Queue<string>(File.ReadAllLines(@"..\..\..\input\day10_test.txt"));
        // var operations = new Queue<string>(File.ReadAllLines(@"..\..\..\input\day10_example.txt"));
        var operations = new Queue<string>(File.ReadAllLines(@"..\..\..\input\day10.txt"));

        var valueDuringCycle = new Dictionary<int, int>(); // first cycle has number 1 (not 0)

        var busyCyclesLeft = 0;
        Action? op = null;
        var cycle = 1;
        
        for (; busyCyclesLeft > 0 || operations.Any() ; ++cycle)
        {
            valueDuringCycle[cycle] = _x;

            // if not busy, look at the next operation
            if (busyCyclesLeft == 0 && operations.Any())
            {
                var operation = operations.Dequeue(); 
                switch (operation.First())
                {
                    case 'n' :
                        // Console.WriteLine($"Cycle {cycle} | noop started");
                        busyCyclesLeft = 1;
                        op = () => { /* NOOP */};
                        break;
                    case 'a' :
                        _pattern.Match(operation);
                        var number = _pattern.GetInt("amount");
                        // Console.WriteLine($"Cycle {cycle} | started addition with {number}");
                        
                        busyCyclesLeft = 2;
                        op = CreateAddOperation(number);
                        break;
                }
            }

            --busyCyclesLeft;
            if (busyCyclesLeft == 0)
            {
                op?.Invoke();
                op = null;
            }
        }

        // add last cycle
        valueDuringCycle[cycle] = _x;

        // foreach(var kek in _valueDuringCycle) Console.WriteLine($"{kek.Key} | {kek.Value}");
        var answer = new[] {20, 60, 100, 140, 180, 220}.Select(n => SignalStrength(n, valueDuringCycle)).Sum();
        Console.WriteLine($"Day 10 part 1: {answer}");
    }

    private Action CreateAddOperation(int value)
    {
        return () => { _x += value; };
    }

    private static int SignalStrength(int cycleNr, IReadOnlyDictionary<int, int> valueDuringCycle)
    {
        return valueDuringCycle[cycleNr] * cycleNr;
    }
}
