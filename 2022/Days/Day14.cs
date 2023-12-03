namespace AdventOfCode2022.Days;

public class Day14
{
    private const char Rock = '#', Air = '.', Source = '+', Sand = 'o';

    public static void Solve(bool part1 = true)
    {
        const string inputPath = @"..\..\..\input\day14.txt";
        string[][] rockFormations;

        if (part1)
        {
            rockFormations = File.ReadAllLines(inputPath)
                .Select(s => s.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .ToArray();    
        }
        else
        {
            const int fakeInfinity = 250;
            var maximumYValue = RetrieveMaximumYValue(inputPath);
            
            rockFormations = File.ReadAllLines(inputPath)
                .Select(s => s.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Append(new []{$"{500 - fakeInfinity},{maximumYValue + 2}", $"{500 + fakeInfinity},{maximumYValue + 2}"})
                .ToArray();
        }

        var coordinates = rockFormations.SelectMany(p => p).Select(p => p.Split(',')).ToArray();

        var xCoordinates = coordinates.Select(c => int.Parse(c.First())).OrderBy(c => c).ToArray();
        var yCoordinates = coordinates.Select(c => int.Parse(c.Last())).OrderBy(c => c).ToArray();

        int xMin = xCoordinates.Min(), xMax = xCoordinates.Max();
        int yMin = yCoordinates.Min(), yMax = yCoordinates.Max();
        int width = (xMax - xMin) + 1, height = yMax + 1;

        var grid = new Grid<char>(width, height, Enumerable.Range(0, width * height).Select(_ => Air), '$');
        
        // offset x axis (grid x doesnt start at 0) TODO: implement offset inside the grid itself
        var sourcePoint = new Point(500 - xMin, 0);
        grid.Set(sourcePoint, Source);
        
        foreach (var formation in rockFormations)
        {
            for (var i = 0; i < formation.Length - 1; ++i)
            {
                var current = formation[i].Split(',').Select(int.Parse).ToArray();
                var next = formation[i + 1].Split(',').Select(int.Parse).ToArray();

                var path = GeneratePath(ToPoint(current, xMin), ToPoint(next, xMin));
                foreach (var p in path)
                    grid.Set(p, Rock);
            }
        }

        var steps = 0;
        var finished = false;
        for (; !finished; ++steps)
        {
            // Write(grid, steps);
            finished = Step(sourcePoint, grid, part1);
        }
        
        // part 1: how many come to rest BEFORE overflowing into abyss (doesn't include last step where the sand falls into the abyss, so steps -1)
        // part 2: how many come to rest UNTIL the source is blocked (includes last step where the source itself gets blocked)
        Console.WriteLine($"Day 14 part {(part1 ? 1 : 2)}: {(part1 ? steps - 1 : steps)}");
    }

    private static bool Step(Point source, Grid<char> grid, bool part1 = true)
    {
        var sandPos = new Point(source);
        try
        {
            for (;;)
            {
                var target = grid.GetNeighbour(sandPos, GridNeighbourType.S)!;
                if (target.Value is Rock or Sand)
                {
                    target = grid.GetNeighbour(sandPos, GridNeighbourType.Sw)!;
                    if (target.Value is Rock or Sand)
                    {
                        target = grid.GetNeighbour(sandPos, GridNeighbourType.Se)!;
                        if (target.Value is Rock or Sand)
                        {
                            // come to rest
                            grid.Set(sandPos, Sand);
                        
                            // part 2 is finished when the source of the sand is blocked
                            if (!part1 && sandPos.Equals(source))
                                return true;
                        
                            break;
                        }
                    }
                }
                sandPos = target.Position;
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            // part 1 is finished when going out of bounds
            return true;
        }

        return false;
    }

    private static void Write(Grid<char> grid, int steps = 0)
    {
        var s = $"\n\n{steps}\n\n{grid}";
        File.AppendAllText(@"..\..\..\output\day14_example.txt", s);
        Console.WriteLine(s);
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
    
    private static int RetrieveMaximumYValue(string inputPath)
    {
        return File.ReadAllLines(inputPath)
            .Select(s => s.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .SelectMany(p => p)
            .Select(p => p.Split(','))
            .Select(c => int.Parse(c.Last()))
            .Max();
    }
}