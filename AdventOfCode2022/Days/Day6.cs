namespace AdventOfCode2022.Days;

public class Day6
{
    public static void Solve(bool part1 = true)
    {
        var input = File.ReadAllLines(@"..\..\..\input\day6.txt").Single();

        var marker = new List<char>();
        var markerSize = part1 ? 4 : 14;
        for (var i = 0; i < input.Length; ++i)
        {
            var idx = marker.FindIndex(x => x == input[i]);
            
            // reset the marker contents to after the first part of the newfound duplicate
            if (idx != -1)
                marker = marker.Skip(idx + 1).ToList();
            
            marker.Add(input[i]);

            if (marker.Count != markerSize)
                continue;
            
            Console.WriteLine(i + 1);
            break;
        }
    }
}