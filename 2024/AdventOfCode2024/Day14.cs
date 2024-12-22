using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day14
{
    private const bool Example = true;
    private const int Width = Example ? 11 : 101;
    private const int Height = Example ? 7 : 103;
    
    public static void Solve()
    {
        // var lines = File.ReadAllLines(Example ? "input/day14e.txt" : "input/day14.txt");
        List<string> lines = ["p=2,4 v=2,-3"];
        
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
        
        var grid = new Grid<List<Point>?>(Width, Height, elements, null); 
        var regex = new RegexHelper(new Regex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)"), "px", "py", "vx", "vy");
        foreach (var line in lines)
        {
            regex.Match(line);
            var position = new Point(regex.GetInt("px"), regex.GetInt("py"));
            var velocity = new Point(regex.GetInt("vx"), regex.GetInt("vy"));

            var current = grid.At(position) ?? [];
            current.Add(velocity);
            grid.Set(position, current);
        }
        var visualisation = new Grid<int>(Width, Height, grid.All().Select(x => x!.Count), -1); // long?
        PrintGrid(grid);
        Console.WriteLine(visualisation);

        const int steps = 1;
        for (var i = 0; i < steps; ++i)
        {
            grid = Step(grid);
            PrintGrid(grid);
            var vis = new Grid<int>(Width, Height, grid.All().Select(x => x!.Count), -1);
            Console.WriteLine(vis);
        }
    }

    private static Grid<List<Point>?> Step(Grid<List<Point>?> grid)
    {
        // deep copy the grid, you don't want to be working on the grid while changing it around
        List<List<Point>?> elements = [];
        foreach (var cell in grid.AllExtended())
        {
            elements.Add(cell.Value!.ToList());
            // be careful here I'm not sure if Point being a reference type is a problem
            // don't think so though since I don't change the point itself, just moving it around
        }
        var workCopy = new Grid<List<Point>?>(Width, Height, elements, null);

        foreach (var cell in grid.AllExtended())
        {
            foreach (var robot in cell.Value!)
            {
                var newPosition = cell.Position + robot;

                // negative wrapping isn't handled by modulo
                var wrappedX = 0;
                var wrappedY = 0;
                if (newPosition.X < 0)
                    wrappedX = Width - newPosition.X;
                if (newPosition.Y < 0)
                    wrappedY = Height - newPosition.Y;
                
                // var wrappedPosition = new Point(newPosition.X % Width, newPosition.Y % Height);
                var wrappedPosition = new Point(wrappedX % Width, wrappedY % Height);

                var list = workCopy.At(cell.Position);
                list!.Remove(robot);
                workCopy.Set(cell.Position, list); // TODO: Verify if needed? Since list is a reference type i don't think i need to set it again

                var newList = workCopy.At(wrappedPosition);
                if (newList is null)
                    throw new Exception("Failed to get NewList");
                
                newList.Add(robot);
                workCopy.Set(wrappedPosition, list); // TODO: Same as before
            }
        }

        return workCopy;
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