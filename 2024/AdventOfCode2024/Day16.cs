using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day16
{
    private record State(Point Position, Direction Direction);
    
    public class Path
    {
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

    public Day16(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        _grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), '@');
        // Console.WriteLine(_grid);
    }

    public void Solve()
    {
        var start = _grid.AllExtended().Single(x => x.Value is Start);
        var endPos = _grid.AllExtended().Single(x => x.Value is End).Position;

        // Use Dijkstra's algorithm with priority queue
        var pq = new PriorityQueue<Path, int>();
        var visited = new Dictionary<State, int>(); // Track best score for each (position, direction) state

        var initialPath = new Path
        {
            CurrentPosition = start.Position,
            CurrentDirection = InitialDirection,
            Score = 0,
            Visited = [start.Position]
        };

        pq.Enqueue(initialPath, 0);

        Path? bestPath = null;
        int bestScore = int.MaxValue;

        while (pq.Count > 0)
        {
            var currentPath = pq.Dequeue();
            var currentState = new State(currentPath.CurrentPosition, currentPath.CurrentDirection);

            // If we've reached the end, check if it's the best score
            if (currentPath.CurrentPosition.Equals(endPos))
            {
                if (currentPath.Score < bestScore)
                {
                    bestScore = currentPath.Score;
                    bestPath = currentPath;
                }

                continue;
            }

            // Skip if we've already found a better path to this state
            if (visited.TryGetValue(currentState, out var prevScore) && prevScore < currentPath.Score)
                continue;

            visited[currentState] = currentPath.Score;

            // Explore neighbors
            var neighbours = _grid.NeighboursExtended(currentPath.CurrentPosition, includeDiagonals: false)
                .Where(n => n.Value is not Wall)
                .ToArray();

            foreach (var neighbour in neighbours)
            {
                var neighbourDirection =
                    Helpers.CalculateCardinalDirection(currentPath.CurrentPosition, neighbour.Position);
                if (neighbourDirection is null)
                    continue;

                // Calculate the new score
                int newScore = currentPath.Score;
                if (neighbourDirection.Value == currentPath.CurrentDirection)
                {
                    // Moving forward
                    newScore += 1;
                }
                else
                {
                    // Need to rotate (90 or 270 degrees = 1000) + move (1)
                    var rotationCost = CalculateRotationCost(currentPath.CurrentDirection, neighbourDirection.Value);
                    if (rotationCost == int.MaxValue)
                        continue; // Can't rotate 180 degrees

                    newScore += rotationCost + 1;
                }

                // Only explore if this state hasn't been visited with a better or equal score
                var newState = new State(neighbour.Position, neighbourDirection.Value);
                if (!visited.TryGetValue(newState, out var existingScore) || newScore < existingScore)
                {
                    var newPath = currentPath.Copy();
                    newPath.CurrentPosition = Point.Copy(neighbour.Position);
                    newPath.Visited.Add(neighbour.Position);
                    newPath.Score = newScore;
                    newPath.CurrentDirection = neighbourDirection.Value;

                    pq.Enqueue(newPath, newPath.Score);
                }
            }
        }

        if (bestPath != null)
        {
            Console.WriteLine($"Winner score: {bestPath.Score}");
            // Console.WriteLine($"Winner score: {bestPath.Score}\n{bestPath.VisualiseOnGrid(_grid)}");
        }
        else
        {
            Console.WriteLine("No path found!");
        }
    }

    private static int CalculateRotationCost(Direction current, Direction target)
    {
        if (current == target)
            return 0;

        // Check if it's a 90-degree turn (cost 1000)
        var is90Degrees = current switch
        {
            Direction.North when target is Direction.East or Direction.West => true,
            Direction.East when target is Direction.North or Direction.South => true,
            Direction.South when target is Direction.East or Direction.West => true,
            Direction.West when target is Direction.South or Direction.North => true,
            _ => false
        };

        if (is90Degrees)
            return 1000;

        // 180-degree turn is not allowed (would mean going backwards)
        return int.MaxValue;
    }
}
