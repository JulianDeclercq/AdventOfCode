using System.Text;

namespace AdventOfCode2022.Days;

public class Grid3D<T>
{
    public Grid3D(int width, int height, int depth, IEnumerable<Grid<T>> elements, T invalid)
    {
        Width = width;
        Height = height;
        Depth = depth;
        _invalid = invalid;
        _grids.AddRange(elements);
    }

    public int Width { get; }
    public int Height { get; }
    public int Depth{ get; }

    private readonly T _invalid;
    private readonly List<Grid<T>> _grids = new();
    
    private bool ValidDepth(int z) => z >= 0 && z < Depth;

    public T At(int x, int y, int z)
    {
        return ValidDepth(z) ? _grids[z].At(x, y) : _invalid;
    }

    public bool Set(int x, int y, int z, T value)
    {
        return _grids[z].Set(x, y, value);
    }

    public bool Set(Point3D p, T value) => Set(p.X, p.Y, p.Z, value);

    public IEnumerable<T> Neighbours(Point3D p, bool includeDiagonals = false)
        => Neighbours(p.X, p.Y, p.Z, includeDiagonals);

    private IEnumerable<T> Neighbours(int x, int y, int z, bool includeDiagonals)
    {
        var neighbours = _grids[z].Neighbours(x, y, includeDiagonals).ToList();

        // higher neighbours
        if (ValidDepth(z - 1))
            neighbours.Add(_grids[z - 1].At(x, y));
        
        // lower neighbours
        if (ValidDepth(z + 1))
            neighbours.Add(_grids[z + 1].At(x, y));

        return neighbours;
    }

    private IEnumerable<T> NeighboursOLD(int x, int y, int z, bool includeDiagonals)
    {
        var neighbours = _grids[z].Neighbours(x, y, includeDiagonals).ToList();

        // higher neighbours
        if (ValidDepth(z - 1))
        {
            var temp = _grids[z - 1].Neighbours(x, y, includeDiagonals);
            neighbours.AddRange(temp);
        }
        
        // lower neighbours
        if (ValidDepth(z + 1))
        {
            var temp = _grids[z + 1].Neighbours(x, y, includeDiagonals);
            neighbours.AddRange(temp);
        }

        return neighbours;
    }

    private bool ValidPoint(Point3D point)
    {
        return point.X >= 0 && point.X <= Width - 1 &&
               point.Y >= 0 && point.Y <= Height - 1 &&
               point.Z >= 0 && point.Z <= Depth - 1;
    }
    
    public override string ToString()
        => $"{_grids.Aggregate(string.Empty, (current, grid) => current + $"{grid}\n")}\n";
}