using AdventOfCode2023.helpers;
using Direction = AdventOfCode2023.helpers.Helpers.Direction;

namespace AdventOfCode2023.days;

public class Day10
{
    public static void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day10.txt");
        var grid = new Grid<char>(input.First().Length, input.Length, input.SelectMany(x => x), '?');
        
        var start = grid.AllExtended().Single(e => e.Value.Equals('S'));
        var pipeNeighbours = grid.NeighboursExtended(start.Position, includeDiagonals: false).Where(n => Pipes.Contains(n.Value));

        // check valid ones
        var validDirections = new Dictionary<char, HashSet<Direction>>
        {
            ['|'] = new() {Direction.North, Direction.South},
            ['-'] = new() {Direction.East, Direction.West},
            ['L'] = new() {Direction.South, Direction.West},
            ['J'] = new() {Direction.East, Direction.South},
            ['7'] = new() {Direction.East, Direction.North},
            ['F'] = new() {Direction.West, Direction.North},
        };
        
        var validNeighbours = new List<(GridElement<char> element, Direction direction)>();
        foreach (var neighbour in pipeNeighbours)
        {
            // determine the direction
            var diff = neighbour.Position - start.Position;
            var direction = diff switch
            {
                { X: 0, Y: -1 } => Direction.North,
                { X: 1, Y: 0 } => Direction.East,
                { X: 0, Y: 1 } => Direction.South,
                { X: -1, Y: 0 } => Direction.West,
                _ => throw new Exception("Couldn't determine direction")
            };
            
            if (validDirections[neighbour.Value].Contains(direction))
                validNeighbours.Add((neighbour, direction));
        }

        var distances = new Dictionary<Point, int>();
        foreach (var neighbour in validNeighbours)
        {
            TraversePipe(neighbour.element, neighbour.direction, grid, distances);
            //break; // PART2: ONLY WALK THROUGH ONCE
        }

        Console.WriteLine(distances.Max(d => d.Value));
    }

    private static void TraversePipe(GridElement<char> start, Direction direction, Grid<char> grid, Dictionary<Point, int> distances) 
    {
        var current = start;
        for (var iterations = 1;; iterations++) // start at one because we skip the first start node (S) node
        {
            Point? nextPosition = null;
            var nextDirection = direction;
            switch (current.Value)
            {
                case '|': // direction stays the same
                {
                    nextPosition = direction switch
                    {
                        Direction.North => current.Position + new Point(0, -1),
                        Direction.South => current.Position + new Point(0, 1),
                        _ => throw new Exception("Wrong pipe direction")
                    };
                    break;
                }
                case '-': // direction stays the same
                {
                    try
                    {
                        nextPosition = direction switch
                        {
                            Direction.East => current.Position + new Point(1, 0),
                            Direction.West => current.Position + new Point(-1, 0),
                        };
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Wrong pipe direction current {current}, dir {direction}");
                    }
                    break;
                }
                case 'L':
                {
                    if (direction is Direction.South)
                    {
                        nextPosition = current.Position + new Point(1, 0); // go east
                        nextDirection = Direction.East;
                    } else if (direction is Direction.West)
                    {
                        nextPosition = current.Position + new Point(0, -1); // go north
                        nextDirection = Direction.North;
                    } else throw new Exception("Wrong pipe direction");
                    break;
                }
                case 'J':
                {
                    if (direction is Direction.East)
                    {
                        nextPosition = current.Position + new Point(0, -1); // go north
                        nextDirection = Direction.North;
                    } else if (direction is Direction.South)
                    {
                        nextPosition = current.Position + new Point(-1, 0); // go west
                        nextDirection = Direction.West;
                    } else throw new Exception("Wrong pipe direction");
                    break;
                }
                case '7':
                {
                    if (direction is Direction.North)
                    {
                        nextPosition = current.Position + new Point(-1, 0); // go west
                        nextDirection = Direction.West;
                    } else if (direction is Direction.East)
                    {
                        nextPosition = current.Position + new Point(0, 1); // go south
                        nextDirection = Direction.South;
                    } else throw new Exception("Wrong pipe direction");
                    break;
                }
                case 'F':
                {
                    if (direction is Direction.West)
                    {
                        nextPosition = current.Position + new Point(0, 1); // go south
                        nextDirection = Direction.South;
                    } else if (direction is Direction.North)
                    {
                        nextPosition = current.Position + new Point(1, 0); // go east
                        nextDirection = Direction.East;
                    } else throw new Exception("Wrong pipe direction");
                    break;
                }
                case 'S': // back at start
                {
                    return;
                }
            }

            var currentDistance = distances.GetValueOrDefault(current.Position, int.MaxValue);
            distances[current.Position] = Math.Min(currentDistance, iterations);
            
            // prepare for next iteration
            var next = new GridElement<char>(nextPosition!, grid.At(nextPosition!));
            current = next;
            direction = nextDirection;
        }
    }

    private static readonly HashSet<char> Pipes = new() { '|', '-', 'L', 'J', '7', 'F', 'S' };
}