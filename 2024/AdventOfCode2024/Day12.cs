using AdventOfCode2024.helpers;

using Region = System.Collections.Generic.List<AdventOfCode2024.helpers.GridElement<char>>;

namespace AdventOfCode2024;

public class Day12
{
    public void Solve()
    {
        var lines = File.ReadAllLines("input/day12.txt");
        var grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(l => l), '@');

        HashSet<Point> visited = [];
        List<Region> regions = [];
        Region region = [];
        
        foreach (var element in grid.AllExtended())
        {
            if (visited.Contains(element.Position))
                continue;
            
            region.Add(element);
            visited.Add(element.Position);
            TryAddNeighboursToRegion(element, grid, region, visited);
            
            // add current region before moving to the next
            regions.Add(region.ToList());
            region.Clear();
        }
        
        // foreach (var r in regions)
        //     Console.WriteLine($"{r.First().Value} perimeter: {Perimeter(r, grid)}, area: {r.Count}");
        
        Console.WriteLine($"Total price {regions.Select(r => Price(r, grid)).Sum()}");
    }

    private static void TryAddNeighboursToRegion(
        GridElement<char> element,
        Grid<char> grid,
        Region region,
        HashSet<Point> visited)
    {
        if (region.Count == 0)
            throw new Exception("Region to try to add to can't be empty");
        
        foreach (var neighbour in grid.NeighboursExtended(element.Position, includeDiagonals: false))
        {
            if (visited.Contains(neighbour.Position))
                continue; 
            
            if (neighbour.Value == region.First().Value)
            {
                region.Add(neighbour);
                visited.Add(neighbour.Position);
                TryAddNeighboursToRegion(neighbour, grid, region, visited);
            }
        }
    }

    private static int Perimeter(Region region, Grid<char> grid)
    {
        // The perimeter of a region is the number of sides of garden plots in the region that
        // do not touch another garden plot in the same region.

        var perimeter = 0;
        foreach (var element in region)
        {
            foreach (var neighbour in grid.NeighboursExtended(element.Position, false, false))
            {
                if (neighbour.Value != region.First().Value)
                    perimeter++;
            }
        }

        return perimeter;
    }

    private static int Price(Region region, Grid<char> grid) => Perimeter(region, grid) * region.Count;
}