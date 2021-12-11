using System.Text.RegularExpressions;

namespace AdventOfCode2021;

public static class Helpers
{
    public static int ToInt(Group m) => int.Parse(m.Value);
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

    public readonly int X, Y;
    
    public override string ToString() => $"{X}, {Y}";
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

public class Grid<T>
{
    public Grid(int width, int height, T invalid)
    {
        if (width < 1 || height < 1)
            throw new Exception("Can't create grid with no rows or columns");

        Width = width;
        Height = height;
        _invalid = invalid;
    }

    public void AddRange(IEnumerable<T> tRange) => _cells.AddRange(tRange);
    public IEnumerable<T> All() => _cells;
    public T At(Point p) => At(p.X, p.Y);
    public T At(int x, int y) => Get(x, y);

    private T Get(int x, int y)
    {
        var idx = Index(x, y);
        return idx == -1 ? _invalid : _cells[idx];
    }
    
    private int Index(int x, int y)
    {
        if (x < 0 || x > Width - 1) return -1;
        if (y < 0 || y > Height - 1) return -1;

        return y * Width + x;
    }

    public void ModifyAt(Point p, Func<T, T> modifier) => ModifyAt(p.X, p.Y, modifier);

    private void ModifyAt(int x, int y, Func<T, T> modifier)
    {
        // calculate the result of the function on the original element and override its value in the list 
        _cells[Index(x, y)] = modifier(Get(x, y));
    }

    // doesn't support wrapping
    public IEnumerable<T> Neighbours(int x, int y, bool includeDiagonals = true)
    {
        var neighbours = new List<T>()
        {
            Get(x, y - 1),
            Get(x + 1, y),
            Get(x, y + 1),
            Get(x - 1, y)
        };

        if (includeDiagonals)
        {
            neighbours.Add(Get(x - 1, y - 1));
            neighbours.Add(Get(x + 1, y - 1));
            neighbours.Add(Get(x + 1, y + 1));
            neighbours.Add(Get(x - 1, y + 1));
        }

        return neighbours.Where(n => n != null && !n.Equals(_invalid));
    }

    public IEnumerable<Point> NeighbouringPoints(Point p, bool includeDiagonals = true)
    {
        var neighbours = new List<Point>()
        {
            new (p.X, p.Y - 1),
            new (p.X + 1, p.Y),
            new (p.X, p.Y + 1),
            new (p.X - 1, p.Y)
        };

        if (includeDiagonals)
        {
            neighbours.Add(new Point(p.X - 1, p.Y - 1));
            neighbours.Add(new Point(p.X + 1, p.Y - 1));
            neighbours.Add(new Point(p.X + 1, p.Y + 1));
            neighbours.Add(new Point(p.X - 1, p.Y + 1));
        }

        return neighbours.Where(ValidPoint);
    }

    // includes line start and line end
    public IEnumerable<Point> PointsOnLine(Point lineStart, Point lineEnd, bool includeDiagonals = true)
    {
        // direction
        var dir = Point.Direction(lineStart, lineEnd);

        // if the direction is diagonal and diagonals should be excluded, return an empty list
        if (!includeDiagonals && dir.X != 0 && dir.Y != 0)
            return Enumerable.Empty<Point>();
        
        // accumulate the points
        var points = new List<Point>(){lineStart};

        var current = lineStart;
        while (!current.Equals(lineEnd))
        {
            current += dir;
            points.Add(current);
        }

        return points;
    }

    // checks if the point is within bounds
    private bool ValidPoint(Point point) 
        => point.X >= 0 && point.X <= Width - 1 && point.Y >= 0 && point.Y <= Height - 1;

    public int Width { get; }
    public int Height { get; }
    private readonly T _invalid;
    private readonly List<T> _cells = new List<T>();

    public override string ToString()
    {
        var s = string.Empty;
        for (var i = 0; i < _cells.Count; ++i)
        {
            if (i != 0 && i % Width == 0)
                s += '\n';
            
            s += _cells[i];
        }

        return s += '\n';
    }
}
