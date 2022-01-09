using System.Globalization;

namespace AdventOfCode2018.days;

public class Day4
{
    public void Part1() => Console.WriteLine($"Day 4 part 1: {Solve()}");
    public void Part2() => Console.WriteLine($"Day 4 part 2: {Solve(part1: false)}");
    
    private static int Solve(bool part1 = true)
    {
        const string dateformat = "yyyy-MM-dd HH:mm";

        var lines = File.ReadAllLines(@"..\..\..\input\day4.txt");
        var entries = new Dictionary<DateTime, string>();
        foreach (var line in lines)
        {
            var split = line.Split(' ');
            var stamp = $"{split[0]} {split[1]}"[1..^1];
            var dateTime = DateTime.ParseExact(stamp, dateformat, CultureInfo.InvariantCulture);
            entries.Add(dateTime, $"{split[2]} {split[3]}");
        }
        
        var guardId = int.MinValue;
        var asleepStamp = DateTime.MinValue;
        var orderedEntries = entries.OrderBy(x => x.Key);
        var minutesAsleepById = new Dictionary<int, List<int>>(); //id, list of minutes where he was asleep
        foreach (var (stamp, info) in orderedEntries)
        {
            var split = info.Split(' ');
            switch (split[0][0])
            {
                case 'G':
                    guardId = int.Parse(split[1][1..]);
                    break;
                case 'f': // falls asleep
                    asleepStamp = stamp;
                    break;
                case 'w': // wakes up
                {
                    var minutesAsleep = (stamp - asleepStamp).Minutes;
                    for (var i = 0; i < minutesAsleep; ++i)
                    {
                        if (!minutesAsleepById.ContainsKey(guardId))
                        {
                            minutesAsleepById[guardId] = new List<int>() {asleepStamp.Minute + i};
                        }
                        else minutesAsleepById[guardId].Add(asleepStamp.Minute + i);
                    }
                }
                    break;
                default: throw new Exception("invalid character");
            }
        }
        
        if (part1)
        {
            // find the guard with the most minutes slept
            var (mmsId, mmsMinutes) = minutesAsleepById.MaxBy(guard => guard.Value.Count);
            
            // find the minute that guard spent asleep the most
            var chosenMinute = Helpers.CountOccurrences(mmsMinutes).MaxBy(x => x.Value).Key;
            return mmsId * chosenMinute;
        }

        
        // part 2
        var answer = (id: int.MinValue, minute: int.MinValue, frequency: int.MinValue);
        foreach (var (id, minutesAsleep) in minutesAsleepById)
        {
            var (minute, frequency) = Helpers.CountOccurrences(minutesAsleep).MaxBy(x => x.Value);

            if (frequency > answer.frequency)
                answer = (id, minute, frequency);
        }
        return answer.id * answer.minute;
    }
}