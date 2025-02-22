using AdventOfCode2024.helpers;

using Region = System.Collections.Generic.List<AdventOfCode2024.helpers.GridElement<char>>;

namespace AdventOfCode2024;

public class Day12
{
    public void Solve()
    {
        var lines = File.ReadAllLines("input/day12e.txt");
        var grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(l => l), '@');
        Console.WriteLine(grid);

        HashSet<Point> visited = [];
        List<Region> regions = [];
        Region region = [];
        
        foreach (var element in grid.AllExtended())
        {
            if (visited.Contains(element.Position))
                continue;

            if (region.Count > 0)
                throw new Exception("didnt expect this");
            
            region.Add(element);
            visited.Add(element.Position);
            TryAddNeighboursToRegion(element, grid, region, visited);
            
            // add current region before moving to the next
            regions.Add(region.ToList());
            region.Clear();
        }
        
        Console.WriteLine(regions.Count);
        foreach (var lel in regions)
            Console.WriteLine(string.Join(",", lel));
    }

    private static void TryAddNeighboursToRegion(
        GridElement<char> element,
        Grid<char> grid,
        Region region,
        HashSet<Point> visited)
    {
        if (region.Count == 0)
            throw new Exception("Region to try to add to can't be empty");
        
        foreach (var neighbour in grid.NeighboursExtended(element.Position))
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
}