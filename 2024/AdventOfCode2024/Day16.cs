using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day16
{
    private Grid<char> _grid;
    public const char Wall = '#';
    public const char End = 'E';
    private List<int> _scores = [];

    public Day16(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        _grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), '@');
        Console.WriteLine(_grid);
    }

    private static bool CanMoveTo(GridElement<char>? neighbour, HashSet<Point> visited)
    {
        return neighbour is not null && neighbour.Value is not Wall && !visited.Contains(neighbour.Position);
    }

    private void Step(Point position, Direction direction, int score, HashSet<Point> visited)
    {
        var visitedCopy = visited.ToHashSet();

        // check easiest route first (for no good reason tho lmao)
        var easyNeighbour = _grid.GetNeighbour(position, direction);
        if (CanMoveTo(easyNeighbour, visitedCopy))
        {
            score++;
            visitedCopy.Add(easyNeighbour!.Position);
            position += Helpers.DirectionToPoint(direction);

            if (easyNeighbour.Value is End)
            {
                _scores.Add(score);
                return;
            }
            
            Step(position, direction, score, visitedCopy);
            return;
        }

        foreach (var neighbour in _grid.NeighboursExtended(position).Where(n => n.Value is not Wall))
        {
            if (visited.Contains(neighbour.Position))
                continue;
        }
    }
}