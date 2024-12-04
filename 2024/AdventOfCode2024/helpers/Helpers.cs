namespace AdventOfCode2024.helpers;

public static class Helpers
{
    public static int ToInt(char c) => int.Parse(c.ToString());

    public static bool AllSmallerThan(this IEnumerable<int> collection, int target)
    {
        return collection.All(x => x < target);
    }
    
    public static void AssertEqual<T>(T expected, T value)
    {
        if (!value.Equals(expected))
            throw new Exception($"got: {value}, expected: {expected}");
    }

    public static Direction CalculateDirection(Point from, Point to)
    {
        return (to - from) switch
        {
            { X: 0, Y: -1 } => Direction.North,
            { X: 1, Y: 0 } => Direction.East,
            { X: 0, Y: 1 } => Direction.South,
            { X: -1, Y: 0 } => Direction.West,
            _ => throw new Exception("Couldn't determine direction")
        };
    }
}

public enum Direction
{
    None = 0,
    North = 1,
    East = 2,
    South = 3,
    West = 4
}