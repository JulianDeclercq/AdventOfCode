using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day3
{
    public void Part1()
    {
        //const string inputFileName = "../../../input/Day3_example.txt";
        const string inputFileName = "../../../input/Day3.txt";
        var lines = File.ReadAllLines(inputFileName);
        var grid = new Grid<char>(lines.First().Length, lines.Length, lines.SelectMany(l => l), 'A');

        var sum = 0;
        var neighbours = new List<char>();
        for (var i = 0; i < grid.Height; ++i)
        {
            var row = grid.Row(i).ToArray();
            var current = "";
            for (var j = 0; j < row.Length; ++j)
            {
                var cell = row[j];
                
                if (char.IsDigit(cell.Value))
                {
                    current += cell.Value;
                    neighbours.AddRange(grid.Neighbours(cell.Position));
                    
                    if (j == row.Length - 1) // handle last
                    {
                        if (neighbours.Any(IsSymbol))
                        {
                            sum += int.Parse(current);
                            neighbours.Clear();
                        }
                    }
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(current))
                {
                    if (neighbours.Any(IsSymbol))
                    {
                        sum += int.Parse(current);
                        neighbours.Clear();
                    }
                    current = "";    
                }
            }
        }
        Console.WriteLine(sum);
    }

    private static readonly HashSet<char> Symbols = new() { '/', '*', '%', '$', '@', '&', '=', '+', '#', '-'};
    private static bool IsSymbol(char c) => Symbols.Contains(c);
}