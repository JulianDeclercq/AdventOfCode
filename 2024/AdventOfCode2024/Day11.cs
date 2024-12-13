﻿namespace AdventOfCode2024;

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

        const int blinks = 75;
        long answer = 0;

        // just to print in the end, so I can see how many occurrences there actually were per stone
        Dictionary<long, long> curiosity = [];
        
        for (var i = 0; i < stones.Count; ++i)
        {
            // Console.WriteLine($"Processing stone {i}");
            var transformed = new Dictionary<long, long>
            {
                [stones[i]] = 1
            };
            
            for (var j = 0; j < blinks; ++j)
            {
                // Console.WriteLine($"Processing stone {i}, blink {j + 1}");
                Dictionary<long, long> stonesToProcess = [];
                foreach (var (stoneNumber, occurrences) in transformed)
                {
                    var nextStep = NextStep(stoneNumber, nextStepMemo);

                    foreach (var nextStone in nextStep)
                    {
                        var currentOccurrences = stonesToProcess.GetValueOrDefault(nextStone, 0);
                        stonesToProcess[nextStone] = currentOccurrences + occurrences;
                    }
                }
                transformed = stonesToProcess;
                // PrintStoneOccurrences(transformed);
            }

            foreach (var (_, occurrences) in transformed)
                answer += occurrences;
            
            curiosity = transformed;
        }
        
        PrintStoneOccurrences(curiosity);
        Console.WriteLine(answer);
    }

    private static List<long> NextStep(long stone, Dictionary<long, List<long>> nextStepMemo)
    {
        if (nextStepMemo.TryGetValue(stone, out var value))
            return value;
        
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
            throw new Exception($"Next step for {stone} was already in the memo");

        return result;
    }

    private static void PrintStoneOccurrences<T>(Dictionary<T, T> toPrint) where T : notnull
    {
        foreach (var (key, value) in toPrint)
            Console.WriteLine($"Stone number {key}, occurrences {value}");
    }
}