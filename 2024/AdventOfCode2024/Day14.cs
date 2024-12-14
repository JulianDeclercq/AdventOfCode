using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day14
{
    public static void Solve()
    {
        const bool example = true;
        var lines = File.ReadAllLines(example ? "input/day14e.txt" : "input/day14.txt");
        const int width = example ? 11 : 101;
        const int height = example ? 7 : 103;
        
        // a robot will be a Point, which is its velocity. Position will be handled by the grid
        // nullable for invalid, not sure if good idea
        List<List<Point>> elements = [];
        // with Enumerable.Repeat you'll get the same reference to the same (empty) list
        for (var i = 0; i < width; ++i)
        {
            for (var j = 0; j < height; ++j)
            {
                elements.Add([]);
            }
        }
        
        var grid = new Grid<List<Point>?>(width, height, elements, null); 
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
        PrintGrid(grid);
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