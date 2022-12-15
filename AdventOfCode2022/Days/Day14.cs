namespace AdventOfCode2022.Days;

public class Day14
{
    public void Solve()
    {
        var rockFormation = File.ReadAllLines(@"..\..\..\input\day14.txt")
            .Select(s => s.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        var coordinates = rockFormation.SelectMany(p => p).Select(p => p.Split(',')).ToArray();

        var xCoordinates = coordinates.Select(c => int.Parse(c.First())).OrderBy(c => c).ToArray();
        var yCoordinates = coordinates.Select(c => int.Parse(c.Last())).OrderBy(c => c).ToArray();

        int xMin = xCoordinates.Min(), xMax = xCoordinates.Max();
        int yMin = yCoordinates.Min(), yMax = yCoordinates.Max();
        int width = (xMax - xMin) + 1, height = yMax + 1;

        var grid = new Grid<char>(width, height, Enumerable.Range(0, width * height).Select(_ => '.'), '$');

        const char rock = '#', air = '.', source = '+', sand = 'o';

        // offset x axis (grid x doesnt start at 0) TODO: implement offset inside the grid itself
        var sourcePoint = new Point(500 - xMin, 0);
        grid.Set(sourcePoint, source);
        foreach (var formation in rockFormation)
        {
            for (var i = 0; i < formation.Length - 1; ++i)
            {
                var current = formation[i].Split(',').Select(int.Parse).ToArray();
                var next = formation[i + 1].Split(',').Select(int.Parse).ToArray();

                var path = GeneratePath(ToPoint(current, xMin), ToPoint(next, xMin));
                foreach (var p in path)
                    grid.Set(p, rock);
            }
        }

        var steps = 0;
        try
        {
            for (;; ++steps)
            {
                var sandPos = new Point(sourcePoint);
                for (;;)
                {
                    var target = grid.GetNeighbour(sandPos, GridNeighbourType.S)!;
                    if (target.Value is rock or sand)
                    {
                        target = grid.GetNeighbour(sandPos, GridNeighbourType.Sw)!;
                        if (target.Value is rock or sand)
                        {
                            target = grid.GetNeighbour(sandPos, GridNeighbourType.Se)!;
                            if (target.Value is rock or sand)
                            {
                                // come to rest
                                grid.Set(sandPos, sand);
                                break;
                            }
                        }
                    }
                    sandPos = target.Position;
                }
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Day 14 part 1: {steps}");
        }
    }

    private static Point ToPoint(int[] parsed, int offset) => new(parsed.First() - offset, parsed.Last());

    private static List<Point> GeneratePath(Point start, Point end)
    {
        var path = new List<Point>();
        if (start.X == end.X)
        {
            int min = Math.Min(start.Y, end.Y), max = Math.Max(start.Y, end.Y);
            path.AddRange(Enumerable.Range(min, (max - min) + 1).Select(y => new Point(start.X, y)));
        }
        else if (start.Y == end.Y)
        {
            int min = Math.Min(start.X, end.X), max = Math.Max(start.X, end.X);
            path.AddRange(Enumerable.Range(min, (max - min) + 1).Select(x => new Point(x, start.Y)));
        }
        else throw new Exception("Invalid path.");

        return path;
    }
}