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
    
    private record PatternContent(int Width, int Height, IReadOnlyList<char> Elements);
    public void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day13.txt");

        var patterns = new List<Grid<char>>();
        var patternContents = new List<PatternContent>();
        var current = new List<string>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                var width = current.First().Length;
                var height = current.Count;
                var elements = current.SelectMany(c => c).ToArray();
                patterns.Add(new Grid<char>(width, height, elements, '?'));
                patternContents.Add(new PatternContent(width, height, elements));
                current.Clear();
                continue;
            }
            current.Add(line);
        }

        var w = current.First().Length;
        var h = current.Count;
        var el = current.SelectMany(c => c).ToArray();
        patterns.Add(new Grid<char>(w, h, el, '?'));
        patternContents.Add(new PatternContent(w, h, el));

        var answer = 0;
        for (var i = 0; i < patterns.Count; ++i)
        {
            var temp = ScorePart2(patterns[i], Variations(patternContents[i]));
            answer += temp;
        }
        Console.WriteLine(answer);

        //Console.WriteLine(patterns.Sum(Score));
    }

    private static IEnumerable<Grid<char>> Variations(PatternContent patternContent)
    {
        var (width, height, original) = patternContent;
        
        var variations = new List<Grid<char>>();
        for (var i = 0; i < original.Count; ++i)
        {
            var copy = original.ToArray();
            copy[i] = original[i] == '.' ? '#' : '.';

            var grid = new Grid<char>(width, height, copy, '?');
            variations.Add(grid);
            //Console.WriteLine(grid);
        }

        return variations;
    }

    private static int Score(Grid<char> pattern)
    {
        var columns = pattern.Columns().Select(column => string.Join("", column.Select(el => el.Value))).ToArray();
        var verticalMirrors = TryFindMirrors(columns, Orientation.Vertical);
        if (verticalMirrors.Any())
            return verticalMirrors.Single();
        
        var rows = pattern.Rows().Select(row => string.Join("", row.Select(el => el.Value))).ToArray();
        var horizontalMirrors = TryFindMirrors(rows, Orientation.Horizontal);
        if (horizontalMirrors.Any())
            return horizontalMirrors.Single();

        return 0;
    }

    private static int ScorePart2(Grid<char> original, IEnumerable<Grid<char>> variations)
    {
        var originalScore = Score(original);
        foreach (var variation in variations)
        {
            var columns = variation.Columns().Select(column => string.Join("", column.Select(el => el.Value))).ToArray();
            var verticalMirrors = TryFindMirrors(columns, Orientation.Vertical);
            var newMirrorScoreIdx = verticalMirrors.FindIndex(vm => vm != originalScore);
            if (newMirrorScoreIdx != -1)
                return verticalMirrors[newMirrorScoreIdx];
        
            var rows = variation.Rows().Select(row => string.Join("", row.Select(el => el.Value))).ToArray();
            var horizontalMirrors = TryFindMirrors(rows, Orientation.Horizontal);
            newMirrorScoreIdx = horizontalMirrors.FindIndex(hm => hm != originalScore);
            if (newMirrorScoreIdx != -1)
                return horizontalMirrors[newMirrorScoreIdx];
        }

        return 0;
    }

    private static List<int> TryFindMirrors(IReadOnlyList<string> rowsOrColumns, Orientation orientation)
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