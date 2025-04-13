using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day14
{
    private const bool Example = true;
    private const int Width = Example ? 11 : 101;
    private const int Height = Example ? 7 : 103;
    private Grid<List<Point>?> _grid = new(1, 1, [[]], null);

    public Grid<List<Point>?> GetGrid() => _grid;

    public void Initialize(IReadOnlyList<string> inputLines, bool example = false)
    {
        // a robot will be a Point, which is its velocity. Position will be handled by the grid
        // nullable for invalid, not sure if good idea
        List<List<Point>> elements = [];
        // with Enumerable.Repeat you'll get the same reference to the same (empty) list
        for (var i = 0; i < Width; ++i)
        {
            for (var j = 0; j < Height; ++j)
            {
                elements.Add([]);
            }
        }

        var regex = new RegexHelper(new Regex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)"), "px", "py", "vx", "vy");
        _grid = new Grid<List<Point>?>(Width, Height, elements, null);

        HashSet<Point> velocities = [];
        foreach (var line in inputLines)
        {
            regex.Match(line);
            var position = new Point(regex.GetInt("px"), regex.GetInt("py"));
            var velocity = new Point(regex.GetInt("vx"), regex.GetInt("vy"));

            // if (velocities.Contains(velocity)) throw new Exception($"Duplicate velocity {velocity} found");

            velocities.Add(velocity);

            var current = _grid.At(position) ?? [];
            current.Add(velocity);
            _grid.Set(position, current);
        }
    }

    public void Solve()
    {
        var visualisation = new Grid<int>(Width, Height, _grid.All().Select(x => x!.Count), -1); // long?
        // PrintGrid(grid);
        Console.WriteLine(visualisation);

        const int steps = 5;
        for (var i = 0; i < steps; ++i)
        {
            Step();
            // PrintGrid(grid);
            var vis = new Grid<int>(Width, Height, _grid.All().Select(x => x!.Count), -1);
            Console.WriteLine(vis);
        }
    }

    public void Step()
    {
        // deep copy the grid, you don't want to be working on the grid while changing it around
        List<List<Point>?> elements = [];
        foreach (var cell in _grid.AllExtended())
        {
            elements.Add(cell.Value!.ToList());
            // be careful here I'm not sure if Point being a reference type is a problem
            // don't think so though since I don't change the point itself, just moving it around
        }

        var workCopy = new Grid<List<Point>?>(Width, Height, elements, null);

        foreach (var cell in _grid.AllExtended())
        {
            foreach (var robot in cell.Value!)
            {
                var newPosition = cell.Position + robot;

                var wrappedX = newPosition.X;
                var wrappedY = newPosition.Y;
                if (newPosition.X < 0) wrappedX = Width + newPosition.X;
                if (newPosition.Y < 0) wrappedY = Height + newPosition.Y;
                if (newPosition.X >= Width) wrappedX %= Width;
                if (newPosition.Y >= Height) wrappedY %= Height;

                var wrappedPosition = new Point(wrappedX, wrappedY);

                var list = workCopy.At(cell.Position);
                list!.Remove(robot);

                // TODO: Verify if needed? Since list is a reference type i don't think i need to set it again
                workCopy.Set(cell.Position, list);

                var newList = workCopy.At(wrappedPosition);
                if (newList is null)
                {
                    int bkpt = -5;
                    throw new Exception("Failed to get NewList");
                }

                // TODO: Do robots need a unique id? Seems like we do if there are robots who share velocity
                newList.Add(robot);
                workCopy.Set(wrappedPosition, newList); // TODO: Same as before
            }
        }

        _grid = workCopy;
    }

    private static void PrintGrid(Grid<List<Point>?> grid)
    {
        foreach (var cell in grid.AllExtended())
        {
            if (cell.Value is null)
                throw new Exception("Cell value was null when trying to print");

            Console.WriteLine($"{cell.Position}, [{string.Join(" | ", cell.Value)}]");
        }
    }
}