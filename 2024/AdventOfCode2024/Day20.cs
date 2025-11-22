using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day20(string inputPath)
{
    public int Solve(bool part1)
    {
        const int minSavings = 100;
        var lines = File.ReadAllLines(inputPath);

        // Parse grid
        var grid = new Grid<char>(lines[0].Length, lines.Length, lines.SelectMany(c => c), '@');

        // Find start
        var start = grid.AllExtended().Single(x => x.Value == 'S').Position;

        // Compute distances from start using BFS
        var distancesFromStart = ComputeDistances(grid, start);

        // Find all valid cheats
        var maxCheatDistance = part1 ? 2 : 20;
        var cheatCount = CountCheats(grid, distancesFromStart, maxCheatDistance, minSavings);

        Console.WriteLine($"Part 1: {cheatCount} cheats save at least {minSavings} picoseconds");
        return cheatCount;
    }
    
    private static Dictionary<Point, int> ComputeDistances(Grid<char> grid, Point start)
    {
        var distances = new Dictionary<Point, int>();
        var queue = new Queue<(Point position, int distance)>();
        
        queue.Enqueue((start, 0));
        distances[start] = 0;
        
        while (queue.Count > 0)
        {
            var (currentPos, currentDist) = queue.Dequeue();
            
            // Explore all valid neighbors (not walls)
            var neighbours = grid.NeighboursExtended(currentPos, includeDiagonals: false)
                .Where(n => n.Value != '#' && !distances.ContainsKey(n.Position))
                .ToArray();
            
            foreach (var neighbour in neighbours)
            {
                distances[neighbour.Position] = currentDist + 1;
                queue.Enqueue((neighbour.Position, currentDist + 1));
            }
        }
        
        return distances;
    }
    
    private static int CountCheats(Grid<char> grid, Dictionary<Point, int> distancesFromStart, 
        int maxCheatDistance, int minSavings)
    {
        var cheatCount = 0;
        
        // For each position on the track
        foreach (var startPoint in distancesFromStart.Keys)
        {
            // Try all positions within Manhattan distance <= maxCheatDistance
            for (int dx = -maxCheatDistance; dx <= maxCheatDistance; dx++)
            {
                for (int dy = -maxCheatDistance; dy <= maxCheatDistance; dy++)
                {
                    var cheatDistance = Math.Abs(dx) + Math.Abs(dy);
                    
                    // Skip if outside cheat range
                    if (cheatDistance == 0 || cheatDistance > maxCheatDistance)
                        continue;
                    
                    var endPoint = new Point(startPoint.X + dx, startPoint.Y + dy);
                    
                    // Check if endpoint is valid and on track
                    if (!grid.ValidPoint(endPoint) || grid.At(endPoint) == '#')
                        continue;
                    
                    // Check if endpoint is reachable in normal path
                    if (!distancesFromStart.TryGetValue(endPoint, out var value))
                        continue;
                    
                    // Calculate time saved
                    var normalDistance = value - distancesFromStart[startPoint];
                    var timeSaved = normalDistance - cheatDistance;
                    
                    if (timeSaved >= minSavings)
                    {
                        cheatCount++;
                    }
                }
            }
        }
        
        return cheatCount;
    }
}
