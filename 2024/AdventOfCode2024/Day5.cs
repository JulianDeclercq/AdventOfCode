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
                .ToList())
            .ToList();

        if (part is 1)
        {
            var validUpdates = updates.Where(u => IsValid(u, reverseLookup)).ToList();
            Console.WriteLine(SumOfMiddlePageNumbers(validUpdates));
        }
        else // part 2
        {
            var invalidUpdates = updates.Where(u => !IsValid(u, reverseLookup));
            var sorted = invalidUpdates.Select(u => Sort(u, reverseLookup)).ToList();
            Console.WriteLine(SumOfMiddlePageNumbers(sorted));
        }
    }

    private static int SumOfMiddlePageNumbers(List<List<int>> updates)
    {
        var answer = 0;
        foreach (var update in updates)
            answer += update[update.Count / 2];

        return answer;
    }

    private static bool IsValid(List<int> update, Dictionary<int, List<int>> reverseLookup)
    {
        for (var i = 0; i < update.Count; ++i)
        {
            if (!reverseLookup.TryGetValue(update[i], out var shouldComeBefore))
                continue;
            
            var comeAfter = update.Skip(i + 1).ToHashSet();
            if (shouldComeBefore.Any(x => comeAfter.Contains(x)))
                return false;
        }

        return true;
    }

    private static List<int> Sort(List<int> current, Dictionary<int, List<int>> lookup, bool verbose = false)
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

            if (verbose)
            {
                Console.WriteLine($"Found invalid page {numbersBefore[idx]} ({idx}) that came " +
                                  $"before page {current[i]} ({i}) but should be after.");
            }

            current.Remove(page);
            current.Insert(idx, page); 
            return Sort(current, lookup);
        }

        return current;
    }
}