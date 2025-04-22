using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day10
{
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        var lines = File.ReadAllLines("input/real/day10.txt");
        const char invalid = '@';
        
        // handle non-digits to try out the examples that have '.'
        var elements = lines.SelectMany(c => c).Select(c => char.IsDigit(c) ? c - '0' : -1);
        var grid = new Grid<int>(lines[0].Length, lines.Length, elements, invalid);

        var trailHeads = grid.AllExtended()
            .Where(cell => cell.Value is 0)
            .ToList();

        var answer = part is 1
            ? trailHeads.Sum(trailHead => ValidTrails(grid, trailHead).Count)
            : trailHeads.Sum(trailHead => DistinctValidTrails(grid, trailHead));
        
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
                trails.Add(neighbour.Position);
                continue;
            }
                
            // continue path from here
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
                trails++;
                continue;
            }
                
            // continue path from here
            trails += DistinctValidTrails(grid, neighbour);
        }

        return trails;
    }
}