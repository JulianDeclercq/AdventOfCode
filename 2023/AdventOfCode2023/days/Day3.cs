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

    private static bool IsGear(GridElement<char> ge) => ge.Value.Equals('*'); 
    
    public void Part2()
    {
        //const string inputFileName = "../../../input/Day3_example.txt";
        const string inputFileName = "../../../input/Day3.txt";
        var lines = File.ReadAllLines(inputFileName);
        var grid = new Grid<char>(lines.First().Length, lines.Length, lines.SelectMany(l => l), 'A');

        var neighbours = new List<GridElement<char>>();
        var gears = new Dictionary<Point, List<int>>();
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
                    neighbours.AddRange(grid.NeighbouringPointsExtended(cell.Position));
                    
                    if (j == row.Length - 1) // handle last
                    {
                        var neighbouringGears = neighbours.Where(IsGear).ToArray();
                        foreach (var gear in neighbouringGears)
                        {
                            var numbers = gears.TryGetValue(gear.Position, out var n) ? n : new List<int>();
                            numbers.Add(int.Parse(current));
                            gears[gear.Position] = numbers;
                        }
                        neighbours.Clear();
                        current = "";
                    }
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(current))
                {
                    var neighbouringGears = neighbours.Where(IsGear).ToArray();
                    foreach (var gear in neighbouringGears)
                    {
                        var numbers = gears.TryGetValue(gear.Position, out var n) ? n : new List<int>();
                        numbers.Add(int.Parse(current));
                        gears[gear.Position] = numbers;
                    }
                    neighbours.Clear();
                    current = "";
                }
            }
        }

        // TODO: MAYBE IN THE INPUT DISTINCT IS TOO NAIVE, IF THE GEAR IS ON THE BORDER OF 2 IDENTICAL VALUE, BUT DIFFERENT NUMBERS
        var answer = gears
            .Select(g => g.Value.Distinct().ToArray())
            .Where(g => g.Length == 2)
            .Sum(g => g.Aggregate(1, (curr, next) => curr * next));
        Console.WriteLine(answer);
    }

    private static readonly HashSet<char> Symbols = new() { '/', '*', '%', '$', '@', '&', '=', '+', '#', '-'};
    private static bool IsSymbol(char c) => Symbols.Contains(c);
}