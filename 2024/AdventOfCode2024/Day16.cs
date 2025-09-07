using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day16
{
    public class Path
    {
        public HashSet<Point> Visited { get; init; } = [];
        public int Score { get; init; }

        public string VisualiseOnGrid(Grid<char> grid, Point? position = null, Direction? direction = null)
        {
            var copy = grid.ShallowCopy();
            
            foreach (var point in Visited)
                copy.Set(point, '@');

            if (position is not null && direction is not null)
            {
                var target = direction switch
                {
                    Direction.North => 'N',
                    Direction.East => 'E',
                    Direction.South => 'S',
                    Direction.West => 'W',
                    _ => throw new Exception("Invalid direction inside VisualiseOnGrid")
                };
                
                copy.Set(position, target);
            }
            
            return copy.ToString();
        }
    }

    private Grid<char> _grid;
    private const char Wall = '#';
    private const char End = 'E';
    private const char Start = 'S';
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
        // Console.WriteLine($"Winner score: {winner!.Score}\n{winner!.VisualiseOnGrid(_grid)}");

        Console.WriteLine($"{_paths.Count} paths found");
        foreach (var path in _paths)
            Console.WriteLine($"Path\n{path.VisualiseOnGrid(_grid)}\nScore: {path.Score}");
    }

    private static bool CanMoveTo(GridElement<char>? neighbour, HashSet<Point> visited)
    {
        return neighbour is not null && neighbour.Value is not Wall && !visited.Contains(neighbour.Position);
    }

    // Check if the target direction is within 90 degrees
    private static bool CanRotateTowards(Direction current, Direction target)
    {
        return current switch
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
    }

    private void Step(Point position, Direction direction, int score, HashSet<Point> visited)
    {
        var dbg = new Path
        {
            Visited = visited,
            Score = 1337
        };
        // Console.WriteLine(dbg.VisualiseOnGrid(_grid, position, direction));
        
        var notVisitedNeighbours = _grid.NeighboursExtended(position, includeDiagonals: false)
            .Where(n => n.Value is not Wall)
            .ToArray();

        if (position.Equals(new Point(3, 9)))
        {
            int kbkpt = 5;
        }

        // foreach (var neighbour in neighbours)
        for (var i = 0; i < notVisitedNeighbours.Length; ++i)
        {
            var neighbour = notVisitedNeighbours[i];
            
            if (position.Equals(new Point(3, 9)))
            {
                int kbkpt = 5;
            }
            if (visited.Contains(neighbour.Position))
                continue;

            var neighbourDirection = Helpers.CalculateCardinalDirection(position, neighbour.Position);
            if (neighbourDirection is null)
                continue;
            
            if (neighbourDirection != direction)
            {
                var canRotate = CanRotateTowards(direction, neighbourDirection.Value);
                if (canRotate)
                {
                    // rotate
                    score += 1000;
                    Step(Point.Copy(position), neighbourDirection.Value, score, visited.ToHashSet());
                    // continue;
                }
                continue; // TODO: Not sure about this continue. It was inside canrotate before. But in that case
                // the following code below seems to move into the neighbours position even when the neighbour is not 
                // in that direction
            }
            
            // TODO: Multiple rotations in the "same" turn? Even though it adds a lot to score, might be some paths.

            
            // Is it possible that canmove is 
            var visitedCopy = visited.ToHashSet();
            if (CanMoveTo(neighbour, visitedCopy))
            {
                score++;
                visitedCopy.Add(neighbour.Position);
                position += Helpers.DirectionToPoint(direction);
                if (position.Equals(new Point(4, 9)))
                {
                    int bkpt = 5;
                }

                if (neighbour.Value is End)
                {
                    _paths.Add(new Path
                    {
                        Visited = visitedCopy,
                        Score = score,
                    });
                    return;
                }

                Step(Point.Copy(position), direction, score, visitedCopy);
            }
        }
    }
}
