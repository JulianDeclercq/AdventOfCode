using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day5
{
    private static void Solve(bool includeDiagonals)
    {
        var input = File.ReadAllLines(@"..\..\..\input\day5.txt");
        var regex = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)");
        var lines = new List<Line>();

        // parse input
        foreach (var s in input)
        {
            if (!regex.IsMatch(s))
            {
                Console.WriteLine($"Failed to parse line {s}");
                return;
            }

            var match = regex.Match(s);
            var ints = match.Groups.Values.Skip(1).Select(Helpers.ToInt).ToList();
            var line = new Line(new Point(ints[0], ints[1]), new Point(ints[2], ints[3]));
            if (includeDiagonals || !line.IsDiagonal)
                lines.Add(line);
        }

        // mark the lines
        var counter = new Dictionary<Point, int>();
        foreach (var line in lines)
        {
            foreach (var point in line.Points)
                counter[point] = (counter.ContainsKey(point) ? counter[point] : 0) + 1;
        }
        Console.WriteLine($"Day 5 part {(includeDiagonals ? 2 : 1)}: {counter.Count(x => x.Value >= 2)}");
    }
    
    public void Part1() => Solve(includeDiagonals: false);
    public void Part2() => Solve(includeDiagonals: true);
}