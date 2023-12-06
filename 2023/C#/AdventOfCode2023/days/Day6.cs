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
    
    public void Part2()
    {
        //var lines = File.ReadAllLines("../../../input/Day6_example.txt");
        var lines = File.ReadAllLines("../../../input/Day6.txt");
        var times = Regex.Replace(lines.First(), @"\s+", " ")["Time: ".Length..].Split(' ').Select(ulong.Parse).ToArray();
        var distances = Regex.Replace(lines.Last(), @"\s+", " ")["Distance: ".Length..].Split(' ').Select(ulong.Parse)
            .ToArray();

        times = new []{ulong.Parse(string.Join("", times))};
        distances = new []{ulong.Parse(string.Join("", distances))};

        var winners = new List<ulong>();
        for (var i = 0; i < times.Length; ++i)
        {
            // populate speeds
            //var speeds = Enumerable.Range(0, (int)times[i] + 1).ToArray(); // speeds = amount of seconds held
            var speeds = new List<ulong>(); // speeds = amount of seconds held
            for (ulong ul = 0; ul < times[i] + 1; ++ul)
                speeds.Add(ul);

            var timeLeft = speeds.ToArray().Reverse().ToArray();
            for (var j = 0; j < speeds.Count; ++j)
            {
                var distance = (ulong)(speeds[j] * timeLeft[j]);
                if (distance > distances[i])
                {
                    //winners[i]++; // amount of winners
                    winners.Add((ulong)speeds[j]);
                }
            }
        }
        Console.WriteLine(winners.Count);
    }
}