using AdventOfCode2023.helpers;
using Direction = AdventOfCode2023.helpers.Helpers.Direction;

namespace AdventOfCode2023.days;

public class Day16
{
    public void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day16e.txt");
        var width = input.First().Length;
        var height = input.Length;
        var grid = new Grid<char>(width, height, input.SelectMany(x => x), '?');
        var energy = new Grid<int>(width, height, Enumerable.Repeat(0, width * height), '?');

//        TraverseBeamPath(grid, energy, new Point(0, 0), Direction.East);
        TraverseBeamPath(grid, energy, new Point(6, 7), Direction.South);
        
        Console.WriteLine(energy);
        Console.WriteLine(energy.All().Count(e => e != 0));

        var dbg = new Grid<char>(energy.Width, energy.Height, Enumerable.Repeat('.', width * height), '?');
        foreach (var ener in energy.AllExtended())
        {
            var set = ener.Value == 0 ? '.' : '#';
            dbg.Set(ener.Position, set);
        }
        Console.WriteLine(dbg);
    }

    private void TraverseBeamPath(Grid<char> grid, Grid<int> energy, Point start, Direction initialDirection)
    {
        var position = start;
        var direction = initialDirection;
        var inGrid = true;
        while (inGrid)
        {
            var current = energy.At(position);
            energy.Set(position, current + 1);
            Console.WriteLine(energy);

            switch (direction)
            {
                case Direction.North:
                    var northernNeighbour = grid.GetNeighbour(position, Direction.North);
                    if (northernNeighbour == null)
                    {
                        inGrid = false;
                        break;
                    }
                    
                    position = northernNeighbour.Position;
                    switch (northernNeighbour.Value)
                    {
                        case '/':
                            direction = Direction.East;
                            break;
                        case '\\':
                            direction = Direction.West;
                            break;
                        case '-':
                            var easternNeighbourN = grid.GetNeighbour(position, Direction.East);
                            if (easternNeighbourN != null)
                                TraverseBeamPath(grid, energy, easternNeighbourN.Position, Direction.East);

                            var westernNeighbourN = grid.GetNeighbour(position, Direction.West);
                            if (westernNeighbourN != null)
                                TraverseBeamPath(grid, energy, westernNeighbourN.Position, Direction.West);
                            break;
                    }
                    break;

                case Direction.East:
                    var easternNeighbour = grid.GetNeighbour(position, Direction.East);
                    if (easternNeighbour == null)
                    {
                        inGrid = false;
                        break;
                    }
                    
                    position = easternNeighbour.Position;
                    switch (easternNeighbour.Value)
                    {
                        case '/': 
                            direction = Direction.North;
                            break;
                        case '\\': 
                            direction = Direction.South;
                            break;
                        case '|':
                            var northernNeighbourE = grid.GetNeighbour(position, Direction.North);
                            if (northernNeighbourE != null)
                                TraverseBeamPath(grid, energy, northernNeighbourE.Position, Direction.North);

                            var southernNeighbourE = grid.GetNeighbour(position, Direction.South);
                            if (southernNeighbourE != null)
                                TraverseBeamPath(grid, energy, southernNeighbourE.Position, Direction.South);
                            break;
                    }
                    break;
                case Direction.South:
                    var southernNeighbour = grid.GetNeighbour(position, Direction.South);
                    if (southernNeighbour == null)
                    {
                        inGrid = false;
                        break;
                    }

                    position = southernNeighbour.Position;
                    switch (southernNeighbour.Value)
                    {
                        case '/':
                            direction = Direction.West;
                            break;
                        case '\\':
                            direction = Direction.East;
                            break;
                        case '-':
                            var easternNeighbourS = grid.GetNeighbour(position, Direction.East);
                            if (easternNeighbourS != null)
                                TraverseBeamPath(grid, energy, easternNeighbourS.Position, Direction.East);

                            var westernNeighbourS = grid.GetNeighbour(position, Direction.West);
                            if (westernNeighbourS != null)
                                TraverseBeamPath(grid, energy, westernNeighbourS.Position, Direction.West);
                            break;
                    }
                    break;
                case Direction.West:
                    var westernNeighbour = grid.GetNeighbour(position, Direction.West);
                    if (westernNeighbour == null)
                    {
                        inGrid = false;
                        break;
                    }

                    position = westernNeighbour.Position;
                    switch (westernNeighbour.Value)
                    {
                        case '/':
                            direction = Direction.South;
                            break;
                        case '\\':
                            direction = Direction.North;
                            break;
                        case '|':
                            var northernNeighbourW = grid.GetNeighbour(position, Direction.North);
                            if (northernNeighbourW != null)
                                TraverseBeamPath(grid, energy, northernNeighbourW.Position, Direction.North);

                            var southernNeighbourW = grid.GetNeighbour(position, Direction.South);
                            if (southernNeighbourW != null)
                                TraverseBeamPath(grid, energy, southernNeighbourW.Position, Direction.South);
                            break;
                    }
                    break;
                default: throw new Exception($"Invalid direction {direction}");
            }
        }
    }
}