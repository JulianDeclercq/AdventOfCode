using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day16
{
    public class Path
    {
        // private void Step(Point position, Direction direction, int score, HashSet<Point> visited)
        public required Point CurrentPosition { get; set; }
        public Direction CurrentDirection { get; set; }
        public HashSet<Point> Visited { get; init; } = [];
        public int Score { get; set; }
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

        public Path Copy()
        {
            return new Path
            {
                CurrentPosition = Point.Copy(CurrentPosition),
                CurrentDirection = CurrentDirection,
                Visited = Visited.ToHashSet(),
                Score = Score,
            };
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
        var path = new Path
        {
            CurrentPosition = start.Position,
            CurrentDirection = InitialDirection,
            Score = 0,
            Visited = [start.Position]
        };
        Step(path);

        var winner = _paths.MinBy(p => p.Score);
        Console.WriteLine($"Winner score: {winner!.Score}\n{winner!.VisualiseOnGrid(_grid)}");

        // Console.WriteLine($"{_paths.Count} paths found");
        // foreach (var path in _paths)
        //     Console.WriteLine($"Path\n{path.VisualiseOnGrid(_grid)}\nScore: {path.Score}");
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

    // private void Step(Point position, Direction direction, int score, HashSet<Point> visited)
    private void Step(Path path)
    {
        var notVisitedNeighbours = _grid.NeighboursExtended(path.CurrentPosition, includeDiagonals: false)
            .Where(n => n.Value is not Wall && !path.Visited.Contains(n.Position))
            .ToArray();

        // foreach (var neighbour in neighbours)
        for (var i = 0; i < notVisitedNeighbours.Length; ++i)
        {
            var neighbour = notVisitedNeighbours[i];
            var neighbourDirection = Helpers.CalculateCardinalDirection(path.CurrentPosition, neighbour.Position);
            if (neighbourDirection is null)
                continue;
            
            if (neighbourDirection != path.CurrentDirection)
            {
                var canRotate = CanRotateTowards(path.CurrentDirection, neighbourDirection.Value);
                if (canRotate)
                {
                    // rotate and step there
                    var newPosition = Point.Copy(neighbour.Position);
                    var branch = path.Copy();
                    branch.Score += 1001;
                    branch.CurrentPosition = newPosition;
                    branch.CurrentDirection = neighbourDirection.Value;
                    branch.Visited.Add(newPosition);
                    if (neighbour.Value is End)
                    {
                        _paths.Add(branch);
                        return;
                    }
                    
                    Step(branch);
                }
                continue;
            }
            
            var branch2 = path.Copy();
            if (CanMoveTo(neighbour, branch2.Visited))
            {
                var newPosition2 = Point.Copy(neighbour.Position);
                branch2.Score++;
                branch2.CurrentPosition = newPosition2;
                branch2.Visited.Add(newPosition2);
                if (neighbour.Value is End)
                {
                    _paths.Add(branch2);
                    return;
                }

                Step(branch2);
            }
        }
    }
}
