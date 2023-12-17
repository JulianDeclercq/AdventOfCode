using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day17
{
    public static void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day17e.txt");
        var grid = new Grid<int>(input.First().Length, input.Length, input.SelectMany(x => x).Select(c => int.Parse($"{c}")), '?');
        Console.WriteLine(grid);
        
        var first = new Point(0, 0);
        var last = new Point(grid.Width - 1, grid.Height - 1);

        var frontier = new PriorityQueue<Point, int>();
        frontier.Enqueue(first, 0);
        var cameFrom = new Dictionary<Point, Point>();
        var costSoFar = new Dictionary<Point, int> {{first, 0}};

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
                
            if (current.Equals(last))
                break;
                        
            foreach (var next in grid.NeighbouringPoints(current, includeDiagonals: false))
            {
                var newCost = costSoFar[current] + grid.At(current);
                    
                // if the new cost is higher than the cheapest cost so far, ignore this path
                if (costSoFar.TryGetValue(next, out var value) && newCost >= value)
                    continue;

                costSoFar[next] = newCost;
                frontier.Enqueue(next, newCost);
                cameFrom[next] = current;
            }
        }

        // reconstruct the path
        var path = new List<Point>();
        var cur = last;
        while (!cur.Equals(first))
        {
            path.Add(cur);
            cur = cameFrom[cur];
        }
        var pathCost = path.Select(grid.At).Sum();

        var vis = grid.ShallowCopy();
        foreach (var p in path)
            vis.Set(p, 0);
        Console.WriteLine(vis);
    }
}