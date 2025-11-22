namespace AdventOfCode2024;

public class Day22
{
    private record PriceChange(int Price, int Change);
    
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        const int steps = 2000;
        var secrets = File.ReadAllLines("input/real/day22.txt").Select(long.Parse).ToList();

        if (part is 1)
        {
            Console.WriteLine(secrets.Sum(secret => NextXSteps(secret, steps)));
            return;
        }

        // For each buyer, generate prices and changes
        Dictionary<long, List<PriceChange>> buyerChanges = [];
        foreach (var secret in secrets)
        {
            if (!buyerChanges.TryAdd(secret, NextXStepsDifferences(secret, steps)))
                throw new Exception($"Duplicate secret {secret}");
        }

        // For each buyer, find first occurrence of each 4-change sequence
        // Key: sequence tuple (c1, c2, c3, c4), Value: price at first occurrence
        Dictionary<(int, int, int, int), int> sequenceFirstPrices = [];
        
        foreach (var (buyerSecret, changes) in buyerChanges)
        {
            // Track which sequences we've seen for this buyer (only first occurrence counts)
            HashSet<(int, int, int, int)> seenSequences = [];
            
            for (var i = 0; i < changes.Count - 3; ++i)
            {
                var sequence = (changes[i].Change, changes[i + 1].Change, changes[i + 2].Change, changes[i + 3].Change);
                
                // Only record first occurrence for this buyer
                if (!seenSequences.Contains(sequence))
                {
                    seenSequences.Add(sequence);
                    
                    // The price at which we sell is the price after the 4th change
                    var sellPrice = changes[i + 3].Price;
                    
                    // Add to total for this sequence (will sum across all buyers)
                    if (!sequenceFirstPrices.ContainsKey(sequence))
                        sequenceFirstPrices[sequence] = 0;
                    
                    sequenceFirstPrices[sequence] += sellPrice;
                }
            }
        }

        // Find the maximum total bananas
        var maxBananas = sequenceFirstPrices.Values.Max();
        Console.WriteLine(maxBananas);
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
        List<PriceChange> changes = [];
        var current = secret;
        var currentPrice = GetOnesDigit(current);
        
        // Generate 2000 new secret numbers, each gives a price
        for (var i = 0; i < steps; ++i)
        {
            var next = Next(current);
            var nextPrice = GetOnesDigit(next);
            var change = nextPrice - currentPrice;

            changes.Add(new PriceChange(nextPrice, change));
            
            current = next;
            currentPrice = nextPrice;
        }

        return changes;
    }

    private static int GetOnesDigit(long number)
    {
        return (int)(Math.Abs(number) % 10);
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