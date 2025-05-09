﻿using System.Text;
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
    private readonly int _part;

    private readonly Dictionary<char, Direction> _directionMap = new()
    {
        ['^'] = Direction.North,
        ['>'] = Direction.East,
        ['v'] = Direction.South,
        ['<'] = Direction.West,
    };

    public Grid<char> GetGrid() => _grid;

    public Day15(string filePath, int part = 1)
    {
        var lines = File.ReadAllLines(filePath);
        var gridLines = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        _part = part;

        // make the grid twice as wide
        if (_part is 2)
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

    private static bool IsBox(GridElement<char>? element)
    {
        if (element is null)
            throw new Exception("Element is null");

        return element.Value is Box or BoxLeft or BoxRight;
    }

    private void PushBoxRow(GridElement<char> firstBox, Direction direction)
    {
        var furthestNeighbour = firstBox;
        do
        {
            furthestNeighbour = _grid.GetNeighbour(furthestNeighbour.Position, direction);
        } while (furthestNeighbour?.Value == Box);

        // only push the row when there's an empty space at the end that the row can be pushed into
        if (furthestNeighbour!.Value is not Empty)
            return;

        // cheat a bit, instead of moving the whole row: move the first box to the end of the row instead
        // where the robot moves is where the first box was
        _grid.Set(_cachedRobotPosition, Empty);
        _grid.Set(firstBox.Position, Robot);
        _grid.Set(furthestNeighbour.Position, Box);
        _cachedRobotPosition = firstBox.Position;
    }

    private void PushBoxRow2(GridElement<char> firstBox, Direction direction)
    {
        switch (direction)
        {
            case Direction.East:
            case Direction.West:
            {
                var furthestNeighbour = firstBox;
                do
                {
                    furthestNeighbour = _grid.GetNeighbour(furthestNeighbour!.Position, direction);
                } while (IsBox(furthestNeighbour));

                // only push the row if we have found an empty space at the end that we can push the row into
                if (furthestNeighbour!.Value is not Empty)
                    return;

                // move the robot
                _grid.Set(firstBox.Position, Robot);
                _grid.Set(_cachedRobotPosition, Empty);
                _cachedRobotPosition = firstBox.Position;

                // shift all boxes
                if (direction is Direction.West)
                {
                    var diff = firstBox.Position.X - furthestNeighbour.Position.X;
                    for (var i = 1; i <= diff; ++i)
                    {
                        var newPos = new Point(firstBox.Position.X - i, firstBox.Position.Y);
                        _grid.Set(newPos, i % 2 == 0 ? BoxLeft : BoxRight);
                    }
                }
                else // east
                {
                    var diff = furthestNeighbour.Position.X - firstBox.Position.X;
                    for (var i = 1; i <= diff; ++i)
                    {
                        var newPos = new Point(firstBox.Position.X + i, firstBox.Position.Y);
                        _grid.Set(newPos, i % 2 != 0 ? BoxLeft : BoxRight);
                    }
                }

                break;
            }
            case Direction.North:
            {
                List<GridElement<char>> toMove = [];
                if (CanPushPart2(firstBox, Direction.North, toMove))
                {
                    // sort to start moving down from the top so that you don't overwrite the ones you still
                    // have to process
                    var sorted = toMove.OrderBy(b => b.Position.Y).ThenBy(b => b.Position.X).ToArray();
                    foreach (var box in sorted)
                    {
                        _grid.Set(box.Position + new Point(0, -1), box.Value);
                        _grid.Set(box.Position, Empty);
                    }

                    _grid.Set(_cachedRobotPosition, Empty);
                    _grid.Set(_cachedRobotPosition + new Point(0, -1), Robot);
                    _cachedRobotPosition += new Point(0, -1);
                }

                break;
            }
            case Direction.South:
            {
                List<GridElement<char>> toMove = [];
                if (CanPushPart2(firstBox, Direction.South, toMove))
                {
                    // sort descending to start moving up from the bottom so that you don't overwrite the ones you still
                    // have to process
                    var sorted = toMove.OrderByDescending(b => b.Position.Y).ThenBy(b => b.Position.X).ToArray();
                    foreach (var box in sorted)
                    {
                        _grid.Set(box.Position + new Point(0, 1), box.Value);
                        _grid.Set(box.Position, Empty);
                    }

                    _grid.Set(_cachedRobotPosition, Empty);
                    _grid.Set(_cachedRobotPosition + new Point(0, 1), Robot);
                    _cachedRobotPosition += new Point(0, 1);
                }

                break;
            }
        }
    }

    private bool CanPushPart2(
        GridElement<char> boxHalf,
        Direction direction,
        List<GridElement<char>> toShove)
    {
        if (direction is not Direction.North and not Direction.South)
            throw new Exception($"Invalid {direction}, this code is only for checking vertical pushing in part 2");

        var otherBoxHalf = GetOtherBoxHalf(boxHalf);
        var neighbour = _grid.GetNeighbour(boxHalf.Position, direction)!;
        var otherNeighbour = _grid.GetNeighbour(otherBoxHalf.Position, direction)!;

        if (neighbour.Value is Edge || otherNeighbour.Value is Edge)
            return false;

        if (neighbour.Value is Empty && otherNeighbour.Value is Empty)
        {
            toShove.Add(boxHalf);
            toShove.Add(otherBoxHalf);
            return true;
        }

        if (IsBox(neighbour) && !IsBox(otherNeighbour))
        {
            if (CanPushPart2(neighbour, direction, toShove))
            {
                toShove.Add(boxHalf);
                toShove.Add(otherBoxHalf);
                return true;
            }

            return false;
        }

        if (IsBox(otherNeighbour) && !IsBox(neighbour))
        {
            if (CanPushPart2(otherNeighbour, direction, toShove))
            {
                toShove.Add(boxHalf);
                toShove.Add(otherBoxHalf);
                return true;
            }

            return false;
        }

        if (CanPushPart2(neighbour, direction, toShove) &&
            CanPushPart2(otherNeighbour, direction, toShove))
        {
            toShove.Add(boxHalf);
            toShove.Add(otherBoxHalf);
            return true;
        }

        return false;
    }

    private GridElement<char> GetOtherBoxHalf(GridElement<char> box)
    {
        switch (box.Value)
        {
            case BoxLeft:
                return _grid.GetNeighbour(box.Position, Direction.East)!;
            case BoxRight:
                return _grid.GetNeighbour(box.Position, Direction.West)!;
            default:
                throw new Exception($"Trying to get other half for invalid box {box}");
        }
    }

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
            case BoxLeft:
            case BoxRight:
            {
                if (_part is 1)
                {
                    PushBoxRow(neighbour, direction);
                }
                else
                {
                    PushBoxRow2(neighbour, direction);
                }
            }
                break;
        }

        _currentStep++;
    }

    public int GpsSum()
    {
        var targetValue = _part is 1 ? Box : BoxLeft;
        return _grid.AllExtended()
            .Where(x => x.Value == targetValue)
            .Sum(box => 100 * box.Position.Y + box.Position.X);
    }
}