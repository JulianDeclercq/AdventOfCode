namespace AdventOfCode2023.helpers;

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
