using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day13
{
    private enum Orientation
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2
    }
    
    public static void Solve(bool part1)
    {
        var input = File.ReadAllLines("../../../input/Day13.txt");

        var patterns = new List<Grid<char>>();
        var current = new List<string>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                patterns.Add(new Grid<char>(current.First().Length, current.Count, current.SelectMany(c => c), '?'));
                current.Clear();
                continue;
            }
            current.Add(line);
        }
        patterns.Add(new Grid<char>(current.First().Length, current.Count, current.SelectMany(c => c), '?'));

        var answer = part1 ? patterns.Sum(Score) : patterns.Sum(p => ScorePart2(p, Variations(p))); 
        Console.WriteLine(answer);
    }

    private static IEnumerable<Grid<char>> Variations(Grid<char> original)
    {
        var variations = new List<Grid<char>>();
        foreach (var element in original.AllExtended())
        {
            var copy = original.ShallowCopy();
            copy.Set(element.Position, element.Value == '.' ? '#' : '.');
            variations.Add(copy);
        }

        return variations;
    }
   
    private static int Score(Grid<char> pattern)
    {
        var columns = pattern.Columns().Select(column => string.Join("", column.Select(el => el.Value))).ToArray();
        var vertical = MirrorScores(columns, Orientation.Vertical);
        if (vertical.Any())
            return vertical.Single();
        
        var rows = pattern.Rows().Select(row => string.Join("", row.Select(el => el.Value))).ToArray();
        var horizontal = MirrorScores(rows, Orientation.Horizontal);
        if (horizontal.Any())
            return horizontal.Single();

        return 0;
    }

    private static int ScorePart2(Grid<char> original, IEnumerable<Grid<char>> variations)
    {
        var originalScore = Score(original);
        foreach (var variation in variations)
        {
            var columns = variation.Columns().Select(column => string.Join("", column.Select(el => el.Value))).ToArray();
            if (TryGetNewMirrorScore(columns, Orientation.Vertical, originalScore, out var verticalScore))
                return verticalScore;

            var rows = variation.Rows().Select(row => string.Join("", row.Select(el => el.Value))).ToArray();
            if (TryGetNewMirrorScore(rows, Orientation.Horizontal, originalScore, out var horizontalScore))
                return horizontalScore;
        }

        return 0;
    }

    private static bool TryGetNewMirrorScore(
        IReadOnlyList<string> rowsOrColumns, Orientation orientation, int originalScore, out int newScore)
    {
        var mirrors = MirrorScores(rowsOrColumns, orientation);
        var idx = mirrors.FindIndex(vm => vm != originalScore);
        if (idx != -1)
        {
            newScore = mirrors[idx];
            return true;
        }

        newScore = 0;
        return false;
    }

    private static List<int> MirrorScores(IReadOnlyList<string> rowsOrColumns, Orientation orientation)
    {
        var mirrorScores = new List<int>();
        for (var i = 0; i < rowsOrColumns.Count - 1; ++i)
        {
            // search for a double occurrence, the possible mirror line
            if (rowsOrColumns[i].Equals(rowsOrColumns[i + 1]))
            {
                // check if it is a mirror
                for (var j = 0;; j++)
                {
                    var leftIdx = i - j;
                    var rightIdx = i + 1 + j;
                    if (leftIdx < 0 || rightIdx == rowsOrColumns.Count) // edge has been reached, mirror successful
                    {
                        // found a mirror, add amount of rows / columns before the mirror
                        var score = i + 1;
                        if (orientation is Orientation.Horizontal)
                            score *= 100;
                        
                        mirrorScores.Add(score);
                        break;
                    }

                    if (!rowsOrColumns[leftIdx].Equals(rowsOrColumns[rightIdx])) // not a mirror
                        break;
                }
            }
        }
        return mirrorScores;
    }
}