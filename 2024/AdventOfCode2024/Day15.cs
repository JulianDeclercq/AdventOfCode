using System.Text;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day15
{
    private readonly Grid<char> _grid;
    private readonly char[] _moves;
    private int _currentStep = 0;
    private Point _cachedRobotPosition;
    public const char Robot = '@';
    public const char Empty = '.';
    public const char Box = 'O';
    public const char BoxLeft = '[';
    public const char BoxRight = ']';
    private const char Edge = '#';

    public Grid<char> GetGrid() => _grid;

    public Day15(string filePath, int part = 1)
    {
        var lines = File.ReadAllLines(filePath);
        var gridLines = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        if (part is 2)
        {
            gridLines = gridLines.Select(s =>
            {
                var sb = new StringBuilder();
                foreach (var c in s)
                {
                    switch (c)
                    {
                        case Robot:
                            sb.Append(Robot);
                            sb.Append(Empty);
                            break;
                        case Empty:
                            sb.Append(Empty, 2);
                            break;
                        case Box:
                            sb.Append(BoxLeft);
                            sb.Append(BoxRight);
                            break;
                        case Edge:
                            sb.Append(Edge, 2);
                            break;
                        default: throw new Exception($"Invalid character {c}");
                    }
                }

                return sb.ToString();
            }).ToArray();
        }

        _moves = lines.Skip(gridLines.Length + 1).SelectMany(l => l).ToArray();
        _grid = new Grid<char>(gridLines.First().Length, gridLines.Length, gridLines.SelectMany(l => l), '-');

        _cachedRobotPosition = _grid.AllExtended().Single(x => x.Value is Robot).Position;
    }

    public void Solve()
    {
        for (var i = 0; i < _moves.Length; ++i)
            Step();

        Console.WriteLine(GpsSum());
    }

    private void MoveRobot(Point target)
    {
        _grid.Set(_cachedRobotPosition, Empty);
        _grid.Set(target, Robot);
        _cachedRobotPosition = target;
    }

    private void PushBoxRow(GridElement<char> firstBox, Direction direction)
    {
        var furthestNeighbour = firstBox;
        do
        {
            furthestNeighbour = _grid.GetNeighbour(furthestNeighbour.Position, direction);
        } while (furthestNeighbour?.Value == Box);

        // only push the row if we have found an empty space at the end that we can push the row into
        if (furthestNeighbour!.Value is not Empty)
            return;

        // we can cheat a bit, instead of moving the whole row: move the first box to the end of the row instead
        // where the robot moves is where the first box was
        _grid.Set(_cachedRobotPosition, Empty);
        _grid.Set(firstBox.Position, Robot);
        _grid.Set(furthestNeighbour.Position, Box);
        _cachedRobotPosition = firstBox.Position;
    }

    private readonly Dictionary<char, Direction> _directionMap = new()
    {
        ['^'] = Direction.North,
        ['>'] = Direction.East,
        ['v'] = Direction.South,
        ['<'] = Direction.West,
    };

    public void Step()
    {
        if (_currentStep >= _moves.Length)
            throw new Exception("No steps left to take!");

        var step = _moves[_currentStep];
        var direction = _directionMap[step];
        var neighbour = _grid.GetNeighbour(_cachedRobotPosition, direction);
        if (neighbour is null)
            throw new Exception("Neighbour is null");

        switch (neighbour.Value)
        {
            case Empty:
                MoveRobot(neighbour.Position);
                break;
            case Box:
                PushBoxRow(neighbour, direction);
                break;
        }

        // Console.WriteLine($"Step {_currentStep}");
        // Console.WriteLine(_grid);
        _currentStep++;
    }

    public int GpsSum()
    {
        return _grid.AllExtended()
            .Where(x => x.Value is Box)
            .Sum(box => 100 * box.Position.Y + box.Position.X);
    }
}