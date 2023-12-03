namespace AdventOfCode2021.days;

public class Day11
{
    private class Octopus
    {
        public Octopus(int energy)
        {
            Energy = energy;
        }

        public void AddEnergyLevel()
        {
            Energy++;
            
            if (Energy > 9)
                Flashed = true;
        }
        
        public int Energy;
        public bool Flashed = false;

        public override string ToString() => Energy.ToString();
    }
    
    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day11.txt");
        var grid = new Grid<Octopus>(lines[0].Length, lines.Length, 
            lines.SelectMany(l => l.ToCharArray())
                .Select(c => new Octopus(Helpers.ToInt(c))), new Octopus(int.MaxValue));

        var answer = 0;
        const int steps = 100;

        for (var i = 0; i < steps; ++i)
            answer += Step(grid);
        
        Console.WriteLine($"Day 11 part 1: {answer}");
    }
    
    public void Part2()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day11.txt");
        var grid = new Grid<Octopus>(lines[0].Length, lines.Length, 
            lines.SelectMany(l => l.ToCharArray())
                .Select(c => new Octopus(Helpers.ToInt(c))), new Octopus(int.MaxValue));

        int answer = 0, amountFlashed = 0, gridSize = grid.Width * grid.Height;
        for (var i = 0; amountFlashed != gridSize; ++i)
        {
            amountFlashed = Step(grid);
            answer = i + 1;
        }
        
        Console.WriteLine($"Day 11 part 2: {answer}");
    }

    private void AddEnergy(Grid<Octopus> grid, Octopus octopus, Point p)
    {
        // if this octopus already flashed, ignore it
        if (octopus.Flashed)
            return;
        
        octopus.AddEnergyLevel();

        // if the octopus didn't flash after adding an energy level, there is nothing left to do
        if (!octopus.Flashed)
            return;
            
        // if the octopus did flash now, transfer 1 energy to all of its neighbours
        foreach (var neighbour in grid.NeighbouringPoints(p))
            AddEnergy(grid, grid.At(neighbour), neighbour);
    }

    // returns the amount of flashes during this step
    private int Step(Grid<Octopus> grid)
    {
        foreach (var (location, octopus) in grid.AllExtended())
            AddEnergy(grid, octopus, location);

        var answer = grid.All().Count(o => o.Flashed);

        // reset the flashed ones
        var flashed = grid.All().Where(o => o.Flashed);
        foreach (var octopus in flashed)
        {
            octopus.Energy = 0;
            octopus.Flashed = false;
        }

        return answer;
    }
}