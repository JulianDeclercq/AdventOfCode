namespace AdventOfCode2023.helpers;

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
}