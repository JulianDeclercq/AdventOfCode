using System.Text.RegularExpressions;

namespace AdventOfCode2018.days;

public class Day3
{
    private class Claim
    {
        public int ID { get; init; }
        public int Left { get; init; }
        public int Top { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
    }
    
    public void Part1()
    {
        var regexHelper = new RegexHelper(new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)"),
            "id", "left", "top", "width", "height");

        var lines = File.ReadAllLines(@"..\..\..\input\day3.txt");
        var claims = new List<Claim>();
        foreach (var line in lines)
        {
            regexHelper.Match(line);
            claims.Add(new Claim
            {
                ID = regexHelper.GetInt("id"),
                Left = regexHelper.GetInt("left"),
                Top = regexHelper.GetInt("top"),
                Width = regexHelper.GetInt("width"),
                Height = regexHelper.GetInt("height")
            });
        }
        
        // determine grid dimensions
        var gridWidth = claims.Max(c => c.Left + c.Width);
        var gridHeight = claims.Max(c => c.Top + c.Height);
        var grid = new Grid<int>(gridWidth, gridHeight, Enumerable.Repeat(0, gridWidth * gridHeight), int.MinValue);

        foreach (var claim in claims)
        {
            for (var c = claim.Left; c < claim.Left + claim.Width; ++c)
            {
                for (var r = claim.Top; r < claim.Top + claim.Height; ++r)
                {
                    grid.Set(c, r, grid.At(c, r) + 1);
                }
            }
        }
        Console.WriteLine($"Day 3 part 1: {grid.All().Count(x => x > 1)}");
    }
}