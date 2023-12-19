namespace AdventOfCode2023.helpers;

public class BigPoint : IEquatable<BigPoint>
{
    public BigPoint(long x, long y)
    {
        X = x;
        Y = y;
    }

    public BigPoint(BigPoint source)
    {
        X = source.X;
        Y = source.Y;
    }

    public readonly long X, Y;
    
    public override string ToString() => $"{X},{Y}";
    public static BigPoint operator +(BigPoint a, BigPoint b) => new(a.X + b.X, a.Y + b.Y);
    public static BigPoint operator -(BigPoint a, BigPoint b) => new(a.X - b.X, a.Y - b.Y);

    #region IEquatable
    
    public bool Equals(BigPoint? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override bool Equals(object? obj) => Equals(obj as BigPoint);
    
    #endregion
}