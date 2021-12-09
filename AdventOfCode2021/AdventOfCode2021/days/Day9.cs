using System.Runtime.InteropServices.ComTypes;

namespace AdventOfCode2021.days;

public class Day9
{
    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day9.txt");
        var width = lines[0].Length;
        var height = lines.Length;
        var heightGrid = new Grid<int>(width, height, int.MaxValue);

        heightGrid.AddRange(lines.SelectMany(l => l.ToCharArray()).Select(c => int.Parse(char.ToString(c))));

        var answer = 0;
        for (var h = 0; h < height; ++h)
        {
            for (var w = 0; w < width; ++w)
            {
                //Console.WriteLine($"{new Point(w, h)}: {heightGrid.At(w, h)}");
                var el = heightGrid.At(w, h);

                var smallestNeighbour = heightGrid.Neighbours(w, h).Min();
                if (el < smallestNeighbour)
                    answer += RiskLevel(el);
            }
        }
        Console.WriteLine($"Day 9 part 1: {answer}");
    }
    
    //The risk level of a low point is 1 plus its height.
    private int RiskLevel(int i) => i + 1;
}
