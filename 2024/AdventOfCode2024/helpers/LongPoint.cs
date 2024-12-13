namespace AdventOfCode2024.helpers;

public class LongPoint : IEquatable<LongPoint>
{
    private static LongPoint Normalized(LongPoint p)
    {
        var distance = Math.Sqrt(p.X * p.X + p.Y * p.Y);
        return new LongPoint(Convert.ToInt64(p.X / distance), Convert.ToInt64(p.Y / distance));
    }
    public static LongPoint Direction(LongPoint from, LongPoint to) => Normalized(to - from);

    public LongPoint(long x, long y)
    {
        X = x;
        Y = y;
    }

    public LongPoint(LongPoint source)
    {
        X = source.X;
        Y = source.Y;
    }

    public readonly long X, Y;
    
    public override string ToString() => $"{X},{Y}";
    public static LongPoint operator +(LongPoint a, LongPoint b) => new(a.X + b.X, a.Y + b.Y);
    public static LongPoint operator -(LongPoint a, LongPoint b) => new(a.X - b.X, a.Y - b.Y);

    #region IEquatable
    
    public bool Equals(LongPoint? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override bool Equals(object? obj) => Equals(obj as LongPoint);
    
    #endregion
}
