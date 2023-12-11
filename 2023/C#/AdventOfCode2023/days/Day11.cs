using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day11
{
    private static int[] _emptyRows, _emptyCols;
    private static ulong _expansionSize = 1_000_000;
    public static void Solve(bool part1)
    {
        _expansionSize = part1 ? (ulong)2 : (ulong)1_000_000;
        
        var lines = File.ReadAllLines("../../../input/Day11.txt").ToList();
        var tempGrid = new Grid<char>(lines.First().Length, lines.Count, lines.SelectMany(x => x), '?');

        _emptyRows = tempGrid.Rows().Select((row, idx) => (row, idx)).Where(r => r.row.All(c => c.Value.Equals('.'))).Select(x => x.idx).ToArray();
        _emptyCols = tempGrid.Columns().Select((col, idx) => (col, idx)).Where(x => x.col.All(c => c.Value.Equals('.'))).Select(x => x.idx).ToArray();

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
        
        var answer = paths.Aggregate<Path, ulong>(0, (current, p) => current + Steps(p.From, p.To)); // sum for ulong
        Console.WriteLine(answer);
    }

    private static ulong Steps(Point from, Point to)
    {
        ulong answer = 0;
        
        // vertical galaxy borders passed
        var west = Math.Min(from.X, to.X);
        var east = Math.Max(from.X, to.X);
        foreach (var galaxyBorder in _emptyCols)
        {
            if (west < galaxyBorder && galaxyBorder < east)
                answer += _expansionSize - 1; // -1 because the non expanded one is counted at the end of the method
        }
        
        // horizontal galaxy borders passed
        var north = Math.Min(from.Y, to.Y);
        var south = Math.Max(from.Y, to.Y);
        foreach (var galaxyBorder in _emptyRows)
        {
            if (north < galaxyBorder && galaxyBorder < south)
                answer += _expansionSize - 1; // -1 because the non expanded one is counted at the end of the method
        }

        answer += (ulong)Math.Abs(from.X - to.X);
        answer += (ulong)Math.Abs(from.Y - to.Y);
        
        return answer;
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
}