using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day6
{
    public static void Solve()
    {
        var lines = File.ReadAllLines("input/day6.txt");
        const char invalid = '@', guard = '^', empty = '.', wall = '#';
        var grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), invalid);

        var current = grid.AllExtended().Single(cell => cell.Value is guard)!;
        var direction = Direction.North;
        HashSet<Point> visited = [];
        for (;;)
        {
            var next = grid.GetNeighbour(current.Position, direction);
            if (next == null)
            {
                visited.Add(current.Position);
                break;
            }
            
            switch (next.Value)
            {
                case wall:
                    // currently not changing the visual direction of the arrow
                    direction = NextClockwise(direction);
                    break;
                case empty:
                    grid.Set(next.Position, guard);  
                    grid.Set(current.Position, empty);
                    visited.Add(current.Position);
                    current = next;
                    break;
                default: throw new Exception($"Unhandled case {next.Value}");
            }
            // Console.WriteLine(grid);
        }
        Console.WriteLine(visited.Count);
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