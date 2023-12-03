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
    
    private class Info
    {
        public List<int> Numbers = new();
        public List<Point> Consumed = new(); // to avoid adding the same number twice
    }

    private class NumberBuilder
    {
        public string Current = "";
        public HashSet<Point> Points = new(); // The points from which the digits were taken

        public void Clear()
        {
            Current = "";
            Points.Clear();
        }
    }
    
    public void Part2()
    {
        //const string inputFileName = "../../../input/Day3_example.txt";
        const string inputFileName = "../../../input/Day3.txt";
        var lines = File.ReadAllLines(inputFileName);
        var grid = new Grid<char>(lines.First().Length, lines.Length, lines.SelectMany(l => l), 'A');

        var neighbours = new List<GridElement<char>>();
        var gears = new Dictionary<Point, Info>();

        foreach (var element in grid.Rows())
        {
            var builder = new NumberBuilder();
            var row = element.ToArray();
            for (var j = 0; j < row.Length; ++j)
            {
                var cell = row[j];
                if (char.IsDigit(cell.Value))
                {
                    builder.Current += cell.Value;
                    builder.Points.Add(cell.Position);
                    neighbours.AddRange(grid.NeighbouringPointsExtended(cell.Position));
                    
                    if (j == row.Length - 1) // handle last
                        ProcessNumber(gears, neighbours, builder);
                }
                else // no more digits, parse the number
                {
                    if (!string.IsNullOrWhiteSpace(builder.Current))
                        ProcessNumber(gears, neighbours, builder);    
                }
            }            
        }

        var answer = gears
            .Where(g => g.Value.Numbers.Count == 2)
            .Sum(g => g.Value.Numbers.Aggregate(1, (curr, next) => curr * next));

        Console.WriteLine(answer);
    }

    private static void ProcessNumber(IDictionary<Point, Info> gears, ICollection<GridElement<char>> neighbours,
        NumberBuilder builder)
    {
        var neighbouringGears = neighbours.Where(IsGear).ToArray();
        foreach (var gear in neighbouringGears)
        {
            var info = gears.TryGetValue(gear.Position, out var existing) ? existing : new Info();
            if (!builder.Points.Overlaps(info.Consumed))
            {
                info.Numbers.Add(int.Parse(builder.Current));
                info.Consumed.AddRange(builder.Points);
                gears[gear.Position] = info;
            }
        }
        neighbours.Clear();
        builder.Clear();
    }

    private static readonly HashSet<char> Symbols = new() { '/', '*', '%', '$', '@', '&', '=', '+', '#', '-'};
    private static bool IsSymbol(char c) => Symbols.Contains(c);
    private static bool IsGear(GridElement<char> ge) => ge.Value.Equals('*');
}