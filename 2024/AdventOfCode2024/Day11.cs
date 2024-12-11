namespace AdventOfCode2024;

public class Day11
{
    public static void Solve()
    {
        // var input = "0 1 10 99 999";
        // var input = "125 17";
        var input = "890 0 1 935698 68001 3441397 7221 27";
        var stones = input.Split(" ").Select(long.Parse).ToList();

        var nextStepMemo = new Dictionary<long, List<long>>
        {
            [0] = [1],
            [1] = [2024]
        };

        var lengthMemo = new Dictionary<long, long>(); // genuinely don't know if needed

        const int blinks = 75;
        long answer = 0;
        
        for (var i = 0; i < stones.Count; ++i)
        {
            List<long> transformed = [stones[i]];
            for (var j = 0; j < blinks; ++j)
            {
                List<long> kek = [];
                foreach (var stone in transformed)
                {
                    var nextStep = NextStep(stone, nextStepMemo);
                    answer += GetProcreationNumba(stone, lengthMemo, nextStepMemo);
                    kek.AddRange(nextStep);
                }

                transformed = kek;
                Console.WriteLine(transformed.Count);
            }
        }
        
        Console.WriteLine(answer);
    }

    private static long GetProcreationNumba(long stone, Dictionary<long, long> lengthMemo, Dictionary<long, List<long>> nextStepMemo)
    {
        // memod
        if (lengthMemo.TryGetValue(stone, out var value))
            return value;

        // not memod
        var nextStep = NextStep(stone, nextStepMemo);
        lengthMemo.TryAdd(stone, nextStep.Count);
        return nextStep.Count;
    }

    private static List<long> NextStep(long stone, Dictionary<long, List<long>> nextStepMemo)
    {
        // Console.WriteLine($"Calculating next step for {stone}");
        if (nextStepMemo.TryGetValue(stone, out var value))
        {
            // Console.WriteLine($"Next step was memod for {stone}");
            return value;
        }
        
        // calculate value
        List<long> result = [];
        var stoneString = stone.ToString();
        if (stoneString.Length % 2 == 0)
        {
            var lhs = stoneString[..(stoneString.Length / 2)];
            var rhs = stoneString[(stoneString.Length / 2)..];
            result.Add(long.Parse(lhs));
            result.Add(long.Parse(rhs));
        }
        else
        {
            result.Add(stone * 2024);
        }
        
        // add to memo and return
        if (!nextStepMemo.TryAdd(stone, result))
        {
            throw new Exception($"Next step for {stone} was already in the memo," +
                                $" was this done in a different execution path meanwhile?");
        }

        return result;
    }
}

// Q: How do I know, for a certain stone, how many it is going to have created after X steps?
// for a certain stone, it is going to be NextStep(long stone).length which is memoable
// for those stones it will be the same, that's the next step
// repeat