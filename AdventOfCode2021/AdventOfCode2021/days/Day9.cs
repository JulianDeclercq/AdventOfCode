namespace AdventOfCode2021.days;

public class Day9
{
    private Grid<int>? _grid = null;
    private Grid<int> Grid
    {
        get
        {
            if (_grid == null)
            {
                var lines = File.ReadAllLines(@"..\..\..\input\day9.txt");
                _grid = new Grid<int>(lines[0].Length, lines.Length, int.MaxValue);
                _grid.AddRange(lines.SelectMany(l => l.ToCharArray()).Select(c => int.Parse(char.ToString(c))));
            }

            return _grid;
        }
    }
    
    public void Part1()
    {
        var answer = 0;
        for (var h = 0; h < Grid.Height; ++h)
        {
            for (var w = 0; w < Grid.Width; ++w)
            {
                var height = Grid.At(w, h);
                if (height < Grid.Neighbours(w, h).Min())
                    answer += height + 1;
            }
        }
        Console.WriteLine($"Day 9 part 1: {answer}");
    }
    
    public void Part2()
    {
        // generate all basins
        var basins = new List<List<Point>>();
        for (var h = 0; h < Grid.Height; ++h)
        {
            for (var w = 0; w < Grid.Width; ++w)
            {
                var p = new Point(w, h);
                
                // peaks (height 9) are not part of any basin
                if (Grid.At(p) == 9)
                    continue;

                // if this point is already part of a basin, move on to the next one
                if (basins.SelectMany(point => point).Contains(p))
                    continue;

                // make sure the basin only contains unique elements and add it to the list of all basins
                basins.Add(GenerateBasin(p, Grid, new List<Point>(){}));
            }
        }

        var answer = basins.Select(x => x.Count)
            .OrderByDescending(x => x)
            .Take(3)
            .Aggregate(1, (current, previous) => current * previous);
        Console.WriteLine($"Day 9 part 2: {answer}");
    }
    
    private List<Point> GenerateBasin(Point point, Grid<int> grid, List<Point> current)
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
