using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day11
{
    public void Part1()
    {
        var lines = File.ReadAllLines("../../../input/Day11e.txt").ToList();
        var tempGrid = new Grid<char>(lines.First().Length, lines.Count, lines.SelectMany(x => x), '?');

        var rows = tempGrid.Rows().ToArray();
        var columns = tempGrid.Columns().ToArray();
        var emptyRows = rows.Select((row, idx) => (row, idx)).Where(r => r.row.All(c => c.Value.Equals('.'))).Select(x => x.idx).ToArray();
        var emptyCols = columns.Select((col, idx) => (col, idx)).Where(x => x.col.All(c => c.Value.Equals('.'))).Select(x => x.idx).ToArray();

        // insert empty columns
        {
            for (var i = 0; i < lines.Count; ++i)
            {
                // offset with the amount that is added by inserting the previous ones in the list (e.g. inserting at the first element will cause the next index to be +1 to account for this insert)
                var offsetCols = emptyCols.Select((ec, idx) => ec + idx);
                foreach(var offsetIdx in offsetCols)
                    lines[i] = lines[i].Insert(offsetIdx, ".");
            }
        }

        // insert empty rows
        var emptyLine = string.Join("", Enumerable.Range(0, lines.First().Length).Select(_ => '.'));
        foreach (var idx in emptyRows) lines.Insert(idx, emptyLine);

        var grid = new Grid<char>(lines.First().Length, lines.Count, lines.SelectMany(x => x), '?');
        Console.WriteLine(grid);
    }
}