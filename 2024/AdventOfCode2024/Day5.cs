namespace AdventOfCode2024;

public class Day5
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/day5e.txt");
        var rules = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        var lookup = new Dictionary<int, List<int>>();
        foreach (var rule in rules)
        {
            var parts = rule.Split('|').Select(int.Parse).ToArray();
            var target = lookup.TryGetValue(parts[0], out var list) ? list : [];
            target.Add(parts[1]);
            lookup[parts[0]] = target;
        }

        var reverseLookup = new Dictionary<int, List<int>>();
        foreach (var rule in rules)
        {
            var parts = rule.Split('|').Select(int.Parse).ToArray();
            var target = reverseLookup.TryGetValue(parts[1], out var list) ? list : [];
            target.Add(parts[0]);
            reverseLookup[parts[1]] = target;
        }
        
        var pages = lines.Skip(rules.Length + 1)
            .Select(l => l
                .Split(',')
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        for (var i = 0; i < pages.Length; ++i)
        {
            var correct = true;
            
            var page = pages[i];
            foreach (var update in page)
            {
                var lel = lookup[update];
                var rev = reverseLookup[update];
                var rest = page.Skip(i + 1).ToHashSet();
                if (rev.Any(x => rest.Contains(x)))
                {
                    
                }

            }
        }
    }
}