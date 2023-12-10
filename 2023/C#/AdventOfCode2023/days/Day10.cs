using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day10
{
    public void Part1()
    {
        var input = File.ReadAllLines("../../../input/Day10e2.txt");
        var grid = new Grid<char>(input.First().Length, input.Length, input.SelectMany(x => x), '?');
        Console.WriteLine(grid);
        
        // locate starting point
        var start = grid.AllExtended().Single(e => e.Value.Equals('S'));
        var neighbours = grid.NeighbouringPointsExtended(start.Position, includeDiagonals: false); // DIAGONALS DONT MAKE SENSE FOR PIPES
        var neighbour = neighbours.First(n => Pipes.Contains(n.Value)); // single path

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

        var visuals = grid.ShallowCopy();
        visuals.Set(start.Position, '*');
        Console.WriteLine(visuals);
 
        var current = neighbour;
        for (ulong iterations = 1;; iterations++)
        {
            // follow the flow
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
                    nextPosition = direction switch
                    {
                        Direction.East => current.Position + new Point(1, 0),
                        Direction.West => current.Position + new Point(-1, 0),
                        _ => throw new Exception("Wrong pipe direction")
                    };
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
                case 'S':
                {
                    Console.WriteLine("BACK AT START");
                    return;
                    break;
                }
            }
            
            // UPDATE VISUALS
            visuals.Set(current.Position, '*');
            Console.WriteLine(visuals);
            //
            
            var next = new GridElement<char>(nextPosition!, grid.At(nextPosition!));
            current = next;
            direction = nextDirection;
        }
    }

    private static readonly HashSet<char> Pipes = new() { '|', '-', 'L', 'J', '7', 'F', 'S' };

    private enum Direction
    {
        None = 0,
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }
}