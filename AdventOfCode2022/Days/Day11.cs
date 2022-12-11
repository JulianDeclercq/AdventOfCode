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
        public Monkey() { }

        public int Number = 0;
        public Queue<int> Items = new();
        public string Operation = "";
        public int TestDivider = 0;
        public int TrueTarget = 0;
        public int FalseTarget = 0;
        public int TotalInspectCount = 0;
    }
    
    public void Solve()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day11.txt").ToList();
        const int monkeyDefinitionLength = 6;
        var amountOfMonkeys = input.Count(s => s.StartsWith("Monkey"));
        
        var monkeys = new List<Monkey>();
        for (var i = 0; i < amountOfMonkeys; ++i)
        {
            var monkeyDefinition = input.Skip(i * (monkeyDefinitionLength + 1)) // +1 for the empty line after definition
                                        .Take(monkeyDefinitionLength);

            _monkeyPattern.Parse(string.Join("", monkeyDefinition));
            monkeys.Add(new Monkey
            {
                Number = _monkeyPattern.GetInt("number"),
                Items = new Queue<int>(_monkeyPattern.Get("items")
                    .Split(',', StringSplitOptions.TrimEntries)
                    .Select(int.Parse)),
                Operation = _monkeyPattern.Get("operation"),
                TestDivider = _monkeyPattern.GetInt("testDivider"),
                TrueTarget = _monkeyPattern.GetInt("trueTarget"),
                FalseTarget = _monkeyPattern.GetInt("falseTarget"),
            });
        }
        
        const int rounds = 20;
        for (var i = 0; i < rounds; ++i)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Any())
                {
                    // inspect worry level of the next item
                    var worryLevel = monkey.Items.Dequeue();
                    monkey.TotalInspectCount++;
                    
                    // execute operation and calculate new value after relief
                    var newWorryLevel = ExecuteOperation(monkey.Operation, worryLevel) / 3;
                    
                    // hand over the item with new worry level to the correct monkey
                    var targetIdx = newWorryLevel % monkey.TestDivider == 0 ? monkey.TrueTarget : monkey.FalseTarget; 
                    monkeys[targetIdx].Items.Enqueue(newWorryLevel);
                }
            }
        }

        var result = monkeys.Select(m => m.TotalInspectCount)
                            .OrderByDescending(m => m)
                            .Take(2)
                            .Aggregate(1, (current, next) => current * next);
        
        Console.WriteLine($"Day 11 part 1: {result}");
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
}