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
    private const char Edge = '#';

    public Grid<char> GetGrid() => _grid;

    public Day15(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var gridLines = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
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

        switch (furthestNeighbour?.Value)
        {
            case Edge: break;
            case Empty:
                // we can cheat a bit, instead of moving the whole row: move the first box to the end of the row instead
                // where the robot moves is where the first box was
                _grid.Set(_cachedRobotPosition, Empty);
                _grid.Set(firstBox.Position, Robot);
                _grid.Set(furthestNeighbour.Position, Box);
                _cachedRobotPosition = firstBox.Position;
                break;
            case Box: break;
            case null: throw new Exception("FurthestNeighbour is null");
        }
    }

    public void Step()
    {
        if (_currentStep >= _moves.Length)
            throw new Exception("No steps left to take!");

        var step = _moves[_currentStep];
        switch (step)
        {
            case '^':
                var north = _grid.GetNeighbour(_cachedRobotPosition, Direction.North);
                switch (north?.Value)
                {
                    case Edge: break;
                    case Empty:
                        MoveRobot(north.Position);
                        break;
                    case Box:
                        PushBoxRow(north, Direction.North);
                        break;
                    case null: throw new Exception("north null");
                }

                break;
            case '>':
                var east = _grid.GetNeighbour(_cachedRobotPosition, Direction.East);
                switch (east?.Value)
                {
                    case Edge: break;
                    case Empty:
                        MoveRobot(east.Position);
                        break;
                    case Box:
                        PushBoxRow(east, Direction.East);
                        break;
                    case null: throw new Exception("east null");
                }

                break;
            case 'v':
                var south = _grid.GetNeighbour(_cachedRobotPosition, Direction.South);
                switch (south?.Value)
                {
                    case Edge: break;
                    case Empty:
                        MoveRobot(south.Position);
                        break;
                    case Box:
                        PushBoxRow(south, Direction.South);
                        break;
                    case null: throw new Exception("south null");
                }

                break;
            case '<':
                var west = _grid.GetNeighbour(_cachedRobotPosition, Direction.West);

                switch (west?.Value)
                {
                    case Edge: break;
                    case Empty:
                        MoveRobot(west.Position);
                        break;
                    case Box:
                        PushBoxRow(west, Direction.West);
                        break;
                    case null:
                        throw new Exception("west null");
                }

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