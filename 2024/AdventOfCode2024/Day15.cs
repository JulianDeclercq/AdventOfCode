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
    public const char Edge = '#';

    public Grid<char> GetGrid() => _grid;

    public Day15(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var gridLines = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        _moves = lines.Skip(gridLines.Length + 1).SelectMany(l => l).ToArray();
        _grid = new Grid<char>(gridLines.First().Length, gridLines.Length, gridLines.SelectMany(l => l), '-');

        foreach (var cell in _grid.AllExtended())
        {
            if (cell.Value is '@')
            {
                _cachedRobotPosition = cell.Position;
                break;
            }
        }
    }

    public void Solve()
    {
        for (var i = 0; i < _moves.Length; ++i)
            Step();
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
                PushBox(firstBox.Position, furthestNeighbour.Position);
                break;
            case Box: break;
            case null: throw new Exception("FurthestNeighbour is null");
        }
    }

    private void PushBox(Point robotTarget, Point emptySpace)
    {
        // we can cheat a bit, instead of moving the whole row: move the first barrel to the end of the box instead
        // where the robot moves is where the first box was
        _grid.Set(robotTarget, Robot);
        _grid.Set(emptySpace, Box);
        _cachedRobotPosition = robotTarget;
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
                        var boxNorth = _grid.GetNeighbour(north.Position, Direction.North);
                        switch (boxNorth?.Value)
                        {
                            case Edge: break;
                            case Empty:
                                PushBox(boxNorth.Position, north.Position);
                                break;
                            case Box:
                                break;
                            case null: throw new Exception("boxnorth null");
                        }

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
                        var boxSouth = _grid.GetNeighbour(south.Position, Direction.South);
                        switch (boxSouth?.Value)
                        {
                            case Edge: break;
                            case Empty:
                                PushBox(boxSouth.Position, south.Position);
                                break;
                            case Box: break;
                            case null: throw new Exception("boxsouth is null");
                        }

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
                        var boxWest = _grid.GetNeighbour(west.Position, Direction.West);

                        switch (boxWest?.Value)
                        {
                            case Edge: break;
                            case Empty:
                                PushBox(boxWest.Position, west.Position);
                                break;
                            case Box: break;
                            case null:
                                throw new Exception(
                                    "West BOX neighbour is null, should never happen it should be # at edges");
                        }

                        break;

                    case null:
                        throw new Exception("West neighbour is null, should never happen it should be # at edges");
                }

                break;
        }

        Console.WriteLine($"Step {_currentStep}");
        Console.WriteLine(_grid);
        _currentStep++;
    }
}