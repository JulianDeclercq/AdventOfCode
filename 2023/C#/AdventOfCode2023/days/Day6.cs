using System.Text.RegularExpressions;

namespace AdventOfCode2023.days;

public class Day6
{
    public static void Solve(bool part1)
    {
        var lines = File.ReadAllLines("../../../input/Day6.txt");
        var times = Regex.Replace(lines.First(), @"\s+", " ")["Time: ".Length..].Split(' ').Select(ulong.Parse).ToArray();
        var distances = Regex.Replace(lines.Last(), @"\s+", " ")["Distance: ".Length..].Split(' ').Select(ulong.Parse).ToArray();

        if (!part1)
        {
            times = new []{ulong.Parse(string.Join("", times))};
            distances = new []{ulong.Parse(string.Join("", distances))};
        }

        var amountOfRaces = times.Length;
        var winners = Enumerable.Repeat((ulong)0, amountOfRaces).ToArray();
        for (var i = 0; i < amountOfRaces; ++i) 
        {
            // speed = amount of seconds held (charge time) since 1 second increases the speed by 1
            var speeds = new List<ulong>();
            for (ulong ul = 0; ul < times[i] + 1; ++ul)
                speeds.Add(ul);

            foreach (var speed in speeds)
            {
                // time left for this game is the total time of the game minus the time spent charging
                var timeLeft = times[i] - speed;
                var distance = speed * timeLeft;
                if (distance > distances[i])
                    winners[i]++; // amount of winners for this race
            }
        }

        var answer = part1 ? winners.Aggregate((ulong)1, (x, y) => x * y) : winners.First();
        Console.WriteLine(answer);
    }
}