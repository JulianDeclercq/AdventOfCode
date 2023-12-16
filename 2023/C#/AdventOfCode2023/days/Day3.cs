using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day3
{
    public static void Part1()
    {
        const string inputFileName = "../../../input/Day3.txt";
        var lines = File.ReadAllLines(inputFileName);
        var grid = new Grid<char>(lines.First().Length, lines.Length, lines.SelectMany(l => l), 'A');

        var sum = 0;
        var neighbours = new List<char>();
        foreach (var r in grid.Rows())
        {
            var row = r.ToArray();
            var current = "";
            for (var j = 0; j < row.Length; ++j)
            {
                var cell = row[j];
                if (char.IsDigit(cell.Value))
                {
                    current += cell.Value;
                    neighbours.AddRange(grid.Neighbours(cell.Position));

                    if (j != row.Length - 1) // done if not last in row, otherwise process number
                        continue;
                }
                sum += ProcessNumberPart1(neighbours, ref current); // no more digits or end of line, parse the number
            }
        }
        Console.WriteLine(sum);
    }
    
    public static void Part2()
    {
        const string inputFileName = "../../../input/Day3.txt";
        var lines = File.ReadAllLines(inputFileName);
        var grid = new Grid<char>(lines.First().Length, lines.Length, lines.SelectMany(l => l), 'A');

        var neighbours = new List<GridElement<char>>();
        var gears = new Dictionary<Point, Info>();

        foreach (var r in grid.Rows())
        {
            var row = r.ToArray();
            var builder = new NumberBuilder();
            for (var j = 0; j < row.Length; ++j)
            {
                var cell = row[j];
                if (char.IsDigit(cell.Value))
                {
                    builder.Current += cell.Value;
                    builder.Points.Add(cell.Position);
                    neighbours.AddRange(grid.NeighboursExtended(cell.Position));

                    if (j != row.Length - 1) // done if not last in row, otherwise process number
                        continue;
                }
                ProcessNumberPart2(gears, neighbours, builder); // no more digits or end of line, parse the number
            }            
        }

        Console.WriteLine(gears
            .Where(g => g.Value.Numbers.Count == 2)
            .Sum(g => g.Value.Numbers.Aggregate(1, (curr, next) => curr * next)));
    }

    #region Part 1
    private static readonly HashSet<char> Symbols = new() { '/', '*', '%', '$', '@', '&', '=', '+', '#', '-'};
    private static bool IsSymbol(char c) => Symbols.Contains(c);
    
    private static int ProcessNumberPart1(ICollection<char> neighbours, ref string current)
    {
        if (string.IsNullOrWhiteSpace(current))
            return 0;

        var number = 0;
        if (neighbours.Any(IsSymbol))
        {
            number = int.Parse(current);
            neighbours.Clear();
        }
        current = "";
        return number;
    }
    #endregion

    #region Part 2
    private class Info
    {
        public readonly List<int> Numbers = new();
        public readonly List<Point> Points = new(); // The points from which the digits were taken
    }

    private class NumberBuilder
    {
        public string Current = "";
        public readonly HashSet<Point> Points = new(); // The points from which the digits were taken

        public void Clear()
        {
            Current = "";
            Points.Clear();
        }
    }
    
    private static bool IsGear(GridElement<char> ge) => ge.Value.Equals('*');
    private static void ProcessNumberPart2(IDictionary<Point, Info> gears, ICollection<GridElement<char>> neighbours,
        NumberBuilder builder)
    {
        if (string.IsNullOrWhiteSpace(builder.Current))
            return;
        
        var neighbouringGears = neighbours.Where(IsGear).ToArray();
        foreach (var gear in neighbouringGears)
        {
            var info = gears.TryGetValue(gear.Position, out var existing) ? existing : new Info();
            
            // Don't add number twice
            if (builder.Points.Overlaps(info.Points))
                continue;
            
            info.Numbers.Add(int.Parse(builder.Current));
            info.Points.AddRange(builder.Points);
            gears[gear.Position] = info;
        }
        neighbours.Clear();
        builder.Clear();
    }
    #endregion
}