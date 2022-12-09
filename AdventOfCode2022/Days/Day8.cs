namespace AdventOfCode2022.Days;

public class Day8
{
    // DISCLAIMER: I HEAVILY OVERCOMPLICATED / OVER-ENGINEERED THIS PROBLEM JUST BECAUSE I WANTED TO USE MY OLD
    // GRID COMPONENT FROM PREVIOUS YEARS, I HAD FUN AT LEAST!
    public void Solve()
    {
        // var input = File.ReadAllLines(@"..\..\..\input\day8_example.txt");
        var input = File.ReadAllLines(@"..\..\..\input\day8.txt");

        int width = input[0].Length, height = input.Length;
        var grid = new Grid<int>(width, height,
            input.SelectMany(l => l.ToCharArray()).Select(Helpers.ToInt), int.MinValue);

        var count = 0;
        for (var col = 0; col < width; ++col)
        {
            for (var row = 0; row < height; ++row)
            {
                var p = new Point(col, row);
                
                if (IsVisible(grid, p))
                    ++count;
            }
        }

        Console.WriteLine($"{count}");
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

        if (visible)
        {
            int brkpt = 5;
        }
        
        return visible;
    }
}