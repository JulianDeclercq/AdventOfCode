using AdventOfCode2024.helpers;
using Path = AdventOfCode2024.helpers.Path;

namespace AdventOfCode2024;

public class Day18(string inputPath)
{
    private record State(Point Position, Direction Direction);

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

        var priorityQueue = new PriorityQueue<Path, int>();
        var visited = new Dictionary<Point, int>(); // Only track position since turning is free

        var start = new Point(0, 0);
        var end = new Point(width - 1, height - 1);

        var initialPath = new Path
        {
            CurrentPosition = start,
            CurrentDirection = Direction.East, // Direction doesn't matter for scoring
            Score = 0, // We don't really need this, but keeping for compatibility
            Visited = [start]
        };

        priorityQueue.Enqueue(initialPath, 0);
        Path? bestPath = null;

        while (priorityQueue.Count > 0)
        {
            var currentPath = priorityQueue.Dequeue();

            // If we've reached the end, this is our answer (Dijkstra guarantees optimal)
            if (currentPath.CurrentPosition.Equals(end))
            {
                bestPath = currentPath;
                break;
            }

            var currentSteps = currentPath.Visited.Count - 1; // -1 because start doesn't count as a step

            // Skip if we've already found a better path to this position
            if (visited.TryGetValue(currentPath.CurrentPosition, out var prevSteps) && prevSteps < currentSteps)
                continue;

            visited[currentPath.CurrentPosition] = currentSteps;

            // Explore all valid neighbors
            var neighbours = grid.NeighboursExtended(currentPath.CurrentPosition, includeDiagonals: false)
                .Where(n => n.Value != '#') // Not a wall
                .ToArray();

            foreach (var neighbour in neighbours)
            {
                var newSteps = currentSteps + 1;

                // Only add to queue if we haven't visited this position or found a better path
                if (!visited.TryGetValue(neighbour.Position, out var existingSteps) || newSteps < existingSteps)
                {
                    var newPath = currentPath.Copy();
                    newPath.CurrentPosition = Point.Copy(neighbour.Position);
                    newPath.Visited.Add(neighbour.Position);

                    // Direction doesn't matter for scoring, but we can set it for completeness
                    var neighbourDirection =
                        Helpers.CalculateCardinalDirection(currentPath.CurrentPosition, neighbour.Position);
                    if (neighbourDirection != null)
                        newPath.CurrentDirection = neighbourDirection.Value;

                    priorityQueue.Enqueue(newPath, newSteps);
                }
            }
        }

        // Print the result
        if (bestPath != null)
        {
            var steps = bestPath.Visited.Count - 1; // -1 because start doesn't count as a step
            Console.WriteLine($"Shortest path found! Steps: {steps}");
        }
        else
        {
            Console.WriteLine("No path found!");
        }
        //
        // Console.WriteLine(grid);
    }
}
