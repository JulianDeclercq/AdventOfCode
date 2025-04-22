namespace AdventOfCode2024;

public class Day1
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/real/day1.txt")
            .Select(l => l.Split("   ")
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        var lhs = lines.Select(l => l.First()).Order().ToArray();
        var rhs = lines.Select(l => l.Last()).Order().ToArray();
        var part1 = lhs.Zip(rhs).Aggregate(0, (total, next) => total + Math.Abs(next.First - next.Second));
        Console.WriteLine(part1);

        var part2 = lhs.Sum(l => l * rhs.Count(r => r == l));
        Console.WriteLine(part2);
    }
}