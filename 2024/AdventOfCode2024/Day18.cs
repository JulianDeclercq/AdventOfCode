using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day18(string inputPath)
{
    public void Solve()
    {
        int width, height, bytesToTake;
        if (inputPath.Contains("example"))
        {
            width = 7;
            height = 7;
            bytesToTake = 12;
        }
        else // real input
        {
            width = 71;
            height = 71;
            bytesToTake = 1024;
        }

        var input = File.ReadAllLines(inputPath);
        var grid = new Grid<char>(width, height, Enumerable.Repeat('.', width * height), '@');
        foreach (var line in input.Take(bytesToTake))
        {
            var split = line.Split(',').Select(int.Parse).ToArray();
            grid.Set(split[0], split[1], '#');
        }

        Console.WriteLine(grid);
    }
}
