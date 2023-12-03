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
                _grid = new Grid<int>(lines[0].Length, lines.Length, 
                    lines.SelectMany(l => l.ToCharArray()).Select(Helpers.ToInt), int.MaxValue);
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

                // if this point is already part of any basin, move on to the next point
                if (basins.SelectMany(point => point).Contains(p))
                    continue;

                // generate the basin and add it to the list
                basins.Add(GenerateBasin(p, new List<Point>()));
            }
        }

        var answer = basins.Select(x => x.Count)
            .OrderByDescending(x => x)
            .Take(3)
            .Aggregate(1, (current, previous) => current * previous);
        Console.WriteLine($"Day 9 part 2: {answer}");
    }

    private List<Point> GenerateBasin(Point point, List<Point> current)
    {
        // check for new points to add
        var newPoints = Grid.NeighbouringPoints(point, false)
            .Where(p => Grid.At(p) != 9 && !current.Contains(p)).ToList();

        // add the new points to current
        current.AddRange(newPoints);
        
        // check for new basin candidates
        foreach (var p in newPoints)
            GenerateBasin(p, current);

        return current;
    }
}
