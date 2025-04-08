using AdventOfCode2024.helpers;
using Region = System.Collections.Generic.List<AdventOfCode2024.helpers.GridElement<char>>;

namespace AdventOfCode2024;

public class Day12(string inputPath)
{
    public Grid<char> Grid;
    public readonly List<Region> Regions = [];

    private void PopulateRegionsFromGrid()
    {
        // already populated
        if (Regions.Count > 0)
            return;
        
        // var lines = File.ReadAllLines("input/day12e5.txt");
        var lines = File.ReadAllLines(inputPath);
        Grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(l => l), '@');

        HashSet<Point> visited = [];
        Region region = [];

        foreach (var element in Grid.AllExtended())
        {
            if (visited.Contains(element.Position))
                continue;

            region.Add(element);
            visited.Add(element.Position);
            TryAddNeighboursToRegion(element, Grid, region, visited);

            // add current region before moving to the next
            Regions.Add(region.ToList());
            region.Clear();
        }

        // foreach (var r in regions)
        //     Console.WriteLine($"{r.First().Value} perimeter: {Perimeter(r, grid)}, area: {r.Count}");
    }

    public int SolvePart(int part)
    {
        PopulateRegionsFromGrid();
        
        // var answer = part is 1
        //     ? Regions.Select(r => Price(r, Grid)).Sum()
        //     : Regions.Select(r => Price2(r, Grid, visualisation)).Sum();
        
        // Console.WriteLine($"Total price for part {part} is {answer}");

        int answer = 0;
        if (part is 2) // 865044 is too low
        {
            foreach (var region in Regions)
            {
                var p = Price2(region, Grid);
                Console.WriteLine($"{region.First().Value}," +
                                  $" corners: {Corners(region, Grid)}," +
                                  $" price for part 2 is {p}");
                answer += p;
            }
        }
        
        // looks like the absolute middle of day12e5 is not being counted, which would give region A 2 more corners,
        // and both region B's 1 more. This would add up to the correct result!
        Console.WriteLine($"Answer is {answer}");
        return answer;
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

    private static bool IsInRegion(GridElement<char>? element, Region region)
    {
        if (element is null)
            return false;
                
        return region.Any(plant => plant.Position.Equals(element.Position));
    }

    // #sides = #corners! :D
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

            if (!north && !east) corners++;
            if (!south && !east) corners++;
            if (!south && !west) corners++;
            if (!north && !west) corners++;

            if (north && east && !northEast) corners++;
            if (south && east && !southEast) corners++;
            if (south && west && !southWest) corners++;
            if (north && west && !northWest) corners++;
        }

        return corners;
    }

    private static Point GetCornerPoint(Point origin, Direction direction)
    {
        return direction switch
        {
            Direction.NorthEast => origin + new Point(1, 0),
            Direction.SouthEast => origin + new Point(1, 1),
            Direction.SouthWest => origin + new Point(0, 1),
            Direction.NorthWest => origin + new Point(0, 0),
            _ => throw new Exception($"Invalid direction {direction}")
        };
    }

    private static int Price(Region region, Grid<char> grid) => Perimeter(region, grid) * region.Count;
    private static int Price2(Region region, Grid<char> grid) => Corners(region, grid) * region.Count;
}