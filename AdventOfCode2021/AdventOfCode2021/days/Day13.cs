using System.Text.RegularExpressions;
namespace AdventOfCode2021.days;

public class Day13
{
    // returns the grid and a list of folds 
    private static (Grid<char>, List<(char, int)>) ParseInput()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day13.txt");
        var pointRegex = new Regex(@"(\d+),(\d+)");
        var foldRegex = new Regex(@".+(x|y)=(\d+)");
        var folds = new List<(char, int)>();
        var width = int.MinValue;
        var height = int.MinValue;

        var points = new List<Point>();
        foreach (var line in lines)
        {
            var match = pointRegex.Match(line);
            if (match.Success)
            {
                points.Add(new Point(Helpers.ToInt(match.Groups[1]), Helpers.ToInt(match.Groups[2])));
                continue;
            }

            match = foldRegex.Match(line);
            if (match.Success)
            {
                var axis = match.Groups[1].ToString().First();
                var value = Helpers.ToInt(match.Groups[2]);

                if (width == int.MinValue && axis == 'x')
                    width = value * 2 + 1;

                if (height == int.MinValue && axis == 'y')
                    height = value * 2 + 1;
                   
                folds.Add((axis, value));
            }
        }

        var grid = new Grid<char>(width, height, Enumerable.Repeat('.', width * height), '@');
        foreach(var p in points) grid.Set(p, '#');
        return (grid, folds);
    }
    
    public void Part1()
    { 
        var (grid, folds) = ParseInput();
        var answer = Fold(grid, folds[0].Item1, folds[0].Item2).All().Count(c => c == '#'); 
        Console.WriteLine($"Answer is {answer}");
    }
    
    public void Part2()
    {
        var (grid, folds) = ParseInput();

        foreach (var fold in folds)
            grid = Fold(grid, fold.Item1, fold.Item2);

        Console.WriteLine(grid);
    }
  
    private static Grid<char> Fold(Grid<char> grid, char axis, int value)
   {
       return axis switch
       {
           'y' => HorizontalFold(grid, value),
           'x' => VerticalFold(grid, value),
           _ => grid // invalid axis
       };
   }

    private static Grid<char> HorizontalFold(Grid<char> grid, int value)
    {
        var result = grid.Copy();

        var size = grid.Width * (grid.Height / 2);
        for (var i = 0; i < size; ++i)
        {
            var p = grid.FromIndex(i);
              
            // already set
            if (grid.At(p) == '#')
                continue;
              
            var distToFold = Math.Abs(value - p.Y);
            var flipped = new Point(p.X, p.Y + (2 * distToFold));
            result.Set(i, grid.At(flipped));
        }

        // cut off folded part
        var all = result.All().ToList();
        var to = result.Index(new Point(0, value));
        var newGrid = new Grid<char>(result.Width, result.Height / 2, all.GetRange(0, to), '@');
           
        return newGrid;
    }

    private static Grid<char> VerticalFold(Grid<char> grid, int value)
    {
        var width = grid.Width / 2;
        var height = grid.Height;
           
        var cells = grid.All().ToArray();
        var leftGrid = new Grid<char>(width, height, Array.Empty<char>(), '@');
        for (var i = 0; i < height; ++i)
            leftGrid.AddRange(cells.Skip(width * i * 2 + i).Take(width)); // +i to skip fold line itself
           
        var result = leftGrid.Copy();
        for (var i = 0; i < width * height; ++i)
        {
            var p = leftGrid.FromIndex(i);
              
            // already set
            if (leftGrid.At(p) == '#')
                continue;
              
            var distToFold = Math.Abs(value - p.X);
            var flipped = new Point(p.X + (2 * distToFold), p.Y);
            result.Set(i, grid.At(flipped));
        }

        return result;
    }
}
