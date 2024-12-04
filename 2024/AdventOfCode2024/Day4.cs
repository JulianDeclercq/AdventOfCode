using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day4
{
    private int _part1Answer = 0;
    public void SolvePart1()
    {
        var lines = File.ReadAllLines("input/day4.txt");
        const char invalid = '@';
        var grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), invalid);
        
        // horizontal
        foreach (var rowCells in grid.Rows())
        {
            var row = new string(rowCells.Select(cell => cell.Value).ToArray());
            CountXmas(row);

            var reverse = new string(row.Reverse().ToArray());
            CountXmas(reverse);
        }
        
        // vertical
        foreach (var columCell in grid.Columns())
        {
            var column = new string(columCell.Select(cell => cell.Value).ToArray());
            CountXmas(column);

            var reverse = new string(column.Reverse().ToArray());
            CountXmas(reverse);
        }

        // SE diagonals right hand side of top left
        Diagonal(grid, grid.Rows().First(), GridNeighbourType.Se);
        
        // SE diagonals left hand side of top left, skip one since that's already handled in previous Diagonal
        Diagonal(grid, grid.Columns().First().Skip(1), GridNeighbourType.Se);
        
        // SW diagonals left hand side of top right
        Diagonal(grid, grid.Rows().First(), GridNeighbourType.Sw);
        
        // SW diagonals right hand side of top right, skip one since that's already handled in the previous Diagonal
        Diagonal(grid, grid.Columns().Last().Skip(1), GridNeighbourType.Sw);
        Console.WriteLine(_part1Answer);
    }

    public void SolvePart2()
    {
        var lines = File.ReadAllLines("input/day4.txt");
        const char invalid = '@';
        var grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), invalid);
        var answer = 0;
        foreach (var element in grid.AllExtended())
        {
            if (element.Value is not 'A')
                continue;
            
            // check from the middle outwards, so ignore elements without all neighbours
            if (grid.Neighbours(element.Position).ToArray().Length != 8)
                continue;

            var nw = grid.GetNeighbour(element.Position, GridNeighbourType.Nw)!.Value;
            var ne = grid.GetNeighbour(element.Position, GridNeighbourType.Ne)!.Value;
            var sw = grid.GetNeighbour(element.Position, GridNeighbourType.Sw)!.Value;
            var se = grid.GetNeighbour(element.Position, GridNeighbourType.Se)!.Value;

            // South-east diagonal
            if ((nw is 'M' && se is 'S') || (nw is 'S' && se is 'M'))
            {
                // South-west diagonal
                if ((sw is 'M' && ne is 'S') || (sw is 'S' && ne is 'M'))
                {
                    answer++;
                }
            }
        }
        Console.WriteLine(answer);
    }

    private void CountXmas(string input)
    {
        var regex = new Regex("XMAS");
        var matches = regex.Matches(input);
        _part1Answer += matches.Count;
        Console.WriteLine($"Found {matches.Count} matches in {input}, total is now at {_part1Answer}");
    }

    private void Diagonal(Grid<char> grid, IEnumerable<GridElement<char>> collection, GridNeighbourType direction)
    {
        foreach (var element in collection)
        {
            var current = element;
            var neighbours = "" + current.Value;
            
            for (;;)
            {
                current = grid.GetNeighbour(current.Position, direction);
                if (current is null)
                    break;

                neighbours += current.Value;
            }

            CountXmas(neighbours);
            CountXmas(new string(neighbours.Reverse().ToArray()));
        }
    }
}