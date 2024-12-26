namespace AdventOfCode2024;

public class Day5
{
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
                
        var lines = File.ReadAllLines("input/day5.txt");
        var rules = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        Dictionary<int, List<int>> lookup = [], reverseLookup = [];
        foreach (var rule in rules)
        {
            var parts = rule.Split('|').Select(int.Parse).ToArray();
            var target = lookup.TryGetValue(parts[0], out var list) ? list : [];
            target.Add(parts[1]);
            lookup[parts[0]] = target;
            
            target = reverseLookup.TryGetValue(parts[1], out var list2) ? list2 : [];
            target.Add(parts[0]);
            reverseLookup[parts[1]] = target;
        }
        
        var updates = lines.Skip(rules.Length + 1)
            .Select(l => l
                .Split(',')
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        List<int[]> validUpdates = [];
        List<int[]> invalidUpdates = [];
        foreach (var update in updates)
        {
            var valid = true;
            
            for (var j = 0; j < update.Length; ++j)
            {
                if (!reverseLookup.TryGetValue(update[j], out var shouldComeBefore))
                    continue;
                
                var comeAfter = update.Skip(j + 1).ToHashSet();
                if (shouldComeBefore.Any(x => comeAfter.Contains(x)))
                {
                    valid = false;
                    break;
                }
            }

            var target = valid ? validUpdates : invalidUpdates;
            target.Add(update);
        }

        var answer = 0;
        foreach (var update in validUpdates)
        {
            answer += update[update.Length / 2];
            // Console.WriteLine(string.Join(",", page));
        }
        Console.WriteLine(answer);
    }

}