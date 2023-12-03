namespace AdventOfCode2023.helpers;

public static class Helpers
{
    public static int ToInt(char c) => int.Parse(c.ToString());

    public static bool AllSmallerThan(this IEnumerable<int> collection, int target)
    {
        return collection.All(x => x < target);
    }
}

public class Point : IEquatable<Point>
{
    private static Point Normalized(Point p)
    {
        var distance = Math.Sqrt(p.X * p.X + p.Y * p.Y);
        return new Point(Convert.ToInt16(p.X / distance), Convert.ToInt16(p.Y / distance));
    }
    public static Point Direction(Point from, Point to) => Normalized(to - from);

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point(Point source)
    {
        X = source.X;
        Y = source.Y;
    }

    public readonly int X, Y;
    
    public override string ToString() => $"{X},{Y}";
    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);

    #region IEquatable
    
    public bool Equals(Point? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override bool Equals(object? obj) => Equals(obj as Point);
    
    #endregion
}

public class Point3D : IEquatable<Point3D>
{
    public Point3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Point3D(Point3D source)
    {
        X = source.X;
        Y = source.Y;
        Z = source.Z;
    }

    public readonly int X, Y, Z;
    
    public override string ToString() => $"{X},{Y},{Z}";
    public static Point3D operator +(Point3D a, Point3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Point3D operator -(Point3D a, Point3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    #region IEquatable
    
    public bool Equals(Point3D? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public override bool Equals(object? obj) => Equals(obj as Point3D);
    
    #endregion
}