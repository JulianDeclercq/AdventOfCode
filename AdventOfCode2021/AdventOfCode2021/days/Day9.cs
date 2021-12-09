namespace AdventOfCode2021.days;

public class Day9
{
    private static int RiskLevel(int i) => i + 1;
    
    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day9.txt");
        var gridWidth = lines[0].Length;
        var gridHeight = lines.Length;
        var heightGrid = new Grid<int>(gridWidth, gridHeight, int.MaxValue);

        heightGrid.AddRange(lines.SelectMany(l => l.ToCharArray()).Select(c => int.Parse(char.ToString(c))));

        var answer = 0;
        for (var h = 0; h < gridHeight; ++h)
        {
            for (var w = 0; w < gridWidth; ++w)
            {
                var height = heightGrid.At(w, h);
                if (height < heightGrid.Neighbours(w, h).Min())
                    answer += RiskLevel(height);
            }
        }
        Console.WriteLine($"Day 9 part 1: {answer}");
    }
    
    public void Part2()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day9.txt");
        var gridWidth = lines[0].Length;
        var gridHeight = lines.Length;
        var heightGrid = new Grid<int>(gridWidth, gridHeight, int.MaxValue);

        heightGrid.AddRange(lines.SelectMany(l => l.ToCharArray()).Select(c => int.Parse(char.ToString(c))));
        
        // generate all basins
        var basins = new List<List<Point>>();
        basins.Add(new List<Point>() {new(1, 2), new(3, 4)});
        for (var h = 0; h < gridHeight; ++h)
        {
            for (var w = 0; w < gridWidth; ++w)
            {
                var p = new Point(w, h);
                
                // peaks (height 9) are not part of any basin
                var value = heightGrid.At(p);
                if (value == 9)
                    continue;

                // if this point is already part of a basin, move on to the next one
                if (basins.SelectMany(x => x).Contains(p))
                    continue;

                // create the basin this point is part of
                var basin = GenerateBasin(p, heightGrid, new List<Point>(){p});
                
                // make sure the basin only contains unique elements and add it to the list of all basins
                basins.Add(basin.ToHashSet().ToList());
            }
        }

        var answer = basins.Select(x => x.Count)
            .OrderByDescending(x => x)
            .Take(3)
            .Aggregate(1, (current, previous) => current * previous);
        Console.WriteLine($"Day 9 part 2: {answer}");
    }

    private IEnumerable<Point> GenerateBasin(Point point, Grid<int> grid, List<Point> current)
    {
        // check for new points to add
        var newPoints = grid.NeighbouringPoints(point, false)
            .Where(x => grid.At(x) != 9 && !current.Contains(x)).ToList();

        // add the new points to current
        current.AddRange(newPoints);
        
        // check for new basin candidates
        foreach (var p in newPoints)
            GenerateBasin(p, grid, current);

        return current;
    }
}
