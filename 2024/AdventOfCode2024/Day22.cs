namespace AdventOfCode2024;

public class Day22
{
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        const int steps = 2000;
        var secrets = File.ReadAllLines("input/day22.txt").Select(long.Parse).ToList();

        if (part is 1)
        {
            Console.WriteLine(secrets.Sum(secret => NextXSteps(secret, steps)));
            return;
        }

        Dictionary<long, List<int>> changes = [];
        foreach (var secret in secrets)
        {
            if (!changes.TryAdd(secret, NextXStepsDifferences(secret, steps)))
                throw new Exception($"Duplicate secret {secret}");
        }
    }

    private static long NextXSteps(long secret, int steps)
    {
        var current = secret;
        for (var i = 0; i < steps; ++i)
            current = Next(current);

        return current;
    }

    private static List<int> NextXStepsDifferences(long secret, int steps)
    {
        List<int> differences = [];
        var current = secret;
        var currentSmall = (int)char.GetNumericValue(secret.ToString()[^1]); 
        for (var i = 0; i < steps; ++i)
        {
            var next = Next(current);
            var nextSmall = (int)char.GetNumericValue(next.ToString()[^1]);
            differences.Add(nextSmall - currentSmall);
            
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