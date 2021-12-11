using System.Text.RegularExpressions;

namespace AdventOfCode2021.days;

public class Day5
{
    private class Line
    {
        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public Point Start;
        public Point End;
    }
    
    public void Solve(bool part2 = false)
    {
        var input = File.ReadAllLines(@"..\..\..\input\day5.txt");
        var regex = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)");
        var lines = new List<Line>();

        int width = 0, height = 0;
        foreach (var s in input)
        {
            if (!regex.IsMatch(s))
            {
                Console.WriteLine($"Failed to parse line {s}");
                return;
            }

            var match = regex.Match(s);
            var ints = match.Groups.Values.Skip(1).Select(Helpers.ToInt).ToList();
            lines.Add(new Line(new Point(ints[0], ints[1]), new Point(ints[2], ints[3])));

            width = Math.Max(width, Math.Max(ints[0], ints[2]));
            height = Math.Max(height, Math.Max(ints[1], ints[3]));
        }

        // compensate for zero indexed list (if max X is 456 that means 457 elements fit per row)
        width++;
        height++;
        
        // create the grid and fill it
        var grid = new Grid<int>(width, height, int.MaxValue);
        grid.AddRange(Enumerable.Repeat(0, width * height));
        
        // mark the lines
        foreach (var line in lines)
        {
            foreach (var point in grid.PointsOnLine(line.Start, line.End, includeDiagonals: part2))
                grid.ModifyAt(point, x => x + 1);
            
            //Console.WriteLine(grid);
        }

        var answer = grid.All().Count(x => x >= 2);
        Console.WriteLine($"Day 5 part 1: {answer}");
    }
}