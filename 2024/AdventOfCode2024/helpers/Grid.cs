namespace AdventOfCode2024.helpers;

public class Grid<T>
{
    public Grid(int width, int height, IEnumerable<T> elements, T invalid)
    {
        if (width < 1 || height < 1)
            throw new Exception("Can't create grid with no rows or columns");

        Width = width;
        Height = height;
        _cells.AddRange(elements);

        if (_cells.Count != width * height)
            throw new Exception($"Amount of elements {_cells.Count} does not match width {width} * height {height})");

        _invalid = invalid;
    }

    // doesn't deep copy the values in _cells if T is a reference type
    public Grid<T> ShallowCopy() => new(Width, Height, _cells, _invalid);
    public void AddRange(IEnumerable<T> tRange) => _cells.AddRange(tRange);
    public T At(Point p) => At(p.X, p.Y);
    public T At(int x, int y) => Get(x, y);
    public T At(int idx) => _cells[idx];

    public IEnumerable<GridElement<T>> Column(int x)
    {
        var list = new List<GridElement<T>>();

        for (var i = 0; i < Height; ++i)
        {
            var index = x + Width * i;
            list.Add(new GridElement<T>(FromIndex(index), _cells[index]));
        }

        return list;
    }

    public void ReplaceColumn(int x, T[] newColumn)
    {
        if (newColumn.Length != Height)
            throw new Exception($"Can't replace column size {newColumn.Length} needs to match grid height {Height}.");

        for (var i = 0; i < newColumn.Length; ++i)
        {
            var index = x + Width * i;
            Set(FromIndex(index), newColumn[i]);
        }
    }

    public IEnumerable<IEnumerable<GridElement<T>>> Columns()
    {
        return Enumerable.Range(0, Width).Select(Column);
    }

    public IEnumerable<GridElement<T>> Row(int y)
    {
        var list = new List<GridElement<T>>();

        var offset = Width * y;
        for (var i = 0; i < Width; ++i)
        {
            var index = offset + i;
            list.Add(new GridElement<T>(FromIndex(index), _cells[index]));
        }

        return list;
    }

    public void ReplaceRow(int y, T[] newRow)
    {
        if (newRow.Length != Width)
            throw new Exception($"Can't replace row size {newRow.Length} needs to match grid width {Width}.");

        var offset = Width * y;
        for (var i = 0; i < newRow.Length; ++i)
        {
            var index = offset + i;
            Set(FromIndex(index), newRow[i]);
        }
    }


    public IEnumerable<IEnumerable<GridElement<T>>> Rows()
    {
        return Enumerable.Range(0, Height).Select(Row);
    }

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
    public Dictionary<Point, T> AllExtendedLookup()
    {
        var extended = new Dictionary<Point, T>();

        for (var i = 0; i < _cells.Count; ++i)
            extended.Add(FromIndex(i), _cells[i]);

        return extended;
    }

    public List<GridElement<T>> AllExtended()
    {
        return _cells.Select((t, i) => new GridElement<T>(FromIndex(i), t)).ToList();
    }

    private T Get(int x, int y)
    {
        var idx = Index(x, y);
        return idx == -1 ? _invalid : _cells[idx];
    }

    public Point FromIndex(int idx) => new(idx % Width, idx / Width);

    public int Index(Point p) => Index(p.X, p.Y);

    private int Index(int x, int y)
    {
        if (x < 0 || x > Width - 1) return -1;
        if (y < 0 || y > Height - 1) return -1;

        return y * Width + x;
    }

    // doesn't support wrapping
    public IEnumerable<T> Neighbours(Point p, bool includeDiagonals = true) => Neighbours(p.X, p.Y, includeDiagonals);

    public IEnumerable<T> Neighbours(int x, int y, bool includeDiagonals = true)
    {
        var neighbours = new List<T>
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

    public GridElement<T>? GetNeighbour(Point p, GridNeighbourType type)
    {
        var neighbour = type switch
        {
            GridNeighbourType.N => new Point(p.X, p.Y - 1),
            GridNeighbourType.Ne => new Point(p.X + 1, p.Y - 1),
            GridNeighbourType.E => new Point(p.X + 1, p.Y),
            GridNeighbourType.Se => new Point(p.X + 1, p.Y + 1),
            GridNeighbourType.S => new Point(p.X, p.Y + 1),
            GridNeighbourType.Sw => new Point(p.X - 1, p.Y + 1),
            GridNeighbourType.W => new Point(p.X - 1, p.Y),
            GridNeighbourType.Nw => new Point(p.X - 1, p.Y - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        if (!ValidPoint(neighbour))
            return null;

        return new GridElement<T>(neighbour, At(neighbour));
    }

    private IEnumerable<Point> NeighbouringPoints(Point p, bool includeDiagonals = true, bool filterInvalid = true)
    {
        var neighbours = new List<Point>
        {
            new(p.X, p.Y - 1),
            new(p.X + 1, p.Y),
            new(p.X, p.Y + 1),
            new(p.X - 1, p.Y)
        };

        if (includeDiagonals)
        {
            neighbours.Add(new Point(p.X - 1, p.Y - 1));
            neighbours.Add(new Point(p.X + 1, p.Y - 1));
            neighbours.Add(new Point(p.X + 1, p.Y + 1));
            neighbours.Add(new Point(p.X - 1, p.Y + 1));
        }

        return filterInvalid ? neighbours.Where(ValidPoint) : neighbours;
    }

    public IEnumerable<GridElement<T>> NeighboursExtended(Point p, bool includeDiagonals = true,
        bool filterInvalid = true)
    {
        return NeighbouringPoints(p, includeDiagonals, filterInvalid).Select(np => new GridElement<T>(np, At(np)));
    }

    private IEnumerable<Point> DiagonalNeighbouringPoints(Point p, bool filterInvalid = true)
    {
        List<Point> neighbours =
        [
            new Point(p.X - 1, p.Y - 1),
            new Point(p.X + 1, p.Y - 1),
            new Point(p.X + 1, p.Y + 1),
            new Point(p.X - 1, p.Y + 1),
        ];

        return filterInvalid ? neighbours.Where(ValidPoint) : neighbours;
    }

    public IEnumerable<GridElement<T>> DiagonalNeighboursExtended(Point p, bool filterInvalid = true)
    {
        return DiagonalNeighbouringPoints(p, filterInvalid).Select(np => new GridElement<T>(np, At(np)));
    }

    public GridElement<T>? GetNeighbour(Point p, Direction direction, bool wrap = false)
    {
        int x = 0, y = 0;
        switch (direction)
        {
            case Direction.North:
                y = p.Y - 1;

                if (p.Y == 0)
                {
                    if (!wrap)
                        return null;

                    y = Height - 1;
                }

                return new GridElement<T>(new Point(p.X, y), At(p.X, y));
            
            case Direction.NorthEast:
                if (wrap)
                    throw new NotImplementedException("Wrap is not implemented for NorthEast");

                if (!wrap && p.Y == 0)
                    return null;

                if (!wrap && p.X == Width - 1)
                    return null;
                
                y = p.Y - 1;
                x = p.X + 1;
                return new GridElement<T>(new Point(x, y), At(x, y));
                
            case Direction.East:
                x = p.X + 1;

                if (p.X == Width - 1)
                {
                    if (!wrap)
                        return null;

                    x = 0;
                }

                return new GridElement<T>(new Point(x, p.Y), At(x, p.Y));
            
            case Direction.SouthEast:
                if (wrap)
                    throw new NotImplementedException("Wrap is not implemented for SouthEast");

                if (!wrap && p.Y == Height - 1)
                    return null;
                
                if (!wrap && p.X == Width - 1)
                    return null;
                
                y = p.Y + 1;
                x = p.X + 1;
                return new GridElement<T>(new Point(x, y), At(x, y));
            case Direction.South:
                y = p.Y + 1;

                if (p.Y == Height - 1)
                {
                    if (!wrap)
                        return null;

                    y = 0;
                }

                return new GridElement<T>(new Point(p.X, y), At(p.X, y));
            case Direction.SouthWest:
                if (wrap)
                    throw new NotImplementedException("Wrap is not implemented for SouthEast");

                if (!wrap && p.X == 0)
                    return null;
                
                if (!wrap && p.Y == Height - 1)
                    return null;
                
                y = p.Y + 1;
                x = p.X - 1;
                
                return new GridElement<T>(new Point(x, y), At(x, y));
            case Direction.West:
                x = p.X - 1;

                if (p.X == 0)
                {
                    if (!wrap)
                        return null;

                    x = Width - 1;
                }

                return new GridElement<T>(new Point(x, p.Y), At(x, p.Y));
            case Direction.NorthWest:
                if (wrap)
                    throw new NotImplementedException("Wrap is not implemented for NorthWest");
                
                if (!wrap && p.Y == 0)
                    return null;
                
                if (!wrap && p.X == 0)
                    return null;
                
                x = p.X - 1;
                y = p.Y - 1;
                
                return new GridElement<T>(new Point(x, y), At(x, y));
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    // checks if the point is within bounds
    public bool ValidPoint(Point point)
    {
        return point.X >= 0 && point.X <= Width - 1 && point.Y >= 0 && point.Y <= Height - 1;
    }

    public int Width { get; }
    public int Height { get; }
    public readonly T _invalid;
    private readonly List<T> _cells = [];

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

public record GridElement<T>(Point Position, T Value);

public enum GridNeighbourType
{
    None = 0,
    N = 1,
    Ne = 2,
    E = 3,
    Se = 4,
    S = 5,
    Sw = 6,
    W = 7,
    Nw = 8
}