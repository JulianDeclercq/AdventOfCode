namespace AdventOfCode2022.Days;

public class Day8
{
    // DISCLAIMER: I HEAVILY OVERCOMPLICATED / OVER-ENGINEERED THiS DAY JUST BECAUSE I WANTED TO USE MY OLD
    // GRID COMPONENT FROM PREVIOUS YEARS.. I HAD FUN AT LEAST!
    public void Solve()
    {
        // var input = File.ReadAllLines(@"..\..\..\input\day8_example.txt");
        var input = File.ReadAllLines(@"..\..\..\input\day8.txt");

        int width = input[0].Length, height = input.Length;
        var grid = new Grid<int>(width, height,
            input.SelectMany(l => l.ToCharArray()).Select(Helpers.ToInt), int.MinValue);

        var count = 0;
        var allPoints = new List<Point>();
        for (var col = 0; col < width; ++col)
        {
            for (var row = 0; row < height; ++row)
            {
                var p = new Point(col, row);
                
                if (IsVisible(grid, p))
                    ++count;
            }
        }

        Console.WriteLine($"Day 8 part 1 {count}");

        var answer = grid.AllExtended().Keys.Max(p => ScenicScore(grid, p));
        Console.WriteLine($"Day 8 part 2 {answer}");
    }

    private static bool IsVisible(Grid<int> grid, Point p)
    {
        // All points on edge are visible.
        if (p.X == 0 || p.Y == 0)
            return true;

        if (p.X == grid.Width - 1 || p.Y == grid.Height - 1)
            return true;

        var column = grid.Column(p.X).ToList();
        var upperNeighbours = column.TakeWhile(ge => ge.Position.Y < p.Y);
        var lowerNeighbours = column.SkipWhile(ge => ge.Position.Y <= p.Y);

        var row = grid.Row(p.Y).ToList();
        var leftNeighbours = row.TakeWhile(ge => ge.Position.X < p.X);
        var rightNeighbours = row.SkipWhile(ge => ge.Position.X <= p.X);

        var pointValue = grid.At(p);
        var visible = upperNeighbours.All(un => un.Value < pointValue) ||
                      lowerNeighbours.All(ln => ln.Value < pointValue) ||
                      leftNeighbours.All(ln => ln.Value < pointValue) ||
                      rightNeighbours.All(rn => rn.Value < pointValue);

        return visible;
    }
    
    private static int ScenicScore(Grid<int> grid, Point p)
    {
        var column = grid.Column(p.X).ToList();
        var upperNeighbours = column.TakeWhile(ge => ge.Position.Y < p.Y).Reverse().ToList(); // reverse because of viewing from the point's perspective
        var lowerNeighbours = column.SkipWhile(ge => ge.Position.Y <= p.Y).ToList();

        var row = grid.Row(p.Y).ToList();
        var leftNeighbours = row.TakeWhile(ge => ge.Position.X < p.X).Reverse().ToList();
        var rightNeighbours = row.SkipWhile(ge => ge.Position.X <= p.X).ToList();

        var visibleTrees = new List<int>();
        var pointValue = grid.At(p);

        visibleTrees.Add(upperNeighbours.All(un => un.Value < pointValue)
                        ? upperNeighbours.Count
                        : upperNeighbours.FindIndex(un => un.Value >= pointValue) + 1); // +1 to include blocking tree
        
        visibleTrees.Add(lowerNeighbours.All(ln => ln.Value < pointValue)
            ? lowerNeighbours.Count
            : lowerNeighbours.FindIndex(ln => ln.Value >= pointValue) + 1);
        
        visibleTrees.Add(leftNeighbours.All(ln => ln.Value < pointValue)
            ? leftNeighbours.Count
            : leftNeighbours.FindIndex(ln => ln.Value >= pointValue) + 1);
        
        visibleTrees.Add(rightNeighbours.All(rn => rn.Value < pointValue)
            ? rightNeighbours.Count
            : rightNeighbours.FindIndex(rn => rn.Value >= pointValue) + 1);

        return visibleTrees.Aggregate(1, (current, next) => current * next);
    }
}