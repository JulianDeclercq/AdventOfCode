using System.Globalization;

namespace AdventOfCode2018.days;

public class Day4
{
    public void Part1()
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
        var lookup = new Dictionary<(int id, int minutes), bool>(); //(guardId, minutes), asleep
        var kek = new Dictionary<int, List<int>>(); //id, list of minutes where he was asleep
        foreach (var (stamp, info) in orderedEntries)
        {
            var split = info.Split(' ');
            switch (split[0][0])
            {
                case 'G':
                {
                    guardId = int.Parse(split[1][1..]);
                }
                    break;
                case 'f': // falls asleep
                {
                    asleepStamp = stamp;
                }
                    break;
                case 'w': // wakes up
                {
                    var minutesAsleep = (stamp - asleepStamp).Minutes;
                    for (var i = 0; i < minutesAsleep; ++i)
                    {
                        lookup[(guardId, asleepStamp.Minute + i)] = true;

                        if (!kek.ContainsKey(guardId)) kek[guardId] = new List<int>() {asleepStamp.Minute + i};
                        else kek[guardId].Add(asleepStamp.Minute + i);
                    }
                }
                    break;
                default: throw new Exception("invalid character");
            }
        }
        
        var longestAsleep = kek.MaxBy(x => x.Value.Count);
        Console.WriteLine($"Guard {longestAsleep.Key} was asleep the longest, {longestAsleep.Value.Count} minutes");
        var counter = new Dictionary<int, int>(); // minute, amount of times the minute occurs
        foreach (var minute in longestAsleep.Value)
        {
            counter.TryGetValue(minute, out var count);
            counter[minute] = count + 1;
        }

        var minuteMostSlept = counter.MaxBy(x => x.Value);
        Console.WriteLine(minuteMostSlept.Key);

        Console.WriteLine($"Day 4 part 1: {longestAsleep.Key * minuteMostSlept.Key}");
    }
    
    public void Part2()
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
        var lookup = new Dictionary<(int id, int minutes), bool>(); //(guardId, minutes), asleep
        var kek = new Dictionary<int, List<int>>(); //id, list of minutes where he was asleep
        foreach (var (stamp, info) in orderedEntries)
        {
            var split = info.Split(' ');
            switch (split[0][0])
            {
                case 'G':
                {
                    guardId = int.Parse(split[1][1..]);
                }
                    break;
                case 'f': // falls asleep
                {
                    asleepStamp = stamp;
                }
                    break;
                case 'w': // wakes up
                {
                    var minutesAsleep = (stamp - asleepStamp).Minutes;
                    for (var i = 0; i < minutesAsleep; ++i)
                    {
                        lookup[(guardId, asleepStamp.Minute + i)] = true;

                        if (!kek.ContainsKey(guardId)) kek[guardId] = new List<int>() {asleepStamp.Minute + i};
                        else kek[guardId].Add(asleepStamp.Minute + i);
                    }
                }
                    break;
                default: throw new Exception("invalid character");
            }
        }

        var answer = (id: int.MinValue, minute: int.MinValue, minuteFrequency: int.MinValue);
        foreach (var el in kek)
        {
            var ctr = new Dictionary<int, int>(); // minute, amount of times the minute occurs
            foreach (var minute in el.Value)
            {
                ctr.TryGetValue(minute, out var count);
                ctr[minute] = count + 1;
            }

            var minuteMostSlept = ctr.MaxBy(x => x.Value);
            if (minuteMostSlept.Value > answer.minuteFrequency)
                answer = (el.Key, minuteMostSlept.Key, minuteMostSlept.Value);
        }
        
        // 37110 is too low
        Console.WriteLine($"Day 4 part 2: {answer.id * answer.minute}");
    }
}