namespace AdventOfCode2023.helpers;

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