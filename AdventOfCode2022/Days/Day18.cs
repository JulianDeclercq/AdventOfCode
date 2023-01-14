namespace AdventOfCode2022.Days;

public class Day18
{
    public void Solve()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day18_example.txt");
        // var lines = File.ReadAllLines(@"..\..\..\input\day18.txt");

        var cubes = lines
            .Select(line => line.Split(','))
            .Select(x => x.Select(int.Parse).ToArray())
            .Select(integers => new Point3D(integers[0], integers[1], integers[2])).ToArray();

        const int dim = 7; // x, y and z (TODO: Make dynamic)
        const char lava = 'o', air = '.', invalid = '$';
        
        var elements = Enumerable.Range(0, dim)
            .Select(_ => new Grid<char>(dim, dim, Enumerable.Range(0, dim * dim).Select(_ => air), invalid));
        
        var grid = new Grid3D<char>(dim, dim, dim, elements, invalid);

        foreach (var cube in cubes)
            grid.Set(cube, lava);

        var epicPoint = new Point3D(2, 2, 5);
        grid.Set(epicPoint, 'W');
        Console.WriteLine(grid);
        
        // Collect all neighbours
        var cubeNeighbours = cubes.SelectMany(c => grid.Neighbours(c)).Count(n => n == lava);
        
        var trappedAir = grid.AllPoints()
            .Where(p => grid.At(p) is not lava)
            .Select(p => grid.Neighbours(p))
            .Count(neighbours => neighbours.All(n => n == lava));

        const int sidesPerCube = 6;
        var part1 = cubes.Length * sidesPerCube - cubeNeighbours;
        Console.WriteLine($"Day 18 part 1: {part1}");
        Console.WriteLine($"Day 18 part 2: {part1 - sidesPerCube * trappedAir}");
        
        // Console.WriteLine(string.Join('|', grid.Row(2).Select(x => x.Position.Y)));
        Console.WriteLine(string.Join('|', grid.Column(2, 0).Select(x => x.Position)));
        Console.WriteLine(grid.Column(2, 0).Count());
        
        // answer to part 2 is answer to part 1 - 6 * amountofentrappedcubes
    }

    // private bool IsInsideDroplet(Point3D p, Grid3D<char> grid)
    // {
    //     var row = grid.
    // }
    
}