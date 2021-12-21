using System.Reflection.Emit;

namespace AdventOfCode2021.days;

public class Day11
{
    public class Octopus
    {
        public Octopus(int energy)
        {
            Energy = energy;
            
        }

        public void AddEnergyLevel()
        {
            if (Flashed)
                return;
            
            Energy += 1;
            if (Energy > 9)
                Flashed = true;
        }
        
        public int Energy;
        public bool Flashed = false;
        public int Index; // index in grid (kinda weird that this has to be saved here, i should find something on that in the generic grid)

        public override string ToString() => Energy.ToString();
    }
    
    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day11.txt");
        var grid = new Grid<Octopus>(lines[0].Length, lines.Length, 
            lines.SelectMany(l => l.ToCharArray())
                .Select(c => new Octopus(Helpers.ToInt(c))), new Octopus(int.MaxValue));

        // assign the indices (TODO: refactor so they don't have to be stored on the object)        
        for (var i = 0; i < grid.Width * grid.Height; ++i)
            grid.At(i).Index = i;

        const int steps = 100;
        const bool print = false;
        var answer = 0;

        if (print) Console.WriteLine(grid); 
        for (var i = 0; i < steps; ++i)
        {
            answer += Step(grid);

            if (!print)
                continue;

            Console.WriteLine($"Grid after {i + 1} step(s)");
            Console.WriteLine(grid);
        }
        
        Console.WriteLine($"Day 11 part 1: {answer}");
    }

    private void AddEnergy(Grid<Octopus> grid, Octopus octo, Point p)
    {
        // if this octo already flashed, ignore it
        if (octo.Flashed)
            return;
        
        octo.AddEnergyLevel();

        // if the octo didn't flash after adding an energy level, there is nothing left to do
        if (!octo.Flashed)
            return;
            
        // if the octo did flash now, transfer 1 energy to all of its neighbours
        foreach (var neighbour in grid.Neighbours(p.X, p.Y))
        {   
            AddEnergy(grid, neighbour, grid.FromIndex(neighbour.Index));
        }
        
    }

    // returns the amount of flashes during this step
    private int Step(Grid<Octopus> grid)
    {
        foreach (var octopus in grid.All())
            AddEnergy(grid, octopus, grid.FromIndex(octopus.Index));

        var answer = grid.All().Count(o => o.Flashed);

        // TODO: Check if linQ where !flashed can work here or if it doesnt work because of references and such shenanigans
        foreach (var octopus in grid.All())
        {
            if (!octopus.Flashed)
                continue;

            octopus.Energy = 0;
            octopus.Flashed = false;
        }

        return answer;
    }
}