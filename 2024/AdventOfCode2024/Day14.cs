using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day14
{
    // private const int Width = Example ? 11 : 101;
    // private const int Height = Example ? 7 : 103;
    private Grid<List<Point>?> _grid = new(1, 1, [[]], null);
    private int _width = 0;
    private int _height = 0;

    public Grid<List<Point>?> GetGrid() => _grid;

    public void InitializeFromFile(string filePath, bool example = false)
    {
        Initialize(File.ReadAllLines(filePath), example);
    }

    public void Initialize(IReadOnlyList<string> inputLines, bool example = false)
    {
        // a robot will be a Point, which is its velocity. Position will be handled by the grid
        // nullable for invalid, not sure if good idea
        List<List<Point>> elements = [];
        
        _width = example ? 11 : 101;
        _height = example ? 7 : 103;
        // with Enumerable.Repeat you'll get the same reference to the same (empty) list
        for (var i = 0; i < _width; ++i)
        {
            for (var j = 0; j < _height; ++j)
            {
                elements.Add([]);
            }
        }

        var regex = new RegexHelper(new Regex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)"), "px", "py", "vx", "vy");
        _grid = new Grid<List<Point>?>(_width, _height, elements, null);

        foreach (var line in inputLines)
        {
            regex.Match(line);
            var position = new Point(regex.GetInt("px"), regex.GetInt("py"));
            var velocity = new Point(regex.GetInt("vx"), regex.GetInt("vy"));

            var current = _grid.At(position) ?? [];
            current.Add(velocity);
            _grid.Set(position, current);
        }
    }

    public void Solve()
    {
        // var visualisation = new Grid<int>(_width, _height, _grid.All().Select(x => x!.Count), -1); // long?
        // PrintGrid(grid);
        // Console.WriteLine(visualisation);

        const int steps = 100;
        for (var i = 0; i < steps; ++i)
        {
            Step();
            // var vis = new Grid<int>(_width, _height, _grid.All().Select(x => x!.Count), -1);
            // Console.WriteLine(vis);
        }
        
        Console.WriteLine(SafetyScore());
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

        var workCopy = new Grid<List<Point>?>(_width, _height, elements, null);

        foreach (var cell in _grid.AllExtended())
        {
            foreach (var robot in cell.Value!)
            {
                var newPosition = cell.Position + robot;

                var wrappedX = newPosition.X;
                var wrappedY = newPosition.Y;
                if (newPosition.X < 0) wrappedX = _width + newPosition.X;
                if (newPosition.Y < 0) wrappedY = _height + newPosition.Y;
                if (newPosition.X >= _width) wrappedX %= _width;
                if (newPosition.Y >= _height) wrappedY %= _height;

                var wrappedPosition = new Point(wrappedX, wrappedY);

                var list = workCopy.At(cell.Position);
                list!.Remove(robot);

                // TODO: Verify if needed? Since list is a reference type i don't think i need to set it again
                workCopy.Set(cell.Position, list);

                var newList = workCopy.At(wrappedPosition);
                if (newList is null) throw new Exception("New List was null, you're probably out of bounds");

                // TODO: Do robots need a unique id? Seems like we do if there are robots who share velocity
                newList.Add(robot);
                workCopy.Set(wrappedPosition, newList); // TODO: Same as before
            }
        }

        _grid = workCopy;
    }

    public int SafetyScore()
    {
        // split the grid in 4 quadrants
        var halfX = (_grid.Width - 1) / 2;
        var halfY = (_grid.Height - 1) / 2;

        // nw
        int nw = 0, ne = 0, se = 0, sw = 0;
        for (var x = 0; x < _grid.Width; ++x)
        {
            for (var y = 0; y < _grid.Height; ++y)
            {
                // ignore middle lines
                if (x == (_grid.Width - 1) / 2)
                    continue;
                
                if (y == (_grid.Height - 1) / 2)
                    continue;
                
                var west = x < _grid.Width / 2;
                var north = y < _grid.Height / 2;

                var robots = _grid.At(x, y)!.Count;
                if (north && west) nw += robots;
                else if (north && !west) ne += robots;
                else if (!north && west) sw += robots;
                else if (!north && !west) se += robots;
            }
        }

        return nw * ne * se * sw;
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