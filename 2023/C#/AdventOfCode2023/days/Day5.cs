using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day5
{
    private class Range
    {
        public Range(long start, long length)
        {
            _min = start;
            _max = start + length - 1;
        }

        public bool Contains(long value) => _min <= value && value <= _max;

        private readonly long _min;
        private readonly long _max;
    }
    
    private class Map
    {
        public Map(long destinationStart, long sourceStart, long length)
        {
            _destinationStart = destinationStart;
            _sourceStart = sourceStart;
            _length = length;    
        }
        
        private readonly long _destinationStart;
        private readonly long _sourceStart;
        private readonly long _length;

        public long MapToDestination(long sourceNr)
        {
            if (sourceNr < _sourceStart || sourceNr >= _sourceStart + _length)
                return sourceNr;

            return _destinationStart + (sourceNr - _sourceStart);
        }

        public long MapToSource(long destinationNr)
        {
            if (destinationNr < _destinationStart || destinationNr >= _destinationStart + _length)
                return destinationNr;

            return _sourceStart + (destinationNr - _destinationStart);
        }

        public bool InSourceBounds(long sourceNr) => _sourceStart <= sourceNr && sourceNr < _sourceStart + _length;
        public bool InDestinationBounds(long destinationNr) => _destinationStart <= destinationNr && destinationNr < _destinationStart + _length;
    }

    private static List<Map> ParseMap(IEnumerable<string> lines, ref int offset)
    {
        var maps = lines
            .Skip(offset) // skip previous maps
            .TakeWhile(l => !string.IsNullOrWhiteSpace(l)) // go until next map
            .Select(l => l.Split(' '))
            .Select(x => new Map(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .ToList();

        offset += maps.Count + 2; // +2 to skip the empty line and the "X-to-X map:" line
        return maps;
    }

    private List<Map>? _seedToSoil, _soilToFertilizer, _fertilizerToWater, _waterToLight, _lightToTemperature,
        _temperatureToHumidity, _humidityToLocation;
    
    public void Solve(bool part1)
    {
        var offset = 0;
        var lines = File.ReadAllLines("../../../input/Day5.txt");
        var seedInput = lines.First()["seeds: ".Length..].Split(' ').Select(long.Parse).ToList();
        offset += 3;

        var seedRanges = new List<Range>();
        if (!part1) // part 2: treat the seeds as ranges
        {
            for (var i = 0; i < seedInput.Count - 1; i += 2)
                seedRanges.Add(new Range(seedInput[i], seedInput[i + 1]));
        }
        
        _seedToSoil = ParseMap(lines, ref offset);
        _soilToFertilizer = ParseMap(lines, ref offset);
        _fertilizerToWater = ParseMap(lines, ref offset);
        _waterToLight = ParseMap(lines, ref offset);
        _lightToTemperature = ParseMap(lines, ref offset);
        _temperatureToHumidity = ParseMap(lines, ref offset);
        _humidityToLocation = ParseMap(lines, ref offset);

        if (part1)
        {
            Console.WriteLine(seedInput.Select(SeedToLocation).Min());
            return;
        }

        // Part 2
        for (long i = 0;; ++i)
        {
            if (i != 0 && i % 1_000_000 == 0)
                Qonsole.OverWrite($"{i / 1_000_000} million iterations{string.Concat(Enumerable.Repeat('.', (int)(i / 1_000_000) % 3 + 1))}");

            var seed = LocationToSeed(i);
            if (seedRanges.Any(range => range.Contains(seed)))
            {
                Qonsole.OverWrite($"{i}");
                break;
            }    
        }
    }
    
    private long SeedToLocation(long seed)
    {
        var soil = GetDestination(_seedToSoil!, seed);
        var fertilizer = GetDestination(_soilToFertilizer!, soil);
        var water = GetDestination(_fertilizerToWater!, fertilizer);
        var light = GetDestination(_waterToLight!, water);
        var temperature = GetDestination(_lightToTemperature!, light);
        var humidity = GetDestination(_temperatureToHumidity!, temperature);
        var location = GetDestination(_humidityToLocation!, humidity);
        return location;
    }
    
    private static long GetDestination(IEnumerable<Map> maps, long source)
    {
        var submap = maps.FirstOrDefault(sm => sm.InSourceBounds(source));
        return submap?.MapToDestination(source) ?? source;
    }

    private long LocationToSeed(long location)
    {
        var humidity = GetSource(_humidityToLocation!, location);
        var temperature = GetSource(_temperatureToHumidity!, humidity);
        var light = GetSource(_lightToTemperature!, temperature);
        var water = GetSource(_waterToLight!, light);
        var fertilizer = GetSource(_fertilizerToWater!, water);
        var soil = GetSource(_soilToFertilizer!, fertilizer);
        var seed = GetSource(_seedToSoil!, soil);

        return seed;
    }

    private static long GetSource(IEnumerable<Map> maps, long destination)
    {
        var submap = maps.FirstOrDefault(sm => sm.InDestinationBounds(destination));
        return submap?.MapToSource(destination) ?? destination;
    }
}