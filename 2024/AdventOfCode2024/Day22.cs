namespace AdventOfCode2024;

public class Day22
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/day22.txt").Select(long.Parse);
        var answer = lines.Sum(secret => NextXSteps(secret, 2000));
        Console.WriteLine(answer);
    }

    private static long NextXSteps(long secret, int steps)
    {
        var current = secret;
        for (var i = 0; i < steps; ++i)
            current = Next(current);

        return current;
    }

    private static long Next(long secret)
    {
        /* Calculate the result of multiplying the secret number by 64.
         * Then, mix this result into the secret number.
         * Finally, prune the secret number. */
        var result = secret * 64;
        secret = Mix(result, secret);
        secret = Prune(secret);
        
        /* Calculate the result of dividing the secret number by 32.
           Round the result down to the nearest integer.
           Then, mix this result into the secret number.
           Finally, prune the secret number. */
        var result2 = (long)(secret / 32d);
        secret = Mix(result2, secret);
        secret = Prune(secret);

        /* Calculate the result of multiplying the secret number by 2048.
           Then, mix this result into the secret number.
           Finally, prune the secret number. */
        var result3 = secret * 2048;
        secret = Mix(result3, secret);
        secret = Prune(secret);

        return secret;
    }

    private static long Mix(long value, long secret) => value ^ secret;
    private static long Prune(long secret) => secret % 16777216;
}