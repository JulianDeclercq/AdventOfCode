using AdventOfCode2023.helpers;
using Direction = AdventOfCode2023.helpers.Helpers.Direction;

namespace AdventOfCode2023.days;

public abstract class Day16
{
    public static void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day16.txt");
        var width = input.First().Length;
        var height = input.Length;
        var grid = new Grid<char>(width, height, input.SelectMany(x => x), '?');
        var energy = new Grid<int>(width, height, Enumerable.Repeat(0, width * height), 1337);

        var starts = new List<(Point, Direction)>();
        var rows = grid.Rows().ToArray();
        var columns = grid.Columns().ToArray();
        starts.AddRange(rows.First().Select(top => (new Point(top.Position.X, top.Position.Y - 1), Direction.South)));
        starts.AddRange(rows.Last().Select(bottom => (new Point(bottom.Position.X, bottom.Position.Y + 1), Direction.North)));
        starts.AddRange(columns.First().Select(left => (new Point(left.Position.X - 1, left.Position.Y), Direction.East)));
        starts.AddRange(columns.Last().Select(right => (new Point(right.Position.X + 1, right.Position.Y), Direction.West)));

        Console.WriteLine(starts.Select(p => TilesEnergized(grid.ShallowCopy(), energy.ShallowCopy(), p.Item1, p.Item2)).Max());     
    }

    private static int TilesEnergized(Grid<char> grid, Grid<int> energy, Point start, Direction initialDirection)
    {
        TraverseBeamPath(grid, energy, start, initialDirection, new HashSet<Point>());
        return energy.All().Count(e => e != 0);
    }

    private static void TraverseBeamPath(Grid<char> grid, Grid<int> energy, Point start, Direction initialDirection,
        HashSet<Point> usedSplitters)
    {
        var position = start;
        var direction = initialDirection;
        var goHard = true;
        while (goHard)
        {
            var current = energy.At(position);
            energy.Set(position, current + 1);

            switch (direction)
            {
                case Direction.North:
                    var northernNeighbour = grid.GetNeighbour(position, Direction.North);
                    if (northernNeighbour == null)
                    {
                        goHard = false;
                        break;
                    }
                    
                    position = northernNeighbour.Position;
                    switch (northernNeighbour.Value)
                    {
                        case '/':
                            direction = Direction.East;
                            break;
                        case '\\':
                            direction = Direction.West;
                            break;
                        case '-':
                            goHard = false;
                            SplitHorizontal(grid, energy, position, usedSplitters);
                            break;
                    }
                    break;

                case Direction.East:
                    var easternNeighbour = grid.GetNeighbour(position, Direction.East);
                    if (easternNeighbour == null)
                    {
                        goHard = false;
                        break;
                    }
                    
                    position = easternNeighbour.Position;
                    switch (easternNeighbour.Value)
                    {
                        case '/': 
                            direction = Direction.North;
                            break;
                        case '\\': 
                            direction = Direction.South;
                            break;
                        case '|':
                            goHard = false;
                            SplitVertical(grid, energy, position, usedSplitters);
                            break;
                    }
                    break;
                case Direction.South:
                    var southernNeighbour = grid.GetNeighbour(position, Direction.South);
                    if (southernNeighbour == null)
                    {
                        goHard = false;
                        break;
                    }

                    position = southernNeighbour.Position;
                    switch (southernNeighbour.Value)
                    {
                        case '/':
                            direction = Direction.West;
                            break;
                        case '\\':
                            direction = Direction.East;
                            break;
                        case '-':
                            goHard = false;
                            SplitHorizontal(grid, energy, position, usedSplitters);
                            break;
                    }
                    break;
                case Direction.West:
                    var westernNeighbour = grid.GetNeighbour(position, Direction.West);
                    if (westernNeighbour == null)
                    {
                        goHard = false;
                        break;
                    }

                    position = westernNeighbour.Position;
                    switch (westernNeighbour.Value)
                    {
                        case '/':
                            direction = Direction.South;
                            break;
                        case '\\':
                            direction = Direction.North;
                            break;
                        case '|':
                            goHard = false;
                            SplitVertical(grid, energy, position, usedSplitters);
                            break;
                    }
                    break;
                default: throw new Exception($"Invalid direction {direction}");
            }
        }
    }

    private static void SplitVertical(Grid<char> grid, Grid<int> energy, Point position, HashSet<Point> usedSplitters)
    {
        if (usedSplitters.Contains(position))
            return;
                            
        usedSplitters.Add(position);

        var northernNeighbour = grid.GetNeighbour(position, Direction.North);
        if (northernNeighbour != null)
            TraverseBeamPath(grid, energy, position, Direction.North, usedSplitters);

        var southernNeighbour = grid.GetNeighbour(position, Direction.South);
        if (southernNeighbour != null)
            TraverseBeamPath(grid, energy, position, Direction.South, usedSplitters);

    }
    
    private static void SplitHorizontal(Grid<char> grid, Grid<int> energy, Point position, HashSet<Point> usedSplitters)
    {
        if (usedSplitters.Contains(position))
            return;
                            
        usedSplitters.Add(position);

        var easternNeighbourS = grid.GetNeighbour(position, Direction.East);
        if (easternNeighbourS != null)
            TraverseBeamPath(grid, energy, position, Direction.East, usedSplitters);

        var westernNeighbourS = grid.GetNeighbour(position, Direction.West);
        if (westernNeighbourS != null)
            TraverseBeamPath(grid, energy, position, Direction.West, usedSplitters);
    }
}