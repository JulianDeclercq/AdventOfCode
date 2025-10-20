namespace AdventOfCode2024.helpers;

public static class Helpers
{
    // returns the int 1 as '1', 2 as '2' etc.
    public static char AsChar(int n)
    {
        if (n is < 0 or > 10)
            throw new Exception($"i needs to be between single character, got {n}");

        return (char)('0' + n);
    }

    public static int ToInt(char c) => int.Parse(c.ToString());

    public static Direction? CalculateCardinalDirection(Point from, Point to)
    {
        return (to - from) switch
        {
            { X: 0, Y: -1 } => Direction.North,
            { X: 1, Y: 0 } => Direction.East,
            { X: 0, Y: 1 } => Direction.South,
            { X: -1, Y: 0 } => Direction.West,
            _ => null // Return null if the direction is not cardinal
        };
    }

    public static Point DirectionToPoint(Direction direction)
    {
        return direction switch
        {
            Direction.North => new Point(0, -1),
            Direction.East => new Point(1, 0),
            Direction.South => new Point(0, 1),
            Direction.West => new Point(-1, 0),
            _ => throw new Exception("Couldn't determine direction point, only the 4 main ones are implement atm")
        };
    }
}

public enum Direction
{
    None = 0,
    North = 1,
    East = 2,
    South = 3,
    West = 4,
    NorthEast = 5,
    SouthEast = 6,
    SouthWest = 7,
    NorthWest = 8
}

public class Path
{
    public required Point CurrentPosition { get; set; }
    public Direction CurrentDirection { get; set; }
    public HashSet<Point> Visited { get; init; } = [];
    public int Score { get; set; }

    public string VisualiseOnGrid(Grid<char> grid, Point? position = null, Direction? direction = null)
    {
        var copy = grid.ShallowCopy();

        foreach (var point in Visited)
            copy.Set(point, '@');

        if (position is not null && direction is not null)
        {
            var target = direction switch
            {
                Direction.North => 'N',
                Direction.East => 'E',
                Direction.South => 'S',
                Direction.West => 'W',
                _ => throw new Exception("Invalid direction inside VisualiseOnGrid")
            };

            copy.Set(position, target);
        }

        return copy.ToString();
    }

    public Path Copy()
    {
        return new Path
        {
            CurrentPosition = Point.Copy(CurrentPosition),
            CurrentDirection = CurrentDirection,
            Visited = Visited.ToHashSet(),
            Score = Score,
        };
    }
}
