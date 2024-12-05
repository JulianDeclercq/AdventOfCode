namespace AdventOfCode2024;

public class Day5
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/day5e.txt");
        var rules = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var pages = lines.Skip(rules.Length + 1).ToArray();
    }
}