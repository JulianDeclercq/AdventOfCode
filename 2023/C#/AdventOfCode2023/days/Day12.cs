using System.Text;

namespace AdventOfCode2023.days;

public class Day12
{
    public record Condition(string Springs, int[] GroupLengths);
    public void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day12.txt")
            .Select(l =>
            {
                var split = l.Split(' ');
                return new Condition(split[0], split[1].Split(',').Select(int.Parse).ToArray());
            })
            .ToList();

        var answer = 0;
        foreach (var condition in input)
        {
            var combinations = GenerateVariations(condition.Springs).ToArray();
            answer += combinations.Count(variation => IsValid(condition with { Springs = variation }));
        }
        
        Console.WriteLine(answer);
    }

    private static IEnumerable<string> GenerateVariations(string input)
    {
        var n = input.Count(c => c.Equals('?'));
        var possibilities = (int)Math.Pow(2, n);

        var combinations = new List<StringBuilder>();
        for (var i = 0; i < possibilities; i++)
        {
            var sb = new StringBuilder();
            for (var j = 0; j < n; j++)
            {
                var bit = (i >> j) & 1;
                sb.Append(bit == 1 ? '.' : '#');
            }
            combinations.Add(sb);
        }

        var answer = new List<string>();
        foreach (var combination in combinations)
        {
            var amountReplaced = 0;
            var edited = new StringBuilder(input);
            for (var j = 0; j < input.Length; ++j)
            {
                if (input[j].Equals('?'))
                    edited[j] = combination[amountReplaced++];
            }
            answer.Add(edited.ToString());
        }
        
        return answer;
    }

    private static bool IsValid(Condition condition)
    {
        // generate all possibilities (probably won't work for part 2 :))
        var groups = condition.Springs.Split(".").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        if (groups.Count != condition.GroupLengths.Length)
            return false;

        for (var i = 0; i < groups.Count; ++i)
        {
            if (groups[i].Length != condition.GroupLengths[i])
                return false;
        }

        return true;
    }
}