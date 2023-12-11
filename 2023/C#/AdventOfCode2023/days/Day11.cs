using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day11
{
    private static HashSet<int> _emptyRows, _emptyCols;
    private static ulong _expansionSize = 1_000_000;
    public void Part1()
    {
        var lines = File.ReadAllLines("../../../input/Day11.txt").ToList();
        var tempGrid = new Grid<char>(lines.First().Length, lines.Count, lines.SelectMany(x => x), '?');

        var rows = tempGrid.Rows().ToArray();
        var columns = tempGrid.Columns().ToArray();
        _emptyRows = rows.Select((row, idx) => (row, idx)).Where(r => r.row.All(c => c.Value.Equals('.'))).Select(x => x.idx).ToHashSet();
        _emptyCols = columns.Select((col, idx) => (col, idx)).Where(x => x.col.All(c => c.Value.Equals('.'))).Select(x => x.idx).ToHashSet();

        // insert empty columns
        // for (var i = 0; i < lines.Count; ++i)
        // {
        //     // offset with the amount that is added by inserting the previous ones in the list (e.g. inserting at the first element will cause the next index to be +1 to account for this insert)
        //     var offsetCols = emptyCols.Select((ec, idx) => ec + idx);
        //     foreach (var offsetIdx in offsetCols)
        //         lines[i] = lines[i].Insert(offsetIdx, ".");
        // }
        //
        // // insert empty rows
        // var emptyLine = string.Join("", Enumerable.Range(0, lines.First().Length).Select(_ => '.'));
        //
        // var offsetRowIdx = emptyRows.Select((er, idx) => er + idx); // same offset reason as before
        // foreach (var offsetIdx in offsetRowIdx)
        //     lines.Insert(offsetIdx, emptyLine);

        var grid = new Grid<char>(lines.First().Length, lines.Count, lines.SelectMany(x => x), '?');
        var galaxies = grid.AllExtended().Where(x => x.Value.Equals('#')).Select(x => x.Position).ToList();

        var registerOpposite = new HashSet<Path>();
        var paths = new List<Path>();
        foreach (var galaxy in galaxies)
        {
            foreach (var otherGalaxy in galaxies)
            {
                // don't add pair with yourself
                if (galaxy.Equals(otherGalaxy))
                    continue;

                var candidate = new Path(from: galaxy, to: otherGalaxy);
                
                // don't add pairs in the other direction
                if (registerOpposite.Contains(candidate))
                    continue;
                
                paths.Add(candidate);
                registerOpposite.Add(new Path(from: candidate.To, to: candidate.From));
            }
        }
        
        Console.WriteLine("STARTING PATHFINDING");

        ulong answer = 0;
        for (var i = 0; i < paths.Count; ++i)
        {
            if (i % 100 == 0)
                Qonsole.OverWrite($"{answer} ({((float)i / paths.Count) * 100} %)");

            var p = paths[i];
            answer += (ulong)ShortestPathLength(p.From, p.To, grid);
        }
        Console.WriteLine();
        Console.WriteLine(answer);
        //Console.WriteLine(paths.Sum(p => ShortestPathLength(p.From, p.To, grid)));
    }

    private static bool IsExpanded(Point p)
    {
        return _emptyRows.Contains(p.Y) || _emptyCols.Contains(p.X);
    }

    private class Path : IEquatable<Path>
    {
        public Path(Point from, Point to)
        {
            From = from;
            To = to;
        }
        
        public readonly Point From;
        public readonly Point To;

        public bool Equals(Path? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Path)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To);
        }
    }
    
    private static ulong ShortestPathLength(Point from, Point to, Grid<char> grid)
    {
        var frontier = new PriorityQueue<Point, ulong>();
        frontier.Enqueue(from, 0);
        var cameFrom = new Dictionary<Point, Point>();
        var costSoFar = new Dictionary<Point, ulong> {{from, 0}};

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            if (current.Equals(to))
                break;

            var neighbours = grid.NeighbouringPoints(current, includeDiagonals: false);
            foreach (var next in neighbours)
            {
                var newCost = costSoFar[current] + (IsExpanded(next) ? _expansionSize : 1);
                
                // if the new cost is higher than the cheapest cost so far, ignore this path
                if (costSoFar.TryGetValue(next, out var value) && newCost >= value)
                    continue;

                costSoFar[next] = newCost;
                frontier.Enqueue(next, newCost);
                cameFrom[next] = current;
            }
        }
        
        var cur = to;
        ulong answer = 0;
        while (!cur.Equals(from))
        {
            cur = cameFrom[cur];
            answer += (IsExpanded(cur) ? _expansionSize : 1);
        }
        return answer;

        // reconstruct the path (From is included instead of To, but this doesn't matter for just count)
        // var reconstructed = new List<Point>();
        // var cur = to;
        // while (!cur.Equals(from))
        // {
        //     reconstructed.Add(cur);
        //     cur = cameFrom[cur];
        // }
        // return reconstructed.Count;

        // debug printing
        // var vis = grid.ShallowCopy();
        // reconstructed.Reverse();
        //
        // for (var i = 0; i < reconstructed.Count; ++i)
        // {
        //     vis.Set(reconstructed[i], $"@".First());
        //     //vis.Set(reconstructed[i], $"{i % 10}".First());
        // }
        //
        // Console.WriteLine(vis);
    }
}