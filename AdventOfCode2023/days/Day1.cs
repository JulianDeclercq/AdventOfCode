namespace AdventOfCode2023;

public class Day1
{
    private readonly Dictionary<string, int> _lookup = new()
    {
        ["one"] = 1, ["two"] = 2, ["three"] = 3, ["four"] = 4, ["five"] = 5,
        ["six"] = 6, ["seven"] = 7, ["eight"] = 8, ["nine"]= 9
    };

    private readonly HashSet<string> _numberStrings = new()
    {
        "1", "2", "3", "4", "5", "6", "7", "8", "9",
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
    };

    private readonly string[] _lines = File.ReadAllLines("../../../input/Day1.txt");

    public void Part1() => Console.WriteLine(_lines.Sum(l => int.Parse($"{l.First(char.IsNumber)}{l.Last(char.IsNumber)}")));
    public void Part2() => Console.WriteLine(_lines.Sum(GetLineNumber));

    private int GetLineNumber(string line)
    {
        string lowestNumber = "", highestNumber = "";
        int lowestIdx = int.MaxValue, highestIdx = int.MinValue;
        foreach (var ns in _numberStrings)
        {
            var firstIdx = line.IndexOf(ns, StringComparison.Ordinal);
            if (firstIdx > -1)
            {
                if (firstIdx < lowestIdx)
                {
                    lowestNumber = ns;
                    lowestIdx = firstIdx;
                }    
            }

            var lastIdx = line.LastIndexOf(ns, StringComparison.Ordinal);
            if (lastIdx > -1)
            {
                if (lastIdx > highestIdx)
                {
                    highestNumber = ns;
                    highestIdx = lastIdx;
                }
            }
        }
        
        var left = StringToNumber(lowestNumber);
        var right = StringToNumber(highestNumber);
        return int.Parse($"{(left > 0 ? left : "")}{(right > 0 ? right : "")}");
    }

    private int StringToNumber(string input)
    {
        if (input.Length == 0)
            return 0;
        
        if (input.Length == 1)
            return int.Parse(input);
       
        return _lookup[input];
    }
}