namespace AdventOfCode2022.Days;

public class Day18
{
    public void Solve()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day18_test.txt");
        // var lines = File.ReadAllLines(@"..\..\..\input\day18_example.txt");

        var droplets = lines
            .Select(line => line.Split(','))
            .Select(x => x.Select(int.Parse).ToArray())
            .Select(integers => new Point3D(integers[0], integers[1], integers[2])).ToArray();

        const int dim = 7; // x, y and z (TODO: Make dynamic)
        const char lava = 'o', empty = '.', invalid = '$';
        
        var elements = Enumerable.Range(0, dim)
            .Select(_ => new Grid<char>(dim, dim, Enumerable.Range(0, dim * dim).Select(_ => empty), invalid));
        
        var grid = new Grid3D<char>(dim, dim, dim, elements, invalid);

        foreach (var droplet in droplets)
            grid.Set(droplet, lava);
        
        Console.WriteLine(grid);
        
        // Collect all neighbours
        var lavaNeighbours = droplets.SelectMany(x => grid.Neighbours(x, false)).Count(n => n == lava);
        Console.WriteLine($"Day 18 part 1: {droplets.Length * 6 - lavaNeighbours}");
    }
    
}