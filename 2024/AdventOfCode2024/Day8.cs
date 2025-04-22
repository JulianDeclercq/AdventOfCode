using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day8
{
    public static void Solve(int part = 1)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        const char invalid = '@', antiNode = '#';
        var lines = File.ReadAllLines("input/real/day8.txt");
        var characters = lines.SelectMany(c => c).ToArray();
        var antennas = new Grid<char>(lines[0].Length, lines.Length, characters, invalid);
        var antiNodes = new Grid<char>(antennas.Width, antennas.Height, 
            Enumerable.Repeat('.', antennas.Width * antennas.Height), invalid);
        
        var frequencies = characters.Where(c => c != '.').ToHashSet();

        // build anti-nodes
        foreach (var frequency in frequencies)
        {
            var frequencyCells = antennas
                .AllExtended()
                .Where(cell => cell.Value == frequency)
                .ToList();

            for (var i = 0; i < frequencyCells.Count; ++i)
            {
                for (var j = 0; j < frequencyCells.Count; ++j)
                {
                    if (i == j)
                        continue;

                    GridElement<char> lhs = frequencyCells[i], rhs = frequencyCells[j];
                    if (lhs.Value == rhs.Value)
                    {
                        var diff = new Point(lhs.Position.X - rhs.Position.X, lhs.Position.Y - rhs.Position.Y);
                        
                        if (part is 1)
                        {
                            antiNodes.Set(lhs.Position + diff, antiNode);
                            continue;
                        }

                        // part 2
                        var current = lhs.Position + diff;
                        while (antennas.ValidPoint(current))
                        {
                            antiNodes.Set(current, antiNode);
                            current += diff;
                        }
                        
                        // by definition, all antennas are now also anti-nodes since they are on their own line
                        antiNodes.Set(lhs.Position, antiNode);
                    }
                }
            }
        }
        
        Console.WriteLine(antiNodes.All().Count(c => c == antiNode));
    }
}