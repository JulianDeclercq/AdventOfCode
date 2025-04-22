namespace AdventOfCode2024;

public class Day22
{
    private record PriceChange(int Price, int Change);
    private record CombinationPrice(string Combination, int Price);
    
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        const int steps = 2000;
        var secrets = File.ReadAllLines("input/real/day22e.txt").Select(long.Parse).ToList();

        if (part is 1)
        {
            Console.WriteLine(secrets.Sum(secret => NextXSteps(secret, steps)));
            return;
        }

        Dictionary<long, List<PriceChange>> changes = [];
        Dictionary<long, List<CombinationPrice>> combinations = [];
        foreach (var secret in secrets)
        {
            if (!changes.TryAdd(secret, NextXStepsDifferences(secret, steps)))
                throw new Exception($"Duplicate secret {secret}");
        }

        foreach (var (key, value) in changes)
        {
            for (var i = 0; i < value.Count - 3; ++i) // TODO: Bound check
            {
                var combos = combinations.TryGetValue(key, out var existing) ? existing : [];
                
                var combinationPrice =
                    new CombinationPrice(
                        $"{value[i].Change}{value[i + 1].Change}{value[i + 2].Change}{value[i + 3].Change}",
                        value[i + 3].Price);
                
                combos.Add(combinationPrice);
                combinations[key] = combos;
            }
        }

        // var distinctCombinations = combinations.Values.SelectMany(combo => combo).ToHashSet();
        var distinctCombinations = combinations.Values
            .SelectMany(combo => combo)
            .DistinctBy(c => c.Combination)
            .ToHashSet();
        
        Console.WriteLine(distinctCombinations.Count);

        var brkpt = 5;
    }

    private static long NextXSteps(long secret, int steps)
    {
        var current = secret;
        for (var i = 0; i < steps; ++i)
            current = Next(current);

        return current;
    }

    private static List<PriceChange> NextXStepsDifferences(long secret, int steps)
    {
        List<PriceChange> differences = [];
        var current = secret;
        var currentSmall = (int)char.GetNumericValue(secret.ToString()[^1]); 
        for (var i = 0; i < steps; ++i)
        {
            var next = Next(current);
            var nextSmall = (int)char.GetNumericValue(next.ToString()[^1]);

            var priceChange = new PriceChange(nextSmall, nextSmall - currentSmall);
            differences.Add(priceChange);
            
            current = next;
            currentSmall = nextSmall;
        }

        return differences;
    }

    private static long Next(long secret)
    {
        var result = secret * 64;
        secret = Mix(result, secret);
        secret = Prune(secret);
        
        result = (long)(secret / 32d);
        secret = Mix(result, secret);
        secret = Prune(secret);

        result = secret * 2048;
        secret = Mix(result, secret);
        secret = Prune(secret);

        return secret;
    }

    private static long Mix(long value, long secret) => value ^ secret;
    private static long Prune(long secret) => secret % 16777216;
}