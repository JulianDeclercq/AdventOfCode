using System.Text.RegularExpressions;
using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day4
{
    private int _answer = 0;
    public void Solve()
    {
        var lines = File.ReadAllLines("input/day4.txt");
        const char invalid = '@';
        var grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), invalid);

        
        // horizontal
        Console.WriteLine("Horizontals");
        foreach (var rowCells in grid.Rows())
        {
            var row = new string(rowCells.Select(cell => cell.Value).ToArray());
            CountXmas(row);

            var reverse = new string(row.Reverse().ToArray());
            CountXmas(reverse);
        }
        
        // vertical
        Console.WriteLine("Verticals");
        foreach (var columCell in grid.Columns())
        {
            var column = new string(columCell.Select(cell => cell.Value).ToArray());
            CountXmas(column);

            var reverse = new string(column.Reverse().ToArray());
            CountXmas(reverse);
        }

        // SE diagonals right hand side of top left
        Console.WriteLine("SE diagonals");
        foreach (var element in grid.Rows().First())
        {
            var current = element;
            var neighbours = "" + current.Value;
            
            for (;;)
            {
                current = grid.GetNeighbour(current.Position, GridNeighbourType.Se);
                if (current is null)
                    break;

                neighbours += current.Value;
            }

            CountXmas(neighbours);
            CountXmas(new string(neighbours.Reverse().ToArray()));
        }
        
        // SE diagonals left hand side of top left
        // skip one since that's already handled in previous foreach
        foreach (var element in grid.Columns().First().Skip(1))
        {
            var current = element;
            var neighbours = "" + current.Value;
                
            for (;;)
            {
                current = grid.GetNeighbour(current.Position, GridNeighbourType.Se);
                if (current is null)
                    break;

                neighbours += current.Value;
            }

            CountXmas(neighbours);
            CountXmas(new string(neighbours.Reverse().ToArray()));
        }
        
        // SW diagonals left hand side of top right
        Console.WriteLine("SW diagonals");
        foreach (var element in grid.Rows().First())
        {
            var current = element;
            var neighbours = "" + current.Value;
            
            for (;;)
            {
                current = grid.GetNeighbour(current.Position, GridNeighbourType.Sw);
                if (current is null)
                    break;

                neighbours += current.Value;
            }

            CountXmas(neighbours);
            CountXmas(new string(neighbours.Reverse().ToArray()));
        }
        
        // SW diagonals right hand side of top right
        // skip one since that's already handled in previous foreach
        foreach (var element in grid.Columns().Last().Skip(1))
        {
            var current = element;
            var neighbours = "" + current.Value;
            
            for (;;)
            {
                current = grid.GetNeighbour(current.Position, GridNeighbourType.Sw);
                if (current is null)
                    break;

                neighbours += current.Value;
            }

            CountXmas(neighbours);
            CountXmas(new string(neighbours.Reverse().ToArray()));
        }
        Console.WriteLine(_answer);
    }

    private void CountXmas(string input)
    {
        var regex = new Regex("XMAS");
        var matches = regex.Matches(input);
        _answer += matches.Count;
        Console.WriteLine($"Found {matches.Count} matches in {input}, total is now at {_answer}");
    }
}