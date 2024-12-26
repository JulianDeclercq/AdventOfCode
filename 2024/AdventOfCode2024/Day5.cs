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
        
        // var masterOrder = Sort(lookup.Keys.Concat(reverseLookup.Keys).ToHashSet().ToList(), lookup);
        // Console.WriteLine($"MASTERORDER {string.Join(",", masterOrder)}");
        
        var updates = lines.Skip(rules.Length + 1)
            .Select(l => l
                .Split(',')
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        var validUpdates = updates.Where(u => IsValid(u, reverseLookup)).ToList();
        var invalidUpdates = updates.Where(u => !IsValid(u, reverseLookup)).ToList();

        var answer = 0;
        foreach (var update in validUpdates)
        {
            answer += update[update.Length / 2];
            // Console.WriteLine(string.Join(",", page));
        }
        Console.WriteLine(answer);
    }

    private static bool IsValid(int[] update, Dictionary<int, List<int>> reverseLookup)
    {
        for (var j = 0; j < update.Length; ++j)
        {
            if (!reverseLookup.TryGetValue(update[j], out var shouldComeBefore))
                continue;
            
            var comeAfter = update.Skip(j + 1).ToHashSet();
            if (shouldComeBefore.Any(x => comeAfter.Contains(x)))
                return false;
        }

        return true;
    }

    private static List<int> Sort(List<int> current, Dictionary<int, List<int>> lookup)
    {
        for (var i = 0; i < current.Count; ++i)
        {
            var page = current[i];
            if (!lookup.TryGetValue(page, out var pageShouldComeBefore))
                continue;

            var numbersBefore = current.Take(i).ToList();
            var idx = numbersBefore.FindIndex(x => pageShouldComeBefore.Contains(x));
            if (idx == -1)
                continue;
            
            Console.WriteLine($"Found invalid page {numbersBefore[idx]} ({idx}) that came " +
                              $"before page {current[i]} ({i}) but should be after.");

            Console.WriteLine($"BEFORE {string.Join(',', current)}");
            current.Remove(page);
            current.Insert(idx, page); 
            Console.WriteLine($"AFTER {string.Join(',', current)}");
            return Sort(current, lookup);
        }

        // not sure
        Console.WriteLine("DONE SORTING");
        return current;
    }
}