using System.Text;
using System.Text.RegularExpressions;
namespace AdventOfCode2021;

public static class Helpers
{
    // for syntactic sugar in linQ expressions
    public static int ToInt(char c) => int.Parse(char.ToString(c));
    public static int ToInt(Group m) => int.Parse(m.Value);
    public static bool InRangeInclusive(int min, int max, int value) => value >= min && value <= max;
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

public class Line
{
    public Line(Point start, Point end)
    {
        _start = start;
        _end = end;
        _direction = Point.Direction(_start, _end);
        Points = CalculatePoints();
    }

    private readonly Point _start;
    private readonly Point _end;
    private readonly Point _direction;
    public readonly IEnumerable<Point> Points;
    public bool IsDiagonal => _direction.X != 0 && _direction.Y != 0;
    
    private IEnumerable<Point> CalculatePoints()
    {
        var points = new List<Point>(){_start};

        // accumulate the points
        var current = _start;
        while (!current.Equals(_end))
        {
            current += _direction;
            points.Add(current);
        }
        return points;
    }
}

public class Grid<T>
{
    public Grid(int width, int height, IEnumerable<T> elements, T invalid)
    {
        if (width < 1 || height < 1)
            throw new Exception("Can't create grid with no rows or columns");

        Width = width;
        Height = height;
        _cells.AddRange(elements);
        _invalid = invalid;
    }

    // doesn't deep copy the values in _cells if T is a reference type
    public Grid<T> ShallowCopy() => new (Width, Height, _cells, _invalid);
    public void AddRange(IEnumerable<T> tRange) => _cells.AddRange(tRange);   
    public T At(Point p) => At(p.X, p.Y);
    public T At(int x, int y) => Get(x, y);
    public T At(int idx) => _cells[idx];

    public bool Set(Point p, T value) => Set(p.X, p.Y, value);
    public bool Set(int x, int y, T value)
    {
        var idx = Index(x, y);
        if (idx == -1)
            return false;

        _cells[idx] = value;
        return true;
    }

    public void Set(int idx, T value) => _cells[idx] = value;  
    
    public IEnumerable<T> All() => _cells;

    // returns a dictionary with key = location
    public Dictionary<Point, T> AllExtended()
    {
        var extended = new Dictionary<Point, T>();

        for (var i = 0; i < _cells.Count; ++i)
            extended.Add(FromIndex(i), _cells[i]);

        return extended;
    }
    
    private T Get(int x, int y)
    {
        var idx = Index(x, y);
        return idx == -1 ? _invalid : _cells[idx];
    }

    public Point FromIndex(int idx) => new (idx % Width, idx / Width);

    public int Index(Point p) => Index(p.X, p.Y);
    private int  Index(int x, int y)
    {
        if (x < 0 || x > Width - 1) return -1;
        if (y < 0 || y > Height - 1) return -1;

        return y * Width + x;
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

    // checks if the point is within bounds
    private bool ValidPoint(Point point) 
        => point.X >= 0 && point.X <= Width - 1 && point.Y >= 0 && point.Y <= Height - 1;
    
    public int Width { get; }
    public int Height { get; }
    private readonly T _invalid;
    private readonly List<T> _cells = new();

    public override string ToString()
    {
        var s = string.Empty;
        for (var i = 0; i < _cells.Count; ++i)
        {
            if (i != 0 && i % Width == 0)
                s += '\n';
            
            s += _cells[i]?.ToString();
        }

        return s += '\n';
    }
}

public class NegativeGrid<T>
{
    public NegativeGrid(int width, int height, Point origin, IEnumerable<T> elements)
    {
        if (width < 1 || height < 1)
            throw new Exception("Can't create grid with no rows or columns");

        Width = width;
        Height = height;
        Origin = origin;
        _cells.AddRange(elements);
    }
    
    public NegativeGrid<T> ShallowCopy() => new (Width, Height, Origin, _cells);

    public T At(Point p) => At(p.X, p.Y);
    public T At(int x, int y) => Get(x, y);
    public T At(int idx) => _cells[idx];

    public bool Set(Point p, T value) => Set(p.X, p.Y, value);
    public bool Set(int x, int y, T value)
    {
        var idx = Index(x, y);
        if (idx == -1)
            return false;

        _cells[idx] = value;
        return true;
    }

    public void Set(int idx, T value) => _cells[idx] = value;  
    
    public IEnumerable<T> All() => _cells;

    private T Get(int x, int y)
    {
        var idx = Index(x, y);
        return _cells[idx];
    }

    public Point FromIndex(int idx) => new (idx % Width, idx / Width);

    public int Index(Point p) => Index(p.X, p.Y);
    private int Index(int x, int y)
    {
        if (!ValidPoint(new Point(x, y))) return -1;
        return (y + Math.Abs(Origin.Y)) * Width + x + Math.Abs(Origin.X);
    }

    // checks if the point is within bounds
    private bool ValidPoint(Point point)
        => point.X >= Origin.X && point.X <= Width - Origin.X - 1 && point.Y >= Origin.Y && point.Y <= Height - Origin.Y - 1;
    
    public int Width { get; }
    public int Height { get; }
    public Point Origin { get; }
    private readonly List<T> _cells = new();

    public override string ToString()
    {
        var s = string.Empty;
        for (var i = 0; i < _cells.Count; ++i)
        {
            if (i != 0 && i % Width == 0)
                s += '\n';
            
            s += _cells[i]?.ToString();
        }

        return s += '\n';
    }
}

public class Grid3D<T>
{
    public Grid3D(int width, int height, int depth, T @default, T invalid)
    {
        if (width < 1 || height < 1 || depth < 1)
            throw new Exception("Can't create Grid3D with no width, height or depth");

        Width = width;
        Height = height;
        Depth = depth;
        _invalid = invalid;
        
        // create the "front" grid
        var grid = new Grid<T>(width, height, Enumerable.Repeat(@default, width * height), invalid);
        for (var i = 0; i < depth; ++i)
            _grids.Add(grid.ShallowCopy());
    }

    public T At(int x, int y, int z) => _grids[z].At(x, y);
    public void Set(int x, int y, int z, T value) => _grids[z].Set(x, y, value);
    public IEnumerable<T> All() => _grids.SelectMany(g => g.All());
    
    public int Width { get; }
    public int Height { get; }
    public int Depth { get; }
    private readonly List<Grid<T>> _grids = new();
    private readonly T _invalid;

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        foreach (var grid in _grids)
        {
            stringBuilder.Append(grid);
            stringBuilder.AppendLine();
        }
        
        return stringBuilder.ToString();
    }
}

public class NegativeGrid3D<T>
{
    public NegativeGrid3D(int width, int height, int depth, int minX, int minY, int minZ, T @default)
    {
        Width = width;
        Height = height;
        Depth = depth;
        Origin = new ValueTuple<int, int, int>(minX, minY, minZ);
        
        // create the "front" grid
        var grid = new NegativeGrid<T>(width, height, new Point(minX, minY), 
            Enumerable.Repeat(@default, width * height));
        
        for (var i = 0; i < depth; ++i)
            _grids.Add(grid.ShallowCopy());
    }

    private int Index(int z)
    {
        return z + Math.Abs(Origin.z); // TODO: +1??
    }

    public T At(int x, int y, int z) => _grids[Index(z)].At(x, y);
    public void Set(int x, int y, int z, T value) => _grids[Index(z)].Set(x, y, value);
    public IEnumerable<T> All() => _grids.SelectMany(g => g.All());

    public int Width { get; }
    public int Height { get; }
    public int Depth { get; }
    public (int x, int y, int z) Origin { get; }
    private readonly List<NegativeGrid<T>> _grids = new();
    
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        foreach (var grid in _grids)
        {
            stringBuilder.Append(grid);
            stringBuilder.AppendLine();
        }
        
        return stringBuilder.ToString();
    }
}

