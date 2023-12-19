using System.Globalization;
using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public static class Day18
{
    public static void Solve(bool part1)
    {
        var input = File
            .ReadAllLines("../../../input/Day18e.txt")
            .Select(l =>
            { 
                var split = l.Split(' ');
                if (part1) {
                    return (Direction: DirectionLookup[split[0][0]], Length: int.Parse(split[1]));
                }
                
                var color = split[2][1..^1];
                return (Direction: DirectionLookup[color[^1]], Length: int.Parse(color[1..^1], NumberStyles.HexNumber));
            })
            .ToArray();

        var start = new BigPoint(0, 0);
        var current = start;
        var points = new List<BigPoint> { start };

        foreach (var (direction, length) in input)
        {
            current += direction switch
            {
                Direction.North => new BigPoint(0, length),
                Direction.East => new BigPoint(length, 0),
                Direction.South => new BigPoint(0, -length),
                Direction.West => new BigPoint(-length, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
            points.Add(current);
        }
        points.Add(start); // close the polygon

        var area = CalculateArea(points);
        var perimeter = CalculatePerimeter(points);
        Console.WriteLine(area + (perimeter / 2) + 1);
    }

    private static readonly Dictionary<char, Direction> DirectionLookup = new()
    {
        ['U'] = Direction.North,
        ['R'] = Direction.East,
        ['D'] = Direction.South,
        ['L'] = Direction.West,
        
        ['0'] = Direction.East,
        ['1'] = Direction.South,
        ['2'] = Direction.West,
        ['3'] = Direction.North,
    };
    
    // Originally used for part 1
    private static void FloodFill(Grid<char> grid, Point start)
    {
        var frontier = new Queue<Point>();
        var visited = new HashSet<Point> { start };

        frontier.Enqueue(start);

        while (frontier.TryDequeue(out var current))
        {
            foreach (var next in grid.NeighboursExtended(current, includeDiagonals: false).Where(n => !n.Value.Equals('#')))
            {
                if (visited.Contains(next.Position))
                    continue;
                
                frontier.Enqueue(next.Position);
                visited.Add(next.Position);
            }
        }

        foreach (var v in visited)
            grid.Set(v, '#');
    }

    private static long CalculateArea(IReadOnlyList<BigPoint> points)
    {
        var area = (long)Math.Round(Math.Abs(points.Take(points.Count - 1)
            .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
            .Aggregate((long)0, (x, y) => x + y) / 2.0));

        return area;
    }

    private static long CalculatePerimeter(IReadOnlyList<BigPoint> points)
    {
        long perimeter = 0;

        for (var i = 0; i < points.Count - 1; i++)
            perimeter += Distance(points[i], points[i + 1]);

        return perimeter;
    }

    private static long Distance(BigPoint p1, BigPoint p2)
    {
        return (long)Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
    }
}