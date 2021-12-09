using System.Data;

namespace AdventOfCode2021;

public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public int X;
    public int Y;
    
    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}

public class Grid<T>
{
    public Grid(int width, int height, T invalid)
    {
        if (width < 1 || height < 1)
            throw new Exception("Can't create grid with no rows or columns");

        Width = width;
        Height = height;
        Invalid = invalid;
    }

    public void AddRange(IEnumerable<T> tRange) => Cells.AddRange(tRange);
    public T At(int x, int y) => Get(x, y);

    public T Get(int x, int y)
    {
        if (x < 0 || x > Width - 1) return Invalid;
        if (y < 0 || y > Height - 1) return Invalid;

        var idx = y * Width + x;
        return Cells[idx];
    }

    // doesn't wrap (currently)
    public IEnumerable<T> Neighbours(int x, int y)
    {
        return new List<T>()
        {
            Get(x - 1, y - 1),
            Get(x, y - 1),
            Get(x + 1, y - 1),
            Get(x + 1, y),
            Get(x + 1, y + 1),
            Get(x, y + 1),
            Get(x - 1, y + 1),
            Get(x - 1, y)
        }.Where(n => n != null && !n.Equals(Invalid));
    }

    private T Invalid;
    private int Width = 0;
    private int Height = 0;
    public List<T> Cells = new List<T>();
}
