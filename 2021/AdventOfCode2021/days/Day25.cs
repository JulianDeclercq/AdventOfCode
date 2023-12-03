namespace AdventOfCode2021.days;

public class Day25
{
    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day25.txt");
        var grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(l => l.ToCharArray()), '#');

        // start at 1 to count the last step itself as well (this wouldn't work for initial states that are stuck)
        var answer = 1;
        for (;; answer++)
        {
            if (Step(grid) == 0)
                break;
        }
        Console.WriteLine(answer);
    }

    // returns the total amount of cucumbers that moved
    private static int Step(Grid<char> grid)
    {
        var cucumbers = grid.AllExtended();
        var eastHerd = cucumbers.Where(c => c.Value == '>');
        var southHerd = cucumbers.Where(c => c.Value == 'v');

        // east herd, item1 is current position, item2 is target position (neighbour)
        var eastNeighbours = eastHerd.Select(c => new ValueTuple<GridElement<char>, GridElement<char>>(
            new GridElement<char>(c.Key, c.Value), 
            grid.GetEasternNeighbour(c.Key)));
        
        var canMoveEast = eastNeighbours.Where(n => n.Item2.Value == '.').ToList();
        foreach (var (original, target) in canMoveEast)
        {
            // move the cucumber
            grid.Set(original.Position, '.');
            grid.Set(target.Position, '>');
        }
        
        // south herd, item1 is current position, item2 is target position (neighbour)
        var southNeighbours = southHerd.Select(c => new ValueTuple<GridElement<char>, GridElement<char>>(
            new GridElement<char>(c.Key, c.Value), 
            grid.GetSouthernNeighbour(c.Key)));
        
        var canMoveSouth = southNeighbours.Where(n => n.Item2.Value == '.').ToList();
        foreach (var (original, target) in canMoveSouth)
        {
            // move the cucumber
            grid.Set(original.Position, '.');
            grid.Set(target.Position, 'v');
        }
        return canMoveEast.Count + canMoveSouth.Count;
    }
}