using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day16
{
    public class Path
    {
        public HashSet<Point> Visited { get; init; }
        public int Score { get; init; }

        public string VisualiseOnGrid(Grid<char> grid)
        {
            var copy = grid.ShallowCopy();
            
            foreach (var point in Visited)
                copy.Set(point, '@');
            
            return copy.ToString();
        }
    }

    private Grid<char> _grid;
    public const char Wall = '#';
    public const char End = 'E';
    public const char Start = 'S';
    private const Direction InitialDirection = Direction.East;
    private List<Path> _paths = [];

    public Day16(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        _grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), '@');
        Console.WriteLine(_grid);
    }

    public void Solve()
    {
        var start = _grid.AllExtended().Single(x => x.Value is Start);
        Step(start.Position, InitialDirection, 0, [start.Position]);

        var winner = _paths.MinBy(p => p.Score);
        Console.WriteLine($"Answer\n{winner!.VisualiseOnGrid(_grid)}");
    }

    private static bool CanMoveTo(GridElement<char>? neighbour, HashSet<Point> visited)
    {
        return neighbour is not null && neighbour.Value is not Wall && !visited.Contains(neighbour.Position);
    }

    // check if the target direction is within 90 degrees
    private static Direction? TryRotateTowards(Direction current, Direction target)
    {
        var canRotate = current switch
        {
            Direction.North when target is Direction.East => true,
            Direction.North when target is Direction.West => true,
            Direction.East when target is Direction.North => true,
            Direction.East when target is Direction.South => true,
            Direction.South when target is Direction.East => true,
            Direction.South when target is Direction.West => true,
            Direction.West when target is Direction.South => true,
            Direction.West when target is Direction.North => true,
            _ => false
        };

        return canRotate ? target : null;
    }

    private void Step(Point position, Direction direction, int score, HashSet<Point> visited)
    {
        var neighbours = _grid.NeighboursExtended(position, includeDiagonals: false)
            .Where(n => n.Value is not Wall)
            .ToArray();

        foreach (var neighbour in neighbours)
        {
            if (visited.Contains(neighbour.Position))
                continue;

            var neighbourDirection = Helpers.CalculateDirection(position, neighbour.Position);
            if (neighbourDirection != direction)
            {
                var target = TryRotateTowards(direction, neighbourDirection);
                if (target is not null)
                {
                    score += 1000;
                    Step(position, target.Value, score, visited.ToHashSet());
                }
            }

            var visitedCopy = visited.ToHashSet();
            if (CanMoveTo(neighbour, visitedCopy))
            {
                score++;
                visitedCopy.Add(neighbour.Position);
                position += Helpers.DirectionToPoint(direction);

                if (neighbour.Value is End)
                {
                    _paths.Add(new Path
                    {
                        Visited = visitedCopy,
                        Score = score,
                    });
                    return;
                }

                Step(position, direction, score, visitedCopy);
                return;
            }
        }
    }
}