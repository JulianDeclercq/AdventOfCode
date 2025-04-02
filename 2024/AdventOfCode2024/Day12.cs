using AdventOfCode2024.helpers;
using Region = System.Collections.Generic.List<AdventOfCode2024.helpers.GridElement<char>>;

namespace AdventOfCode2024;

public class Day12
{
    public static void Solve(int part = 1)
    {
        var lines = File.ReadAllLines("input/day12e5.txt");
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

        Console.WriteLine(part is 1
            ? $"Total price part 1 {regions.Select(r => Price(r, grid)).Sum()}"
            : $"Total price part 2 {regions.Select(r => Price2(r, grid)).Sum()}");

        if (part is 2) // 865044 is too low
        {
            foreach (var wegion in regions)
            {
                var p = Price2(wegion, grid);
                Console.WriteLine($"{wegion.First().Value}, corners: {Corners(wegion, grid)}, price for part 2 is {p}");
            }
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

// #sides = #corners! :D
    private static int Corners(Region region, Grid<char> grid)
    {
        if (region.First().Value is 'C')
        {
            // C is currently wrong with EXACTLY a factor of 1.5x
            int bkpt = 5;
        }

        var corners = 0;
        HashSet<Point> cornersHashset = [];
        foreach (var element in region)
        {
            // var diagonals = grid.DiagonalNeighboursExtended(element.Position, false);
            List<GridElement<char>?> topLeft =
            [
                grid.GetNeighbour(element.Position, Direction.West),
                grid.GetNeighbour(element.Position, Direction.NorthWest),
                grid.GetNeighbour(element.Position, Direction.North),
            ];
            var topLeftDifferentCount = topLeft.Count(x => x is not null && x.Value != region.First().Value);
            if (topLeftDifferentCount is 1 or 3 || topLeft.All(x => x is null))
            {
                corners++;
                var cp = GetCornerPoint(element.Position, Direction.NorthWest);
                if (cp.X is 2 && cp.Y is 2)
                {
                    var bkpt = 5;
                }
                cornersHashset.Add(cp);
            }

            List<GridElement<char>?> topRight =
            [
                grid.GetNeighbour(element.Position, Direction.North),
                grid.GetNeighbour(element.Position, Direction.NorthEast),
                grid.GetNeighbour(element.Position, Direction.East),
            ];
            var topRightDifferentCount = topRight.Count(x => x is not null && x.Value != region.First().Value);
            if (topRightDifferentCount is 1 or 3 || topRight.All(x => x is null))
            {
                corners++;
                var cp = GetCornerPoint(element.Position, Direction.NorthEast);
                if (cp.X is 2 && cp.Y is 2)
                {
                    var bkpt = 5;
                }
                cornersHashset.Add(cp);
            }

            List<GridElement<char>?> bottomRight =
            [
                grid.GetNeighbour(element.Position, Direction.East),
                grid.GetNeighbour(element.Position, Direction.SouthEast),
                grid.GetNeighbour(element.Position, Direction.South),
            ];
            var bottomRightDifferentCount = bottomRight.Count(x => x is not null && x.Value != region.First().Value);
            if (bottomRightDifferentCount is 1 or 3 || bottomRight.All(x => x is null))
            {
                corners++;
                var cp = GetCornerPoint(element.Position, Direction.SouthEast);
                if (cp.X is 2 && cp.Y is 2)
                {
                    var bkpt = 5;
                }
                cornersHashset.Add(cp);
            }

            List<GridElement<char>?> bottomLeft =
            [
                grid.GetNeighbour(element.Position, Direction.South),
                grid.GetNeighbour(element.Position, Direction.SouthWest),
                grid.GetNeighbour(element.Position, Direction.West),
            ];
            var bottomLeftDifferentCount = bottomLeft.Count(x => x is not null && x.Value != region.First().Value);
            if (bottomLeftDifferentCount is 1 or 3 || bottomLeft.All(x => x is null))
            {
                corners++;
                var cp = GetCornerPoint(element.Position, Direction.SouthWest);
                if (cp.X is 2 && cp.Y is 2)
                {
                    var bkpt = 5;
                }
                cornersHashset.Add(cp);
            }
        }

        // return corners;
        return cornersHashset.Count;
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