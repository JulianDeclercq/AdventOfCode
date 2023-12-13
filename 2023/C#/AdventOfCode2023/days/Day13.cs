using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day13
{
    public void Solve()
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

        Console.WriteLine(patterns.Sum(Score));
    }

    private static int Score(Grid<char> pattern)
    {
        var columns = pattern.Columns().Select(column => string.Join("", column.Select(el => el.Value))).ToArray();
        if (TryFindMirror(columns, out var vertical))
            return vertical;
        
        var rows = pattern.Rows().Select(row => string.Join("", row.Select(el => el.Value))).ToArray();
        if (TryFindMirror(rows, out var horizontal))
            return horizontal * 100;

        return 0;
    }

    private static bool TryFindMirror(IReadOnlyList<string> rowsOrColumns, out int score)
    {
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
                        score = i + 1; // amount of rows / columns before the mirror
                        return true;
                    }

                    if (!rowsOrColumns[leftIdx].Equals(rowsOrColumns[rightIdx])) // not a mirror
                        break;
                }
            }
        }
        score = 0;
        return false;
    }
}