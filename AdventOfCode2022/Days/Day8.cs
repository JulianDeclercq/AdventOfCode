namespace AdventOfCode2022.Days;

public class Day8
{
    public static void Solve()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day8.txt");

        int width = input[0].Length, height = input.Length;
        var grid = new Grid<int>(width, height,
            input.SelectMany(l => l.ToCharArray()).Select(Helpers.ToInt), int.MinValue);

        var gridPoints = grid.AllExtended().Keys;
        Console.WriteLine($"Day 8 part 1 {gridPoints.Count(p => IsVisible(grid, p))}");
        Console.WriteLine($"Day 8 part 2 {gridPoints.Max(p => ScenicScore(grid, p))}");
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

        var pointValue = grid.At(p);
        var visibleTrees = new List<int>
        {
            upperNeighbours.All(un => un.Value < pointValue)
                ? upperNeighbours.Count
                : upperNeighbours.FindIndex(un => un.Value >= pointValue) + 1, // +1 to include blocking tree itself
            
            lowerNeighbours.All(ln => ln.Value < pointValue)
                ? lowerNeighbours.Count
                : lowerNeighbours.FindIndex(ln => ln.Value >= pointValue) + 1,
            
            leftNeighbours.All(ln => ln.Value < pointValue)
                ? leftNeighbours.Count
                : leftNeighbours.FindIndex(ln => ln.Value >= pointValue) + 1,
            
            rightNeighbours.All(rn => rn.Value < pointValue)
                ? rightNeighbours.Count
                : rightNeighbours.FindIndex(rn => rn.Value >= pointValue) + 1
        };

        return visibleTrees.Aggregate(1, (current, next) => current * next);
    }
}