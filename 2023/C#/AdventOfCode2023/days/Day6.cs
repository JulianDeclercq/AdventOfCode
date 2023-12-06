using System.Text.RegularExpressions;

namespace AdventOfCode2023.days;

public class Day6
{
    public static void Solve(bool part1)
    {
        var lines = File.ReadAllLines("../../../input/Day6.txt");
        var times = Regex.Replace(lines.First(), @"\s+", " ")["Time: ".Length..].Split(' ').Select(ulong.Parse).ToArray();
        var distances = Regex.Replace(lines.Last(), @"\s+", " ")["Distance: ".Length..].Split(' ').Select(ulong.Parse)
            .ToArray();

        if (!part1)
        {
            times = new []{ulong.Parse(string.Join("", times))};
            distances = new []{ulong.Parse(string.Join("", distances))};
        }

        var wins = 0;
        var winners = Enumerable.Repeat((ulong)0, times.Length).ToArray();
        for (var i = 0; i < times.Length; ++i)
        {
            // populate, speed = amount of seconds held since 1 second increases the speed by 1
            var speeds = new List<ulong>(); // 
            for (ulong ul = 0; ul < times[i] + 1; ++ul)
                speeds.Add(ul);

            var timeLeft = speeds.ToArray().Reverse().ToArray();
            for (var j = 0; j < speeds.Count; ++j)
            {
                var distance = speeds[j] * timeLeft[j];
                if (distance > distances[i])
                {
                    if (part1)
                    {
                        winners[i]++; // amount of winners
                    }
                    else
                    {
                        wins++;
                    }
                }
            }
        }

        var answer = part1 ? winners.Aggregate((ulong)1, (x, y) => x * y) : (ulong)wins;
        Console.WriteLine(answer);
    }
}