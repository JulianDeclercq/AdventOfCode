using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day18(string inputPath)
{
    private Grid<char> ParseInput(int? bytesToTakeOverride = null)
    {
        int width, height, bytesToTake;
        if (inputPath.Contains("example"))
        {
            width = 7;
            height = 7;
            bytesToTake = bytesToTakeOverride ?? 12;
        }
        else // real input
        {
            width = 71;
            height = 71;
            bytesToTake = bytesToTakeOverride ?? 1024;
        }

        var input = File.ReadAllLines(inputPath);
        var grid = new Grid<char>(width, height, Enumerable.Repeat('.', width * height), '@');
        foreach (var line in input.Take(bytesToTake))
        {
            var split = line.Split(',').Select(int.Parse).ToArray();
            grid.Set(split[0], split[1], '#');
        }

        return grid;
    }

    public int Part1()
    {
        var grid = ParseInput();
        return Solve(grid);
    }

    public string Part2()
    {
        // optimization: we know that part1 always returns a legit path, so start from there to find a solution without a path
        var startAt = Part1() - 1;
        for (var i = startAt;; ++i)
        {
            var grid = ParseInput(i);
            if (Solve(grid) == -1)
            {
                var index = i - 1;
                var corrupted = File.ReadAllLines(inputPath);
                var blockerCoordinate = corrupted[index].Split(',').Select(int.Parse).ToArray();
                return $"{blockerCoordinate[0]},{blockerCoordinate[1]}";
            }
        }
    }

    private static int Solve(Grid<char> grid)
    {
        // Use simple BFS since all moves cost 1
        var queue = new Queue<(Point position, int steps)>();
        var visited = new HashSet<Point>();

        var start = new Point(0, 0);
        var end = new Point(grid.Width - 1, grid.Height - 1);

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
            return shortestPath.Value;

        return -1; // Return -1 to indicate no path found
    }
}
