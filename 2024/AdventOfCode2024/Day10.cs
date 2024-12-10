using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day10
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/day10.txt");
        const char invalid = '@';
        var elements = lines.SelectMany(c => c).Select(c => char.IsDigit(c) ? c - '0' : -1); // for example
        var grid = new Grid<int>(lines[0].Length, lines.Length, elements, invalid);

        var trailHeads = grid.AllExtended()
            .Where(cell => cell.Value is 0)
            .ToList();

        // var answer = trailHeads.Sum(trailHead => ValidTrails(grid, trailHead).Count);
        var answer = trailHeads.Sum(trailHead => DistinctValidTrails(grid, trailHead));
        Console.WriteLine(answer);
    }

    private static HashSet<Point> ValidTrails(Grid<int> grid, GridElement<int> current)
    {
        HashSet<Point> trails = [];
        foreach (var neighbour in grid.NeighboursExtended(current.Position, includeDiagonals: false))
        {
            if (neighbour.Value != current.Value + 1)
                continue;
            
            // end of trail
            if (neighbour.Value is 9)
            {
                // Console.WriteLine($"Found trail at {neighbour}, coming from {current}");
                trails.Add(neighbour.Position);
                continue;
            }
                
            // continue path from here
            // Console.WriteLine($"Continuing from {current}");
            trails.UnionWith(ValidTrails(grid, neighbour));
        }

        return trails;
    }
    
    private static int DistinctValidTrails(Grid<int> grid, GridElement<int> current)
    {
        var trails = 0;
        foreach (var neighbour in grid.NeighboursExtended(current.Position, includeDiagonals: false))
        {
            if (neighbour.Value != current.Value + 1)
                continue;
            
            // end of trail
            if (neighbour.Value is 9)
            {
                // Console.WriteLine($"Found trail at {neighbour}, coming from {current}");
                trails++;
                continue;
            }
                
            // continue path from here
            // Console.WriteLine($"Continuing from {current}");
            trails += DistinctValidTrails(grid, neighbour);
        }

        return trails;
    }
}