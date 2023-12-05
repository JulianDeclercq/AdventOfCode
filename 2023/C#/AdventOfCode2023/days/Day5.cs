using AdventOfCode2023.helpers;

namespace AdventOfCode2023.days;

public class Day5
{
      private class Map
    {
        public Map(List<SubMap> orderedSubmaps)
        {
            OrderedSubMaps = orderedSubmaps;
        }
        
        private long MinElement => OrderedSubMaps.First().MinimumSource;
        private long MaxElement => OrderedSubMaps.Last().MaximumSource;

        public bool InRange(long nr) => MinElement <= nr && nr <= MaxElement;
        public List<SubMap> OrderedSubMaps;
        
        public SubMap? AdaptedBinarySearchCorrectMap(long value)
        {
            return OrderedSubMaps.FirstOrDefault(osm => osm.InBounds(value));
            var left = 0;
            var right = OrderedSubMaps.Count - 1;
            while (left <= right)
            {
                var m = (left + right) / 2;

                var subMap = OrderedSubMaps[m];
                if (subMap.MinimumSource < value)
                {
                    if (subMap.MaximumSource >= value)
                        return subMap;

                    left = m + 1;
                } else if (subMap.MinimumSource > value)
                {
                    right = m - 1;
                }
                else return subMap; // only checks for match with minimumscore
            }
            return null;
        }
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

        public (bool inRange, long result) MapToDestination(long sourceNr)
        {
            if (sourceNr < _sourceStart || sourceNr >= _sourceStart + _length)
                return new(false, sourceNr);

            return new(true, _destinationStart + (sourceNr - _sourceStart));
        }

        public (bool inRange, long result) MapToSource(long destinationNr)
        {
            if (destinationNr < _destinationStart || destinationNr >= _destinationStart + _length)
                return new(false, destinationNr);

            return new(true, _sourceStart + (destinationNr - _destinationStart));
        }

        public bool InBounds(long sourceNr)
        {
            if (sourceNr < _sourceStart || sourceNr >= _sourceStart + _length)
                return false;

            return true;
        }

        public bool InBoundsDestination(long destinationNr)
        {
            if (destinationNr < _destinationStart || destinationNr >= _destinationStart + _length)
                return false;

            return true;
        }

        public long MinimumSource => _sourceStart;
        public long MaximumSource => _sourceStart + _length - 1;
        public long MinimumDestination => _destinationStart;
        public long MaximumDestination => _destinationStart + _length - 1;
       
        public IEnumerable<long> PossibleSources()
        {
            for (var i = MinimumSource; i <= MaximumSource; ++i)
                yield return i;
        }
        
        public IEnumerable<long> PossibleDestinations()
        {
            for (var i = MinimumDestination; i <= MaximumDestination; ++i)
                yield return i;
        }
    }

    private static Map ParseMap(IEnumerable<string> lines, ref int offset)
    {
        var subMaps = lines
            .Skip(offset) // skip previous maps
            .TakeWhile(l => !string.IsNullOrWhiteSpace(l)) // go until next map
            .Select(l => l.Split(' '))
            .Select(x => new SubMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .OrderBy(x => x.MinimumSource) // this sort is safe because no ranges overlap in the input.
            //.OrderBy(x => x.MinimumDestination) // IDK IF THIS IS FINE, PROBALBY NOT
            .ToList();

        offset += subMaps.Count + 2; // +2 to skip the empty line and the "X-to-X map:" line
        return new Map(subMaps);
    }

    private Map? _seedToSoil, _soilToFertilizer, _fertilizerToWater, _waterToLight, _lightToTemperature,
        _temperatureToHumidity, _humidityToLocation;

    private class JulleRange
    {
        public JulleRange(long start, long length)
        {
            Min = start;
            Max = start + length - 1;
            Length = length;
        }

        public bool Contains(long value) => Min <= value && value <= Max;
        
        public long Min;
        public long Max;
        public long Length;
    }
    public void Solve()
    {
        const bool part1 = true;
        var console = new ConsoleWriter();
        
        var offset = 0;
        //var lines = File.ReadAllLines("../../../input/Day5_example.txt");
        var lines = File.ReadAllLines("../../../input/Day5.txt");
        var seedInput = lines.First()["seeds: ".Length..].Split(' ').Select(long.Parse).ToList();
        offset += 3;

        Console.WriteLine("Parsing seeds");
        var parsedSeeds = new HashSet<long>();
        var ranges = new List<JulleRange>();
        if (part1) 
        {
            parsedSeeds = seedInput.ToHashSet();
        }
        else // part 2: treat the seeds as ranges
        {
            for (var i = 0; i < seedInput.Count - 1; i += 2)
                ranges.Add(new JulleRange(seedInput[i], seedInput[i + 1]));
        }
        
        _seedToSoil = ParseMap(lines, ref offset);
        _soilToFertilizer = ParseMap(lines, ref offset);
        _fertilizerToWater = ParseMap(lines, ref offset);
        _waterToLight = ParseMap(lines, ref offset);
        _lightToTemperature = ParseMap(lines, ref offset);
        _temperatureToHumidity = ParseMap(lines, ref offset);
        _humidityToLocation = ParseMap(lines, ref offset);

        for (long i = 0;; ++i)
        {
            if (i % 1_000_000 == 0)
            {
                if (i == 500_000_000)
                {
                    Console.WriteLine($"Failed to find solution after 500 million iterations");
                    break;
                }
                console.OverWrite($"{i / 1_000_000} million iterations");
                //Console.WriteLine($"{i / 1_000_000} million iterations");
            }

            var seed = LocationToSeed(i);

            if (part1)
            {
                if (parsedSeeds.Contains(seed))
                {
                    Console.WriteLine($"answer is {i}");
                    break;
                }    
            }
            else
            {
                if (ranges.Any(r => r.Contains(seed)))
                {
                    Console.WriteLine($"answer is {i}");
                    break;
                }    
            }
        }
        
    }

    private long LocationToSeed(long location)
    {
        var humidity = GetSourceDebug(_humidityToLocation!, location, "_temperatureToHumidity");
        var temperature = GetSourceDebug(_temperatureToHumidity!, humidity, "_temperatureToHumidity");
        var light = GetSourceDebug(_lightToTemperature!, temperature, "_lightToTemperature");
        var water = GetSourceDebug(_waterToLight!, light, "_waterToLight");
        var fertilizer = GetSourceDebug(_fertilizerToWater!, water, "_fertilizerToWater");
        var soil = GetSourceDebug(_soilToFertilizer!, fertilizer, "_soilToFertilizer");
        var seed = GetSourceDebug(_seedToSoil!, soil, "_seedToSoil");

        return seed;
    }
    
    private long SeedToLocation(long seed)
    {
        var soil = GetDestinationDebug(_seedToSoil!, seed, "_seedToSoil");
        var fertilizer = GetDestinationDebug(_soilToFertilizer!, soil, "_soilToFertilizer");
        var water = GetDestinationDebug(_fertilizerToWater!, fertilizer, "_fertilizerToWater");
        var light = GetDestinationDebug(_waterToLight!, water, "_waterToLight");
        var temperature = GetDestinationDebug(_lightToTemperature!, light, "_lightToTemperature");
        var humidity = GetDestinationDebug(_temperatureToHumidity!, temperature, "_temperatureToHumidity");
        var location = GetDestinationDebug(_humidityToLocation!, humidity, "_humidityToLocation");
        return location;
    }

    private static long GetDestinationDebug(Map map, long value, string name)
    {
        var before = value;
        var after = GetDestination(map, value);
        Console.WriteLine($"{name}: {before} => {after}");
        return after;
    }
    
    private static long GetDestination(Map map, long source)
    {
        if (!map.InRange(source))
            return source;

        var submap = map.OrderedSubMaps.FirstOrDefault(osm => osm.InBounds(source));
        if (submap == null)
            return source;
        
        var (inRange, result) = submap.MapToDestination(source);

        if (!inRange) throw new Exception("Incorrect submap");

        return result;
    }
    
    private static long GetSourceDebug(Map map, long destination, string name)
    {
        var before = destination;
        var after = GetSource(map, destination);
        //Console.WriteLine($"{name}: {before} => {after}");
        return after;
    }
    
    private static long GetSource(Map map, long destination)
    {
        foreach (var el in map.OrderedSubMaps)
        {
            if (el.MinimumDestination <= destination && destination <= el.MaximumDestination)
            {
                var (_, result) = el.MapToSource(destination);
                return result;
            }
        }
        
        // nothing found, return destination itself (ASSUMING THAT IS ALLRIGHT THE OTHER WAY AROUND WHICH IS WHAT I DID)
        return destination;
    }
}