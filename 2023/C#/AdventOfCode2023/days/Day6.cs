using System.Text.RegularExpressions;

namespace AdventOfCode2023.days;

public class Day6
{
    public void Part1()
    {
        //var lines = File.ReadAllLines("../../../input/Day6_example.txt");
        var lines = File.ReadAllLines("../../../input/Day6.txt");
        var times = Regex.Replace(lines.First(), @"\s+", " ")["Time: ".Length..].Split(' ').Select(int.Parse).ToArray();
        var distances = Regex.Replace(lines.Last(), @"\s+", " ")["Distance: ".Length..].Split(' ').Select(int.Parse)
            .ToArray();

        var winners = Enumerable.Repeat(0, times.Length).ToArray();
        for (var i = 0; i < times.Length; ++i)
        {
            var speeds = Enumerable.Range(0, times[i] + 1).ToArray(); // speeds = amount of seconds held
            var timeLeft = speeds.Reverse().ToArray();
            for (var j = 0; j < speeds.Length; ++j)
            {
                var distance = speeds[j] * timeLeft[j];
                if (distance > distances[i])
                {
                    winners[i]++; // amount of winners
                    //winners.Add(speeds[j]);
                }
            }
        }
        Console.WriteLine(winners.Aggregate(1, (x, y) => x * y));
    }
}