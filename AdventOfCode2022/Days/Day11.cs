using System.Text.RegularExpressions;

namespace AdventOfCode2022.Days;

public class Day11
{
    private readonly RegexHelper _monkeyPattern = new(new Regex(@"Monkey (\d+):  Starting items: (.+)  Operation: new = (.+)  Test: divisible by (\d+)    If true: throw to monkey (\d+)    If false: throw to monkey (\d+)"),
        "number", "items", "operation", "testDivider", "trueTarget", "falseTarget");

    private readonly RegexHelper _operationPattern = new(new Regex(@"(.+) (\+|\*) (.+)"),
        "lhs", "operator", "rhs");

    private class Monkey
    {
        public Queue<int> Items = new();
        public string Operation = "";
        public int TestDivider = 0;
        public int TrueTarget = 0;
        public int FalseTarget = 0;
        public int TotalInspectCount = 0;
    }
    
    public void Solve()
    {
        const int monkeyDefinitionLength = 6;
        var input = File.ReadAllLines(@"..\..\..\input\day11_example.txt").ToList();
        var amountOfMonkeys = input.Count(s => s.StartsWith("Monkey"));
        
        var monkeys = new List<Monkey>();
        for (var i = 0; i < amountOfMonkeys; ++i)
        {
            var monkeyDefinition = input.Skip(i * (monkeyDefinitionLength + 1)) // +1 for the empty line after definition
                                        .Take(monkeyDefinitionLength);

            _monkeyPattern.Parse(string.Join("", monkeyDefinition));
            monkeys.Add(new Monkey
            {
                Items = new Queue<int>(_monkeyPattern.Get("items")
                    .Split(',', StringSplitOptions.TrimEntries)
                    .Select(int.Parse)),
                Operation = _monkeyPattern.Get("operation"),
                TestDivider = _monkeyPattern.GetInt("testDivider"),
                TrueTarget = _monkeyPattern.GetInt("trueTarget"),
                FalseTarget = _monkeyPattern.GetInt("falseTarget"),
            });
        }

        var dividers = monkeys.Select(m => m.TestDivider).ToArray();
        var lcm = Lcm(dividers);
        const int rounds = 20;
        for (var i = 0; i < rounds; ++i)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Any())
                {
                    // inspect worry level of the next item
                    var worryLevel = monkey.Items.Dequeue();
                    monkey.TotalInspectCount += 1;
                    
                    // execute operation and calculate new value after relief
                    // var newWorryLevel = ExecuteOperation(monkey.Operation, worryLevel) / 3; // part 1
                    var newWorryLevel = ExecuteOperation(monkey.Operation, worryLevel) % lcm;
                    
                    // hand over the item with new worry level to the correct monkey
                    var targetIdx = newWorryLevel % monkey.TestDivider == 0 ? monkey.TrueTarget : monkey.FalseTarget; 
                    monkeys[targetIdx].Items.Enqueue(newWorryLevel);
                }
            }
        }

        for (var i = 0; i < monkeys.Count; ++i)
            Console.WriteLine($"Monkey {i} inspected items {monkeys[i].TotalInspectCount} times.");

        const int start = 1;
        var result = monkeys.Select(m => m.TotalInspectCount)
                            .OrderByDescending(m => m)
                            .Take(2)
                            .Aggregate(start, (current, next) => current * next);
        
        Console.WriteLine($"Day 11 part 2: {result}");
    }

    private int ExecuteOperation(string operation, int worryLevel)
    {
        _operationPattern.Parse(operation);
        var lhs = _operationPattern.Get("lhs");
        var lhsInt = lhs.Equals("old") ? worryLevel : int.Parse(lhs);
        
        var rhs = _operationPattern.Get("rhs");
        var rhsInt = rhs.Equals("old") ? worryLevel : int.Parse(rhs);

        return _operationPattern.Get("operator") switch
        {
            "+" => lhsInt + rhsInt,
            "*" => lhsInt * rhsInt,
            _ => 0
        };
    }

    private static int Gcd(int n1, int n2)
    {
        while (true)
        {
            if (n2 == 0) return n1;
            var n3 = n1;
            n1 = n2;
            n2 = n3 % n2;
        }
    }

    private static int Lcm(IEnumerable<int> numbers)
    {
        return numbers.Aggregate((s, val) => s * val / Gcd(s, val));
    }
}