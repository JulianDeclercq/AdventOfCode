using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day16
{
    private Grid<char> _grid;
    public const char Wall = '#';
    public const char End = 'E';
    public const char Start = 'E';
    private const Direction InitialDirection = Direction.East;
    private List<int> _scores = [];

    public Day16(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        _grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), '@');
        Console.WriteLine(_grid);
    }

    public void Solve()
    {
        var start = _grid.AllExtended().Single(x => x.Value is Start);
        Step(start.Position, InitialDirection, 0, []);
        Console.WriteLine($"Answer {_scores.Max()}");
    }

    private static bool CanMoveTo(GridElement<char>? neighbour, HashSet<Point> visited)
    {
        return neighbour is not null && neighbour.Value is not Wall && !visited.Contains(neighbour.Position);
    }

    private void Step(Point position, Direction direction, int score, HashSet<Point> visited)
    {
        var neighbours = _grid.NeighboursExtended(position, includeDiagonals: false);
        foreach (var neighbour in neighbours.Where(n => n.Value is not Wall))
        {
            if (visited.Contains(neighbour.Position))
                continue;
            
            var visitedCopy = visited.ToHashSet();
            if (CanMoveTo(neighbour, visitedCopy))
            {
                score++;
                visitedCopy.Add(neighbour.Position);
                position += Helpers.DirectionToPoint(direction);

                if (neighbour.Value is End)
                {
                    _scores.Add(score);
                    return;
                }

                Step(position, direction, score, visitedCopy);
                return;
            }
        }
    }
}