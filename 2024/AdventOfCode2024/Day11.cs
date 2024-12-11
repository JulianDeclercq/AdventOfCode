namespace AdventOfCode2024;

public class Day11
{
    public static void Solve()
    {
        // var input = "0 1 10 99 999";
        // var input = "125 17";
        var input = "890 0 1 935698 68001 3441397 7221 27";
        var stones = input.Split(" ").Select(long.Parse).ToList();

        const int steps = 25; 
        for (var i = 0; i < steps; ++i)
        {
            for (var j = 0; j < stones.Count; ++j)
            {
                if (stones[j] is 0)
                {
                    stones[j] = 1;
                    continue;
                }

                var stoneString = stones[j].ToString();
                if (stoneString.Length % 2 == 0)
                {
                    var lhs = stoneString[..(stoneString.Length / 2)];
                    var rhs = stoneString[(stoneString.Length / 2)..];
                    stones[j] = long.Parse(lhs);
                    stones.Insert(j + 1, long.Parse(rhs)); // off by one?
                    j++; // update loop index since we dont want to process the newly added stone
                    continue;
                }

                stones[j] *= 2024;
            }
        }
        
        Console.WriteLine(stones.Count);
    }
}