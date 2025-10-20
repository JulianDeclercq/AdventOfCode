using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day18(string inputPath)
{

    public int Solve()
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

        // Use simple BFS since all moves cost 1
        var queue = new Queue<(Point position, int steps)>();
        var visited = new HashSet<Point>();

        var start = new Point(0, 0);
        var end = new Point(width - 1, height - 1);

        queue.Enqueue((start, 0));
        visited.Add(start);

        int? shortestPath = null;

        while (queue.Count > 0)
        {
            var (currentPos, currentSteps) = queue.Dequeue();

            // If we've reached the end, this is our answer (BFS guarantees optimal for unweighted graphs)
            if (currentPos.Equals(end))
            {
                shortestPath = currentSteps;
                break;
            }

            // Explore all valid neighbors
            var neighbours = grid.NeighboursExtended(currentPos, includeDiagonals: false)
                .Where(n => n.Value != '#' && !visited.Contains(n.Position)) // Not a wall and not visited
                .ToArray();

            foreach (var neighbour in neighbours)
            {
                visited.Add(neighbour.Position);
                queue.Enqueue((neighbour.Position, currentSteps + 1));
            }
        }

        // Print and return the result
        if (shortestPath.HasValue)
        {
            Console.WriteLine($"Shortest path found! Steps: {shortestPath.Value}");
            return shortestPath.Value;
        }
        else
        {
            Console.WriteLine("No path found!");
            return -1; // Return -1 to indicate no path found
        }
    }
}
