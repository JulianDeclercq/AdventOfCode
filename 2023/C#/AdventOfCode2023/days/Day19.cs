using System.Text.RegularExpressions;
using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public partial class Day19
{
    private class Rule
    {
        public char Property;
        public char Operation;
        public int CompareTo;
        public string Destination;

        public bool IsOnlyDestination;
    }

    private class Part
    {
        public int X;
        public int M;
        public int A;
        public int S;

        public int Rating => X + M + A + S;
    }
    
    private static readonly Dictionary<string, List<Rule>> Workflows = new();
    private static readonly List<Part> AcceptedParts = new();
    public static void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day19.txt");

        var workflowHelper = new RegexHelper(WorkflowPattern(), "name", "rules");
        var ruleHelper = new RegexHelper(RulePattern(), "property", "operation", "compareTo", "destination");

        foreach (var workflow in input.TakeWhile(x => !string.IsNullOrEmpty(x)))
        {
            if (!workflowHelper.Match(workflow))
                throw new Exception($"Didn't match workflow {workflow}");

            var name = workflowHelper.Get("name");
            if (Workflows.ContainsKey(name))
                throw new Exception($"Workflow {name} already exists");

            var rules = new List<Rule>();
            var rulesToParse = workflowHelper.Get("rules").Split(',');
            foreach (var toParse in rulesToParse)
            {
                if (!ruleHelper.Match(toParse))
                {
                    rules.Add(new Rule
                    {
                        IsOnlyDestination = true,
                        Destination = toParse
                    });
                    continue;
                }

                rules.Add(new Rule
                {
                    Property = ruleHelper.Get("property")[0],
                    Operation = ruleHelper.Get("operation")[0],
                    CompareTo = ruleHelper.GetInt("compareTo"),
                    Destination = ruleHelper.Get("destination")
                });
            }

            Workflows[name] = rules;
        }

        var parts = new List<Part>();
        var partHelper = new RegexHelper(PartPattern(), "X", "M", "A", "S");
        foreach (var part in input.Skip(Workflows.Count + 1))
        {
            if (!partHelper.Match(part))
                throw new Exception($"Part {part} didn't match");

            parts.Add(new Part
            {
                X = partHelper.GetInt("X"),
                M = partHelper.GetInt("M"),
                A = partHelper.GetInt("A"),
                S = partHelper.GetInt("S")
            });
        }

        foreach (var part in parts)//.Skip(1).Take(1))
        {
            foreach (var rule in Workflows["in"])
            {
                if (ProcessRule(part, rule))
                    break;
            }
        }

        checked
        {
            Console.WriteLine(AcceptedParts.Sum(p => p.Rating));
        }
    }

    private static bool ProcessRule(Part part, Rule rule)
    {
        if (!rule.IsOnlyDestination)
        {
            var property = rule.Property switch
            {
                'x' => part.X,
                'm' => part.M,
                'a' => part.A,
                's' => part.S,
                _ => throw new Exception($"Invalid property {rule.Property}")
            };

            var shouldBeSmaller = rule.Operation == '<';
            if (shouldBeSmaller)
            {
                if (property >= rule.CompareTo)
                    return false;
            }
            else
            {
                if (property <= rule.CompareTo)
                    return false;
            }
        }

        if (rule.Destination == "A")
        {
            AcceptedParts.Add(part);
            return true;
        }
            
        if (rule.Destination == "R")
            return true;

        foreach (var r in Workflows[rule.Destination])
        {
            //Console.WriteLine($"Processing rule from {rule.Destination}");
            if (ProcessRule(part, r))
                break;
        }

        return true; // not sure about this guy
    }

    [GeneratedRegex(@"(\w+)\{(.+)\}")]
    private static partial Regex WorkflowPattern();
    
    [GeneratedRegex(@"([xmas])([<>])(\d+):(\w+)")]
    private static partial Regex RulePattern();
    
    [GeneratedRegex(@"\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)\}")]
    private static partial Regex PartPattern();
}