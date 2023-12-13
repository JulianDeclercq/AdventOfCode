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
        // vertical
        // search for a double occurrence, the possible mirror line
        var columns = pattern.Columns().Select(column => string.Join("", column.Select(el => el.Value))).ToArray();
        for (var i = 0; i < columns.Length - 1; ++i)
        {
            if (columns[i].Equals(columns[i + 1]))
            {
                // check if it is a mirror
                for (var j = 0;; j++)
                {
                    var leftIdx = i - j;
                    var rightIdx = i + 1 + j;
                    if (leftIdx < 0 || rightIdx == columns.Length) // edge has been reached, mirror successful
                    {
                        return i + 1; // amount of columns to the left of the mirror
                    }

                    if (!columns[leftIdx].Equals(columns[rightIdx])) // not a mirror
                        break;
                }
            }
        }
        
        // horizontal
        var rows = pattern.Rows().Select(row => string.Join("", row.Select(el => el.Value))).ToArray();
        for (var i = 0; i < rows.Length - 1; ++i)
        {
            if (rows[i].Equals(rows[i + 1]))
            {
                // check if it is a mirror
                for (var j = 0;; j++)
                {
                    var leftIdx = i - j;
                    var rightIdx = i + 1 + j;
                    if (leftIdx < 0 || rightIdx == rows.Length) // edge has been reached, mirror successful
                    {
                        return (i + 1) * 100; // amount of rows above the mirror * 100
                    }

                    if (!rows[leftIdx].Equals(rows[rightIdx])) // not a mirror
                        break;
                }
            }
        }
        return 0;
    }
}