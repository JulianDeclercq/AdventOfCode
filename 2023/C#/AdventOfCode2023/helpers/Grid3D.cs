﻿namespace AdventOfCode2023.helpers;

public record GridElement3D<T>(Point3D Position, T Value);

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
    private readonly Dictionary<Point3D, List<T>> _memodNeighbours = new();

    private bool ValidDepth(int z) => z >= 0 && z < Depth;

    public T At(Point3D p) => At(p.X, p.Y, p.Z); 

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

    // TODO: This method probably doesn't work as intended for includeDiagonals since that's never been tested
    private IEnumerable<T> Neighbours(int x, int y, int z, bool includeDiagonals)
    {
        var point = new Point3D(x, y, z);
        if (_memodNeighbours.TryGetValue(point, out var n))
        {
            Console.WriteLine($"HIT MEMO FOR {point}");
            return n;
        }
        
        var neighbours = _grids[z].Neighbours(x, y, includeDiagonals).ToList();

        // higher neighbours
        if (ValidDepth(z - 1))
            neighbours.Add(_grids[z - 1].At(x, y));
        
        // lower neighbours
        if (ValidDepth(z + 1))
            neighbours.Add(_grids[z + 1].At(x, y));
        
        _memodNeighbours.Add(point, neighbours);

        return neighbours;
    }
    
    public IEnumerable<GridElement3D<T>> Row(int y, int z)
    {
        return Enumerable.Range(0, Width)
            .Select(x => new GridElement3D<T>(new Point3D(x, y, z), _grids[z].At(x, y)));
    }
    
    public IEnumerable<GridElement3D<T>> Column(int x, int z)
    {
        return Enumerable.Range(0, Height)
            .Select(y => new GridElement3D<T>(new Point3D(x, y, z), _grids[z].At(x, y)));
    }

    public IEnumerable<GridElement3D<T>> Slice(int x, int y)
    {
        return Enumerable.Range(0, Depth)
            .Select(z => new GridElement3D<T>(new Point3D(x, y, z), _grids[z].At(x, y)));
    }

    // public IEnumerable<GridElement3D<T>> Row(int y)
    // {
    //     var row = new List<GridElement3D<T>>();
    //     for (var z = 0; z < Depth; ++z)
    //     {
    //         for (var x = 0; x < Width; ++x)
    //         {
    //             var point = new Point3D(x, y, z);
    //             row.Add(new GridElement3D<T>(point, _grids[z].At(x, y)));
    //         }
    //     }
    //     return row;
    // }

    private bool ValidPoint(Point3D point)
    {
        return point.X >= 0 && point.X <= Width - 1 &&
               point.Y >= 0 && point.Y <= Height - 1 &&
               point.Z >= 0 && point.Z <= Depth - 1;
    }
    
    public override string ToString()
        => $"{_grids.Aggregate(string.Empty, (current, grid) => current + $"{grid}\n")}\n";

    public IEnumerable<Point3D> AllPoints()
    {
        var points = new List<Point3D>();
        for (var x = 0; x < Width; ++x)
        {
            for (var y = 0; y < Height; ++y)
            {
                for (var z = 0; z < Depth; ++z)
                {
                    points.Add(new Point3D(x, y, z));
                }    
            }   
        }
        return points;
    }
}