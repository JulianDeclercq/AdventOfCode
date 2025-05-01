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

    public static Direction CalculateDirection(Point from, Point to)
    {
        return (to - from) switch
        {
            { X: 0, Y: -1 } => Direction.North,
            { X: 1, Y: 0 } => Direction.East,
            { X: 0, Y: 1 } => Direction.South,
            { X: -1, Y: 0 } => Direction.West,
            _ => throw new Exception("Couldn't determine direction, only the 4 main ones are implement atm")
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