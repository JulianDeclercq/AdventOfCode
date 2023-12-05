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
        public Map(List<SubMap> submaps)
        {
            SubMaps = submaps;
        }

        public readonly List<SubMap> SubMaps;
    }
      
    private class SubMap
    {
        public SubMap(long destinationStart, long sourceStart, long length)
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

        public bool InBounds(long sourceNr)
        {
            if (sourceNr < _sourceStart || sourceNr >= _sourceStart + _length)
                return false;

            return true;
        }

        public long MinimumSource => _sourceStart;
        public long MinimumDestination => _destinationStart;
        public long MaximumDestination => _destinationStart + _length - 1;
    }

    private static Map ParseMap(IEnumerable<string> lines, ref int offset)
    {
        var subMaps = lines
            .Skip(offset) // skip previous maps
            .TakeWhile(l => !string.IsNullOrWhiteSpace(l)) // go until next map
            .Select(l => l.Split(' '))
            .Select(x => new SubMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .OrderBy(x => x.MinimumSource) // this sort is safe because no ranges overlap in the input.
            .ToList();

        offset += subMaps.Count + 2; // +2 to skip the empty line and the "X-to-X map:" line
        return new Map(subMaps);
    }

    private Map? _seedToSoil, _soilToFertilizer, _fertilizerToWater, _waterToLight, _lightToTemperature,
        _temperatureToHumidity, _humidityToLocation;
    
    public void Solve(bool part1)
    {
        var offset = 0;
        //var lines = File.ReadAllLines("../../../input/Day5_example.txt");
        var lines = File.ReadAllLines("../../../input/Day5.txt");
        var seedInput = lines.First()["seeds: ".Length..].Split(' ').Select(long.Parse).ToList();
        offset += 3;

        var ranges = new List<Range>();
        if (!part1) // part 2: treat the seeds as ranges
        {
            for (var i = 0; i < seedInput.Count - 1; i += 2)
                ranges.Add(new Range(seedInput[i], seedInput[i + 1]));
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
                Qonsole.OverWrite($"{i / 1_000_000} million iterations{string.Concat(Enumerable.Repeat('.', (int)(i / 1_000_000) % 3))}");

            var seed = LocationToSeed(i);
            if (ranges.Any(range => range.Contains(seed)))
            {
                Qonsole.OverWrite($"answer is {i}");
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

    private static long GetDestination(Map map, long source)
    {
        var submap = map.SubMaps.FirstOrDefault(osm => osm.InBounds(source));
        return submap?.MapToDestination(source) ?? source;
    }
    
    private static long GetSource(Map map, long destination)
    {
        foreach (var el in map.SubMaps)
        {
            if (el.MinimumDestination <= destination && destination <= el.MaximumDestination)
            {
                return el.MapToSource(destination);
            }
        }
        
        // nothing found, return destination itself
        return destination;
    }
}