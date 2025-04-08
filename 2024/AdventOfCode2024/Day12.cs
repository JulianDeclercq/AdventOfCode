using AdventOfCode2024.helpers;
using Region = System.Collections.Generic.List<AdventOfCode2024.helpers.GridElement<char>>;

namespace AdventOfCode2024;

public class Day12(string inputPath)
{
    private Grid<char> _grid = new(1, 1, ['.'], '@');  
    private readonly List<Region> _regions = [];

    public int SolvePart(int part)
    {
        ParseInput();

        return part is 1
            ? _regions.Select(r => Price(r, _grid)).Sum()
            : _regions.Select(r => Price2(r, _grid)).Sum();
    }
    
    private void ParseInput()
    {
        // already populated
        if (_regions.Count > 0)
            return;

        var lines = File.ReadAllLines(inputPath);
        _grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(l => l), '@');

        HashSet<Point> visited = [];
        Region region = [];

        foreach (var element in _grid.AllExtended())
        {
            if (visited.Contains(element.Position))
                continue;

            region.Add(element);
            visited.Add(element.Position);
            TryAddNeighboursToRegion(element, _grid, region, visited);

            // add current region before moving to the next
            _regions.Add(region.ToList());
            region.Clear();
        }
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
        // it's the "outline" pretty much

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

    // counting corners is the same as counting sides
    private static int Corners(Region region, Grid<char> grid)
    {
        var corners = 0;
        foreach (var element in region)
        {
            var north = IsInRegion(grid.GetNeighbour(element.Position, Direction.North), region);
            var northEast = IsInRegion(grid.GetNeighbour(element.Position, Direction.NorthEast), region);
            var east = IsInRegion(grid.GetNeighbour(element.Position, Direction.East), region);
            var southEast = IsInRegion(grid.GetNeighbour(element.Position, Direction.SouthEast), region);
            var south = IsInRegion(grid.GetNeighbour(element.Position, Direction.South), region);
            var southWest = IsInRegion(grid.GetNeighbour(element.Position, Direction.SouthWest), region);
            var west = IsInRegion(grid.GetNeighbour(element.Position, Direction.West), region);
            var northWest = IsInRegion(grid.GetNeighbour(element.Position, Direction.NorthWest), region);

            // outside corners
            if (!north && !east) corners++;
            if (!south && !east) corners++;
            if (!south && !west) corners++;
            if (!north && !west) corners++;

            // inside corners
            if (north && east && !northEast) corners++;
            if (south && east && !southEast) corners++;
            if (south && west && !southWest) corners++;
            if (north && west && !northWest) corners++;
        }

        return corners;
    }
    
    private static bool IsInRegion(GridElement<char>? element, Region region)
    {
        if (element is null)
            return false;

        return region.Any(plant => plant.Position.Equals(element.Position));
    }

    private static int Price(Region region, Grid<char> grid) => Perimeter(region, grid) * region.Count;
    private static int Price2(Region region, Grid<char> grid) => Corners(region, grid) * region.Count;
}