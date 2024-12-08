using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day6
{
    private const char Invalid = '@', Guard = '^', Empty = '.', Obstacle = '#';
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        var lines = File.ReadAllLines("input/day6.txt");
        var grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), Invalid);
        var start = grid.AllExtended().Single(cell => cell.Value is Guard).Position;

        if (part is 1)
        {
            Part1(grid, start);
            return;
        }

        // Part 2
        List<Grid<char>> possibilities = [];
        foreach (var cell in grid.AllExtended())
        {
            if (cell.Value is not Empty)
                continue;
            
            var copy = grid.ShallowCopy();
            copy.Set(cell.Position, Obstacle);
            possibilities.Add(copy);
        }

        Console.WriteLine(possibilities.Count(p => IsLoop(p, start)));
    }

    private static void Part1(Grid<char> grid, Point start)
    {
        var current = start;
        var direction = Direction.North;
        HashSet<Point> visited = [];
        for (;;)
        {
            var next = grid.GetNeighbour(current, direction);
            if (next == null)
            {
                visited.Add(current);
                break;
            }

            switch (next.Value)
            {
                case Obstacle:
                    // currently not changing the visual direction of the arrow
                    direction = NextClockwise(direction);
                    break;
                case Empty:
                    grid.Set(next.Position, Guard);
                    grid.Set(current, Empty);
                    visited.Add(current);
                    current = next.Position;
                    break;
                default: throw new Exception($"Unhandled case {next.Value}");
            }
        }

        Console.WriteLine(visited.Count);
    }

    private static bool IsLoop(Grid<char> grid, Point start)
    {
        var current = start;
        var direction = Direction.North;
        Dictionary<Point, Direction> visitedExt = [];
        for (;;)
        {
            if (visitedExt.TryGetValue(current, out var visitedDirection))
            {
                if (visitedDirection == direction) // loop detected
                    return true;
            }
            
            var next = grid.GetNeighbour(current, direction);
            if (next == null)
            {
                visitedExt.TryAdd(current, direction);
                break;
            }
            
            switch (next.Value)
            {
                case Obstacle:
                    // currently not changing the visual direction of the arrow
                    direction = NextClockwise(direction);
                    break;
                case Empty:
                    grid.Set(next.Position, Guard);  
                    grid.Set(current, Empty);
                    
                    visitedExt.TryAdd(current, direction);
                        
                    current = next.Position;
                    
                    break;
                default: throw new Exception($"Unhandled case {next.Value}");
            }
            // Console.WriteLine(grid);
        }

        return false;
    }

    private static Direction NextClockwise(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new Exception($"Invalid direction {direction}"),
        };
    }
}