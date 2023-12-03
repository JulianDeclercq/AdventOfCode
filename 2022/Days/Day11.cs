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
        public Queue<ulong> Items = new();
        public string Operation = "";
        public ulong TestDivider = 0;
        public int TrueTarget = 0;
        public int FalseTarget = 0;
        public ulong TotalInspectCount = 0;
    }
    
    public void Solve(bool part1 = true)
    {
        const int monkeyDefinitionLength = 6;
        var input = File.ReadAllLines(@"..\..\..\input\day11.txt").ToList();
        var amountOfMonkeys = input.Count(s => s.StartsWith("Monkey"));
        
        var monkeys = new List<Monkey>();
        for (var i = 0; i < amountOfMonkeys; ++i)
        {
            var monkeyDefinition = input.Skip(i * (monkeyDefinitionLength + 1)) // +1 for the empty line after definition
                                        .Take(monkeyDefinitionLength);

            _monkeyPattern.Match(string.Join("", monkeyDefinition));
            monkeys.Add(new Monkey
            {
                Items = new Queue<ulong>(_monkeyPattern.Get("items")
                    .Split(',', StringSplitOptions.TrimEntries)
                    .Select(ulong.Parse)),
                Operation = _monkeyPattern.Get("operation"),
                TestDivider = ulong.Parse(_monkeyPattern.Get("testDivider")),
                TrueTarget = _monkeyPattern.GetInt("trueTarget"),
                FalseTarget = _monkeyPattern.GetInt("falseTarget"),
            });
        }

        var rounds = part1 ? 20 : 10000;
        
        // Credit to https://nickymeuleman.netlify.app/garden/aoc2022-day11 for the common multiple idea
        var lcm = Lcm(monkeys.Select(m => m.TestDivider));
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
                    var newWorryLevel = ExecuteOperation(monkey.Operation, worryLevel);
                    newWorryLevel = part1 ? newWorryLevel / 3 : newWorryLevel % lcm;
                    
                    // hand over the item with new worry level to the correct monkey
                    var targetIdx = newWorryLevel % monkey.TestDivider == 0 ? monkey.TrueTarget : monkey.FalseTarget; 
                    monkeys[targetIdx].Items.Enqueue(newWorryLevel);
                }
            }
        }

        const ulong start = 1;
        var result = monkeys.Select(m => m.TotalInspectCount)
                            .OrderByDescending(m => m)
                            .Take(2)
                            .Aggregate(start, (current, next) => current * next);
        
        Console.WriteLine($"Day 11 part {(part1 ? 1 : 2)}: {result}");
    }

    private ulong ExecuteOperation(string operation, ulong worryLevel)
    {
        _operationPattern.Match(operation);
        var lhs = _operationPattern.Get("lhs");
        var lhsInt = lhs.Equals("old") ? worryLevel : ulong.Parse(lhs);
        
        var rhs = _operationPattern.Get("rhs");
        var rhsInt = rhs.Equals("old") ? worryLevel : ulong.Parse(rhs);

        checked
        {
            return _operationPattern.Get("operator") switch
            {
                "+" => lhsInt + rhsInt,
                "*" => lhsInt * rhsInt,
                _ => 0
            };
        }
    }

    private static ulong Gcd(ulong n1, ulong n2)
    {
        while (true)
        {
            if (n2 == 0) return n1;
            var n3 = n1;
            n1 = n2;
            n2 = n3 % n2;
        }
    }

    private static ulong Lcm(IEnumerable<ulong> numbers)
    {
        return numbers.Aggregate((s, val) => s * val / Gcd(s, val));
    }
}