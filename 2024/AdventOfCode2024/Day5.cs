namespace AdventOfCode2024;

public class Day5
{
    public static void Solve(int part)
    {
        var lines = File.ReadAllLines("input/day5.txt");
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


        List<int[]> validPages = [];
        for (var i = 0; i < pages.Length; ++i)
        {
            var correct = true;
            
            var page = pages[i];
            for (var j = 0; j < page.Length; ++j)
            {
                // var lel = lookup[update];
                if (!reverseLookup.TryGetValue(page[j], out var rev))
                    continue;
                
                var rest = page.Skip(j + 1).ToHashSet();
                if (rev.Any(x => rest.Contains(x)))
                {
                    correct = false;
                    break;
                }
            }
            if (correct)
                validPages.Add(page);
        }

        var answer = 0;
        foreach (var page in validPages)
        {
            answer += page[page.Length / 2];
            // Console.WriteLine(string.Join(",", page));
        }
        Console.WriteLine(answer);
    }
}