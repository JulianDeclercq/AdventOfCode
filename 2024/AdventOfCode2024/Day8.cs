﻿using AdventOfCode2024.helpers;

namespace AdventOfCode2024;

public class Day8
{
    public static void Solve()
    {
        const char invalid = '@', antiNode = '#';
        var lines = File.ReadAllLines("input/day8.txt");
        var characters = lines.SelectMany(c => c).ToArray();
        var antennas = new Grid<char>(lines[0].Length, lines.Length, characters, invalid);
        var antiNodes = new Grid<char>(antennas.Width, antennas.Height, 
            Enumerable.Repeat('.', antennas.Width * antennas.Height), invalid);
        
        var frequencies = characters.Where(c => c != '.').ToHashSet();

        // build anti-nodes
        foreach (var frequency in frequencies)
        {
            var positions = antennas
                .AllExtended()
                .Where(cell => cell.Value == frequency)
                .ToList();

            for (var i = 0; i < positions.Count; ++i)
            {
                for (var j = 0; j < positions.Count; ++j)
                {
                    if (i == j)
                        continue;

                    GridElement<char> lhs = positions[i], rhs = positions[j];
                    if (lhs.Value == rhs.Value)
                    {
                        // create an anti-node
                        
                        // by definition, all antennas are now also anti-nodes since they are on their own line
                        antiNodes.Set(lhs.Position, antiNode);

                        var diff = new Point(lhs.Position.X - rhs.Position.X, lhs.Position.Y - rhs.Position.Y);
                        var current = lhs.Position;
                        while (antennas.ValidPoint(current + diff))
                        {
                            antiNodes.Set(current + diff, antiNode);
                            current += diff;
                        }
                    }
                }
            }
        }
        
        // Console.WriteLine(antiNodes);
        Console.WriteLine(antiNodes.All().Count(c => c == antiNode));
    }
}