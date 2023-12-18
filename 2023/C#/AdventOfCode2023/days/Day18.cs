using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public static class Day18
{
    public static void Solve()
    {
        var input = File
            .ReadAllLines("../../../input/Day18.txt")
            .Select(l =>
            { 
                var split = l.Split(' ');
                return (Direction: DirectionLookup[split[0][0]], Length: int.Parse(split[1]), Color: split[2][1..^1]);
            })
            .ToArray();

        int minX = int.MaxValue, maxX = int.MinValue, curX = 0;
        int minY = int.MaxValue, maxY = int.MinValue, curY = 0;
        foreach (var (direction, length, _) in input)
        {
            switch (direction)
            {
                case Direction.North: curY -= length; break;
                case Direction.East: curX += length; break;
                case Direction.South: curY += length; break;
                case Direction.West: curX -= length; break;
            }
            minX = Math.Min(minX, curX);
            maxX = Math.Max(maxX, curX);
            minY = Math.Min(minY, curY);
            maxY = Math.Max(maxY, curY);
        }

        var width = Math.Max(Math.Abs(minX), maxX) * 2 + 10; // + 10 to create some margin for easy starting the middle
        var height = Math.Max(Math.Abs(minY), maxY) * 2 + 10;
        var grid = new Grid<char>(width, height, Enumerable.Repeat('.', width * height), '?');

        var start = new Point(width / 2, height / 2);
        var position = start;
        
        foreach (var (direction, length, color) in input)
        {
            for (var i = 0; i < length; ++i)
            {
                position += direction switch
                {
                    Direction.North => new Point(0, -1),
                    Direction.East => new Point(1, 0),
                    Direction.South => new Point(0, 1),
                    Direction.West => new Point(-1, 0),
                    _ => throw new ArgumentOutOfRangeException()
                };
                grid.Set(position, '#');
            }
        }
        FloodFill(grid, start + new Point(1, -1)); // trial and error for where to fill, filled the outside first by accident
        Console.WriteLine(grid.All().Count(a => a.Equals('#')));
    }

    private static readonly Dictionary<char, Direction> DirectionLookup = new()
    {
        ['U'] = Direction.North,
        ['R'] = Direction.East,
        ['D'] = Direction.South,
        ['L'] = Direction.West,
    };
    
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
}